using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFTaskAuthModel
    {
        public string auth_id { get; set; }

        public string task_id { get; set; }

        public string flow_id { get; set; }

        public string instance_id { get; set; }

        public string process_id { get; set; }

        public string processname { get; set; }

        public string nodename { get; set; }

        public string user_id { get; set; }

        public string username { get; set; }

    }
}
