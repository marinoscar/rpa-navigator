using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules
{
    /// <summary>
    /// Interface of how a rule needs to be implemented
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Execute the rule
        /// </summary>
        /// <param name="release">The elements to be evaluated</param>
        /// <returns>The results of the execution of the rule</returns>
        IEnumerable<Result> Execute(Release release);
    }
}
