using luval.rpa.common.model.bp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ApplicationTypeInfo : XmlItem
    {
        public ApplicationTypeInfo(XElement xml) : base(xml)
        {
            Parameters = new List<ApplicationTypeInfoParameter>();
            LoadParameters();
        }

        public string Id
        {
            get { return GetElementValue("id"); }
            set { TrySetElValue("id", value); }
        }

        public List<ApplicationTypeInfoParameter> Parameters { get; set; }

        private void LoadParameters()
        {
            if (!HasElement("parameters")) return;
            var paramsEl = GetElement("parameters");
            var elements = paramsEl.Elements().ToList();
            foreach(var el in elements)
            {
                Parameters.Add(new ApplicationTypeInfoParameter(el));
            }
        }
    }
}
