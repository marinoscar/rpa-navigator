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
            throw new NotImplementedException();
        }
    }
}
