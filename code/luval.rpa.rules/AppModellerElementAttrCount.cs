using luval.rpa.rules.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.core.Attributes;

namespace luval.rpa.rules
{
    [Name("App Modeller Element Attribute Count"),
     Description("Checks that a reasonable amount of attributes are used to select the element")]
    public class AppModellerElementAttrCount : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            foreach (var obj in release.Objects)
            {
                foreach (var el in obj.ApplicationDefinition.Elements.Where(i => i.Type.ToLowerInvariant().Contains("html")))
                {
                    var atts = el.Attributes.Where(i => i.IsInUse).ToList();
                    var max = GetMaxCount();
                    if (atts.Count < max)
                        continue;

                    res.Add(new Result()
                    {
                        Parent = obj.Name,
                        Page = "Application Modeller",
                        Scope = "Object",
                        RuleName = GetRuleName(),
                        RuleDescription = GetRuleDescription(),
                        Message = string.Format("Too many attributes are used to identify the element, a total of {0} are in used, and the max allowed is {1}. List attributes {2}"
                        , atts.Count, max, string.Join(", ", atts.Select(i => i.Name)))
                    });
                }
            }
            return res;
        }

        //TODO: Make sure this comes from a configuration setting
        private int GetMaxCount()
        {
            return 4;
        }
    }
}
