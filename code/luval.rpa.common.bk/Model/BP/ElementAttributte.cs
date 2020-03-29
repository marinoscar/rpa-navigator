using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ElementAttribute : XmlItem
    {
        private XElement _process;
        public ElementAttribute(XElement element) : base(element)
        {
            _process = GetElement("ProcessValue");
        }

        public string Name
        {
            get { return GetAttributeValue("name"); }
            set { TrySetAttValue("name", value); }
        }

        public string InUse
        {
            get { return GetAttributeValue("inuse"); }
            set { TrySetAttValue("inuse", value); }
        }
        public string DataType
        {
            get { return GetAttributeValue(_process, "datatype"); }
            set { TrySetAttValue(_process, "datatype", value); }
        }

        public string Value
        {
            get { return GetAttributeValue(_process, "value"); }
            set { TrySetAttValue(_process, "value", value); }
        }

        public bool IsInUse { get { return (InUse == "True"); } }

    }
}
