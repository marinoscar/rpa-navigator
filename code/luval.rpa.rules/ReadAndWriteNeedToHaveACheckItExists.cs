﻿using luval.rpa.rules.core;
using luval.rpa.rules.core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;

namespace luval.rpa.rules
{
    [Name("Read And Write Stages Need To Check That Element Exists"),
     Description("Checks that before a read or write operation is executed there is a check that element exists")]
    public class ReadAndWriteNeedToHaveACheckItExists : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var units = release.GetAnalysisUnits();
            var readsAndWrites = units.Where(i => i.Stage.Type == "Read" || i.Stage.Type == "Write").ToList();
            foreach(var u in readsAndWrites)
            {
                if (HasExclusion(u.Stage)) continue;
                if (!HasCheckBefore(u.Stage, units))
                    res.Add(FromStageAnalysis(u, ResultType.Error,
                        string.Format("{0} stage {1} is not preceeded by a proper wait stage", u.Stage.Type, u.Stage.Name), ""));
            }
            return res;
        }

        private bool HasExclusion(Stage stage)
        {
            if(stage.Type == "Read")
            {
                var read = (ReadStage)stage;
                var ex = GetActionExlusion();
                return read.Actions.Any(i => ex.Contains(i.Action));
            }
            return false;
        }

        private bool HasCheckBefore(Stage stage, IEnumerable<StageAnalysisUnit> units)
        {
            var waits = units.Where(i => i.Stage.Type == "WaitStart").ToList();
            foreach(var wait in waits)
            {
                foreach(var c in ((WaitStartStage)wait.Stage).Choices)
                {
                    var nextStage = GetNextStage(c.OnTrue, units);
                    if (nextStage.Id == stage.Id) return true;
                }
            }
            return false;
        }

        private Stage GetNextStage(string nextId, IEnumerable<StageAnalysisUnit> units)
        {
            if (string.IsNullOrWhiteSpace(nextId)) return null;
            var unit = units.FirstOrDefault(i => i.Stage.Id == nextId);
            if (unit == null) return null;
            if (unit.Stage.Type == "Anchor") return GetNextStage(unit.Stage.OnSuccess, units);
            return unit.Stage;
        }

        private List<string> GetActionExlusion() {
            return GetActionExlusionSetting().Split(",".ToArray()).ToList();
        }

        private string GetActionExlusionSetting()
        {
            var defaultVal = "IsConnected,Snapshot,HTML Snapshot,Get Document URL,Get Document URL Domain,Get Screen Bounds,Get Bounds";
            return GetSetting<string>("Exclusions", defaultVal);
        }
    }
}
