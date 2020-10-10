using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class eleTree
    {
        public string id { get; set; }

        public string label { get; set; }

        public bool disabled { get; set; }

        public List<eleTree> children { get; set; }

        public string parentId { get; set; }

        public Dictionary<string, string> ext { get; set; }

        public bool isleaf { get; set; }
    }
}
