using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.Model
{
    public class ObjectStage : Stage
    {
        public ObjectStage()
        {
            Actions = new List<ActionStage>();
        }
        public List<ActionStage> Actions { get; set; }
        public ApplicationDefinition ApplicationDefinition { get; set; }
    }
}
