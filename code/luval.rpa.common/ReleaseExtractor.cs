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
            var res = new List<ObjectStage>();
            var objects = _xmlDOM.Elements()
                .Where(i => i.Name.LocalName == "contents").Elements()
                .Where(i => i.Name.LocalName == "object").ToList();
            foreach(var obj in objects)
            {
                res.Add(new ObjectStage()
                {
                    Id = obj.Attribute("id").Value,
                    Name = obj.Attribute("name").Value,
                    Type = "Object",
                    Actions = new List<ActionStage>(GetActions(obj))
                });
            }
            return res;
        }

        private IEnumerable<ActionStage> GetActions(XElement obj)
        {
            var res = new List<ActionStage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            var subsheets = elements.Where(i => i.Name.LocalName == "subsheet").ToList();
            foreach(var sheet in subsheets)
            {
                var stageExtractor = new StageExtractor(obj, sheet.Attribute("subsheetid").Value);
                stageExtractor.Load();
                res.Add(CreateActionStage( sheet, stageExtractor.Stages));
            }
            return res;
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
