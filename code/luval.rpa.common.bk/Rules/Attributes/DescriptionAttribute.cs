using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.rules.attributes
{
    /// <summary>
    /// Description for the rule
    /// </summary>
    public class DescriptionAttribute : StringValueAttributte
    {
        /// <summary>
        /// Description for the rule
        /// </summary>
        /// <param name="description">A description for the rule</param>
        public DescriptionAttribute(string description) : base(description)
        {
        }
    }
}
