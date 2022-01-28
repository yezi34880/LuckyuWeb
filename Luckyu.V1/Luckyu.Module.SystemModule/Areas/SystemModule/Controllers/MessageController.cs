using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        private readonly IHubContext<MessageHub> _hubContext;
        public MessageController(IHubContext<MessageHub> messageHubContext)
        {
            _hubContext = messageHubContext;
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
            await messageBLL.Send(entity, loginInfo, _hubContext);
            return Success();
        }
        #endregion

        #region ShowIndex
        public IActionResult GetNotReadCount()
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var count = messageBLL.GetNotReadCount(loginInfo);
            return Success(count);
        }

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
