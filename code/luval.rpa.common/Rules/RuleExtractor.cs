using luval.rpa.common.rules.configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.rules
{
    public class RuleExtractor
    {
        public IEnumerable<IRule> GetRulesFromProfile(RuleProfile profile)
        {
            var rules = new List<IRule>();
            foreach (var ruleConfig in profile.Rules)
            {
                var file = GetAbsolutePath(ruleConfig.AssemblyFile);
                var ass = Assembly.LoadFile(file);
                rules.AddRange(GetAllRules(ass));
            }
            return rules;
        }

        private string GetAbsolutePath(string fileName)
        {
            var fileInfo = new FileInfo( Path.Combine(RuleProfile.GetRuleDir().FullName, fileName));
            if (!fileInfo.Exists)
            {
                fileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, fileName));
                if (!fileInfo.Exists)
                    throw new InvalidOperationException(string.Format("File {0} in profile not found", fileInfo.Name));
            }
            return fileInfo.FullName;
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
