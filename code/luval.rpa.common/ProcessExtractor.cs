using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ProcessExtractor : PageBasedExtractor
    {
        public ProcessExtractor(string xml) : this(XElement.Parse(xml))
        {
        }

        public ProcessExtractor(XElement xml) : base(xml)
        {
            Process = new List<ProcessStage>();
        }
        public List<ProcessStage> Process { get; set; }

        public void Load()
        {
            Process = GetPages<ProcessStage>("process", null, CreateStage).ToList();
        }

        private ProcessStage CreateStage(XElement xml, IEnumerable<PageStage> pages)
        {
            var obj = new ProcessStage(xml)
            {
                Pages = new List<PageStage>(pages),
            };
            return obj;
        }
    }
}
