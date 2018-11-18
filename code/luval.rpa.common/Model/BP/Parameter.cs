using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class Parameter : XmlItem
    {
        public Parameter(XElement element) : base(element)
        {
        }

        public string Type
        {
            get { return GetAttributeValue("type"); }
            set { TrySetAttValue("type", value); }
        }

        public string Name
        {
            get { return GetAttributeValue("name"); }
            set { TrySetAttValue("name", value); }
        }

        public string Description
        {
            get { return GetAttributeValue("narrative"); }
            set { TrySetAttValue("narrative", value); }
        }

        public string Expression
        {
            get { return GetAttributeValue("expr"); }
            set { TrySetAttValue("expr", value); }
        }

        public string Stage
        {
            get { return GetAttributeValue("stage"); }
            set { TrySetAttValue("stage", value); }
        }
    }
}
