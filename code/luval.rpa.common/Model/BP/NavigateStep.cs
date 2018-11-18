using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class NavigateStep : XmlItem
    {
        public NavigateStep(XElement element) : base(element)
        {
            Arguments = new List<Argument>();
            var action = GetElement("action");
            if (action == null) return;
            var args = GetElement(action, "arguments");
            if (args == null) return;
            foreach (var arg in args.Elements().ToList())
            {
                Arguments.Add(new Argument(arg));
            }
        }

        public string Stage
        {
            get { return GetAttributeValue("stage"); }
            set { TrySetAttValue("stage", value); }
        }

        public string Expression
        {
            get { return GetAttributeValue("expr"); }
            set { TrySetAttValue("expr", value); }
        }

        public List<Argument> Arguments { get; set; }

        public string ElementId
        {
            get { return GetAttributeValue(GetElement("element"), "id"); }
            set { TrySetAttValue(GetElement("element"), "id", value); }
        }
        public string Action
        {
            get { return GetElementValue(GetElement("action"), "id"); }
            set { TrySetElValue(GetElement("action"), "id", value); }
        }
    }

}

