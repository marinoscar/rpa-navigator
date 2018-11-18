using luval.rpa.common.rules;
using luval.rpa.common.rules.attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{
    [Name("Navigate Actions Need To Check That Element Exist"),
     Description("Checks that before a navigation item that works on a element, there is a wait stage")]
    public class NavigateNeedToCheckElementExists : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var helper = new StageHelper();
            var res = new List<Result>();
            var units = release.GetAnalysisUnits();
            var navigateStages = FilterStagesToAnalize(release.GetAnalysisUnits());
            foreach (var u in navigateStages)
            {
                var stages = helper.FilterStagesByPage(u, units);
                if (!helper.HasAnImediatePreviousWait(u.Stage, units.Where(i => i.PageId == u.PageId).Select(i => i.Stage)))
                    res.Add(FromStageAnalysis(u, ResultType.Error,
                        string.Format("{0} stage {1} is not preceeded by a proper wait stage", u.Stage.Type, u.Stage.Name), ""));
            }
            return res;
        }

        private IEnumerable<StageAnalysisUnit> FilterStagesToAnalize(IEnumerable<StageAnalysisUnit> units)
        {
            var res = new List<StageAnalysisUnit>();
            var navigateUnits = units.Where(i => i.Stage.Type == "Navigate").ToList();
            var actions = new[] { "click", "press", "select", "focus" };
            foreach(var unit in navigateUnits)
            {
                var stage = ((NavigateStage)unit.Stage);
                foreach(var action in actions)
                {
                    if (stage.Actions != null && stage.Actions.Any(i => !string.IsNullOrWhiteSpace(i.Action) 
                        &&  i.Action.ToLowerInvariant().Contains(action)))
                        res.Add(unit);
                }
            }
            return res;
        }
    }
}
