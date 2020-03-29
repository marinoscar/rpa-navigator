using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules.attributes;
using luval.rpa.common.rules;
using System.Configuration;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{
    public abstract class BPRuleBase : RuleBase
    {
        

        public override RuleTarget RuleTarget { get { return RuleTarget.BP; } }

        public abstract IEnumerable<Result> Execute(Release release);

        public override IEnumerable<Result> Execute(XmlItem package)
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

        protected virtual T GetSetting<T>(string name, T defaultValue)
        {
            var settingName = string.Format("{0}.{1}", GetType().Name, name);
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;
            return ((T)Convert.ChangeType(value, typeof(T)));
        }
    }
}
