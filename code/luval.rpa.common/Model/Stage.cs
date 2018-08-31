using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class Stage : Item
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public XElement Xml { get; set; }
    }
}
