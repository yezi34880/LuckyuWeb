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
    /// 单据编码规则  /SystemModule/CodeRule
    /// </summary>
    [Area("SystemModule")]
    public class CodeRuleController : MvcControllerBase
    {
        #region Var
        private CodeRuleBLL codeBLL = new CodeRuleBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqpage)
        {
            var page = codeBLL.Page(jqpage);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = codeBLL.DeleteForm(keyValue, loginInfo);
            return Json(res);
        }

        #endregion

        #region From
        public IActionResult Form()
        {
            return View();
        }
        public IActionResult RuleJsonForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var res = codeBLL.GetFormData(keyValue);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = codeBLL.SaveForm(keyValue, strEntity, loginInfo);
            return Json(res);
        }
        #endregion
    }
}
