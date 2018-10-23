using luval.rpa.common;
using luval.rpa.rules;
using luval.rpa.rules.core;
using luval.rpa.rules.core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace luval.rpa.commands
{
    class Program
    {
        static void Main(string[] args)
        {
            var prof = @"C:\Users\oscar.marin\Desktop\TMP\profile.xml";
            var ser = new XmlSerializer(typeof(RuleProfile));
            //var profile = new RuleProfile();
            //profile.Rules.Add(new Rule() { AssemblyName = typeof(DataItemsNotInitialized).AssemblyQualifiedName });
            //var sw = new StringWriter();
            //ser.Serialize(sw, profile);
            //File.WriteAllText(prof, sw.ToString());

            var newProfile = (RuleProfile)ser.Deserialize(File.OpenText(prof));
            var xml = File.ReadAllText(@"C:\Users\oscar.marin\Desktop\TMP\NA_CS_OTC_0001_Order Entry_v3.bprelease");
            var release = new ReleaseExtractor(xml);
            release.Load();
            var ruleEngine = new Runner();
            var results = ruleEngine.RunProfile(newProfile, release.Release);
            var report = new ReportGenerator(results);
            report.ToCsv(@"C:\Users\oscar.marin\Desktop\TMP\report.csv");
        }
    }
}
