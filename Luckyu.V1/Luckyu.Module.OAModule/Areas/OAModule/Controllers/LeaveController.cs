using Luckyu.App.OA;
using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SaveForm(string keyValue, string strEntity, int isSubmit)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = leaveBLL.SaveForm(keyValue, strEntity, isSubmit, loginInfo);
            return Json(res);
        }

        #endregion

    }
}
