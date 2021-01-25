using Luckyu.App.Organization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpcontext = Context.GetHttpContext();
            //var loginInfo = LoginUserInfo.Instance.GetLoginUser(httpcontext);
            LoginUserInfo.Instance.AddSingalIRConnection(httpcontext, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpcontext = Context.GetHttpContext();
            LoginUserInfo.Instance.RemoveSingalIRConnection(httpcontext, Context.ConnectionId);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
