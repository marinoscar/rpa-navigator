using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.terminal
{
    /// <summary>
    /// Provides an abstraction to handle common console switches
    /// </summary>
    public class ConsoleSwitches
    {
        private List<string> _args;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="args">Collection of arguments</param>
        public ConsoleSwitches(IEnumerable<string> args)
        {
            _args = new List<string>(args);
        }

        /// <summary>
        /// Gets the value for the switch in the argument collection
        /// </summary>
        /// <param name="name">The name of the switch</param>
        /// <returns>The switch value if present, otherwise null</returns>
        public string this[string name]
        {
            get
            {
                var idx = _args.IndexOf(name);
                if (idx == (_args.Count - 1)) return null;
                return _args[idx + 1];
            }
        }

        /// <summary>
        /// Indicates if the switch exists in the argument collection
        /// </summary>
        /// <param name="name">The name of the switch</param>
        /// <returns>True if the switch name is on the colleciton, otherwise false</returns>
        public bool ContainsSwitch(string name)
        {
            return _args.Contains(name);
        }

        public FileInfo SourceFile { get { return new FileInfo(this["-s"]); } }
        public bool HasSourceFile { get { return ContainsSwitch("-s"); } }
        public FileInfo ReportFile
        {
            get
            {
                return ContainsSwitch("-o") ? new FileInfo(this["-o"]) : new FileInfo(Path.Combine(SourceFile.DirectoryName, "report.xlsx"));
            }
        }
    }
}
