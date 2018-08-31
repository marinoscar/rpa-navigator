﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class ApplicationDefinition : Item
    {

        public ApplicationDefinition()
        {
            Elements = new List<ApplicationElement>();
        }

        public string Type { get; set; }
        public XElement Xml { get; set; }

        public List<ApplicationElement> Elements { get; set; }
    }
}
