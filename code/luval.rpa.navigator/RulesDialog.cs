using luval.rpa.common.rules;
using luval.rpa.common.rules.configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace luval.rpa.navigator
{
    public partial class RulesDialog : BaseForm
    {

        private RuleProfile _profile;

        public RulesDialog()
        {
            InitializeComponent();
        }

        private void Rules_Load(object sender, EventArgs e)
        {
            fileName.Width = -2;
            _profile = RuleProfile.LoadFromFile();
            LoadRules();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(AddRule, null, null);
        }

        private object AddRule()
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Add files required for the rules",
                Filter = "DLL (*.dll)|*.dll",
                CheckFileExists = true,
                Multiselect = true
            };
            var ruleDir = RuleProfile.GetRuleDir();
            if (!ruleDir.Exists) ruleDir.Create();
            if (dialog.ShowDialog() == DialogResult.Cancel) return null;
            var files = dialog.FileNames.Select(i => new FileInfo(i)).ToList();
            foreach(var file in files)
            {
                var newFile = new FileInfo(Path.Combine(ruleDir.FullName, file.Name));
                var over = false;
                if (newFile.Exists)
                    over = MessageBox.Show(string.Format("File {0} has been already imported, do you want to override?", file.Name), "File Exists", MessageBoxButtons.YesNo) == DialogResult.Yes;
                File.Copy(file.FullName, newFile.FullName, over);
                
            }
            var rules = new List<string>();
            foreach (var file in files)
            {
                if (HasRule(file.Name))
                    rules.Add(file.Name);
            }
            if(!rules.Any())
            {
                MessageBox.Show("The files provided doesn't contain rules");
                files.ForEach(i => File.Delete(Path.Combine(ruleDir.FullName, i.Name)));
                return null;
            }
            RegisterRule(rules);
            return null;
        }

        private void RegisterRule(IEnumerable<string> rules)
        {
            foreach(var rule in rules)
            {
                _profile.Rules.Add(new RuleInfo() { AssemblyFile = rule });
            }
            _profile.Rules = _profile.Rules.Distinct().ToList();
            _profile.Save();
            LoadRules();
        }

        private void LoadRules()
        {
            listView.Items.Clear();
            foreach(var rule in _profile.Rules)
            {
                listView.Items.Add(new ListViewItem(rule.AssemblyFile) { Tag = rule });
            }
        }

        private bool HasRule(string name)
        {
            var ass = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, name));
            return ass.GetTypes().Any(i => typeof(IRule).IsAssignableFrom(i));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(DoDelete, null, null);
        }

        private object DoDelete()
        {
            if(listView.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Please select a rule to remove");
                return null;
            }
            if (MessageBox.Show("Are you sure you want to remove the rule file?", "Remove", MessageBoxButtons.YesNo) == DialogResult.No) return null;
            _profile.Rules.Remove((RuleInfo)listView.SelectedItems.Cast<ListViewItem>().First().Tag);
            _profile.Save();
            LoadRules();
            return null;
        }
    }
}
