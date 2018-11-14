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
    [Name("Data Items Are Inside Block Stage"),
     Description("Checks that all data items are inside a block stage")]
    public class TagQueueItemOnException : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var helper = new StageHelper();
            var res = new List<Result>();
            var units = release.GetAnalysisUnits(i => i.Type == "Action" || i.Type == "Anchor");
            var stages = units.Select(i => i.Stage).ToList();
            var mark = GetQueueStages(stages.Where(i => i.Type == "Action"), "Mark Exception").ToList();
            var tag = GetQueueStages(stages.Where(i => i.Type == "Action"), "Tag Item").ToList();
            foreach (var ac in mark)
            {
                //no tag for the mark exception
                var tagFound = tag.Any(i => helper.GetNextStage(i.OnSuccess, stages).Id == ac.Id);
                if (!tagFound)
                    res.Add(FromStageAnalysis(units.Single(i => i.Stage.Id == ac.Id),
                        ResultType.Error, string.Format(@"Mark exception stage ""{0}"" requires that item has the exception labeled", ac.Name),
                        ""
                        ));
            }            
            return res;
        }

        private IEnumerable<ActionStage> GetQueueStages(IEnumerable<Stage> stages)
        {
            //!string.IsNullOrWhiteSpace(i.Resource.Action) && i.Resource.Action == "Mark Exception"
            return stages.Select(i => (ActionStage)i)
                .Where(i => i.Resource != null && !string.IsNullOrWhiteSpace(i.Resource.Object) &&
                i.Resource.Object == "Blueprism.Automate.clsWorkQueuesActions").ToList();
        }

        private IEnumerable<ActionStage> GetQueueStages(IEnumerable<Stage> stages, string actionFilter)
        {
            return GetQueueStages(stages).Where(i => !string.IsNullOrWhiteSpace(i.Resource.Action) && i.Resource.Action == actionFilter).ToList();
        }
    }
}