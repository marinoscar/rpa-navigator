using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class EndStage : ParameterBasedStage
    {
        public EndStage(XElement xml) : base(xml)
        {
            Outputs = InitParams("outputs");
        }

        public List<Parameter> Outputs { get; private set; }
    }
}
