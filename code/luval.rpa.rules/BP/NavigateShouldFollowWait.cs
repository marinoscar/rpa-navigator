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
    [Name("Navigate Should Have a Wait"),
     Description("Checks that after a navigate there is a proper wait")]
    public class NavigateShouldFollowWait : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            var units = release.GetAnalysisUnits();
            var navs = units.Where(i => i.Stage.Type == "Navigate").ToList();
            foreach (var nav in navs)
            {
                var navStage = ((NavigateStage)nav.Stage);
                if (!navStage.Actions.Any(i => !string.IsNullOrWhiteSpace(i.Action) && i.Action == "Navigate"))
                    continue;
                if (!IsNextWait(navStage, units))
                    res.Add(FromStageAnalysis(nav, ResultType.Error,
                        string.Format("A navigation stage needs to follow a wait stage to control the flow"), ""));
            }
            return res;
        }

        private bool IsNextWait(Stage stage, IEnumerable<StageAnalysisUnit> units)
        {
            if (stage == null || string.IsNullOrWhiteSpace(stage.OnSuccess)) return false;
            var next = units.FirstOrDefault(i => i.Stage.Id == stage.OnSuccess);
            if ((next == null || next.Stage.Type == "End")) return false;
            if (next.Stage.Type.ToLowerInvariant() == "anchor") return IsNextWait(next.Stage, units);
            if (next.Stage.Type.ToLowerInvariant().Contains("wait")) return true;
            return false;
        }

        private string[] _navigateActions = {
            "AttachApplication",
            "ActivateApp",
            "SendKeys",
            "Launch",
            "Press",
            "Default",
            "Select",
            "SetFocus",
            "SendKeyEvents",
            "AAFocus",
            "MouseClickCentre",
            "AAClickCentre",
            "Terminate",
            "HTMLInsertJavascriptFragment",
            "HTMLInvokeJavascriptMethod",
            "HTMLClickCentre",
            "HTMLSelectItem",
            "HTMLFocus",
            "DetachApplication",
            "AASendKeys",
            "MouseClick",
        };
    }
}
