using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ApplicationDefinition : Stage
    {

        public ApplicationDefinition(XElement el) : base(el)
        {
            Elements = new List<ApplicationElement>();
            LoadAppTypeInfo();
        }

        public List<ApplicationElement> Elements { get; set; }

        public ApplicationTypeInfo ApplicationTypeInfo { get; set; }

        private void LoadAppTypeInfo()
        {
            var appDef = Xml.Elements().Where(i => i.Name.LocalName == "process").Elements().FirstOrDefault();
            if (appDef == null) return;
            if (!HasElement(appDef, "apptypeinfo")) return;
            var appDefInfo = GetElement(appDef, "apptypeinfo");
            ApplicationTypeInfo = new ApplicationTypeInfo(appDefInfo);
        }
    }
}
