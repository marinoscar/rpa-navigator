using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ParameterBasedStage : Stage
    {
        public ParameterBasedStage(XElement xml) : base(xml)
        {
        }

        protected virtual List<Parameter> InitParams(string elName)
        {
            var res = new List<Parameter>();
            var element = Xml.Elements().Where(i => i.Name.LocalName == elName).FirstOrDefault();
            if (element == null) return res;
            element.Elements().ToList()
                .ForEach(e => res.Add(new Parameter(e)));
            return res;
        }
    }
}
