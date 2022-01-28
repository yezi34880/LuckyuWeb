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
        /// 确认当前节点是否可选，0=不选节点，默认往下走；1-节点可选择；2-节点单选
        /// </summary>
        public int comfirm_node { get; set; }

        /// <summary>
        /// 确认当前步骤人员，0=不选人；1=选人；2-单选人
        /// </summary>
        public int comfirm_user { get; set; }

        /// <summary>
        /// 相同用户自动跳过
        /// </summary>
        public int autoskip { get; set; }

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
