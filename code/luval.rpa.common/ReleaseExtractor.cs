using luval.rpa.common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common
{
    public class ReleaseExtractor
    {
        private XElement _xmlDOM;

        public ReleaseExtractor(string xml)
        {
            _xmlDOM = XElement.Parse(xml);
        }

        public Release Release { get; private set; }

        public List<ObjectStage> Objects { get; private set; }

        public void Load()
        {
            var objectExtractor = new ObjectExtractor(_xmlDOM);
            objectExtractor.Load();
            Objects = new List<ObjectStage>(objectExtractor.Objects);
            Release = new Release()
            {
                Objects = Objects
            };
        }

             

       
    }
}
