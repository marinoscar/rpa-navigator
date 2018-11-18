﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules.attributes;
using luval.rpa.common.rules;
using System.Configuration;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules
{
    public abstract class RuleBase : IRule
    {
        /// <summary>
        /// Gets the name of the rule
        /// </summary>
        public string Name { get { return GetRuleName(); } }
        public string Description { get { return GetRuleDescription(); } }

        public RuleTarget RuleTarget { get { return RuleTarget.BP; } }

        public abstract IEnumerable<Result> Execute(Release release);

        public IEnumerable<Result> Execute(XmlItem package)
        {
            return Execute((Release)package);
        }

        protected virtual Result FromStageAnalysis(StageAnalysisUnit unit, ResultType type, string message, string description)
        {
            return new Result()
            {
                RuleName = Name,
                RuleDescription = GetRuleDescription(),
                Type = type,
                Scope = unit.ParentType,
                Parent = unit.ParentName,
                Page = unit.Page,
                Stage = unit.Stage.Name,
                StageType = unit.Stage.Type,
                StageId = unit.Stage.Id,
                Message = message,
                Description = description
            };
        }

        protected virtual Result FromPageBasedStage(PageBasedStage stage, ResultType type, string scope, string message, string description)
        {
            return new Result()
            {
                RuleName = GetRuleName(),
                RuleDescription = GetRuleDescription(),
                Type = type,
                Parent = stage.Name,
                Scope = scope,
                Message = message,
                Description = description
            };
        }

        protected virtual string GetRuleName()
        {
            var name = GetType().GetCustomAttributes(typeof(NameAttribute), true).FirstOrDefault();
            if (name == null) return string.Empty;
            return ((NameAttribute)name).Value;
        }

        protected virtual string GetRuleDescription()
        {
            var name = GetType().GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (name == null) return string.Empty;
            return ((DescriptionAttribute)name).Value;
        }

        protected virtual T GetSetting<T>(string name, T defaultValue)
        {
            var settingName = string.Format("{0}.{1}", GetType().Name, name);
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;
            return ((T)Convert.ChangeType(value, typeof(T)));
        }
    }
}