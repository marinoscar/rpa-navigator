using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            // there is a problem with the BP XML, the location data for some reason does not enable
            // to locate the data item inside the box, but for some reason it does rendered inside in the 
            // BP interactive client, adding a buffer of 20% to see if that helps

            //we clone the item
            var xml = XElement.Parse(block.Location.Xml.ToString());
            // upgrade the size
            var bc = new ItemLocation(xml)
            {
                X = ApplyLocationOffSet(block.Location.X, 0.1f, true),
                //Y = ApplyLocationOffSet(block.Location.Y, 0.1f, true),
                Y = block.Location.Y,
                Width = ApplyLocationOffSet(block.Location.Width, 0.9f, false),
                //Height = ApplyLocationOffSet(block.Location.Height, 0.2f, false)
                Height = block.Location.Height
            };
            //var bc = block.Location;
            return stages.Where(i => i.Location != null &&
                InsideX(bc, i.Location) &&
                InsideY(bc, i.Location)).ToList();
        }

        private int ApplyLocationOffSet(int original, float offset, bool substract)
        {
            var current = Math.Abs(original);
            var isNegative = original < 0;
            var res = (int)(current * offset);
            res = substract && !isNegative ? current - res : current + res;
            return isNegative ? res * -1 : res;
        }

        private bool InsideX(ItemLocation block, ItemLocation item)
        {
            return block.X <= item.X && block.X2 >= item.X2;
        }

        private bool InsideY(ItemLocation block, ItemLocation item)
        {
            var res = false;
            res = block.Y2 >= item.Y2 && block.Y <= item.Y;
            return res;
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
