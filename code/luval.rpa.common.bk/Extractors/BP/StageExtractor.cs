using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.extractors.bp
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

        public override void Load()
        {
            Stages = GetStages(_xmlDOM, _id).ToList();
        }

        private IEnumerable<Stage> GetStages(XElement obj, string id)
        {
            var res = new List<Stage>();
            var process = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            var elements = process.Where(i => i.Name.LocalName == "stage").ToList();
            foreach (var el in elements)
            {
                var sId = default(string);
                if (el.Elements().Any(i => i.Name.LocalName == "subsheetid"))
                {
                    sId = el.Elements().Single(i => i.Name.LocalName == "subsheetid").Value;
                    if (sId == id)
                        res.Add(CreateStage(el));
                }
                else //applies for the initialize page
                {
                    if (sId == id)
                        res.Add(CreateStage(el));
                }
            }
            return res;
        }

        private Stage CreateStage(XElement xml)
        {
            Stage stage = new Stage(xml);
            switch (stage.Type)
            {
                case "Data":
                    stage = new DataItem(xml);
                    break;
                case "Action":
                    stage = new ActionStage(xml);
                    break;
                case "Code":
                    stage = new CodeStage(xml);
                    break;
                case "Exception":
                    stage = new ExceptionStage(xml);
                    break;
                case "WaitStart":
                    stage = new WaitStartStage(xml);
                    break;
                case "WaitEnd":
                    stage = new WaitEndStage(xml);
                    break;
                case "Start":
                    stage = new StartStage(xml);
                    break;
                case "End":
                    stage = new EndStage(xml);
                    break;
                case "Process":
                    stage = new SubProcessStage(xml);
                    break;
                case "Navigate":
                    stage = new NavigateStage(xml);
                    break;
                case "Read":
                    stage = new ReadStage(xml);
                    break;
                case "Write":
                    stage = new WriteStage(xml);
                    break;
                case "SubSheet":
                    stage = new SubPageStage(xml);
                    break;
            }
            return stage;
        }
    }
}
