using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class Argument : XmlItem
    {
        public Argument(XElement element) : base(element)
        {

        }

        public string Name
        {
            get { return GetElementValue("id"); }
            set { TrySetElValue("id", value); }
        }

        public string Value
        {
            get { return GetElementValue("value"); }
            set { TrySetElValue("value", value); }
        }
    }
}
