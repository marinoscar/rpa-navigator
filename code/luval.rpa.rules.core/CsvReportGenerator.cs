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
    public class CsvReportGenerator
    {

        private RuleProfile _profile;
        private Release _release;
        private IEnumerable<IRule> _rules;

        public CsvReportGenerator(RuleProfile profile, Release release, IEnumerable<Result> results, IEnumerable<IRule> rules)
        {
            _profile = profile;
            _release = release;
            _rules = rules;
            Results = new List<Result>(results);
        }

        public List<Result> Results { get; private set; }

        public void ToCsv(string outputfile)
        {
            var generator = new CsvOutputGenerator();
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var properties = typeof(Result).GetProperties();
            var headers = properties.Select(i => i.Name).ToList();
            var sw = new StringWriter();
            sw.WriteLine(generator.Create(GetRunProperites()));
            sw.WriteLine(generator.Create(GetRuleResults()));
            sw.WriteLine(generator.Create(Results));
            File.WriteAllText(outputfile, sw.ToString());
        }

        public IEnumerable<object> GetRunProperites()
        {
            return GetRunProperites(_release, _profile);
        }

        public static IEnumerable<object> GetRunProperites(Release release, RuleProfile profile)
        {
            var units = release.GetAnalysisUnits();
            var res = new List<dynamic>
            {
                new { Parameter = "Run At UTC", Value = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") },
                new { Parameter = "Run At Local Time", Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                new { Parameter = "Run On Machine", Value = Environment.MachineName },
                new { Parameter = "Run By User", Value = Environment.UserName },
                new { Parameter = "Profile Exclusions", Value = string.Join("*", profile.Exclusions.Select(i => i.Name)) },
                new { Parameter = "Package Name", Value = release.PackageName },
                new { Parameter = "Release Name", Value = release.Name },
                new { Parameter = "Object Count", Value = release.Objects.Count.ToString() },
                new { Parameter = "Process Count", Value = release.Processes.Count.ToString() },
                new { Parameter = "Total Stages", Value = units.Count().ToString() }
            };
            return res;
        }

        public IEnumerable<object> GetRuleResults()
        {
            return GetRuleResults(_rules, Results);
        }

        public static IEnumerable<object> GetRuleResults(IEnumerable<IRule> rules, IEnumerable<Result> results)
        {
            var res = new List<dynamic>();
            foreach (var rule in rules)
            {
                var count = results.Count(i => i.RuleName == rule.Name);
                res.Add(new { Rule = rule.Name, Count = Convert.ToString(count), RuleDescription = rule.Description });
            }
            return res;
        }
    }
}
