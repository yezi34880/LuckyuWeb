using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class CommonTree<T>
    {

        public T Main { get; set; }

        public List<CommonTree<T>> Children { get; set; }
    }
}
