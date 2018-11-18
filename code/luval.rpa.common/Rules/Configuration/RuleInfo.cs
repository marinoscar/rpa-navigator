using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace luval.rpa.common.rules.configuration
{
    [XmlType("rule")]
    public class RuleInfo
    {
        [XmlElement(ElementName ="assemblyFile")]
        public string AssemblyFile { get; set; }
    }
}
