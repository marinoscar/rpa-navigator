using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ApplicationElement : Item
    {

        public ApplicationElement(XElement el): base(el)
        {
            Attributes = new List<ElementAttribute>();
        }
        public string Type { get; set; }
        public string DataType { get; set; }

        public List<ElementAttribute> Attributes { get; set; }
    }
}
