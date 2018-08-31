using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.Model
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Expression { get; set; }
        public string Type { get; set; }
        public string Stage { get; set; }
        public bool IsOutput { get; set; }
    }
}
