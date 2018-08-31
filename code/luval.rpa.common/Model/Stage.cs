using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class Stage : Item
    {

        public Stage()
        {

        }

        public Stage(XElement xml)
        {
            Id = xml.Attribute("stageid").Value;
            Name = xml.Attribute("name").Value;
            Type = xml.Attribute("type").Value;
            Xml = xml;
        }

        public string Type { get; set; }
        public string Description { get; set; }
        public XElement Xml { get; set; }
    }
}
