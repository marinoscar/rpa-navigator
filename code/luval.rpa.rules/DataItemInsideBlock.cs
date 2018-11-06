using luval.rpa.rules.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.Model;
using luval.rpa.rules.core.Attributes;

namespace luval.rpa.rules
{

    [Name("Data Items Are Inside Block Stage"),
     Description("Checks that all data items are inside a block stage")]
    public class DataItemInsideBlock : RuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var units = release.GetAnalysisUnits().Where(i => i.Stage.Type == "Data" || i.Stage.Type == "Block").ToList();
            throw new NotImplementedException();
        }

        
    }
}
