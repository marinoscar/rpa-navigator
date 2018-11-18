using luval.rpa.rules.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.rules.core.Attributes;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules
{
    [Name("No Business Logic In Object"),
     Description("Checks for a reasonable amount of decisions in the actions of an object")]
    public class NoBusinessLogicInObject : RuleBase, IRule
    {
        private int _maxCount;

        public NoBusinessLogicInObject()
        {
            _maxCount = GetMaxCount();
        }

        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            foreach(var obj in release.Objects)
            {
                var count = GetDecisionCount(obj.MainPage);
                if(count > _maxCount) AddResult(res, "Initialize", obj);
                foreach(var page in obj.Pages)
                {
                    count = GetDecisionCount(page.Stages);
                    if (count > _maxCount) AddResult(res, page.Name, obj);
                }
            }
            return res;
        }

        private void AddResult(List<Result> res, string page, ObjectStage obj)
        {
            res.Add(new Result()
            {
                RuleName = Name,
                RuleDescription = GetRuleDescription(),
                Page = page,
                Type = ResultType.Warning,
                Parent = obj.Name,
                Scope = "Object",
                Message = string.Format("There are many decisions in action {0} the max allowed is {1}", page, _maxCount)
            });
        }

        private int GetDecisionCount(IEnumerable<Stage> stages)
        {
            return stages.Count(i => i.Type == "Decision");
        }

        private int GetMaxCount()
        {
            return GetSetting<int>("MaxCount", 7);
        }
    }
}
