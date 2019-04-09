using luval.rpa.common.model.bp;
using luval.rpa.rules.bp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luval.rpa.navigator
{
    public class Reports
    {
        #region Non-Invasive

        public object ExecuteNonInvasiveReport(Release release, string fileName)
        {
            if (release == null) return null;
            var appModelerResult = new List<dynamic>();
            var stagesResult = new List<dynamic>();
            foreach (var obj in release.Objects)
            {
                var item = GetNonIvasiveReportItem(obj);
                if (item != null) appModelerResult.Add(item);
                var stages = GetNonInvasiveReportItemFromStages(obj);
                if (stages.Any()) stagesResult.AddRange(stages);
            }
            var generator = new ExcelOutputGenerator();
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            RunReport(() =>
            {
                generator.CreateReport(fileName, new[] {
                    new ExcelDataSheet() { SheetName = "AppModeler", TableName = "AppModelerTable", Data = appModelerResult },
                    new ExcelDataSheet() { SheetName = "NavigateStages", TableName = "NavigateStagesTable", Data = stagesResult }
                });
                return fileName;
            });
            return null;
        }

        private dynamic GetNonInvasiveReportItemFromAppStage(ObjectStage obj)
        {
            if (obj.ApplicationDefinition == null)
            {
                return new
                {
                    ObjectName = obj.Name,
                    AppDefinitionType = string.Empty,
                    AppTypeInfoId = string.Empty,
                    IsNonInvasive = string.Empty,
                    Description = obj.Description,
                    Id = obj.Id
                };
            };
            if (obj.ApplicationDefinition.ApplicationTypeInfo == null)
            {
                return new
                {
                    ObjectName = obj.Name,
                    AppDefinitionType = string.Empty,
                    AppTypeInfoId = string.Empty,
                    IsNonInvasive = string.Empty,
                    Description = obj.Description,
                    Id = obj.Id
                };
            }
            if (!obj.ApplicationDefinition.ApplicationTypeInfo.Parameters.Any())
            {
                return new
                {
                    ObjectName = obj.Name,
                    AppDefinitionType = obj.ApplicationDefinition.Type,
                    AppTypeInfoId = obj.ApplicationDefinition.ApplicationTypeInfo.Id,
                    IsNonInvasive = string.Empty,
                    Description = obj.Description,
                    Id = obj.Id
                };
            }
            if (!obj.ApplicationDefinition.ApplicationTypeInfo.Parameters.Any(i => !string.IsNullOrWhiteSpace(i.Parameter) &&
                                    i.Parameter == "NonInvasive" &&
                                    !string.IsNullOrWhiteSpace(i.Value)))
            {
                return new
                {
                    ObjectName = obj.Name,
                    AppDefinitionType = obj.ApplicationDefinition.Type,
                    AppTypeInfoId = obj.ApplicationDefinition.ApplicationTypeInfo.Id,
                    IsNonInvasive = string.Empty,
                    Description = obj.Description,
                    Id = obj.Id
                };
            }
            else
            {
                return new
                {
                    ObjectName = obj.Name,
                    AppDefinitionType = obj.ApplicationDefinition.Type,
                    AppTypeInfoId = obj.ApplicationDefinition.ApplicationTypeInfo.Id,
                    IsNonInvasive = obj.ApplicationDefinition
                    .ApplicationTypeInfo
                    .Parameters.First(i => i.Parameter.ToLowerInvariant().Equals("noninvasive")).Value.ToLowerInvariant().Equals("true"),
                    Description = obj.Description,
                    Id = obj.Id
                };
            }
        }

        private dynamic GetNonIvasiveReportItem(ObjectStage obj)
        {
            var item = GetNonInvasiveReportItemFromAppStage(obj);
            return item;
        }

        private IEnumerable<dynamic> GetNonInvasiveReportItemFromStages(ObjectStage obj)
        {
            var result = new List<dynamic>();
            var stages = obj.GetAllStages()
                .Where(i => typeof(NavigateStage)
                .IsAssignableFrom(i.GetType())).Cast<NavigateStage>().ToList();
            foreach (var stage in stages)
            {
                var invasive = stage.Actions.SelectMany(i => i.Arguments)
                    .Where(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.ToLowerInvariant().Equals("noninvasive")).ToList();
                foreach (var inv in invasive)
                {
                    result.Add(new
                    {
                        Id = obj.Id,
                        ObjectName = obj.Name,
                        PageId = stage.PageId,
                        PageName = string.IsNullOrWhiteSpace(stage.PageName) ? "Main" : stage.PageName,
                        StageId = stage.Id,
                        StageName = stage.Name,
                        StageType = stage.Type,
                        AttributeName = inv.Name,
                        AttributeValue = inv.Value
                    });
                }
            }
            return result;
        }
        #endregion

        public object ExecuteHookingBug(Release release, string fileName)
        {
            if (release == null) return null;
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            var win32Objects = release.Objects.Where(i => i.ApplicationDefinition != null &&
                                                i.ApplicationDefinition.ApplicationTypeInfo != null &&
                                                !string.IsNullOrWhiteSpace(i.ApplicationDefinition.ApplicationTypeInfo.Id) &&
                                                i.ApplicationDefinition.ApplicationTypeInfo.Id.ToLowerInvariant().StartsWith("win32")).ToList();
            var result = new List<dynamic>();
            foreach(var obj in win32Objects)
            {
                if (!obj.ApplicationDefinition.ApplicationTypeInfo.Parameters.Any(i => i.Parameter == "Path")) continue;
                result.Add(new {
                    ObjectName = obj.Name,
                    ApplicationTypeInfoId = obj.ApplicationDefinition.ApplicationTypeInfo.Id,
                    Path = obj.ApplicationDefinition.ApplicationTypeInfo.Parameters.FirstOrDefault(i => i.Parameter.Equals("Path")).Value
                });
            }
            var generator = new ExcelOutputGenerator();
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            RunReport(() =>
            {
                generator.CreateReport(fileName, new[] {
                    new ExcelDataSheet() { SheetName = "HookReport", TableName = "HookReportTable", Data = result }
                });
                return fileName;
            });
            return null;
        }

        public static void RunReport(Func<string> runReport)
        {
            var fileName = runReport();
            if (string.IsNullOrWhiteSpace(fileName)) return;
            var res = MessageBox.Show("Report Succesfully Saved. Do you want to open the file?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.No) return;
            Process.Start(fileName);
        }
    }
}
