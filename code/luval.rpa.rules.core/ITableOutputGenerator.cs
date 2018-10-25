using System.Collections.Generic;

namespace luval.rpa.rules.core
{
    public interface ITableOutputGenerator
    {
        string Create(IEnumerable<object> items);
    }
}