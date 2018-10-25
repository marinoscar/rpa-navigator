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
    public class CodeReviewReportGenerator
    {

        private RuleProfile _profile;
        private Release _release;
        private IEnumerable<IRule> _rules;

        public CodeReviewReportGenerator(RuleProfile profile, Release release, IEnumerable<Result> results, IEnumerable<IRule> rules)
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
            sw.WriteLine(GetRunProperites());
            sw.WriteLine(GetRuleResults());
            sw.WriteLine(Results);
            File.WriteAllText(outputfile, sw.ToString());
        }

        public IEnumerable<object> GetRunProperites()
        {
            var res = new List<dynamic>
            {
                new { Paramter = "Run At UTC", Value = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") },
                new { Paramter = "Run At Local Time", Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                new { Paramter = "Run On Machine", Value = Environment.MachineName },
                new { Paramter = "Run By User", Value = Environment.UserName },
                new { Paramter = "Profile Exclusions", Value = string.Join("*", _profile.Exclusions.Select(i => i.Name)) },
                new { Paramter = "Package Name", Value = _release.PackageName },
                new { Paramter = "Release Name", Value = _release.Name },
                new { Paramter = "Object Count", Value = _release.Objects.Count.ToString() },
                new { Paramter = "Process Count", Value = _release.Processes.Count.ToString() }
            };
            return res;
        }

        public IEnumerable<object> GetRuleResults()
        {
            var res = new List<dynamic>();
            foreach (var rule in _rules)
            {
                var count = Results.Count(i => i.RuleName == rule.Name);
                res.Add(new { Rule = rule.Name, Count = Convert.ToString(count) });
            }
            return res;
        }
    }
}
