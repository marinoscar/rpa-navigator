using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.rules.bp
{
    public class StageHelper
    {
        /// <summary>
        /// Checks if the current stage is preceded from a valid wait stage
        /// </summary>
        /// <param name="currentStage">The current stage</param>
        /// <param name="stages">The stages in which to look for a valid wait</param>
        /// <returns></returns>
        public bool HasAnImediatePreviousWait(Stage currentStage, IEnumerable<Stage> stages)
        {
            var waits = stages.Where(i => !string.IsNullOrWhiteSpace(i.Type) &&
                i.Type.ToLowerInvariant().Equals("waitstart"));
            foreach (var wait in waits)
            {
                foreach (var c in ((WaitStartStage)wait).Choices)
                {
                    var nextStage = GetNextStage(c.OnTrue, stages);
                    if (nextStage != null && nextStage.Id == currentStage.Id) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the next valid stage for the current stage
        /// </summary>
        /// <param name="nextId">The id of the next stage</param>
        /// <param name="stages">Stages to check</param>
        /// <returns></returns>
        public Stage GetNextStage(string nextId, IEnumerable<Stage> stages)
        {
            if (string.IsNullOrWhiteSpace(nextId)) return null;
            var stage = stages.FirstOrDefault(i => i.Id == nextId);
            if (stage == null) return null;
            if (stage.Type == "Anchor") return GetNextStage(stage.OnSuccess, stages);
            return stage;
        }

        /// <summary>
        /// Filters the stages by page
        /// </summary>
        /// <param name="unit">The unit to check</param>
        /// <param name="units">The units to checks</param>
        /// <returns></returns>
        public IEnumerable<Stage> FilterStagesByPage(StageAnalysisUnit unit, IEnumerable<StageAnalysisUnit> units)
        {
            return units.Where(i => i.ParentType == unit.ParentType && i.ParentName == unit.ParentName && i.PageId == unit.PageId)
                .Select(i => i.Stage);
        }

        /// <summary>
        /// Checks the stages that are inside a block
        /// </summary>
        /// <param name="block">The block to inspect</param>
        /// <param name="stages">The stages to analize</param>
        /// <returns></returns>
        public IEnumerable<Stage> GetStagesInBlock(Stage block, IEnumerable<Stage> stages)
        {
            return stages.Where(i => i.Location != null &&
                i.Location.X >= block.Location.X &&
                i.Location.Y >= block.Location.Y &&
                (i.Location.X + i.Location.Width) <= (block.Location.X + block.Location.Width) &&
                (i.Location.Y + i.Location.Height) <= (block.Location.Y + block.Location.Height)).ToList();
        }


        /// <summary>
        /// Provide the stages where an element is in use
        /// </summary>
        /// <param name="el">The element to search</param>
        /// <param name="stages">The stages to use in the search</param>
        /// <returns></returns>
        public IEnumerable<Stage> ElementUses(ApplicationElement el, IEnumerable<Stage> stages)
        {
            var res = new List<Stage>();
            if (el == null || string.IsNullOrWhiteSpace(el.Id)) return res;
            var navs = new[] { "Read", "Write", "Navigate" };
            var navigates = stages.Where(i => navs.Contains(i.Type)).Select(i => (NavigateStage)i).ToList();
            res.AddRange(navigates.Where(i => i.Actions != null && i.Actions.Any(a => 
                !string.IsNullOrWhiteSpace(a.ElementId) && a.ElementId == el.Id)));
            var waits = stages.Where(i => i.Type == "WaitStart").Select(i => (WaitStartStage)i).ToList();
            res.AddRange(waits.Where(i => i.Choices != null && i.Choices.Any(c => !string.IsNullOrWhiteSpace(c.ElementId) &&
                c.ElementId == el.Id)));
            return res;
        }

        /// <summary>
        /// Provide the stages where an element is in use
        /// </summary>
        /// <param name="el">The element to search</param>
        /// <param name="obj">The object to use in the search</param>
        /// <returns></returns>
        public IEnumerable<Stage> ElementUses(ApplicationElement el, ObjectStage obj)
        {
            var res = new List<Stage>();
            res.AddRange(ElementUses(el, obj.MainPage));
            res.AddRange(ElementUses(el, obj.Pages.SelectMany(i => i.Stages)));
            return res;
        }
    }
}
