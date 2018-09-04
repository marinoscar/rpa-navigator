using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules
{
    public class Runner
    {
        public IEnumerable<Result> RunAll(Release release)
        {
            return RunRules(release, GetAllRules());
        }

        public IEnumerable<Result> RunRules(Release release, IEnumerable<IRule> rules)
        {
            var res = new List<Result>();
            foreach(var rule in rules)
            {
                res.AddRange(rule.Execute(release));
            }
            return res;
        }

        private IEnumerable<IRule> GetAllRules()
        {
            var instances = new List<IRule>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(i => typeof(IRule).IsAssignableFrom(i));
            foreach (var t in types)
            {
                instances.Add((IRule)Activator.CreateInstance(t));
            }
            return instances;
        }

    }
}
