﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class NavigateStage : Stage
    {
        public NavigateStage(XElement xml) : base(xml)
        {
            Actions = new List<NavigateAction>();
            var steps = Xml.Elements().Where(i => i.Name.LocalName == "step").ToList();
            foreach(var step in steps)
            {
                Actions.Add(new NavigateAction(step));
            }
        }

        public List<NavigateAction> Actions { get; set; }
    }
}
