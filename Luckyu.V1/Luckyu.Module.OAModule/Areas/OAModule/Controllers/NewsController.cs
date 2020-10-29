using Luckyu.App.OA;
using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Luckyu.Module.OAModule.Controllers
{
    /// <summary>
    /// 请假  /OAModule/News
    /// </summary>
    [Area("OAModule")]
    public class NewsController : MvcControllerBase
    {
        #region Var
        private NewsBLL newsBLL = new NewsBLL();
        private WFTaskBLL taskBLL = new WFTaskBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = newsBLL.Page(jqPage, loginInfo);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = newsBLL.DeleteForm(keyValue, loginInfo);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        public IActionResult Publish(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = newsBLL.Publish(keyValue, loginInfo);
            return Json(res);
        }
        [HttpPost, AjaxOnly]
        public IActionResult SetTop(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = newsBLL.SetTop(keyValue, loginInfo);
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
            var res = newsBLL.GetFormData(keyValue);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = newsBLL.SaveForm(keyValue, strEntity, loginInfo);
            return Json(res);
        }

        #endregion

        #region Show
        public IActionResult ShowIndex()
        {
            return View();
        }
        public IActionResult ShowPage(JqgridPageRequest jqPage)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = newsBLL.ShowPage(jqPage, loginInfo);
            return Json(page);
        }
        public IActionResult Show(string keyValue)
        {
            var news = newsBLL.GetEntity(r => r.id == keyValue) ?? new oa_newsEntity();
            return View(news);
        }
        #endregion

        #region Home 展示
        public IActionResult HomeShow()
        {
            var jqPage = new JqgridPageRequest
            {
                page = 1,
                rows = 5,
                sidx = "createtime",
                sord = "DESC"
            };
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = newsBLL.ShowPage(jqPage, loginInfo);
            var dic = new Dictionary<string, object>();
            dic.Add("News", page.rows);
            return Success(dic);
        }
        #endregion
    }
}
