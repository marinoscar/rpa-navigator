using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ParameterBasedStage : Stage
    {
        public ParameterBasedStage(XElement xml) : base(xml)
        {
        }

        protected virtual List<Parameter> InitParams(string elName)
        {
            var res = new List<Parameter>();
            Xml.Elements().Where(i => i.Name.LocalName == elName).ToList()
                .ForEach(e => res.Add(new Parameter(e)));
            return res;
        }
    }
}
