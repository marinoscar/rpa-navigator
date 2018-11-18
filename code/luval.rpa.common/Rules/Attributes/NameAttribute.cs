using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.rules.attributes
{
    /// <summary>
    /// Provides the name for the rule
    /// </summary>
    public class NameAttribute : StringValueAttributte
    {

        /// <summary>
        /// Name of the rule 
        /// </summary>
        /// <param name="name">Name to provide the rule</param>
        public NameAttribute(string name) : base(name)
        {
        }
    }
}
