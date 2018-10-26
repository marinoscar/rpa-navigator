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

        public event EventHandler<RunnerMessageEventArgs> RuleRun;

        public IEnumerable<Result> RunProfile(RuleProfile profile, Release release)
        {
            return RunRules(profile, release, GetRulesFromProfile(profile).ToList());
        }


        protected virtual void OnRuleRun(RunnerMessageEventArgs e)
        {
            RuleRun?.Invoke(this, e);
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

        public IEnumerable<Result> RunRules(RuleProfile profile, Release release, IList<IRule> rules)
        {
            CleanReleaseFromProfileExclusions(profile, release);
            var res = new List<Result>();
            foreach(var rule in rules)
            {
                var msg = string.Format("Running {0} {1} out of {2}", rule.Name, rules.IndexOf(rule) + 1, rules.Count);
                try
                {
                    OnRuleRun(new RunnerMessageEventArgs(msg));
                    res.AddRange(rule.Execute(release));
                }
                catch (Exception ex)
                {
                    OnRuleRun(new RunnerMessageEventArgs(string.Format("Failed to run rule", rule.Name)));
                    throw new Exception(string.Format("Unable to run rule: {0}\n{1}", rule.Name, ex.Message), ex);
                }
            }
            OnRuleRun(new RunnerMessageEventArgs("Completed"));
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

    public class RunnerMessageEventArgs: EventArgs
    {
        public RunnerMessageEventArgs(string message)
        {
            Message = message;
        }
        public string Message { get; private set; }
    }
}
