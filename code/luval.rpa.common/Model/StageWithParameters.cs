using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class StageWithParameters : Stage
    {
        public StageWithParameters(XElement el):base(el)
        {
            Parameters = new List<Parameter>();
        }

        public List<Parameter> Parameters { get; set; }
    }
}
