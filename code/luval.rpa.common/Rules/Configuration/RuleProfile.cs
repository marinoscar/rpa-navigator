using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace luval.rpa.common.rules.configuration
{
    [XmlRoot(ElementName = "ruleProfile")]
    public class RuleProfile
    {
        private const string nameOfFile = "profile.xml";
        public RuleProfile()
        {
            Rules = new List<RuleInfo>();
            Exclusions = new List<Exclusion>();
        }

        [XmlArray(ElementName = "rules")]
        public List<RuleInfo> Rules { get; set; }
        [XmlArray(ElementName = "exclusions")]
        public List<Exclusion> Exclusions { get; set; }

        public static RuleProfile LoadFromFile()
        {
            return LoadFromFile(nameOfFile);
        }
        public static RuleProfile LoadFromFile(string fileName)
        {
            var ser = new XmlSerializer(typeof(RuleProfile));
            var res = default(RuleProfile);
            using (var reader = new StreamReader(fileName))
            {
                res = (RuleProfile)ser.Deserialize(reader);
                reader.Close();
            }
            return res;
        }

        public static RuleProfile LoadDefault()
        {
            return new RuleProfile()
            {
                Rules = new List<RuleInfo>(new[] { new RuleInfo() { AssemblyFile = "luval.rpa.rules.dll" } }),
                Exclusions = new List<Exclusion>(new[] {
                    new Exclusion() { Name = "MS Excel VBO" },
                    new Exclusion() { Name = "Utility - Environment" },
                    new Exclusion() { Name = "Utility - General" },
                    new Exclusion() { Name = "Utility - Collection Manipulation" },
                    new Exclusion() { Name = "Email - POP3/SMTP" },
                    new Exclusion() { Name = "Calendars" },
                    new Exclusion() { Name = "Data - OLEDB" },
                    new Exclusion() { Name = "Data - SQL Server" },
                    new Exclusion() { Name = "MS Outlook Email VBO" },
                    new Exclusion() { Name = "MS Word VBO" },
                    new Exclusion() { Name = "System - Active Directory" },
                    new Exclusion() { Name = "" },
                    new Exclusion() { Name = "" },
                    new Exclusion() { Name = "" },
                    new Exclusion() { Name = "" },
                    new Exclusion() { Name = "" },
                    new Exclusion() { Name = "" },
                    new Exclusion() { Name = "" },
                })
            };
        }

        public void Save()
        {
            Save(nameOfFile);
        }

        public void Save(string fileName)
        {
            var ser = new XmlSerializer(typeof(RuleProfile));
            using (var writer = new StreamWriter(fileName))
            {
                ser.Serialize(writer, this);
                writer.Close();
            }
        }

        public static DirectoryInfo GetRuleDir()
        {
            return new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "CustomRules"));
        }
    }
}
