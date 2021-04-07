using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.OA
{
    /// <summary>
    /// 测试流程
    /// </summary>
    public class WFLeaveNode : IWFNodeProcess
    {
        public void Approve(string instanceId, string processId, int result, string opinion)
        {
            //var str = $"result {result}  opinion  {opinion}";
            //var logger = NLog.LogManager.GetCurrentClassLogger();
            //logger.Debug(str);
        }

        public ResponseResult CheckApprove(string instanceId, string processId, int result, string opinion)
        {
            return ResponseResult.Success();
        }
    }
}
