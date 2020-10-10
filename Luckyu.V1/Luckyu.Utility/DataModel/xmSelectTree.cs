using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class xmSelectTree
    {
        public string id { get; set; }

        public string name { get; set; }

        public string value { get; set; }

        public bool selected { get; set; }

        public bool disabled { get; set; }

        public List<xmSelectTree> children { get; set; }
    }
}
