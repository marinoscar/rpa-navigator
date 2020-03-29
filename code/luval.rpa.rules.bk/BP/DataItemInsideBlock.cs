using luval.rpa.common.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using luval.rpa.common.model;
using luval.rpa.common.rules.attributes;
using luval.rpa.common.model.bp;

namespace luval.rpa.rules.bp
{

    [Name("Data Items Are Inside Block Stage"),
     Description("Checks that all data items are inside a block stage")]
    public class DataItemInsideBlock : BPRuleBase, IRule
    {
        public override IEnumerable<Result> Execute(Release release)
        {
            var res = new List<Result>();
            ProcessItem(res, release.Processes, "Process");
            ProcessItem(res, release.Objects, "Object");
            return res;
        }

        private void ProcessItem(List<Result> res, IEnumerable<PageBasedStage> items, string type)
        {
            foreach (var item in items)
            {
                var pageBlock = new PageBlockGroup();
                pageBlock.Load(FilterByPage(item.MainPage), item.Name, "Main", type);
                LoadErrors(res, pageBlock);
                foreach (var page in item.Pages)
                {
                    pageBlock.Load(FilterByPage(page.Stages), item.Name, page.Name, type);
                    LoadErrors(res, pageBlock);
                }
            }
        }

        private void LoadErrors(List<Result> results, PageBlockGroup pageBlock)
        {
            foreach(var stage in pageBlock.StagesOutsideOfBlock)
            {
                results.Add(new Result() {
                    RuleName = Name, RuleDescription = GetRuleDescription(),
                    Parent = pageBlock.Parent, Page = pageBlock.PageName,
                    Scope = pageBlock.Type, Type = ResultType.Warning,
                    Stage = stage.Name, StageType = stage.Type, StageId = stage.Id,
                    Message = string.Format(@"Data Item ""{0}"" is outside of a block stage", stage.Name)
                });
            }
        }

        private IEnumerable<Stage> FilterByPage(IEnumerable<Stage> stages)
        {
            return stages.Where(i => i.Type == "Block" || i.Type == "Data");
        }

        
    }
}
