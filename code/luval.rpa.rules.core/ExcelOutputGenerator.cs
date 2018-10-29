using luval.rpa.common.Model;
using luval.rpa.rules.core.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class ExcelOutputGenerator
    {
        public void CreateReport(string fileName, RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release)
        {
            var ds = (new DataSetGeneator()).Create(profile, rules, results, release, false);
            using (var p = new ExcelPackage())
            {
                var summaryWs = p.Workbook.Worksheets.Add("Summary");
                var row = LoadCollection(ds.RunProperties, summaryWs, "RunPropertiesTable", 0, true);
                LoadCollection(ds.RuleResults, summaryWs, "RuleResutsTable", row + 1, true);
                var resultsWs = p.Workbook.Worksheets.Add("Results");
                LoadCollection(ds.Results, resultsWs, "ResultsTable", 0);
                var dataItemsWs = p.Workbook.Worksheets.Add("DataItems");
                LoadCollection(ds.DataItems, dataItemsWs, "DataItemsTable", 0);
                var elementsWs = p.Workbook.Worksheets.Add("Elements");
                LoadCollection(ds.Elements, elementsWs, "ElementsTable", 0);
                var exceptionsWs = p.Workbook.Worksheets.Add("Exceptions");
                LoadCollection(ds.Exceptions, exceptionsWs, "ExceptionsTable", 0);
                p.SaveAs(new FileInfo(fileName));
            }
        }

        private int LoadCollection(IEnumerable<object> items, ExcelWorksheet ws, string tableName, int startIndex, bool tryCast = false)
        {
            if (!items.Any()) return 0;
            var row = 1 + startIndex;
            var start = row;
            var colIdx = 1;
            var props = GetProperties(items.First());
            bool isFirst = true;
            colIdx = 1;
            foreach (var item in items)
            {
                foreach (var prop in props)
                {
                    if (!isFirst)
                        ws.Cells[row, colIdx].Value = TryConvert(prop.GetValue(item), tryCast);
                    else
                        ws.Cells[row, colIdx].Value = prop.Name;
                    colIdx++;
                }
                if (isFirst) isFirst = false;
                colIdx = 1;
                row++;
            }
            CreateTable(start, row - 1, props.Count, tableName, ws);
            return row;
        }

        private object TryConvert(object val, bool tryCast)
        {
            if (!tryCast) return val;
            if (decimal.TryParse(Convert.ToString(val), out decimal num)) return num;
            if (DateTime.TryParse(Convert.ToString(val), out DateTime dt)) return dt;
            return Convert.ToString(val);

        }

        private void CreateTable(int startRow, int endRow, int columnCount, string name, ExcelWorksheet ws)
        {
            var range = ws.Cells[startRow, 1, endRow, columnCount];
            var table = ws.Tables.Add(range, name);

        }
        private List<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }
    }
}
