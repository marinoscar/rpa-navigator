using luval.rpa.common.rules;
using luval.rpa.common.rules.attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{
    [Name("Wait Stage Should Throw Exception On Timeout"),
     Description("Checks that when a timeout occurs an exception is thrown")]
    public class WaitShouldThrowExceptionOnTimeout : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var units = release.GetAnalysisUnits();
            var waits = units.Where(i => i.Stage != null && i.Stage.Type == "WaitEnd").ToList();
            foreach (var wait in waits)
            {
                var ex = GetNextException(wait.Stage, units);
                if (ex == null)
                    res.Add(FromStageAnalysis(wait, ResultType.Error,
                        string.Format("Unable to find a exception after the timeout")
                        , ""));
                else if (!IsSystemException(ex))
                    res.Add(FromStageAnalysis(wait, ResultType.Error,
                        string.Format("Exception type after timeout should be a System Exception, current exception type is {0} with message {1}", ex.Details.Type, ex.Details.Detail)
                        , ""));
            }
            return res;
        }

        private bool IsSystemException(ExceptionStage ex)
        {
            return ex.Details != null && 
                !string.IsNullOrWhiteSpace(ex.Details.Type) &&
                CleanString(ex.Details.Type).ToLowerInvariant().Equals("system exception");
        }

        private string CleanString(string value)
        {
            return string.Join(" ", value.Trim().Split(" ".ToArray())
                .Where(i => !string.IsNullOrWhiteSpace(i))).Trim();
        }

        private ExceptionStage GetNextException(Stage stage, IEnumerable<StageAnalysisUnit> units)
        {
            if (stage.Type.ToLowerInvariant().Contains("exception")) return (ExceptionStage)stage;
            if (string.IsNullOrWhiteSpace(stage.OnSuccess)) return null;
            if (stage.Type.ToLowerInvariant() == "end") return null;
            var next = units.FirstOrDefault(i => i.Stage.Id == stage.OnSuccess);
            if (next == null || next.Stage == null) return null;
            return GetNextException(next.Stage, units);
        }
    }
}
