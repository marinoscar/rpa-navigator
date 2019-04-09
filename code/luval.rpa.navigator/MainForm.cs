using luval.rpa.common.extractors.bp;
using luval.rpa.common.model.bp;
using luval.rpa.common.rules;
using luval.rpa.common.rules.configuration;
using luval.rpa.rules.bp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace luval.rpa.navigator
{
    public partial class MainForm : BaseForm
    {
        private string _fileName;
        private Release _release;
        public MainForm()
        {
            InitializeComponent();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            this.ExecuteAction(() =>
            {
                var openDlg = new OpenFileDialog()
                {
                    Filter = "(*.bprelease)|*.bprelease",
                    Title = "Open Releae",
                    CheckFileExists = true,
                    CheckPathExists = true,
                    RestoreDirectory = true
                };
                var res = openDlg.ShowDialog();
                if (res == DialogResult.Cancel) return null;
                var file = openDlg.FileName;
                var extractor = new ReleaseExtractor(File.ReadAllText(file));
                extractor.Load();
                LoadTree(extractor.Release);
                _fileName = file;
                _release = extractor.Release;
                return null;
            }, null, (ex) =>
            {
                _fileName = null;
                treeView.Nodes.Clear();
            });
        }

        private void LoadTree(Release release)
        {
            treeView.Nodes.Clear();
            var root = new TreeNode("Release");
            var objects = new TreeNode("Objects");
            var process = new TreeNode("Process");
            root.Nodes.Add(process);
            root.Nodes.Add(objects);
            LoadProcess(process, release.Processes);
            LoadObjects(objects, release.Objects);
            treeView.Nodes.Add(root);
        }

        private void LoadProcess(TreeNode parent, IEnumerable<ProcessStage> processes)
        {
            foreach (var proc in processes)
            {
                var objectNode = new TreeNode(proc.Name) { Tag = proc };
                parent.Nodes.Add(objectNode);
                var actionsNode = new TreeNode("Pages");
                objectNode.Nodes.Add(actionsNode);
                LoadPages("Main", actionsNode, proc);
            }
        }

        private void LoadObjects(TreeNode parent, IEnumerable<ObjectStage> objects)
        {
            foreach (var obj in objects)
            {
                var objectNode = new TreeNode(obj.Name) { Tag = obj };
                parent.Nodes.Add(objectNode);
                var actionsNode = new TreeNode("Actions");
                objectNode.Nodes.Add(actionsNode);
                LoadPages("Initialize", actionsNode, obj);
                LoadAppModel(objectNode, obj);
            }
        }

        private void LoadAppModel(TreeNode parent, ObjectStage obj)
        {
            var root = new TreeNode("App Definition");
            parent.Nodes.Add(root);
            if (obj.ApplicationDefinition == null) return;
            var app = new TreeNode(string.Format("{0} - {1}", obj.ApplicationDefinition.Name, obj.ApplicationDefinition.Type)) { Tag = obj.ApplicationDefinition.ApplicationTypeInfo };
            root.Nodes.Add(app);
            foreach (var el in obj.ApplicationDefinition.Elements)
            {
                var elNode = new TreeNode(string.Format("{0} - {1}", el.Name, el.Type)) { Tag = el };
                app.Nodes.Add(elNode);
                elNode.Tag = el;
                var attNode = new TreeNode("Attributes");
                elNode.Nodes.Add(attNode);
                foreach (var att in el.Attributes.OrderByDescending(i => i.InUse))
                {
                    var aNode = new TreeNode(string.Format("{0} InUse: {1}", att.Name, att.InUse)) { Tag = att };
                    attNode.Nodes.Add(aNode);
                }
            }
        }
        private void LoadPages(string mainPageName, TreeNode parent, PageBasedStage pagedObject)
        {
            var mainNode = new TreeNode(mainPageName);
            parent.Nodes.Add(mainNode);
            LoadStages(mainNode, pagedObject.MainPage);
            foreach (var page in pagedObject.Pages)
            {
                var pageNode = new TreeNode(page.Name) { Tag = page };
                parent.Nodes.Add(pageNode);
                LoadStages(pageNode, page.Stages);
            }
        }

        private void LoadStages(TreeNode parent, IEnumerable<Stage> stages)
        {
            foreach (var stage in stages)
            {
                parent.Nodes.Add(new TreeNode(string.Format("{0} - {1}", stage.Type, stage.Name)) { Tag = stage });
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                propertyGrid.SelectedObject = null;
                return;
            }
            propertyGrid.SelectedObject = e.Node.Tag;
            if (typeof(common.model.XmlItem).IsAssignableFrom(e.Node.Tag.GetType()))
            {
                txtArea.Clear();
                txtArea.Text = Convert.ToString(((common.model.XmlItem)(e.Node.Tag)).Xml);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            var version = GetVersionNumber();
            var sw = new StringWriter();
            sw.WriteLine();
            sw.WriteLine("Tool Created By Oscar Marin");
            sw.WriteLine();
            sw.WriteLine("https://marin.cr");
            sw.WriteLine("https://github.com/marinoscar/rpa-navigator");
            sw.WriteLine();
            sw.WriteLine("App Version: {0}", version);
            sw.WriteLine();
            MessageBox.Show(sw.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuContents_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/marinoscar/rpa-navigator/wiki");
        }

        private void mnuRunCodeReview_Click(object sender, EventArgs e)
        {
            if (!IsFileLoaded()) return;
            this.ExecuteAction(ExecuteRules, null, null);
        }

        private bool IsFileLoaded()
        {
            if (treeView.Nodes.Count <= 0)
            {
                MessageBox.Show("Please open a release file first", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private object ExecuteRules()
        {
            var profile = RuleProfile.LoadFromFile();
            var ruleEngine = new BPRunner();
            ruleEngine.RuleRun += RuleEngine_RuleRun;
            var rules = ruleEngine.GetRulesFromProfile(profile);
            var results = ruleEngine.RunRules(profile, _release, rules.ToList());
            Reports.RunReport(() => { return SaveReport(profile, rules, results, _release); });
            return null;
        }

        private void RuleEngine_RuleRun(object sender, RunnerMessageEventArgs e)
        {
            lblStatus.Text = e.Message;
            Application.DoEvents();
        }

        private string SaveReport(RuleProfile profile, IEnumerable<IRule> rules, IEnumerable<Result> results, Release release)
        {
            var fileName = GetExcelFileName();
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            var fileInfo = new FileInfo(fileName);
            new ExcelOutputGenerator().CreateReport(fileInfo.FullName, profile, rules, results, release);
            return fileName;
        }

        private string GetExcelFileName()
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Save Results",
                RestoreDirectory = true,
                Filter = "Excel (*.xlsx)|*.xlsx"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return null;
            return dialog.FileName;
        }

        private void mnuCheckForUpdates_Click(object sender, EventArgs e)
        {
            InstallUpdateSyncWithInfo();
        }

        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
            }
        }

        private string GetVersionNumber()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return "1.0.0.0";
            var ad = ApplicationDeployment.CurrentDeployment;
            return Convert.ToString(ad.CurrentVersion);
        }

        private void mnuRules_Click(object sender, EventArgs e)
        {
            var rules = new RulesDialog();
            rules.ShowDialog();
        }

        private void mnuNonInvasiveReport_Click(object sender, EventArgs e)
        {
            if (!IsFileLoaded()) return;
            var fileName = GetExcelFileName();
            var reports = new Reports();
            this.ExecuteAction(() => reports.ExecuteNonInvasiveReport(_release, fileName), null, null);
        }

        
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtArea.SelectedText);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtArea.SelectAll();
        }

        private void mnuHookingBugReport_Click(object sender, EventArgs e)
        {
            if (!IsFileLoaded()) return;
            var fileName = GetExcelFileName();
            var reports = new Reports();
            this.ExecuteAction(() => reports.ExecuteHookingBug(_release, fileName), null, null);
        }
    }
}
