using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class PageBasedStage : Stage
    {

        public PageBasedStage(XElement xml) : base(xml)
        {
            Pages = new List<PageStage>();
            IdName = "id";
            ProcessXML = GetElement("process");
        }

        protected virtual XElement ProcessXML { get; private set; }

        public List<Stage> MainPage { get; set; }
        
        public List<PageStage> Pages { get; set; }

        public override string Description
        {
            get { return GetAttributeValue(ProcessXML, "narrative"); }
            set { TrySetAttValue(ProcessXML, "narrative", value); }
        }

        public string BpVersion
        {
            get { return GetAttributeValue(ProcessXML, "bpversion"); }
            set { TrySetAttValue(ProcessXML, "bpversion", value); }
        }

        public IEnumerable<Stage> GetAllStages()
        {
            return MainPage.Union(Pages.SelectMany(i => i.Stages));
        }
    }
}
