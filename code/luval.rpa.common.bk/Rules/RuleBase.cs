using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules.attributes;

namespace luval.rpa.common.rules
{
    public abstract class RuleBase : IRule
    {
        
        public abstract RuleTarget RuleTarget { get; }

        public abstract IEnumerable<Result> Execute(XmlItem package);

        /// <summary>
        /// Gets the name of the rule
        /// </summary>
        public string Name { get { return GetRuleName(); } }
        /// <summary>
        /// Gets the rule description
        /// </summary>
        public string Description { get { return GetRuleDescription(); } }

        protected virtual string GetRuleName()
        {
            var name = GetType().GetCustomAttributes(typeof(NameAttribute), true).FirstOrDefault();
            if (name == null) return string.Empty;
            return ((NameAttribute)name).Value;
        }

        protected virtual string GetRuleDescription()
        {
            var name = GetType().GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (name == null) return string.Empty;
            return ((DescriptionAttribute)name).Value;
        }
    }
}
