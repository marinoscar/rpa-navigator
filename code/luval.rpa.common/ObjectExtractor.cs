using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ObjectExtractor : ExtractorBase
    {
        private XElement _xmlDOM;

        public ObjectExtractor(string xml): this(XElement.Parse(xml))
        {
        }

        public ObjectExtractor(XElement xml)
        {
            _xmlDOM = xml;
            Objects = new List<ObjectStage>();
        }

        public List<ObjectStage> Objects { get; set; }

        public void Load()
        {
            Objects = GetObjects().ToList();
        }

        private IEnumerable<ObjectStage> GetObjects()
        {
            var root = _xmlDOM.Elements().ToList();
            var res = new List<ObjectStage>();
            var objects = _xmlDOM.Elements()
                .Where(i => i.Name.LocalName == "contents").Elements()
                .Where(i => i.Name.LocalName == "object").ToList();
            foreach (var obj in objects)
            {
                res.Add(new ObjectStage()
                {
                    Id = obj.Attribute("id").Value,
                    Name = obj.Attribute("name").Value,
                    Type = "Object",
                    Actions = new List<ActionStage>(GetActions(obj)),
                    ApplicationDefinition = GetDefinition(obj)
                });
            }
            return res;
        }

        private IEnumerable<ActionStage> GetActions(XElement obj)
        {
            var res = new List<ActionStage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            var subsheets = elements.Where(i => i.Name.LocalName == "subsheet").ToList();
            foreach (var sheet in subsheets)
            {
                var stageExtractor = new StageExtractor(obj, sheet.Attribute("subsheetid").Value);
                stageExtractor.Load();
                res.Add(CreateActionStage(sheet, stageExtractor.Stages));
            }
            return res;
        }

        private ActionStage CreateActionStage(XElement obj, IEnumerable<Stage> stages)
        {
            var actionStage = new ActionStage()
            {
                Id = obj.Attribute("subsheetid").Value,
                Type = obj.Attribute("type").Value,
                Stages = stages.ToList()
            };
            actionStage.Name = obj.Elements().Single(i => i.Name.LocalName == "name").Value;
            return actionStage;
        }

        private ApplicationDefinition GetDefinition(XElement obj)
        {
            var res = new ApplicationDefinition();
            var process = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().FirstOrDefault();
            if (process == null) return res;
            var appDef = process.Elements().FirstOrDefault(i => i.Name.LocalName == "appdef");
            if (appDef == null) return res;
            res.Elements = GetElements(appDef);
            return res;
        }

        private List<ApplicationElement> GetElements(XElement xml)
        {
            var res = new List<ApplicationElement>();
            var xmlEls = xml.Elements().Where(i => i.Name.LocalName == "element").ToList();
            foreach(var el in xmlEls)
            {
                res.Add(GetElement(el));
                var elementsFromGroup = GetElementsFromGroup(el);
                if (elementsFromGroup.Any()) res.AddRange(elementsFromGroup);
                var elements = GetElements(el);
                if (elements.Any()) res.AddRange(elements);
            }
            return res;
        }

        private List<ApplicationElement> GetElementsFromGroup(XElement xml)
        {
            var res = new List<ApplicationElement>();
            var groups = xml.Elements().Where(i => i.Name.LocalName == "group").ToList();
            foreach(var group in groups)
            {
                var elements = GetElements(group);
                res.AddRange(elements);
                var elementsFromGroup = GetElementsFromGroup(group);
                if (elementsFromGroup.Any()) res.AddRange(elementsFromGroup);
            }
            return res;
        }

        private ApplicationElement GetElement(XElement el)
        {
            var res = new ApplicationElement() {
                Name = GetAttributeText(el, "name"),
                Id = GetElementValue(el, "id"),
                DataType = GetElementValue(el, "datatype"),
                Type = GetElementValue(el, "type"),
                Attributes = GetAttributes(el)
            };
            return res;
        }

        private List<ElementAttribute> GetAttributes(XElement el)
        {
            var res = new List<ElementAttribute>();
            var atts = el.Elements().Where(i => i.Name.LocalName == "attributes").ToList();
            foreach(var att in atts)
            {
                var val = att.Elements().FirstOrDefault(i => i.Name.LocalName == "ProcessValue");
                var item = new ElementAttribute()
                {
                    Name = GetAttributeText(att, "name"),
                    DataType = GetAttributeText(val, "datatype"),
                    Value = GetAttributeText(val, "value"),
                    InUse = GetInUse(GetAttributeText(att, "inuse"))
                };
            }
            return res;
        }

        private bool GetInUse(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? false : true;
        }
    }
}
