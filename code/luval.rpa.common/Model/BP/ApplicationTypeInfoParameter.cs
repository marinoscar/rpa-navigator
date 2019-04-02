using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ApplicationTypeInfoParameter : XmlItem
    {
        public ApplicationTypeInfoParameter(XElement xml) : base(xml)
        {
        }

        public string Parameter
        {
            get { return GetElementValue("name"); }
            set { TrySetAttValue("name", value); }
        }

        public string Value
        {
            get { return GetElementValue("value"); }
            set { TrySetAttValue("value", value); }
        }
    }
}
