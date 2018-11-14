﻿using luval.rpa.common.Model;
using luval.rpa.rules.core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class DataSetGeneator
    {
        public dynamic Create(RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release, bool groupRuleResult)
        {
            var units = release.GetAnalysisUnits();
            var result = new
            {
                RunProperties = CsvReportGenerator.GetRunProperites(release, profile),
                RuleResults = CsvReportGenerator.GetRuleResults(rules, results),
                DataItems = GetVariableData(units),
                Elements = GetElements(release),
                Exceptions = GetExceptionDetails(units),
                Results =groupRuleResult ? GetGroupedRules(rules, results) : results,
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
    }
}