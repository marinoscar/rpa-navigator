using luval.rpa.common.extractors.bp;
using luval.rpa.common.model.bp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.navigator
{
    public class DuplicateProcessesInRelease
    {
        private ReleaseExtractor _extractor;
        private Release _release;

        public DuplicateProcessesInRelease(string releaseFile)
        {
            _extractor = new ReleaseExtractor(File.ReadAllText(releaseFile));
            _extractor.Load();
            _release = _extractor.Release;
        }

        public void RenameProcess(string startWith, string newValue)
        {
            RenamePageBasedItem(startWith, newValue, _release.Processes);
        }

        public void RenameObjects(string startWith, string newValue)
        {
            RenamePageBasedItem(startWith, newValue, _release.Objects);
        }

        public void ReplaceText(string current, string newText)
        {
            var xml = _release.Xml.ToString();
            var newXml = xml.Replace(current, newText);
            _extractor = new ReleaseExtractor(newXml);
            _extractor.Load();
            _release = _extractor.Release;
        }

        public void Save(string fileName)
        {
            var xml = _release.Xml.ToString();
            File.WriteAllText(fileName, xml);
        }

        private void RenamePageBasedItem(string startWith, string newValue, IEnumerable<PageBasedStage> items)
        {
            var itemList = new List<PageBasedStage>(items);
            foreach (var item in itemList.Where(i => i.Name.StartsWith(startWith)).ToList())
            {
                var currentId = item.Id;
                var newId = Guid.NewGuid().ToString();
                item.Id = newId;
                item.Name = item.Name.Replace(startWith, newValue);
                ReplaceId(currentId, newId);
            }
        }

        private void ReplaceId(string currentId, string newId)
        {
            var xml = _release.Xml.ToString();
            var newXml = xml.Replace(currentId, newId);
            _extractor = new ReleaseExtractor(newXml);
            _extractor.Load();
            _release = _extractor.Release;
        }


    }
}
