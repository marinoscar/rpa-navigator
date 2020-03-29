using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ApplicationElement : Stage
    {

        public ApplicationElement(XElement el) : base(el)
        {
            Attributes = new List<ElementAttribute>();
            IdName = "id";
        }
        public string DataType
        {
            get { return GetElementValue("datatype"); }
            set { TrySetElValue("datatype", value); }
        }

        public string BaseType
        {
            get { return GetElementValue("basetype"); }
            set { TrySetElValue("basetype", value); }
        }

        public override string Type
        {
            get { return GetElementValue("type"); }
            set { TrySetElValue("type", value); }
        }

        public override string Id
        {
            get { return GetElementValue("id"); }
            set { TrySetElValue("id", value); }
        }

        public string Diagnose
        {
            get { return GetElementValue("diagnose"); }
            set { TrySetElValue("diagnose", value); }
        }

        public List<ElementAttribute> Attributes { get; set; }
    }
}
