using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class StartStage : ParameterBasedStage
    {
        public StartStage(XElement xml) : base(xml)
        {
            Inputs = InitParams("inputs");
        }

        public List<Parameter> Inputs { get; private set; }
    }
}
