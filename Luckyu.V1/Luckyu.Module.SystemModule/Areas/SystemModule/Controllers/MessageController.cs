using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.SystemModule.Controllers
{
    /// <summary>
    /// 消息  /SystemModule/Message
    /// </summary>
    [Area("SystemModule")]
    public class MessageController : MvcControllerBase
    {
        #region Var
        private MessageBLL messageBLL = new MessageBLL();

        private readonly Microsoft.AspNetCore.SignalR.IHubContext<MessageHub> _messageHubContext;
        public MessageController(Microsoft.AspNetCore.SignalR.IHubContext<MessageHub> messageHubContext)
        {
            _messageHubContext = messageHubContext;
        }

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var page = messageBLL.Page(jqPage);
            return Json(page);
        }
        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForm(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var entity = strEntity.ToObject<sys_messageEntity>();
            await messageBLL.Send(entity, loginInfo, _messageHubContext);
            return Success();
        }
        #endregion

        #region ShowIndex
        public IActionResult ShowIndex()
        {
            return View();
        }
        public IActionResult ShowPage(JqgridPageRequest jqPage)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = messageBLL.ShowPage(jqPage, loginInfo);
            return Json(page);
        }

        #endregion

    }
}
