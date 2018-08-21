using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ReleaseExtractor
    {
        private XElement _xmlDOM;

        public ReleaseExtractor(string xml)
        {
            _xmlDOM = XElement.Parse(xml);
        }

        public List<ObjectStage> Objects { get; private set; }

        public void Load()
        {
            Objects = new List<ObjectStage>(GetObjects());
        }

        private IEnumerable<ObjectStage> GetObjects()
        {
            var root = _xmlDOM.Elements().ToList();
            var objects = _xmlDOM.Elements()
                .Where(i => i.Name.LocalName == "contents").Elements()
                .Where(i => i.Name.LocalName == "object").ToList();
            foreach(var obj in objects)
            {
                var actions = GetActions(obj);
            }
            return new List<ObjectStage>();
        }

        private IEnumerable<ActionStage> GetActions(XElement obj)
        {
            var res = new List<ActionStage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            var subsheets = elements.Where(i => i.Name.LocalName == "subsheet").ToList();
            foreach(var sheet in subsheets)
            {
                res.Add(CreateActionStage( sheet,GetStages(obj, sheet.Attribute("subsheetid").Value)));
            }
            return new List<ActionStage>();
        }

        private IEnumerable<Stage> GetStages(XElement obj, string id)
        {
            var res = new List<Stage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            foreach(var el in elements)
            {
                if(el.Elements().Any(i => i.Name.LocalName == "subsheetid") && el.Attribute("type").Value != "SubSheetInfo")
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
                Type = obj.Attribute("type").Value
            };
            return stage;
        }

        private ActionStage CreateActionStage(XElement obj, IEnumerable<Stage> stages)
        {
            var actionStage = new ActionStage() {
                Id = obj.Attribute("subsheetid").Value,
                Type = obj.Attribute("type").Value,
                Stages = stages.ToList()
            };
            actionStage.Name = obj.Elements().Single(i => i.Name.LocalName == "name").Value;
            return actionStage;
        }

    }
}
