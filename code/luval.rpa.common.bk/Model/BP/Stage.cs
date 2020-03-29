using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class Stage : XmlItem
    {
        public Stage(XElement xml) : base(xml)
        {
            IdName = "stageid";
            Location = new ItemLocation(xml);
        }

        protected string IdName { get; set; }

        public virtual string Id
        {
            get { return GetAttributeValue(IdName); }
            set { TrySetAttValue(IdName, value); }
        }

        public virtual string Name
        {
            get { return GetAttributeValue("name"); }
            set { TrySetAttValue("name", value); }
        }

        public virtual string Type
        {
            get { return GetAttributeValue("type"); }
            set { TrySetAttValue("type", value); }
        }

        public virtual string PageId
        {
            get { return GetElementValue("subsheetid"); }
            set { TrySetElValue("subsheetid", value); }
        }

        public virtual string PageName { get; set; }

        public virtual string Description
        {
            get { return GetElementValue("narrative"); }
            set { TrySetElValue("narrative", value); }
        }

        public virtual string OnSuccess
        {
            get { return GetElementValue("onsuccess"); }
            set { TrySetElValue("onsuccess", value); }
        }

        public virtual ItemLocation Location { get; set; }
    }
}
