using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ExceptionStageDetail : XmlItem
    {
        public ExceptionStageDetail(XElement element) : base(element)
        {
        }

        public string Type
        {
            get { return GetAttributeValue("type"); }
            set { TrySetAttValue("type", value); }
        }

        public string Detail
        {
            get { return GetAttributeValue("detail"); }
            set { TrySetAttValue("detail", value); }
        }

        public string UseCurrent
        {
            get { return GetAttributeValue("usecurrent"); }
            set { TrySetAttValue("usecurrent", value); }
        }

        public string SaveScreenCapture
        {
            get { return GetAttributeValue("savescreencapture"); }
            set { TrySetAttValue("savescreencapture", value); }
        }
    }
}
