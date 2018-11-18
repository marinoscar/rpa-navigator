using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ObjectStage : PageBasedStage
    {
        public ObjectStage(XElement el) : base(el)
        {

        }

        public ApplicationDefinition ApplicationDefinition { get; set; }

        public string RunMode
        {
            get { return GetAttributeValue(ProcessXML, "runmode"); }
            set { TrySetAttValue(ProcessXML, "runmode", value); }
        }
    }
}
