using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace luval.rpa.common.rules.configuration
{
    [XmlType("object")]
    public class Exclusion
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
