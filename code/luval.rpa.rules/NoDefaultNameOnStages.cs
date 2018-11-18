using luval.rpa.rules.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.core.Attributes;
using System.Text.RegularExpressions;
using luval.rpa.common.Model.BP;

namespace luval.rpa.rules
{
    [Name("No Default Name On Stages"),
     Description("Checks that there is a meaningful name for the stage")]
    public class NoDefaultNameOnStages : RuleBase, IRule
    {
        const string _exp = "{0} *[1-9]*";
        public override IEnumerable<Result> Execute(Release release)
        {
            var exclusions = GetExlusions().Split(",".ToArray());
            var units = release.GetAnalysisUnits().Where(i =>
                !exclusions.Contains(i.Stage.Type) &&
                Regex.IsMatch(i.Stage.Name.ToLowerInvariant(), string.Format(_exp, i.Stage.Type.ToLowerInvariant()))).ToList();
            var res = units.Select(i => FromStageAnalysis(i, ResultType.Warning, 
                string.Format("Stage name {0} is not a proper name", i.Stage.Name), "")).ToList();
            return res;
        }

        private string GetExlusions()
        {
            return GetSetting<string>("Exclusions", "Anchor,Start,End,Note,Recover,Resume");
        }
    }
}
