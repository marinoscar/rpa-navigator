using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.Attributes;

namespace luval.rpa.rules
{
    [Name("Data Items Not Initialized"),
     Description("Checks that there is no data item with an initial value")]
    public class DataItemsNotInitialized : IRule
    {
        public IEnumerable<Result> Execute(Release release)
        {
            var items = release.GetAnalysisUnits()
                .Where(i => i.Stage.Type == "Data" && ((DataItem)(i.Stage)).HasInitialValue).ToList();
            return items.Select(i => new Result() {
                Type = ResultType.Error,
                Parent = i.ParentName,
                Page = i.Page,
                StageId = i.Stage.Id,
                Stage = i.Stage.Name,
                StageType = i.Stage.Type,
                Message = string.Format("Data item {0} is initialized", i.Stage.Name),
                Description = "Data items should not be initialized",
                Scope = i.ParentType
            });
        }
    }
}
