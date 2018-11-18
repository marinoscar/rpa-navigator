using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules;
using luval.rpa.common.rules.attributes;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{
    [Name("Exception Recovery On  Main Page"),
     Description("Checks that the main page of the process has a way to recover from an exception")]
    public class ExceptionRecoveryOnMainPage : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var processes = release.Processes.Where(i => !i.MainPage.Any(e => e.Type == "Recover")).ToList();
            return processes.Select(i => FromPageBasedStage(i, ResultType.Error, "Process", 
                string.Format("Process {0} does not have a recover stage on the main page", i.Name), ""));
        }
    }
}
