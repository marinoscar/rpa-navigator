using luval.rpa.common.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules.attributes;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{
    [Name("App Modeller Element Attribute Count"),
     Description("Checks that a reasonable amount of attributes are used to select the element")]
    public class AppModellerElementAttrCount : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var helper = new StageHelper();
            foreach (var obj in release.Objects)
            {
                foreach (var el in obj.ApplicationDefinition.Elements.Where(i => !string.IsNullOrWhiteSpace(i.Type) && i.Type.ToLowerInvariant().Contains("html")))
                {
                    var atts = el.Attributes.Where(i => i.IsInUse).ToList();
                    var max = GetMaxCount();
                    if (atts.Count <= max)
                        continue;
                    var uses = helper.ElementUses(el, obj);
                    if (uses.Count() <= 0) continue;
                    res.Add(new Result()
                    {
                        Type = ResultType.Warning,
                        Parent = obj.Name,
                        Page = "Application Modeller",
                        Scope = "Object",
                        RuleName = Name,
                        RuleDescription = GetRuleDescription(),
                        Message = string.Format("Too many attributes are used to identify the element {0}, a total of {1} are in used, and the max allowed is {2}. List attributes {3}. Element is used {4} times in stages {5}"
                        , el.Name, atts.Count, max, string.Join(", ", atts.Select(i => i.Name)), uses.Count(), string.Join(",", uses.Select(i => i.Name)))
                    });
                }
            }
            return res;
        }

        private int GetMaxCount()
        {
            return GetSetting<int>("MaxCount", 5);
        }
    }
}
