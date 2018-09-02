using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class Choice : Item
    {

        private XElement _element;
        private XElement _condition;

        public Choice(XElement element) : base(element)
        {
            _element = GetElement("element");
            _condition = GetElement("condition");
        }

        public string Name
        {
            get { return GetElementValue("name"); }
            set { TrySetElValue("name", value); }
        }

        public string CompareType
        {
            get { return GetElementValue("comparetype"); }
            set { TrySetElValue("comparetype", value); }
        }

        public string ElementId
        {
            get { return GetAttributeValue(_element, "id"); }
            set { TrySetElValue(_element, "id", value); }
        }

        public string Condition
        {
            get { return GetAttributeValue(_element, "id"); }
            set { TrySetElValue(_element, "id", value); }
        }


    }
}
