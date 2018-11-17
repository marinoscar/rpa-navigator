using luval.rpa.rules.core;
using luval.rpa.rules.core.Configuration;
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
    public partial class Rules : BaseForm
    {

        private RuleProfile _profile;

        public Rules()
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
            if (dialog.ShowDialog() == DialogResult.Cancel) return null;
            var files = dialog.FileNames.Select(i => new FileInfo(i)).ToList();
            var rules = new List<string>();
            foreach (var file in files)
            {
                File.Copy(file.FullName, Path.Combine(Environment.CurrentDirectory, file.Name));
                if (HasRule(file.Name))
                    rules.Add(file.Name);
            }
            if(!rules.Any())
            {
                MessageBox.Show("The files provided do not contain rules");
                files.ForEach(i => File.Delete(Path.Combine(Environment.CurrentDirectory, i.Name)));
                return null;
            }
            RegisterRule(rules);
            return null;
        }

        private void RegisterRule(IEnumerable<string> rules)
        {
            foreach(var rule in rules)
            {
                _profile.Rules.Add(new rules.core.Configuration.Rule() { AssemblyFile = string.Format(".\\{0}", rule) });
            }
            _profile.Rules = _profile.Rules.Distinct().ToList();
            _profile.Save();
            LoadRules();
        }

        private void LoadRules()
        {
            listView.Clear();
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
            _profile.Rules.Remove((rules.core.Configuration.Rule)listView.SelectedItems.Cast<ListViewItem>().First().Tag);
            _profile.Save();
            LoadRules();
            return null;
        }
    }
}
