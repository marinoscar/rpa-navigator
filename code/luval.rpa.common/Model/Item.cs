using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class Item
    {

        public Item(XElement element)
        {
            Xml = element;
        }

        public XElement Xml { get; private set; }


        protected virtual string GetAttributeValue(string name)
        {
            var att = GetAttribute(name);
            if (att == null) return default(string);
            return att.Value;
        }

        protected virtual XAttribute GetAttribute(string name)
        {
            return Xml.Attributes().FirstOrDefault(i => i.Name.LocalName == name);
        }

        protected virtual XElement GetElement(string name)
        {
            return Xml.Elements().FirstOrDefault(i => i.Name.LocalName == name);
        }

        protected virtual string GetElementValue(string name)
        {
            var element = GetElement(name);
            if (element == null) return default(string);
            return element.Value;
        }

        protected virtual IEnumerable<string> GetElementValues(string name)
        {
            return Xml.Elements().Where(i => i.Name.LocalName == name).Select(i => i.Value).ToList();
        }

        protected virtual void TrySetAttValue(string name, string value)
        {
            var att = GetAttribute(name);
            if (att == null) return;
            att.Value = value;
        }

        protected virtual void TrySetElValue(string name, string value)
        {
            var el = GetElement(name);
            if (el == null) return;
            el.Value = value;
        }
    }
}
