using luval.rpa.common.model;
using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.extractors.bp
{
    public abstract class PageBasedExtractor : ExtractorBase
    {

        public PageBasedExtractor(XElement xml)
        {
            RootXML = xml;
        }

        protected virtual XElement RootXML { get; private set; }

        protected virtual IEnumerable<T> GetPages<T>(string pageElementName, string id, Func<XElement, IEnumerable<Stage>, IEnumerable<PageStage>, T> create ) where T : PageBasedStage
        {
            var root = RootXML.Elements().ToList();
            var res = new List<T>();
            var pages = RootXML.Elements()
                .Where(i => i.Name.LocalName == "contents").Elements()
                .Where(i => i.Name.LocalName == pageElementName).ToList();
            foreach (var page in pages)
            {
                var extractors = new StageExtractor(page, id);
                extractors.Load();
                res.Add(create(page, extractors.Stages, GetPages(page)));
            }
            return res;
        }

        protected virtual IEnumerable<PageStage> GetPages(XElement obj)
        {
            var res = new List<PageStage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            var subsheets = elements.Where(i => i.Name.LocalName == "subsheet").ToList();
            foreach (var sheet in subsheets)
            {
                var stageExtractor = new StageExtractor(obj, sheet.Attribute("subsheetid").Value);
                stageExtractor.Load();
                var page = new PageStage(sheet);
                stageExtractor.Stages.ForEach(i => i.PageName = page.Name);
                page.Stages = stageExtractor.Stages;
                res.Add(page);
            }
            return res;
        }

    }
}
