using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.core.Attributes;
using luval.rpa.rules.core;
using luval.rpa.common.Model.BP;

namespace luval.rpa.rules
{
    [Name("Data Item Not Initialized"),
     Description("Checks that there is no data item with an initial value")]
    public class DataItemNotInitialized : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var items = release.GetAnalysisUnits()
                .Where(i => i.Stage.Type == "Data" && ((DataItem)(i.Stage)).HasInitialValue).ToList();
            return items.Select(i => FromStageAnalysis(i, ResultType.Error,
                string.Format("Data item {0} is initialized", i.Stage.Name),
                "Data items should not be initialized"));
        }
    }
}
