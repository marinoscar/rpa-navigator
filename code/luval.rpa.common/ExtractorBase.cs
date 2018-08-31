using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ExtractorBase
    {
        public string Decode(string text)
        {
            return WebUtility.HtmlDecode(text);
        }

        public string GetAttributeText(XElement el, string name)
        {
            var attr = el.Attribute(name);
            return attr == null || string.IsNullOrWhiteSpace(attr.Value) ? string.Empty : Decode(attr.Value); 
        }
    }
}
