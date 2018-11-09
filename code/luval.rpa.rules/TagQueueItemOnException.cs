using luval.rpa.rules.core;
using luval.rpa.rules.core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;

namespace luval.rpa.rules
{
    [Name("Data Items Are Inside Block Stage"),
     Description("Checks that all data items are inside a block stage")]
    public class TagQueueItemOnException : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();

            return res;
        }
    }
}
