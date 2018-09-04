using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.Attributes;

namespace luval.rpa.rules
{
    [Name("Arbitrary Waits"),
     Description("Checks if there are arbitrary waits in the release")]
    public class ArbitraryWaits : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var waits = release.GetAnalysisUnits()
                .Where(i => i.ParentType == "Object" && i.Stage.Type == "WaitStart" &&
                       ((WaitStage)(i.Stage)).IsArbitraryWait).ToList();
            return waits.Select(i => FromStageAnalysis(i,
                ResultType.Warning, string.Format("Please check stage {0} for arbitrary waits", i.Stage.Name), ""));
        }
    }
}
