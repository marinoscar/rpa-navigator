using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ReadStage : NavigateStage
    {
        public ReadStage(XElement xml) : base(xml)
        {
        }
    }
}
