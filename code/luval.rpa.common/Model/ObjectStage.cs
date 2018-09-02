using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ObjectStage : Stage
    {
        public ObjectStage(XElement el) : base(el)
        {
            Actions = new List<ActionStage>();
            IdName = "id";
        }

        public List<ActionStage> Actions { get; set; }
        public ApplicationDefinition ApplicationDefinition { get; set; }
    }
}
