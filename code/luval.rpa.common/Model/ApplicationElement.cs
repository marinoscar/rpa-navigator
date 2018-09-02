using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ApplicationElement : Stage
    {

        public ApplicationElement(XElement el): base(el)
        {
            Attributes = new List<ElementAttribute>();
            IdName = "id";
        }
        public string DataType
        {
            get { return GetAttributeValue("datatype"); }
            set { TrySetAttValue("datatype", value); }
        }

        public List<ElementAttribute> Attributes { get; set; }
    }
}
