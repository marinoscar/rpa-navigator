using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class StageExtractor : ExtractorBase
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

        private Stage CreateStage(XElement xml)
        {
            if (xml.Elements().Any(i => i.Name.LocalName == "inputs" || i.Name.LocalName == "outputs"))
                return CreateStageWithParam(xml);
            return new Stage(xml);
        }

        private StageWithParameters CreateStageWithParam(XElement obj)
        {
            var newStage = new StageWithParameters(obj);
            newStage.Parameters = new List<Parameter>();
            newStage.Parameters.AddRange(GetInputs(obj));
            newStage.Parameters.AddRange(GetOutputs(obj));
            return newStage;
        }

        private IEnumerable<Parameter> GetInputs(XElement obj)
        {
            return GetParameters(obj, "inputs", false);
        }

        private IEnumerable<Parameter> GetOutputs(XElement obj)
        {
            return GetParameters(obj, "outputs", false);
        }

        private IEnumerable<Parameter> GetParameters(XElement obj, string node, bool isOutput)
        {
            var res = new List<Parameter>();
            var inputNode = obj.Elements().Where(i => i.Name.LocalName == node).FirstOrDefault();
            if (inputNode == null) return res;
            var inputs = inputNode.Elements().ToList();
            foreach(var input in inputs)
            {
                res.Add(new Parameter() {
                    Name = GetAttributeText(input, "name"),
                    Description = GetAttributeText(input, "narrative"),
                    IsOutput = isOutput,
                    Type =  GetAttributeText( input,"type"),
                    Stage = GetAttributeText(input, "stage"),
                    Expression = GetAttributeText(input,"expr")
                });
            }
            return res;
        }
    }
}
