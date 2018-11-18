using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.bp
{
    public class PageBlockGroup
    {
        public PageBlockGroup()
        {
            Blocks = new List<BlockGroupItem>();
            StagesOutsideOfBlock = new List<Stage>();
        }

        public string Parent { get; set; }
        public string PageName { get; set; }
        public string Type { get; set; }
        public List<BlockGroupItem> Blocks { get; set; }
        public List<Stage> StagesOutsideOfBlock { get; set; }

        public void Load(IEnumerable<Stage> stages, string parent, string pageName, string type)
        {
            Parent = parent; PageName = pageName; Type = type;
            var helper = new StageHelper();
            var blocks = stages.Where(i => i.Type == "Block").ToList();
            var otherStages = stages.Where(i => i.Type != "Block").ToList();
            foreach (var block in blocks)
            {
                var inBlock = helper.GetStagesInBlock(block, otherStages);
                if (!inBlock.Any()) continue;
                Blocks.Add(new BlockGroupItem() {
                    Block = block, Stages = inBlock.ToList()
                });
            }
            StagesOutsideOfBlock = stages.Where(i => i.Type != "Block" && !Blocks.SelectMany(s => s.Stages).Contains(i)).ToList();
        }
    }

    public class BlockGroupItem
    {
        public BlockGroupItem()
        {
            Stages = new List<Stage>();
        }
        public Stage Block { get; set; }
        public List<Stage> Stages { get; set; }
    }
}
