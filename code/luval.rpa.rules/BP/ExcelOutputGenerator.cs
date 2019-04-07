using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using luval.rpa.common.rules;
using luval.rpa.common.rules.configuration;
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

namespace luval.rpa.rules.bp
{
    public class ExcelOutputGenerator
    {
        public void CreateReport(string fileName, IEnumerable<ExcelDataSheet> sheets)
        {
            using (var p = new ExcelPackage())
            {
                foreach (var sheet in sheets)
                {
                    var ws = p.Workbook.Worksheets.Add(sheet.SheetName);
                    LoadCollection(sheet.Data, ws, sheet.TableName, 0);
                }
                p.SaveAs(new FileInfo(fileName));
            }
        }

        public void CreateReport(string fileName, RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release)
        {
            var ds = (new DataSetGeneator()).Create(profile, rules, results, release, false);
            using (var p = new ExcelPackage())
            {
                var releaseWs = p.Workbook.Worksheets.Add("ReleaseInfo");
                LoadCollection(ds.RunProperties, releaseWs, "ReleaseInfoTable", 0, true);
                var rulesWs = p.Workbook.Worksheets.Add("Rules");
                LoadCollection(ds.RuleResults, rulesWs, "RulesTable", 0, true);
                var resultsWs = p.Workbook.Worksheets.Add("Results");
                LoadCollection(ds.Results, resultsWs, "ResultsTable", 0);
                var summaryWs = p.Workbook.Worksheets.Add("Summary");
                CreateResultsPivot(summaryWs, resultsWs.Tables["ResultsTable"]);
                var dataItemsWs = p.Workbook.Worksheets.Add("DataItems");
                LoadCollection(ds.DataItems, dataItemsWs, "DataItemsTable", 0);
                var elementsWs = p.Workbook.Worksheets.Add("Elements");
                LoadCollection(ds.Elements, elementsWs, "ElementsTable", 0);
                var actionsWs = p.Workbook.Worksheets.Add("Actions");
                LoadCollection(ds.Actions, actionsWs, "ActionsTable", 0);
                var navigationsWs = p.Workbook.Worksheets.Add("Navigations");
                LoadCollection(ds.Navigations, navigationsWs, "NavigationTable", 0);
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
            colIdx = 1;
            //headers
            foreach (var prop in props)
            {
                ws.Cells[row, colIdx].Value = prop.Name;
                colIdx++;
            }
            row++;
            colIdx = 1;
            foreach (var item in items)
            {
                foreach (var prop in props)
                {
                    try
                    {
                        var itemProp = item.GetType().GetProperty(prop.Name);
                        ws.Cells[row, colIdx].Value = TryConvert(itemProp.GetValue(item, null), tryCast);
                        colIdx++;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(string.Format("Failed to enter value for {0}", prop.Name), ex);
                    }
                }
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
                ws.Column(i + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                ws.Column(i + 1).Style.WrapText = true;
            }
            var table = ws.Tables.Add(range, name);
        }
        private List<PropertyInfo> GetProperties(object obj)
        {
            return obj.GetType().GetProperties().ToList();
        }
    }

    public class ExcelDataSheet
    {
        public string SheetName { get; set; }
        public string TableName { get; set; }
        public IEnumerable<object> Data { get; set; }
    }
}
