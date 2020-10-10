using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFSchemeNodeModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public int confluence_type { get; set; }

        public decimal confluence_rate { get; set; }

        public List<WFAuthorizeModel> authusers { get; set; }

        public List<WFFormModel> forms { get; set; }

        public string sqlsuccess { get; set; }

        public string sqlfail { get; set; }

        public string injectassembly { get; set; }

        public string injectclass { get; set; }

        public string sqlcondition { get; set; }
    }
}
