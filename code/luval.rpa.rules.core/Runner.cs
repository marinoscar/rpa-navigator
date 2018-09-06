using luval.rpa.common.Model;
using luval.rpa.rules.core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.core
{
    public class Runner
    {

        public IEnumerable<Result> RunProfile(RuleProfile profile, Release release)
        {
            return RunRules(release, GetRulesFromProfile(profile));
        }

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

        private IEnumerable<IRule> GetRulesFromProfile(RuleProfile profile)
        {
            var rules = new List<IRule>();
            foreach(var ruleConfig in profile.Rules)
            {
                var ass = Assembly.LoadFile(ruleConfig.AssemblyFile);
                rules.AddRange(GetAllRules(ass));
            }
            return rules;
        }

        private IEnumerable<IRule> GetAllRules()
        {
            return GetAllRules(Assembly.GetExecutingAssembly());
        }

        private IEnumerable<IRule> GetAllRules(Assembly ass)
        {
            var instances = new List<IRule>();
            var types = ass.GetTypes().Where(
                i => typeof(IRule).IsAssignableFrom(i) &&
                !i.IsInterface && !i.IsAbstract).ToList();
            foreach (var t in types)
            {
                instances.Add((IRule)Activator.CreateInstance(t));
            }
            return instances;
        }

    }
}
