﻿using luval.rpa.common.Model;
using luval.rpa.common.Model.BP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    /// <summary>
    /// Interface of how a rule needs to be implemented
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Execute the rule
        /// </summary>
        /// <param name="package">The elements to be evaluated</param>
        /// <returns>The results of the execution of the rule</returns>
        IEnumerable<Result> Execute(XmlItem package);
        /// <summary>
        /// Gets the name of the rule
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the description for the rule
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the rule target platform
        /// </summary>
        RuleTarget RuleTarget { get; }

    }
}
