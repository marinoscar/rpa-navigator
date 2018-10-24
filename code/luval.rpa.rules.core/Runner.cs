using luval.rpa.common.Model;
using luval.rpa.rules.core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
            return RunRules(profile, release, GetRulesFromProfile(profile));
        }

        
        private void CleanReleaseFromProfileExclusions(RuleProfile profile, Release release)
        {
            foreach(var exclusion in profile.Exclusions)
            {
                var item = release.Objects.FirstOrDefault(i => i.Name == exclusion.Name);
                if (item == null) continue;
                release.Objects.Remove(item);
                item = null;
            }
        }

        private IEnumerable<Result> RunRules(RuleProfile profile, Release release, IEnumerable<IRule> rules)
        {
            CleanReleaseFromProfileExclusions(profile, release);
            var res = new List<Result>();
            foreach(var rule in rules)
            {
                res.AddRange(rule.Execute(release));
            }
            return res;
        }

        public IEnumerable<IRule> GetRulesFromProfile(RuleProfile profile)
        {
            var rules = new List<IRule>();
            foreach(var ruleConfig in profile.Rules)
            {
                var file = GetAbsolutePath(ruleConfig.AssemblyFile);
                var ass = Assembly.LoadFile(file);
                rules.AddRange(GetAllRules(ass));
            }
            return rules;
        }

        private string GetAbsolutePath(string fileName)
        {
            var absolutePath = Path.Combine(Environment.CurrentDirectory, fileName);
            return Path.GetFullPath((new Uri(absolutePath)).LocalPath);
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
