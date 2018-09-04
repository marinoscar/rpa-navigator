using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules
{
    public class ReportGenerator
    {
        public ReportGenerator(IEnumerable<Result> results)
        {
            Results = new List<Result>(results);
        }

        public List<Result> Results { get; private set; }

        public void ToCsv(string outputfile)
        {
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var properties = typeof(Result).GetProperties();
            var headers = properties.Select(i => i.Name).ToList();
            var sw = new StringWriter();
            sw.WriteLine(string.Join(separator, headers));
            foreach(var res in Results)
            {
                var values = properties.Select(i => Convert.ToString(i.GetValue(res)));
                sw.WriteLine(string.Join(separator, values));
            }
            File.WriteAllText(outputfile, sw.ToString());
        }
    }
}
