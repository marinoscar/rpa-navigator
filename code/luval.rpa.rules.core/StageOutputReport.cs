using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class StageOutputReport
    {
        public string Generate(string fileName, Release release)
        {
            var items = release.GetAnalysisUnits().Select(i => new Result()
            {
                Type = i.ParentType,
                Parent = i.ParentName,
                Page = i.Page,
                StageId = i.Stage.Id,
                Stage = i.Stage.Name,
                StageType = i.Stage.Type,
                //Xml = Convert.ToString(i.Stage.Xml).Replace(Environment.NewLine, "")
            });
            var csv = new CsvOutputGenerator();
            var content = csv.Create(items);
            File.WriteAllText(fileName, content);
            return fileName;
        }

        public class Result
        {
            public string Type { get; set; }
            public string Parent { get; set; }
            public string Page { get; set; }
            public string StageId { get; set; }
            public string Stage { get; set; }
            public string StageType { get; set; }
            public string Xml { get; set; }
        }
    }
}
