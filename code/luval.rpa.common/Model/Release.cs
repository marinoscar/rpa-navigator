using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class Release : Item
    {
        public Release(XElement xml) : base(xml)
        {
            Objects = new List<ObjectStage>();
        }
        public List<ObjectStage> Objects { get; set; }

        public string Name
        {
            get { return GetElementValue("name"); }
            set { TrySetElValue("name", value); }
        }

        public string ReleaseNotes
        {
            get { return GetElementValue("release-notes"); }
            set { TrySetElValue("release-notes", value); }
        }

        public string PackageId
        {
            get { return GetElementValue("package-id"); }
            set { TrySetElValue("package-id", value); }
        }

        public string PackageName
        {
            get { return GetElementValue("package-name"); }
            set { TrySetElValue("package-name", value); }
        }

        public string CreatedBy
        {
            get { return GetElementValue("user-created-by"); }
            set { TrySetElValue("user-created-by", value); }
        }

        public string CreatedOn
        {
            get { return GetElementValue("created"); }
            set { TrySetElValue("created", value); }
        }
    }
}
