using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
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
        /// <param name="result"></param>
        /// <param name="opinion"></param>
        public void Approve(string instanceId, string processId, int result, string opinion);
    }
}
