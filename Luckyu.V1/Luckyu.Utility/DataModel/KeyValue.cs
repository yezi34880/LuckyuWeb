using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class KeyValue<T1, T2>
    {
        public T1 Key { get; set; }

        public T2 Value { get; set; }
    }

    public class KeyValueList<T1, T2>
    {
        public T1 Key { get; set; }

        public List<T2> ValueList { get; set; }
    }
}
