using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFTaskModel
    {
        public string history_id { get; set; }
        public string task_id { get; set; }
        public string flow_id { get; set; }
        public string instance_id { get; set; }
        public string process_id { get; set; }
        public string flowname { get; set; }
        public string nodetype { get; set; }
        public string nodename { get; set; }
        public string processname { get; set; }
        public int is_finished { get; set; }
        public string submit_user_id { get; set; }
        public string submit_username { get; set; }
        public string department_id { get; set; }
        public string company_id { get; set; }
        public string createtime { get; set; }


    }
}
