using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core.Configuration
{
    public class RuleConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("assembly", DefaultValue = "", IsRequired = true)]
        public string Assembly
        {
            get { return Convert.ToString(this["assembly"]); }
            set { this["assembly"] = value; }
        }
    }
}
