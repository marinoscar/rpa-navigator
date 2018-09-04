using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ReleaseExtractor : ExtractorBase
    {
        private XElement _xmlDOM;

        public ReleaseExtractor(string xml)
        {
            _xmlDOM = XElement.Parse(xml);
        }

        public ReleaseExtractor(XElement xml)
        {
            _xmlDOM = xml;
        }

        public Release Release { get; private set; }

        public override void Load()
        {
            var processExtractor = new ProcessExtractor(_xmlDOM);
            var objectExtractor = new ObjectExtractor(_xmlDOM);
            objectExtractor.Load();
            processExtractor.Load();
            Release = new Release(_xmlDOM)
            {
                Objects = objectExtractor.Objects.ToList(),
                Processes = processExtractor.Process.ToList()
            };
        }

             

       
    }
}
