using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFSchemeLineModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public int wftype { get; set; }

        public string from { get; set; }

        public string to { get; set; }
    }
}
