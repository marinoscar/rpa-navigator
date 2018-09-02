using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ActionStage : Stage
    {
        public ActionStage(XElement el): base(el)
        {
            Stages = new List<Stage>();
            IdName = "subsheetid";
        }
        public List<Stage> Stages { get; set; }
    }
}
