using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class CsvOutputGenerator : ITableOutputGenerator
    {
        public string Create(IEnumerable<object> items)
        {
            var sw = new StringWriter();
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var properties = typeof(Result).GetProperties();
            var headers = properties.Select(i => i.Name).ToList();
            foreach(var item in items)
            {
                var values = properties.Select(i => Convert.ToString(i.GetValue(item)));
                sw.WriteLine(string.Join(separator, values));
            }
            return sw.ToString();
        }
    }
}
