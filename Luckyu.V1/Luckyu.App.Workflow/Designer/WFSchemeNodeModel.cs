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

        public string code { get; set; }

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
        /// 相同用户是否自动跳过
        /// </summary>
        public int autoskip { get; set; }

        /// <summary>
        /// 超时选项：0-无动作；1-超时自动通过；2-超时自动退回起草；3-超时自动退回上一步
        /// </summary>
        public int timeout_type { get; set; }

        /// <summary>
        /// 超时小时
        /// </summary>
        public decimal timeout { get; set; }

        /// <summary>
        /// 会签类型 1-全部通过 2-任一人通过 3-按比例
        /// </summary>
        public int confluence_type { get; set; }

        /// <summary>
        /// 会签通过比例
        /// </summary>
        public decimal confluence_rate { get; set; }

        public List<WFAuthorizeModel> authusers { get; set; }

        public List<WFFormModel> forms { get; set; }

        /// <summary>
        /// 通过之后执行的sql
        /// </summary>
        public string sqlsuccess { get; set; }

        /// <summary>
        /// 拒绝之后执行的sql
        /// </summary>
        public string sqlfail { get; set; }

        /// <summary>
        /// 执行程序所在的程序集
        /// </summary>
        public string injectassembly { get; set; }

        /// <summary>
        /// 执行的类，需继承 IWFNodeProcess 接口
        /// </summary>
        public string injectclass { get; set; }

        /// <summary>
        /// 条件节点 执行的sql select 1 from XXX
        /// </summary>
        public string sqlcondition { get; set; }
    }
}
