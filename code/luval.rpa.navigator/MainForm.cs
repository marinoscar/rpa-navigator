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
                LoadAppModel(objectNode, obj);
            }
        }

        private void LoadAppModel(TreeNode parent, ObjectStage obj)
        {
            var root = new TreeNode("App Definition");
            parent.Nodes.Add(root);
            if (obj.ApplicationDefinition == null) return;
            var app = new TreeNode(string.Format("{0} - {1}", obj.ApplicationDefinition.Name, obj.ApplicationDefinition.Type)) { Tag = obj.ApplicationDefinition };
            root.Nodes.Add(app);
            foreach(var el in obj.ApplicationDefinition.Elements)
            {
                var elNode = new TreeNode(string.Format("{0} - {1}", el.Name, el.Type)) { Tag = el };
                app.Nodes.Add(elNode);
                var attNode = new TreeNode("Attributes");
                elNode.Nodes.Add(attNode);
                foreach(var att in el.Attributes.OrderByDescending(i => i.InUse))
                {
                    var aNode = new TreeNode(string.Format("{0} InUse: {1}", att.Name, att.InUse)) { Tag = att };
                    attNode.Nodes.Add(aNode);
                }
            }
        }
        private void LoadActions(TreeNode parent, IEnumerable<ActionPage> actions)
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
