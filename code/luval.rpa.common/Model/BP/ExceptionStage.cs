using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ExceptionStage : Stage
    {
        public ExceptionStage(XElement xml) : base(xml)
        {
            var el = GetElement("exception");
            Details = new ExceptionStageDetail(el);
        }

        public ExceptionStageDetail Details { get; private set; }
    }
}
