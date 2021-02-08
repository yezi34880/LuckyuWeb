using Luckyu.App.OA;
using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.OAModule.Controllers
{
    /// <summary>
    /// 请假  /OAModule/Leave
    /// </summary>
    [Area("OAModule")]
    public class LeaveController : MvcControllerBase
    {
        #region Var
        private LeaveBLL leaveBLL = new LeaveBLL();

        private readonly IHubContext<MessageHub> _hubContext;
        public LeaveController(IHubContext<MessageHub> messageHubContext)
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
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = leaveBLL.Page(jqPage, loginInfo);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = leaveBLL.DeleteForm(keyValue, loginInfo);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        public async Task<IActionResult> Revoke(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = leaveBLL.Revoke(keyValue, loginInfo, _hubContext);
            return Json(res);
        }
        #endregion

        #region From
        public IActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var res = leaveBLL.GetFormData(keyValue);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForm(string keyValue, string strEntity, List<string> deleteAnnex, int isSubmit)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = await leaveBLL.SaveForm(keyValue, strEntity, deleteAnnex, isSubmit, loginInfo, _hubContext);
            return Json(res);
        }

        #endregion

    }
}
