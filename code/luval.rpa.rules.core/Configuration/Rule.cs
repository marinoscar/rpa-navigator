using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace luval.rpa.rules.core.Configuration
{
    [XmlRoot(ElementName = "rule")]
    public class Rule
    {
        [XmlElement(ElementName ="assemblyFile")]
        public string AssemblyFile { get; set; }
    }
}
