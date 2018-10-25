using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class CsvOutputGenerator : ITableOutputGenerator
    {
        public string Create(IEnumerable<object> items)
        {
            if (items == null || !items.Any()) return null;
            var sw = new StringWriter();
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            var properties = items.First().GetType().GetProperties();
            var headers = properties.Select(i => i.Name).ToList();
            sw.WriteLine(string.Join(separator, headers));
            foreach(var item in items)
            {

                var values = properties.Select(i => Convert.ToString(i.GetValue(item)));
                sw.WriteLine(string.Join(separator, values));
            }
            return sw.ToString();
        }
    }
}
