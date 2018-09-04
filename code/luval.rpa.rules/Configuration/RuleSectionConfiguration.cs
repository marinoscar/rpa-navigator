using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core.Configuration
{
    public class RuleAssembly : ConfigurationSection
    {
        [ConfigurationProperty("assembly", DefaultValue ="", IsRequired =true)]
        public string Assembly
        {
            get { return Convert.ToString(this["assembly"]); }
            set { this["assembly"] = value; }
        }

        [ConfigurationProperty("loadAllRulesInAssembly", DefaultValue = false, IsRequired = true)]
        public bool LoadAllRulesInAssembly
        {
            get { return Convert.ToBoolean(this["loadAllRulesInAssembly"]); }
            set { this["loadAllRulesInAssembly"] = value; }
        }
    }
}
