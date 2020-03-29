using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.rules
{
    public class Result
    {
        /// <summary>
        /// The name of the rule being applied
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// Description about the rule
        /// </summary>
        public string RuleDescription { get; set; }
        /// <summary>
        /// Type of result
        /// </summary>
        public ResultType Type { get; set; }
        /// <summary>
        /// General message of the rule result
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Detailed description of the rule result
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Scope of where the rule is applied
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// Parent stage of the rule, for example a Release, Process or Object
        /// </summary>
        public string Parent { get; set; }
        /// <summary>
        /// Name of the page where the rule is evaluated
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// Stage where the rule is evaluated
        /// </summary>
        public string Stage { get; set; }
        /// <summary>
        /// The type of stage
        /// </summary>
        public string StageType { get; set; }
        /// <summary>
        /// Id of the stage where the rule is evaluated
        /// </summary>
        public string StageId { get; set; }
    }
}
