using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using luval.rpa.common.rules;
using luval.rpa.common.rules.configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.navigator
{
    public class BPRunner
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
                    res.AddRange(rule.Execute((XmlItem)release));
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
            var ruleExtractor = new RuleExtractor();
            return ruleExtractor.GetRulesFromProfile(profile);
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
