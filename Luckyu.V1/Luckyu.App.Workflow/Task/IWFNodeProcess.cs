using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.App.Workflow
{
    public interface IWFNodeProcess
    {
        /// <summary>
        /// 审批前验证
        /// </summary>
        public ResponseResult CheckApprove(string instanceId, string processId, int result, string opinion);

        /// <summary>
        /// 审批后执行
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="processId"></param>
        /// <param name="result"></param>
        /// <param name="opinion"></param>
        /// <param name="listTask"></param>
        /// <param name="listHistory"></param>
        /// <param name="listSql"></param>
        public void Approve(string instanceId, string processId, int result, string opinion,ref List<wf_taskEntity> listTask, ref List<wf_taskhistoryEntity> listHistory, ref List<string> listSql);
    }

}
