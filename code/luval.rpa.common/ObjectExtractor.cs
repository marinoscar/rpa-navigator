
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

        public ObjectExtractor(string xml) : this(XElement.Parse(xml))
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
                var extractors = new StageExtractor(obj, default(string));
                extractors.Load();
                res.Add(new ObjectStage(obj)
                {
                    InitializeAction = extractors.Stages,
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
                res.Add(new ActionStage(obj) { Stages = stageExtractor.Stages });
            }
            return res;
        }

        private ApplicationDefinition GetDefinition(XElement obj)
        {
            var res = new ApplicationDefinition(obj);
            var appDef = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().FirstOrDefault();
            if (appDef == null) return res;
            res.Elements = GetElements(appDef);
            return res;
        }

        private List<ApplicationElement> GetElements(XElement xml)
        {
            var res = new List<ApplicationElement>();
            var xmlEls = xml.Elements().Where(i => i.Name.LocalName == "element").ToList();
            foreach (var el in xmlEls)
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
            foreach (var group in groups)
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
            var res = new ApplicationElement(el)
            {
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
            var node = el.Elements().Where(i => i.Name.LocalName == "attributes").FirstOrDefault();
            if (node == null) return res;
            var attributes = node.Elements().Where(i => i.Name.LocalName == "attribute").ToList();
            foreach (var att in attributes)
            {
                var val = att.Elements().FirstOrDefault(i => i.Name.LocalName == "ProcessValue");
                res.Add(new ElementAttribute()
                {
                    Name = GetAttributeText(att, "name"),
                    DataType = GetAttributeText(val, "datatype"),
                    Value = GetAttributeText(val, "value"),
                    InUse = GetInUse(GetAttributeText(att, "inuse"))
                });
            }
            return res;
        }

        private bool GetInUse(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? false : true;
        }
    }
}
