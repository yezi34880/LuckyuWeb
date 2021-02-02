using Luckyu.App.Organization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luckyu.Utility;
using Mapster;

namespace Luckyu.App.System
{
    public class MessageBLL
    {
        #region Var
        private sys_nmessageService messageService = new sys_nmessageService();
        private UserBLL userBLL = new UserBLL();
        #endregion

        public JqgridPageResponse<sys_messageEntity> Page(JqgridPageRequest jqPage)
        {
            var page = messageService.Page(jqPage);
            return page;
        }

        public JqgridPageResponse<sys_messageEntity> ShowPage(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var page = messageService.ShowPage(jqPage, loginInfo);
            return page;
        }

        public List<sys_messageEntity> Send(sys_messageEntity entity, UserModel loginInfo)
        {
            var list = new List<sys_messageEntity>();
            var userIds = entity.to_userid.SplitNoEmpty(',');
            foreach (var userId in userIds)
            {
                var item = new sys_messageEntity();
                item = entity.Adapt<sys_messageEntity>();
                var user = userBLL.GetEntityByCache(r => r.user_id == userId) ?? new sys_userEntity();
                item.to_userid = userId;
                item.to_username = $"{user.realname}-{user.loginname}";
                list.Add(item);
            }
            messageService.Insert(list, loginInfo);
            return list;
        }

        public async Task Send(sys_messageEntity entity, UserModel loginInfo, IHubContext<MessageHub> hubContext)
        {
            var list = Send(entity, loginInfo);
            foreach (var item in list)
            {
                var user = userBLL.GetEntityByCache(r => r.user_id == item.to_userid);
                if (user != null)
                {
                    var res = new ResponseResult();
                    res.info = entity.contents;
                    res.data = "系统通知";
                    await SignalRHelper.SendMessageToUser(hubContext, user.loginname, res);
                }
            }
        }

    }
}
