using luval.rpa.common;
using luval.rpa.common.Model;
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

namespace luval.rpa.navigator
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
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
            if (res == DialogResult.Cancel) return;
            var file = openDlg.FileName;
            var extractor = new ReleaseExtractor(File.ReadAllText(file));
            extractor.Load();
            LoadTree(extractor.Release);
        }

        private void LoadTree(Release release)
        {
            treeView.Nodes.Clear();
            var root = new TreeNode("Release");
            var objects = new TreeNode("Objects");
            root.Nodes.Add(objects);
            LoadObjects(objects, release.Objects);
            treeView.Nodes.Add(root);
        }

        private void LoadObjects(TreeNode parent, IEnumerable<ObjectStage> objects)
        {
            foreach(var obj in objects)
            {
                var objectNode = new TreeNode(obj.Name) { Tag = obj};
                parent.Nodes.Add(objectNode);
                var actionsNode = new TreeNode("Actions");
                objectNode.Nodes.Add(actionsNode);
                LoadActions(actionsNode, obj.Actions);
            }
        }

        private void LoadActions(TreeNode parent, IEnumerable<ActionStage> actions)
        {
            foreach(var action in actions)
            {
                var actionNode = new TreeNode(action.Name)
                {
                    Tag = action
                };
                var stagesType = action.Stages.Select(i => i.Type).Distinct().ToList();
                foreach(var type in stagesType)
                {
                    var nodeType = new TreeNode(type);
                    actionNode.Nodes.Add(nodeType);
                    var stages = action.Stages.Where(i => i.Type == type).ToList();
                    foreach(var stage in stages)
                    {
                        var stageNode = new TreeNode(stage.Name)
                        {
                            Tag = stage
                        };
                        nodeType.Nodes.Add(stageNode);
                    }
                }
                parent.Nodes.Add(actionNode);
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
        }
    }
}
