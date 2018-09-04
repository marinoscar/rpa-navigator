using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core.Configuration
{
    public class RuleConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleConfiguration)element).Assembly;
        }

        public RuleConfiguration this[int idx]
        {
            get
            {
                return (RuleConfiguration)BaseGet(idx);
            }
        }
    }
}
