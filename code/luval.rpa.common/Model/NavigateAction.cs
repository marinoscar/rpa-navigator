using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class NavigateAction : Item
    {
        public NavigateAction(XElement element) : base(element)
        {
            Arguments = new List<Argument>();
            var action = GetElement("action");
            var args = GetElement(action, "arguments");
            if (args == null) return;
            foreach(var arg in args.Elements().ToList())
            {
                Arguments.Add(new Argument(arg));
            }
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

