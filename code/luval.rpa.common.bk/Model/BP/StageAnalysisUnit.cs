using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.model.bp
{
    /// <summary>
    /// Provides an analysis unit of a stage
    /// </summary>
    public class StageAnalysisUnit
    {
        /// <summary>
        /// Gets or sets the id of the page
        /// </summary>
        public string PageId { get; set; }
        /// <summary>
        /// Gets or sets the type of parent, Process or Object
        /// </summary>
        public string ParentType { get; set; }
        /// <summary>
        /// Gets or sets the name of the parent
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// Gets or sets the name of the page
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// Gets or sets the stage
        /// </summary>
        public Stage Stage { get; set; }
    }
}
