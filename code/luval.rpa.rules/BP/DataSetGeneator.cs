using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using luval.rpa.common.rules;
using luval.rpa.common.rules.configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.bp
{
    public class DataSetGeneator
    {
        public dynamic Create(RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release, bool groupRuleResult)
        {
            var units = release.GetAnalysisUnits();
            var result = new
            {
                RunProperties = GetRunProperites(release, profile),
                RuleResults = GetRuleResults(rules, results),
                DataItems = GetVariableData(units),
                Actions = units.Where(i => i.Stage.Type == "Action").Select(i => new {
                    ParentType = i.ParentType,
                    Parent = i.ParentName,
                    Page = i.Page,
                    Stage = i.Stage.Name,
                    Object = ((ActionStage)i.Stage).Resource.Object,
                    Action = ((ActionStage)i.Stage).Resource.Action
                }),
                Elements = GetElements(release),
                Exceptions = GetExceptionDetails(units),
                Navigations = GetNavigateStages(units),
                Results =groupRuleResult ? GetGroupedRules(rules, results) : results.Select(i => new {
                    RuleName = i.RuleName, 
                    Type = i.Type,
                    Message = i.Message,
                    Scope = i.Scope,
                    Parent = i.Parent,
                    Page = i.Page,
                    Stage = i.Stage,
                    StageType = i.StageType,
                    StageId = i.StageId
                }).ToList(),
            };
            return result;
        }

        private IEnumerable<object> GetExceptionDetails(IEnumerable<StageAnalysisUnit> units)
        {
            return units.Where(i => !string.IsNullOrWhiteSpace(i.Stage.Type) && i.Stage.Type == "Exception")
                .Select(i => new
                {
                    Parent = i.ParentName,
                    ParentType = i.ParentType,
                    Page = i.Page,
                    Name = ((ExceptionStage)i.Stage).Name,
                    Detail = ((ExceptionStage)i.Stage).Details.Detail,
                    Type = ((ExceptionStage)i.Stage).Details.Type,
                    UseCurrent = ((ExceptionStage)i.Stage).Details.UseCurrent
                });
        }

        private IEnumerable<object> GetNavigateStages(IEnumerable<StageAnalysisUnit> units)
        {
            return units.Where(i => !string.IsNullOrWhiteSpace(i.Stage.Type) && i.Stage.Type == "Navigate")
                .Select(i => {
                    var stage = ((NavigateStage)i.Stage);
                    var step = default(NavigateStep);
                    if (stage.Actions != null && stage.Actions.Any())
                        step = stage.Actions.First();
                    var res = new
                    {
                        Parent = i.ParentName,
                        ParentType = i.ParentType,
                        Page = i.Page,
                        Name = stage.Name,
                        Action = step != null ? step.Action : "",
                        ElementId = step != null ? step.ElementId : "",
                        Expression = step != null ? step.Expression : "",
                        Arguments = step == null ? "" : string.Join("|", step.Arguments.Select(j => string.Format("{0}={1}", j.Name, j.Value))) 
                    };
                    return res;
                });
        }

        private IEnumerable<object> GetVariableData(IEnumerable<StageAnalysisUnit> units)
        {
            return units.Where(i => i.Stage.Type == "Data").Select(i =>
                new {
                    Parent = i.ParentName,
                    ParentType = i.ParentType,
                    Page = i.Page,
                    Name = i.Stage.Name,
                    DataType = ((DataItem)i.Stage).DataType,
                    InitialValue = ((DataItem)i.Stage).InitialValue,
                    Exposure = ((DataItem)i.Stage).Exposure,
                    IsPrivate = ((DataItem)i.Stage).IsPrivate
                }).ToList();
        }

        private IEnumerable<object> GetGroupedRules(IEnumerable<IRule> rules, IEnumerable<Result> results)
        {
            var res = new List<object>();
            foreach (var rule in rules)
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
            foreach (var obj in release.Objects)
            {
                foreach (var el in obj.ApplicationDefinition.Elements)
                {
                    var stages = helper.ElementUses(el, obj);
                    foreach (var att in el.Attributes.Where(i => i.IsInUse))
                    {
                        res.Add(new
                        {
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

        public static IEnumerable<object> GetRuleResults(IEnumerable<IRule> rules, IEnumerable<Result> results)
        {
            var res = new List<dynamic>();
            foreach (var rule in rules)
            {
                var count = results.Count(i => i.RuleName == rule.Name);
                res.Add(new { Rule = rule.Name, Count = Convert.ToString(count), RuleDescription = rule.Description });
            }
            return res;
        }

        public static IEnumerable<object> GetRunProperites(Release release, RuleProfile profile)
        {
            var units = release.GetAnalysisUnits();
            var res = new List<dynamic>
            {
                new { Parameter = "Run At UTC", Value = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") },
                new { Parameter = "Run At Local Time", Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                new { Parameter = "Run On Machine", Value = Environment.MachineName },
                new { Parameter = "Run By User", Value = Environment.UserName },
                new { Parameter = "Profile Exclusions", Value = string.Join("*", profile.Exclusions.Select(i => i.Name)) },
                new { Parameter = "Package Name", Value = release.PackageName },
                new { Parameter = "Release Name", Value = release.Name },
                new { Parameter = "Object Count", Value = release.Objects.Count.ToString() },
                new { Parameter = "Process Count", Value = release.Processes.Count.ToString() },
                new { Parameter = "Total Stages", Value = units.Count().ToString() }
            };
            return res;
        }
    }
}
