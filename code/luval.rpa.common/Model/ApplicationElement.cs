using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.Model
{
    public class ApplicationElement : Item
    {

        public ApplicationElement()
        {
            Attributes = new List<ElementAttribute>();
        }
        public string Type { get; set; }
        public string DataType { get; set; }

        public List<ElementAttribute> Attributes { get; set; }
    }
}
