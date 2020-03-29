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
    public class ReleaseExtractor : ExtractorBase
    {
        private XElement _xmlDOM;

        public ReleaseExtractor(string xml)
        {
            InitializeXml(xml);
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

        private void InitializeXml(string xml)
        {
            var fix = "&#x0;";
            try
            {
                DoInitialize(xml, true);
            }
            catch(Exception ex)
            {
                xml = xml.Replace(fix, string.Empty);
                if (!DoInitialize(xml, false)) throw ex;
            }
        }
        
        private bool DoInitialize(string xml, bool raise)
        {
            try
            {
                _xmlDOM = XElement.Parse(xml);
            }
            catch (Exception ex)
            {
                if (raise) throw ex;
                return false;
            }
            return true;
        }

       
    }
}
