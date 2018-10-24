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

        public ReportGenerator(RuleProfile profile, Release release, IEnumerable<Result> results)
        {
            _profile = profile;
            _release = release;
            Results = new List<Result>(results);
        }

        public List<Result> Results { get; private set; }

        public void ToCsv(string outputfile)
        {
            var runData = GetRunProperites();
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var properties = typeof(Result).GetProperties();
            var headers = properties.Select(i => i.Name).ToList();
            var sw = new StringWriter();
            foreach(var rd in runData)
            {
                sw.WriteLine("{0}{1}{2}", rd.Key, separator, rd.Value);
            }
            sw.WriteLine();
            sw.WriteLine(string.Join(separator, headers));
            foreach(var res in Results)
            {
                var values = properties.Select(i => Convert.ToString(i.GetValue(res)));
                sw.WriteLine(string.Join(separator, values));
            }
            File.WriteAllText(outputfile, sw.ToString());
        }

        public Dictionary<string, string> GetRunProperites()
        {
            var dic = new Dictionary<string, string>();
            dic["RunAtUTC"] = DateTime.UtcNow.ToString("YYYY-MM-DD hh:mm:ss");
            dic["RunAtLocalTime"] = DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss");
            dic["RunOnMachine"] = Environment.MachineName;
            dic["RunByUser"] = Environment.UserName;
            dic["ProfileExclusions"] = string.Join("*", _profile.Exclusions.Select(i => i.Name));
            dic["PackageName"] = _release.PackageName;
            dic["ReleaseName"] = _release.Name;
            dic["ReleaseObjectCount"] = _release.Objects.Count.ToString();
            dic["ReleaseProcessCount"] = _release.Processes.Count.ToString();
            return dic;
        }
    }
}
