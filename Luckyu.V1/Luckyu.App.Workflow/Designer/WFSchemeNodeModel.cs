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

        /// <summary>
        /// 节点类型
        /// 开始-startround 结束-endround 条件判断-conditionnode 会签-confluencenode 执行-processnode 审批-stepnode 传阅-auditornode
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 确认下步节点（如果下一步有多个节点，可由当前审批人自行选择一个）
        /// </summary>
        public int comfirm_node { get; set; }

        /// <summary>
        /// 确认当前步骤人员，如果有多人，需要选择
        /// </summary>
        public int comfirm_user { get; set; }

        /// <summary>
        /// 选择用户时单选还是多选，0为多选（注意，多选时相当于会签，必须多人全部同意之后才能继续），1为单选，以后可以扩展为选择人数
        /// </summary>
        public int user_num { get; set; }

        /// <summary>
        /// 不自动跳过，默认为0，即如果下一步审批人与当前相同则自动跳过，除非下一步节点这个属性为1
        /// </summary>
        public int not_autoskip { get; set; }

        /// <summary>
        /// 会签类型 1-全部通过 2-任一人通过 3-按比例
        /// </summary>
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
