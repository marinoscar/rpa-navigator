﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules.attributes;
using luval.rpa.common.rules;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{
    [Name("Data Item Not Initialized"),
     Description("Checks that there is no data item with an initial value")]
    public class DataItemNotInitialized : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var items = release.GetAnalysisUnits()
                .Where(i => i.Stage.Type == "Data")
                .Where(
                    i => {
                        var item = ((DataItem)(i.Stage));
                        return item.HasInitialValue &&
                        string.IsNullOrWhiteSpace(item.Exposure);
                    }).ToList();
            return items.Select(i => FromStageAnalysis(i, ResultType.Error,
                string.Format("Data item {0} is initialized", i.Stage.Name),
                "Data items should not be initialized"));
        }
    }
}
