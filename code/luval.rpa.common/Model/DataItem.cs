using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.Model
{
    public class DataItem : Stage
    {
        public DataItem(XElement xml) : base(xml)
        {
        }

        public string DataType
        {
            get { return GetElementValue("datatype"); }
            set { TrySetElValue("datatype", value); }
        }

        public string InitialValue
        {
            get { return GetElementValue("initialvalue"); }
            set { TrySetElValue("initialvalue", value); }
        }

        public string Exposure
        {
            get { return GetElementValue("exposure"); }
            set { TrySetElValue("exposure", value); }
        }

        public string Private
        {
            get { return GetElementValue("private"); }
            set { TrySetElValue("private", value); }
        }

        public string AlwaysInitialized
        {
            get { return GetElementValue("alwaysinit"); }
            set { TrySetElValue("alwaysinit", value); }
        }

        public bool IsPrivate { get { return Private == "Yes";  } }
        public bool HasInitialValue { get { return !string.IsNullOrWhiteSpace(InitialValue); } }
    }
}
