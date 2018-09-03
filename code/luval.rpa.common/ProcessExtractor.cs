using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ProcessExtractor : ExtractorBase
    {
        private XElement _xmlDOM;

        public ProcessExtractor(string xml) : this(XElement.Parse(xml))
        {
        }

        public ProcessExtractor(XElement xml)
        {
            _xmlDOM = xml;
            Process = new List<ProcessStage>();
        }
        public List<ProcessStage> Process { get; set; }
    }
}
