using luval.rpa.common.Model;
using luval.rpa.common.Model.BP;
using luval.rpa.rules.core;
using luval.rpa.rules.core.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules
{
    public class ExcelOutputGenerator
    {
        public void CreateReport(string fileName, RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release)
        {
            var ds = (new DataSetGeneator()).Create(profile, rules, results, release, false);
            using (var p = new ExcelPackage())
            {
                var releaseWs = p.Workbook.Worksheets.Add("ReleaseInfo");
                var row = LoadCollection(ds.RunProperties, releaseWs, "ReleaseInfoTable", 0, true);
                
                var resultsWs = p.Workbook.Worksheets.Add("Results");
                LoadCollection(ds.Results, resultsWs, "ResultsTable", 0);
                var summaryWs = p.Workbook.Worksheets.Add("Summary");
                CreateResultsPivot(summaryWs, resultsWs.Tables["ResultsTable"]);
                var dataItemsWs = p.Workbook.Worksheets.Add("DataItems");
                LoadCollection(ds.DataItems, dataItemsWs, "DataItemsTable", 0);
                var elementsWs = p.Workbook.Worksheets.Add("Elements");
                LoadCollection(ds.Elements, elementsWs, "ElementsTable", 0);
                var exceptionsWs = p.Workbook.Worksheets.Add("Exceptions");
                LoadCollection(ds.Exceptions, exceptionsWs, "ExceptionsTable", 0);
                p.SaveAs(new FileInfo(fileName));
            }
        }

        private void CreateResultsPivot(ExcelWorksheet ws, ExcelTable table)
        {
            var range = table.WorkSheet.Cells[table.Address.Address];
            var pivotTable = ws.PivotTables.Add(ws.Cells["A1"], range, "ResultPivotTable");
            pivotTable.RowFields.Add(pivotTable.Fields["RuleName"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Scope"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Parent"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Page"]);
            pivotTable.RowFields.Add(pivotTable.Fields["Message"]);
            var dataField = pivotTable.DataFields.Add(pivotTable.Fields["Message"]);
            dataField.Name = "Count";
            dataField.Function = DataFieldFunctions.Count;
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
            range.AutoFitColumns(9, 80);
            for (int i = 0; i < range.Columns; i++)
            {
                ws.Column(i+1).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                ws.Column(i+1).Style.WrapText = true;
            }
            var table = ws.Tables.Add(range, name);
        }
        private List<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }
    }
}
