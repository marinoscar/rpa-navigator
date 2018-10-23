﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace luval.rpa.rules.core.Configuration
{
    [XmlRoot(ElementName = "ruleProfile")]
    public class RuleProfile
    {
        public RuleProfile()
        {
            Rules = new List<Rule>();
        }

        [XmlArray(ElementName = "rules")]
        public List<Rule> Rules { get; set; }
    }
}