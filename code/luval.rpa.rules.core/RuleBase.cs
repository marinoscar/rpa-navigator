using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.core.Attributes;
using luval.rpa.rules.core;
using System.Configuration;

namespace luval.rpa.rules
{
    public abstract class RuleBase : IRule
    {
        public abstract IEnumerable<Result> Execute(Release release);

        protected virtual Result FromStageAnalysis(StageAnalysisUnit unit, ResultType type, string message, string description)
        {
            return new Result()
            {
                RuleName = GetRuleName(),
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
