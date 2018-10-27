using luval.rpa.common.Model;
using luval.rpa.rules.core.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class JsonOuput
    {
        public string CreateReport(RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release)
        {
            var units = release.GetAnalysisUnits();
            var result = new
            {
                RunProperties = CodeReviewReportGenerator.GetRunProperites(release, profile),
                RuleResults = CodeReviewReportGenerator.GetRuleResults(rules, results),
                DataItems = GetVariableData(units).Take(50),
                Elements = GetElements(release).Take(50),
                Results = GetGroupedRules(rules, results).Take(50),
            };
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        private IEnumerable<object> GetVariableData(IEnumerable<StageAnalysisUnit> units)
        {
            return units.Where(i => i.Stage.Type == "Data").Select(i =>
                new { Parent = i.ParentName, ParentType = i.ParentType, Page = i.Page,
                      Name = i.Stage.Name, DataType = ((DataItem)i.Stage).DataType,
                      InitialValue = ((DataItem)i.Stage).InitialValue, Exposure = ((DataItem)i.Stage).Exposure,
                      IsPrivate = ((DataItem)i.Stage).IsPrivate}).ToList();
        }

        private IEnumerable<object> GetGroupedRules(IEnumerable<IRule> rules, IEnumerable<Result> results)
        {
            var res = new List<object>();
            foreach(var rule in rules)
            {
                var item = new
                {
                    Name = rule.Name,
                    Description = "",
                    Results = results.Where(i => i.RuleName == rule.Name).Select(i => 
                        new { Type = i.Scope, Parent = i.Parent, Page = i.Page, Stage = i.Stage, StageType = i.StageType, Message = i.Message }).ToList().Take(10)
                };
                res.Add(item);
            }
            return res;
        }

        private IEnumerable<object> GetElements(Release release)
        {
            var res = new List<object>();
            var helper = new StageHelper();
            foreach(var obj in release.Objects)
            {
                foreach(var el in obj.ApplicationDefinition.Elements)
                {
                    var stages = helper.ElementUses(el, obj);
                    foreach(var att in el.Attributes.Where(i=> i.IsInUse))
                    {
                        res.Add(new {
                            Object = obj.Name,
                            Element = el.Name,
                            Type = el.Type,
                            Name = att.Name,
                            DataType = att.DataType,
                            Value = att.Value,
                            UseCount = stages.Count()
                        });
                    }
                }
            }
            return res;
        }
    }
}
