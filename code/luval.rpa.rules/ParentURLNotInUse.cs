﻿using luval.rpa.rules.core;
using luval.rpa.rules.core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.common.Model.BP;

namespace luval.rpa.rules
{
    [Name("Parent Url Not In Use"),
     Description("Checks that html elements are not using the parent url")]
    public class ParentURLNotInUse : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var helper = new StageHelper();
            foreach (var obj in release.Objects)
            {
                foreach (var el in obj.ApplicationDefinition.Elements.Where(i => i.Type.ToLowerInvariant().Contains("html")))
                {
                    foreach (var at in el.Attributes)
                    {
                        if (at.Name.ToLowerInvariant() == "purl" && at.IsInUse)
                        {
                            var uses = helper.ElementUses(el, obj);
                            if (uses.Count() <= 0) continue;
                            res.Add(new Result()
                            {
                                Type = ResultType.Error,
                                Parent = obj.Name,
                                Page = "Application Modeller",
                                Scope = "Object",
                                RuleName = Name,
                                RuleDescription = GetRuleDescription(),
                                Message = string.Format(@"Parent Url is being used for element ""{0}"" the element is being used {1} times in these stages {2}", 
                                el.Name, uses.Count(), string.Join(",", uses.Select(i => i.Name)))
                            });
                        }
                    }
                }
            }
            return res;
        }
    }
}
