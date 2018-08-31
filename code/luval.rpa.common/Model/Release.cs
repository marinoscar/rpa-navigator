using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.Model
{
    public class Release
    {
        public Release()
        {
            Objects = new List<ObjectStage>();
        }
        public List<ObjectStage> Objects { get; set; }
    }
}
