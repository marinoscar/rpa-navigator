using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
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

        public bool IsPrivate
        {
            get
            {
                return HasElement("private") || Private == "Yes";
            }
        }
        public bool HasInitialValue { get { return !string.IsNullOrWhiteSpace(InitialValue); } }

        public bool IsEnvironment
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Exposure)) return false;
                return Exposure.ToLowerInvariant().Equals("environment");
            }
        }

        public bool IsSession
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Exposure)) return false;
                return Exposure.ToLowerInvariant().Equals("session");
            }
        }

    }
}
