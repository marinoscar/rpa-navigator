using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model
{
    public class XmlItem
    {

        public XmlItem(XElement element)
        {
            Xml = element;
        }

        public XElement Xml { get; private set; }


        protected virtual string GetAttributeValue(string name)
        {
            return GetAttributeValue(Xml, name);
        }

        protected virtual string GetAttributeValue(XElement el, string name)
        {
            var att = GetAttribute(el, name);
            if (att == null) return default(string);
            return att.Value;
        }

        protected virtual bool HasElement(string name)
        {
            return HasElement(Xml, name);
        }

        protected virtual bool HasElement(XElement el, string name)
        {
            return GetElement(el, name) != null;
        }

        protected virtual bool HasAttribute(string name)
        {
            return HasAttribute(Xml, name);
        }

        protected virtual bool HasAttribute(XElement el, string name)
        {
            return GetAttribute(el, name) != null;
        }

        protected virtual XAttribute GetAttribute(string name)
        {
            return GetAttribute(Xml, name);
        }

        protected virtual XAttribute GetAttribute(XElement el, string name)
        {
            if (el == null) return null;
            return el.Attributes().FirstOrDefault(i => i.Name.LocalName == name);
        }

        protected virtual XElement GetElement(string name)
        {
            return GetElement(Xml, name);
        }

        protected virtual XElement GetElement(XElement el, string name)
        {
            if (el == null) return null;
            return el.Elements().FirstOrDefault(i => i.Name.LocalName == name);
        }

        protected virtual string GetElementValue(string name)
        {
            return GetElementValue(Xml, name);
        }

        protected virtual string GetElementValue(XElement el, string name)
        {
            if (el == null) return default(string);
            var element = GetElement(el, name);
            if (element == null) return default(string);
            return element.Value;
        }

        protected virtual IEnumerable<string> GetElementValues(string name)
        {
            return GetElementValues(Xml, name);
        }

        protected virtual IEnumerable<string> GetElementValues(XElement el, string name)
        {
            return el.Elements().Where(i => i.Name.LocalName == name).Select(i => i.Value).ToList();
        }

        protected virtual void TrySetAttValue(string name, string value)
        {
            TrySetAttValue(Xml, name, value);
        }

        protected virtual void TrySetAttValue(XElement el, string name, string value)
        {
            var att = GetAttribute(el, name);
            if (att == null) return;
            att.Value = value;
        }

        protected virtual void TrySetElValue(string name, string value)
        {
            TrySetElValue(Xml, name, value);
        }

        protected virtual void TrySetElValue(XElement xml, string name, string value)
        {
            var el = GetElement(xml, name);
            if (el == null) return;
            el.Value = value;
        }
    }
}
