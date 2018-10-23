using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.core;
using luval.rpa.rules.core.Attributes;

namespace luval.rpa.rules
{
    [Name("Exception handling on main page"),
     Description("Checks that the main page of the process has exception handling")]
    public class ExceptionHandlingOnMainPage : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var processes = release.Processes.Where(i => !i.MainPage.Any(e => e.Type == "Exception")).ToList();
            return processes.Select(i => FromPageBasedStage(i, ResultType.Error, "Process", 
                string.Format("Process {0} does not have a exception hadling on the main page", i.Name), ""));
        }
    }
}
