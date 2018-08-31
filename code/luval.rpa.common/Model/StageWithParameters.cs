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
        public StageWithParameters()
        {
            Parameters = new List<Parameter>();
        }

        public StageWithParameters(XElement xml): base(xml)
        {

        }

        public List<Parameter> Parameters { get; set; }
    }
}
