using luval.rpa.common.rules;
using luval.rpa.rules.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model.bp;
using luval.rpa.common.rules.attributes;

namespace luval.rpa.rules.bp
{
    [Name("Invasive Technique Being Used"),
     Description("Checks if the objects or stages are using the invasive techniques")]
    public class InvasiveTechniqueBeingUsed : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var objs = release.Objects.ToList();
            foreach(var obj in objs)
            {
                if (obj.ApplicationDefinition == null ||
                   obj.ApplicationDefinition.ApplicationTypeInfo == null ||
                   string.IsNullOrWhiteSpace(obj.ApplicationDefinition.ApplicationTypeInfo.Id) ||
                   !obj.ApplicationDefinition.ApplicationTypeInfo.Id.ToLowerInvariant().StartsWith("win32") ||
                   obj.ApplicationDefinition.ApplicationTypeInfo.Parameters == null) continue;

                var nonInvasive = obj.ApplicationDefinition.ApplicationTypeInfo.Parameters.FirstOrDefault(i => i.Parameter == "NonInvasive");
                if (nonInvasive != null && 
                    !string.IsNullOrWhiteSpace(nonInvasive.Value) &&
                    nonInvasive.Value.ToLowerInvariant().Equals("false"))
                {
                    res.Add(GetFromObject(obj, 
                        "Object is using invasive techniques"));
                }
                if (obj.ApplicationDefinition.ApplicationTypeInfo.Id.ToLowerInvariant().Equals("win32attach"))
                {
                    var path = obj.ApplicationDefinition.ApplicationTypeInfo.Parameters.FirstOrDefault(i => i.Parameter == "Path");
                    if(path != null && !string.IsNullOrWhiteSpace(path.Value))
                    {
                        res.Add(GetFromObject(obj,
                        "Object is setup to attach a process, by having the Path setup could create an out of memory exception, this is a known bug in all BP versions prior to 6.5"));
                    }
                }
                var navigateStages = obj.GetAllStages().Where(i => i.Type == "Navigate").Select(i => (NavigateStage)i).ToList();
                foreach(var stage in navigateStages)
                {
                    if (stage.Actions
                        .SelectMany(i => i.Arguments)
                        .Any(i => i.Name == "NonInvasive" &&
                        !string.IsNullOrWhiteSpace(i.Value) &&
                        i.Value.ToLowerInvariant().Equals("false")))
                        res.Add(new Result() {
                            StageId = stage.Id,
                            Stage = stage.Name,
                            StageType = stage.Type,
                            Page = stage.PageName,
                            Parent = obj.Name,
                            Scope = "Object",
                            RuleName = GetRuleName(),
                            RuleDescription = GetRuleDescription(),
                            Type = ResultType.Error,
                            Message = "The stage is using invasive techniques"
                        });
                }
            }
            return res;
        }

        private Result GetFromObject(ObjectStage obj, string message)
        {
            return new Result()
            {
                RuleName = GetRuleName(),
                RuleDescription = GetRuleDescription(),
                Parent = obj.Name,
                Type = ResultType.Error,
                Scope = "Object",
                Page = null,
                StageId = null,
                Stage = null,
                StageType = null,
                Message = message
            };
        }
    }
}
