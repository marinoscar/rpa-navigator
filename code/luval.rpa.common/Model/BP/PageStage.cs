using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class PageStage : Stage
    {
        public PageStage(XElement el) : base(el)
        {
            Stages = new List<Stage>();
            IdName = "subsheetid";
        }

        public override string Name
        {
            get { return GetElementValue("name"); }
            set { TrySetElValue("name", value); }
        }

        public List<Stage> Stages { get; set; }
    }
}
