using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class Release : XmlItem
    {
        public Release(XElement xml) : base(xml)
        {
            Objects = new List<ObjectStage>();
        }
        public List<ObjectStage> Objects { get; set; }
        public List<ProcessStage> Processes { get; set; }

        public string Name
        {
            get { return GetElementValue("name"); }
            set { TrySetElValue("name", value); }
        }

        public string ReleaseNotes
        {
            get { return GetElementValue("release-notes"); }
            set { TrySetElValue("release-notes", value); }
        }

        public string PackageId
        {
            get { return GetElementValue("package-id"); }
            set { TrySetElValue("package-id", value); }
        }

        public string PackageName
        {
            get { return GetElementValue("package-name"); }
            set { TrySetElValue("package-name", value); }
        }

        public string CreatedBy
        {
            get { return GetElementValue("user-created-by"); }
            set { TrySetElValue("user-created-by", value); }
        }

        public string CreatedOn
        {
            get { return GetElementValue("created"); }
            set { TrySetElValue("created", value); }
        }

        /// <summary>
        /// Gets the stage analysis units for the release
        /// </summary>
        /// <returns>A collection of stage analysis units</returns>
        public IEnumerable<StageAnalysisUnit> GetAnalysisUnits()
        {
            return GetAnalysisUnits(i => true);
        }

        /// <summary>
        /// Gets the stage analysis units for the release
        /// </summary>
        /// <param name="filter">the predicate to filter the stages</param>
        /// <returns>A collection of stage analysis units</returns>
        public IEnumerable<StageAnalysisUnit> GetAnalysisUnits(Func<Stage,bool> filter)
        {
            var res = new List<StageAnalysisUnit>();
            foreach (var prc in Processes)
            {
                res.AddRange(GetAnalysisUnits(prc, "Process", filter));
            }
            foreach (var obj in Objects)
            {
                res.AddRange(GetAnalysisUnits(obj, "Object", filter));
            }
            return res;
        }

        /// <summary>
        /// Gets the stage analysis units for the release
        /// </summary>
        /// <param name="parent">The page based stage</param>
        /// <param name="type">The parent type</param>
        /// <returns>A collection of stage analysis units</returns>
        public IEnumerable<StageAnalysisUnit> GetAnalysisUnits(PageBasedStage parent, string type)
        {
            return GetAnalysisUnits(parent, type, i => true);
        }

        /// <summary>
        /// Gets the stage analysis units for the release
        /// </summary>
        /// <param name="parent">The page based stage</param>
        /// <param name="type">The parent type</param>
        /// <param name="filter">the predicate to filter the stages</param>
        /// <returns>A collection of stage analysis units</returns>
        public IEnumerable<StageAnalysisUnit> GetAnalysisUnits(PageBasedStage parent, string type, Func<Stage, bool> filter)
        {
            var res = new List<StageAnalysisUnit>();
            res.AddRange(parent.MainPage.Where(filter).Select(i => new StageAnalysisUnit()
            {
                Page = "Main",
                ParentName = parent.Name,
                ParentType = type,
                Stage = i
            }));

            foreach (var page in parent.Pages)
            {
                res.AddRange(GetAnalysisUnits(page, type, parent.Name, filter));
            }
            return res;
        }

        /// <summary>
        /// Gets the stage analysis units for the release
        /// </summary>
        /// <param name="page">The page based stage</param>
        /// <param name="type">The parent type</param>
        /// <param name="parent">The parent name</param>
        /// <returns>A collection of stage analysis units</returns>
        public IEnumerable<StageAnalysisUnit> GetAnalysisUnits(PageStage page, string type, string parent)
        {
            return GetAnalysisUnits(page, type, parent, i => true);
        }

        /// <summary>
        /// Gets the stage analysis units for the release
        /// </summary>
        /// <param name="page">The page based stage</param>
        /// <param name="type">The parent type</param>
        /// <param name="parent">The parent name</param>
        /// <param name="filter">the predicate to filter the stages</param>
        /// <returns>A collection of stage analysis units</returns>
        public IEnumerable<StageAnalysisUnit> GetAnalysisUnits(PageStage page, string type, string parent, Func<Stage,bool> filter)
        {
            var res = new List<StageAnalysisUnit>();
            res.AddRange(page.Stages.Where(filter).Select(i => new StageAnalysisUnit()
            {
                PageId = page.Id,
                Page = page.Name,
                ParentName = parent,
                ParentType = type,
                Stage = i
            }));
            return res;
        }
    }
}
