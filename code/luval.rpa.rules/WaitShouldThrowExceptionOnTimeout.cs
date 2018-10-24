using luval.rpa.rules.core;
using luval.rpa.rules.core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;

namespace luval.rpa.rules
{
    [Name("Wait Stage Should Throw Exception On Timeout"),
     Description("Checks that when a timeout occurs an exception is thrown")]
    public class TimeoutShouldThrowException : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var units = release.GetAnalysisUnits();
            var waits = units.Where(i => i.Stage != null && i.Stage.Type == "WaitEnd").ToList();
            foreach (var wait in waits)
            {
                if (!HasExceptionInPath(wait.Stage, units))
                    res.Add(FromStageAnalysis(wait, ResultType.Error,
                        string.Format("Unable to find a exception after the timeout")
                        , ""));

            }
            return res;
        }

        private bool HasExceptionInPath(Stage stage, IEnumerable<StageAnalysisUnit> units)
        {
            if (stage.Type.ToLowerInvariant().Contains("exception")) return true;
            if (string.IsNullOrWhiteSpace(stage.OnSuccess)) return false;
            if (stage.Type.ToLowerInvariant() == "end") return false;
            var next = units.FirstOrDefault(i => i.Stage.Id == stage.OnSuccess);
            if (next == null || next.Stage == null) return false;
            return HasExceptionInPath(next.Stage, units);
        }
    }
}
