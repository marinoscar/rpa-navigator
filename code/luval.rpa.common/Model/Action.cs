using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.Model
{
    public class ActionStage : Stage
    {
        public ActionStage()
        {
            Stages = new List<Stage>();
        }
        public List<Stage> Stages { get; set; }
    }
}
