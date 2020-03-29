
using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.extractors.bp
{
    public class ObjectExtractor : PageBasedExtractor
    {

        public ObjectExtractor(string xml) : this(XElement.Parse(xml))
        {
        }

        public ObjectExtractor(XElement xml) : base(xml)
        {
            Objects = new List<ObjectStage>();
        }

        public List<ObjectStage> Objects { get; set; }

        public override void Load()
        {
            Objects = GetObjects().ToList();
        }

        private IEnumerable<ObjectStage> GetObjects()
        {
            return GetPages<ObjectStage>("object", default(string), CreateStage);
        }

        private ObjectStage CreateStage(XElement xml, IEnumerable<Stage> mainStages, IEnumerable<PageStage> pages)
        {
            var obj = new ObjectStage(xml)
            {
                MainPage = mainStages.ToList(),
                Pages = new List<PageStage>(pages),
                ApplicationDefinition = GetDefinition(xml)
            };
            return obj;
        }

        private ApplicationDefinition GetDefinition(XElement obj)
        {
            var res = new ApplicationDefinition(obj);
            var appDef = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().FirstOrDefault();
            if (appDef == null) return res;
            res.Elements = GetElements(appDef);
            foreach(var el in res.Elements)
            {
                el.Attributes = GetAttributes(el.Xml);
            }
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
            var res = new ApplicationElement(el);
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
                res.Add(new ElementAttribute(att));
            }
            return res;
        }

        private bool GetInUse(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? false : true;
        }
        
    }
}
