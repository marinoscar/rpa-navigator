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
    [Name("Global And Session Data Items On Main Page"),
    Description("Global And Session Data Items On Main Page")]
    public class GlobalAndSessionDataItemsOnMainPage : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var errors = release.GetAnalysisUnits()
                .Where(i => i.Page != "Main" && i.Stage.Type == "Data" && !((DataItem)i.Stage).IsPrivate).ToList();
            foreach(var error in errors)
            {
                res.Add(FromStageAnalysis(error, ResultType.Error, string.Format("Global Data Items need to be in the main or initialize page"), ""));
            }
            return res;
        }
    }
}
