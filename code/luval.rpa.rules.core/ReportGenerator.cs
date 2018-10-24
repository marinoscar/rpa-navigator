using luval.rpa.common.Model;
using luval.rpa.rules.core.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class ReportGenerator
    {

        private RuleProfile _profile;
        private Release _release;
        private IEnumerable<IRule> _rules;

        public ReportGenerator(RuleProfile profile, Release release, IEnumerable<Result> results, IEnumerable<IRule> rules)
        {
            _profile = profile;
            _release = release;
            _rules = rules;
            Results = new List<Result>(results);
        }

        public List<Result> Results { get; private set; }

        public void ToCsv(string outputfile)
        {
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var properties = typeof(Result).GetProperties();
            var headers = properties.Select(i => i.Name).ToList();
            var sw = new StringWriter();
            PrintDicAsCsv("Run Property", "Value", GetRunProperites(), separator, sw);
            PrintDicAsCsv("Rule Name", "Instance Count", GetRuleResults(), separator, sw);
            sw.WriteLine(string.Join(separator, headers));
            foreach(var res in Results)
            {
                var values = properties.Select(i => Convert.ToString(i.GetValue(res)));
                sw.WriteLine(string.Join(separator, values));
            }
            File.WriteAllText(outputfile, sw.ToString());
        }

        private void PrintDicAsCsv(string col1, string col2, Dictionary<string, string> dic, string sep, StringWriter sw)
        {
            sw.WriteLine("{0}{1}{2}", col1, sep, col2);
            foreach (var e in dic)
            {
                sw.WriteLine("{0}{1}{2}", e.Key, sep, e.Value);
            }
            sw.WriteLine();
        }

        public Dictionary<string, string> GetRunProperites()
        {
            var dic = new Dictionary<string, string>();
            dic["Run At UTC"] = DateTime.UtcNow.ToString("YYYY-MM-DD hh:mm:ss");
            dic["Run At Local Time"] = DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss");
            dic["Run On Machine"] = Environment.MachineName;
            dic["Run By User"] = Environment.UserName;
            dic["Profile Exclusions"] = string.Join("*", _profile.Exclusions.Select(i => i.Name));
            dic["Package Name"] = _release.PackageName;
            dic["Release Name"] = _release.Name;
            dic["Release Object Count"] = _release.Objects.Count.ToString();
            dic["Release Process Count"] = _release.Processes.Count.ToString();
            return dic;
        }

        public Dictionary<string, string> GetRuleResults()
        {
            var dic = new Dictionary<string, string>();
            foreach (var rule in _rules)
            {
                var count = Results.Count(i => i.RuleName == rule.Name);
                dic[rule.Name] = count.ToString();
            }
            return dic;
        }
    }
}
