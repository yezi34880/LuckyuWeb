using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luckyu.App.Workflow;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.OA
{
    /// <summary>
    /// 测试流程
    /// </summary>
    public class WFLeaveNode : IWFNodeProcess
    {
        public ResponseResult CheckApprove(string instanceId, string processId, int result, string opinion)
        {
            return ResponseResult.Success();
        }

        public void Approve(string instanceId, string processId, int result, string opinion, ref List<wf_taskEntity> listTask, ref List<wf_taskhistoryEntity> listHistory, ref List<string> listSql)
        {
            //var str = $"result {result}  opinion  {opinion}";
            //var logger = NLog.LogManager.GetCurrentClassLogger();
            //logger.Debug(str);
        }
    }
}
