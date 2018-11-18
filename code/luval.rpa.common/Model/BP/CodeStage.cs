using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class CodeStage : ParameterBasedStage
    {
        public CodeStage(XElement xml) : base(xml)
        {
            Inputs = InitParams("inputs");
            Outputs = InitParams("outputs");
        }

        public List<Parameter> Inputs { get; private set; }
        public List<Parameter> Outputs { get; private set; }
    }
}
