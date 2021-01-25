using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class SignalRHelper
    {
        /// <summary>
        /// 发消息给所有人
        /// </summary>
        public static async Task SendMessageToAll(IHubContext<MessageHub> hubContext, ResponseResult res)
        {
            var result = JsonConvert.SerializeObject(res);
            await hubContext.Clients.All.SendAsync("ReceiveMessage", result);
        }

        /// <summary>
        /// 发消息给个人
        /// </summary>
        public static async Task SendMessageToUser(IHubContext<MessageHub> hubContext, string loginname, ResponseResult res)
        {
            var result = JsonConvert.SerializeObject(res);
            var connectionIds = LoginUserInfo.Instance.GetConnectionIdByLoginname(loginname);
            if (!connectionIds.IsEmpty())
            {
                foreach (var connection_id in connectionIds)
                {
                    await hubContext.Clients.Client(connection_id).SendAsync("ReceiveMessage", result);
                }
            }
        }

    }
}
