using Luckyu.App.Organization;
using Luckyu.App.System;
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
    /// 系统配置    /SystemModule/Config
    /// </summary>
    [Area("SystemModule")]
    public class ConfigController : MvcControllerBase
    {
        #region Var
        private ConfigBLL configBLL = new ConfigBLL();

        #endregion

        #region Index

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Page(JqgridPageRequest jqpage)
        {
            var page = configBLL.Page(jqpage);
            return Json(page);
        }
        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = configBLL.DeleteForm(keyValue, loginInfo);
            return Json(res);
        }

        #endregion

        #region Form

        public IActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var res = configBLL.GetFormData(keyValue);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = configBLL.SaveForm(keyValue, strEntity, loginInfo);
            return Json(res);
        }

        #endregion

        #region Interface

        #endregion

    }
}
