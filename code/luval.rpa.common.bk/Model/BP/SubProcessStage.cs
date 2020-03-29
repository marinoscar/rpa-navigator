using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class SubProcessStage : ParameterBasedStage
    {
        public SubProcessStage(XElement el): base(el)
        {
            Inputs = InitParams("inputs");
            Outputs = InitParams("outputs");
        }

        public string ProcessId
        {
            get { return GetElementValue("processid"); }
            set { TrySetElValue("processid", value); }
        }
        public List<Parameter> Inputs { get; private set; }
        public List<Parameter> Outputs { get; private set; }
    }
}
