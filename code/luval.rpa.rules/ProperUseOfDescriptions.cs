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
    [Name("Proper use of descriptions"),
     Description("Checks that main pages, inputs and outputs have descriptions")]
    public class ProperUseOfDescriptions : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var units = release.GetAnalysisUnits();
            VerifyMainPages(res, release.Processes, "Process");
            VerifyMainPages(res, release.Processes, "Object");
            VerifyStartAndEnd(res, units);
            return res;
        }

        private void VerifyMainPages(List<Result> res, IEnumerable<PageBasedStage> items, string scope)
        {
            foreach (var i in items)
            {
                if (string.IsNullOrWhiteSpace(i.Description) || i.Description.Length <= 4)
                    res.Add(new Result()
                    {
                        Page = "Main",
                        Parent = i.Name,
                        Scope = scope,
                        RuleName = GetRuleName(),
                        RuleDescription = GetRuleDescription(),
                        Type = ResultType.Error,
                        Message = string.Format("Main page needs to have a proper description")
                    });
            }
        }

        private void VerifyStartAndEnd(List<Result> res, IEnumerable<StageAnalysisUnit> units)
        {
            var starts = units.Where(i => i.Stage.Type == "Start").ToList();
            foreach (var start in starts)
            {
                foreach (var param in ((StartStage)start.Stage).Inputs)
                    VerifyParam(res, param, start);
            }
            var ends = units.Where(i => i.Stage.Type == "End").ToList();
            foreach (var end in ends)
            {
                foreach (var param in ((EndStage)end.Stage).Outputs)
                    VerifyParam(res, param, end);
            }
        }

        private void VerifyParam(List<Result> res, Parameter param, StageAnalysisUnit unit)
        {
            if (!string.IsNullOrWhiteSpace(param.Description) && param.Description.Length > 3) return;
            res.Add(FromStageAnalysis(unit, ResultType.Error,
                string.Format("Parameter is missing a description"), ""));
        }
    }
}