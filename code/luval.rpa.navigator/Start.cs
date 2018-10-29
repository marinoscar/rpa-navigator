using luval.rpa.common;
using luval.rpa.rules.core;
using luval.rpa.rules.core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace luval.rpa.navigator
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void btnRules_Click(object sender, EventArgs e)
        {
            try
            {
                var file = GetReleaseFile();
                if (string.IsNullOrWhiteSpace(file))
                    return;
                var report = RunRules(file);
                SaveReport(report);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private CsvReportGenerator RunRules(string file)
        {
            var prof = @"profile.xml";
            var ser = new XmlSerializer(typeof(RuleProfile));
            var newProfile = (RuleProfile)ser.Deserialize(File.OpenText(prof));
            var xml = File.ReadAllText(file);
            var release = new ReleaseExtractor(xml);
            release.Load();
            var ruleEngine = new Runner();
            var rules = ruleEngine.GetRulesFromProfile(newProfile);
            var results = ruleEngine.RunRules(newProfile, release.Release, rules.ToList());
            return new CsvReportGenerator(newProfile, release.Release, results, rules);
        }

        private string GetReleaseFile()
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Open Release",
                RestoreDirectory = true,
                CheckFileExists = true,
                Multiselect = false,
                Filter = "BP Release (*.bprelease)|*.bprelease|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return null;
            return dialog.FileName;
        }

        private void SaveReport(CsvReportGenerator report)
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Save Results",
                RestoreDirectory = true,
                Filter = "csv (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            report.ToCsv(dialog.FileName);
        }
    }
}
