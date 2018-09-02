﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class Resource : Item
    {
        public Resource(XElement element) : base(element)
        {
        }

        public string Object
        {
            get { return GetAttributeValue("object"); }
            set { TrySetAttValue("object", value); }
        }

        public string Action
        {
            get { return GetAttributeValue("action"); }
            set { TrySetAttValue("action", value); }
        }
    }
}
