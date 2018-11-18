using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ActionStage : ParameterBasedStage
    {
        public ActionStage(XElement xml) : base(xml)
        {
            Resource = new Resource(GetElement("resource"));
            Inputs = InitParams("inputs");
            Outputs = InitParams("outputs");

        }

        public Resource Resource { get; private set; }
        public List<Parameter> Inputs { get; private set; }
        public List<Parameter> Outputs { get; private set; }
    }
}
