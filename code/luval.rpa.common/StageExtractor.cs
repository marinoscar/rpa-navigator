using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class StageExtractor
    {
        private XElement _xmlDOM;
        private string _id;

        public StageExtractor(XElement xml, string parentId)
        {
            _xmlDOM = xml;
            _id = parentId;
            Stages = new List<Stage>();
        }

        public List<Stage> Stages { get; private set; }

        public void Load()
        {
            Stages = GetStages(_xmlDOM, _id).ToList();
        }

        private IEnumerable<Stage> GetStages(XElement obj, string id)
        {
            var res = new List<Stage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            foreach (var el in elements)
            {
                if (el.Elements().Any(i => i.Name.LocalName == "subsheetid") && el.Attribute("type").Value != "SubSheetInfo")
                {
                    var sId = el.Elements().Single(i => i.Name.LocalName == "subsheetid").Value;
                    if (sId == id)
                        res.Add(CreateStage(el));
                }
            }
            return res;
        }

        private Stage CreateStage(XElement obj)
        {
            var stage = new Stage()
            {
                Id = obj.Attribute("stageid").Value,
                Name = obj.Attribute("name").Value,
                Type = obj.Attribute("type").Value,
                Xml = obj
            };
            return stage;
        }
    }
}
