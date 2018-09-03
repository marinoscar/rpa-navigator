using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public abstract class ExtractorBase
    {
        public string Decode(string text)
        {
            return WebUtility.HtmlDecode(text);
        }

        public string GetAttributeText(XElement el, string name)
        {
            if (el == null) return string.Empty;
            var attr = el.Attribute(name);
            return attr == null || string.IsNullOrWhiteSpace(attr.Value) ? string.Empty : Decode(attr.Value); 
        }

        public string GetElementValue(XElement el, string name)
        {
            if (el == null) return string.Empty;
            var element = el.Elements().FirstOrDefault(i => i.Name.LocalName == name);
            if (element == null) return string.Empty;
            return Decode(element.Value);
        }

        public abstract void Load();
    }
}
