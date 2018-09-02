using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ObjectStage : Stage
    {

        private XElement _processXml;

        public ObjectStage(XElement el) : base(el)
        {
            Actions = new List<ActionPage>();
            IdName = "id";
            _processXml = GetElement("process");
        }

        public List<Stage> InitializeAction { get; set; }
        public List<ActionPage> Actions { get; set; }
        public ApplicationDefinition ApplicationDefinition { get; set; }

        public string BpVersion
        {
            get { return GetAttributeValue(_processXml, "bpversion"); }
            set { TrySetAttValue(_processXml, "bpversion", value); }
        }

        public string Description
        {
            get { return GetAttributeValue(_processXml, "narrative"); }
            set { TrySetAttValue(_processXml, "narrative", value); }
        }

        public string RunMode
        {
            get { return GetAttributeValue(_processXml, "runmode"); }
            set { TrySetAttValue(_processXml, "runmode", value); }
        }
    }
}
