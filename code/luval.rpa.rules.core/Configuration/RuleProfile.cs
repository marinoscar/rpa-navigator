using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace luval.rpa.rules.core.Configuration
{
    [XmlRoot(ElementName = "ruleProfile")]
    public class RuleProfile
    {
        public RuleProfile()
        {
            Rules = new List<Rule>();
            Exclusions = new List<Exclusion>();
        }

        [XmlArray(ElementName = "rules")]
        public List<Rule> Rules { get; set; }
        [XmlArray(ElementName = "exclusions")]
        public List<Exclusion> Exclusions { get; set; }

        public static RuleProfile LoadFromFile()
        {
            return LoadFromFile("profile.xml");
        }
        public static RuleProfile LoadFromFile(string fileName)
        {
            var ser = new XmlSerializer(typeof(RuleProfile));
            return (RuleProfile)ser.Deserialize(File.OpenRead(fileName));
        }

        public void Save()
        {
            Save("porfile.xml");
        }

        public void Save(string fileName)
        {
            var ser = new XmlSerializer(typeof(RuleProfile));
            ser.Serialize(File.OpenWrite(fileName), this);
        }
    }
}
