using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class NavigateStage : Stage
    {
        public NavigateStage(XElement xml) : base(xml)
        {
            Actions = new List<NavigateStep>();
            var steps = Xml.Elements().Where(i => i.Name.LocalName == "step").ToList();
            foreach(var step in steps)
            {
                Actions.Add(new NavigateStep(step));
            }
        }

        public List<NavigateStep> Actions { get; set; }
    }
}
