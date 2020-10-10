using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.OA
{
    public class WFLeaveNode : IWFNodeProcess
    {
        public void Approve(int result, string opinion)
        {
            var str = $"result {result}  opinion  {opinion}";
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Debug(str);
        }
    }
}
