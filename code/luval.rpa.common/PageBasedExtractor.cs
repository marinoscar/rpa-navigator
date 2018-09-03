using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class PageBasedExtractor : ExtractorBase
    {
        protected virtual XElement RootXML { get; private set; }

        private IEnumerable<T> GetPages<T>(string pageElementName, Func<XElement, IEnumerable<PageStage>, T> create ) where T : PageStage
        {
            var root = RootXML.Elements().ToList();
            var res = new List<T>();
            var pages = RootXML.Elements()
                .Where(i => i.Name.LocalName == "contents").Elements()
                .Where(i => i.Name.LocalName == pageElementName).ToList();
            foreach (var page in pages)
            {
                var extractors = new StageExtractor(page, default(string));
                extractors.Load();
                res.Add(create(page, GetPages(page)));
            }
            return res;
        }

        private IEnumerable<PageStage> GetPages(XElement obj)
        {
            var res = new List<PageStage>();
            var elements = obj.Elements().Where(i => i.Name.LocalName == "process").Elements().ToList();
            var subsheets = elements.Where(i => i.Name.LocalName == "subsheet").ToList();
            foreach (var sheet in subsheets)
            {
                var stageExtractor = new StageExtractor(obj, sheet.Attribute("subsheetid").Value);
                stageExtractor.Load();
                res.Add(new PageStage(obj) { Stages = stageExtractor.Stages });
            }
            return res;
        }

    }
}
