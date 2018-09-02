using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ApplicationDefinition : Stage
    {

        public ApplicationDefinition(XElement el) : base(el)
        {
            Elements = new List<ApplicationElement>();
        }

        public List<ApplicationElement> Elements { get; set; }
    }
}
