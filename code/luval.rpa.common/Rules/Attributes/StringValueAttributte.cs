using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.rpa.common.rules.attributes
{
    public  class StringValueAttributte : Attribute
    {
        public StringValueAttributte(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
