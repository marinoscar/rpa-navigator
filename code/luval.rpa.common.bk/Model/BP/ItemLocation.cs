using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace luval.rpa.common.model.bp
{
    public class ItemLocation : XmlItem
    {
        public ItemLocation(XElement element) : base(element)
        {
        }

        public int X
        {
            get { return Convert.ToInt32(GetElementValue("displayx"));}
            set { TrySetElValue("displayx", Convert.ToString(value)); }
        }

        public int X2 { get { return X + Width; } }

        public int Y
        {
            get { return Convert.ToInt32(GetElementValue("displayy")); }
            set { TrySetElValue("displayy", Convert.ToString(value)); }
        }

        public int Y2 { get { return Y + Height; } }

        public int Width
        {
            get { return Convert.ToInt32(GetElementValue("displaywidth")); }
            set { TrySetElValue("displaywidth", Convert.ToString(value)); }
        }

        public int Height
        {
            get { return Convert.ToInt32(GetElementValue("displayheight")); }
            set { TrySetElValue("displayheight", Convert.ToString(value)); }
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y:{1}, Width:{2}, Height: {3}", X, Y, Width, Height);
        }
    }
}
