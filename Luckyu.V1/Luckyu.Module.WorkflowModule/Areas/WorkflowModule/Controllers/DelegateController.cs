using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.WorkflowModule.Controllers
{
    /// <summary>
    /// 任务委托  /WorkflowModule/Delegate
    /// </summary>
    [Area("WorkflowModule")]
    public class DelegateController : MvcControllerBase
    {
        #region Var
        private WFDelegateBLL delegateBLL = new WFDelegateBLL();
        private WorkflowBLL flowBLL = new WorkflowBLL();
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = delegateBLL.Page(jqPage, loginInfo);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = delegateBLL.DeleteForm(keyValue, loginInfo);
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
            var res = delegateBLL.GetFormData(keyValue);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = delegateBLL.SaveForm(keyValue, strEntity, loginInfo);
            return Json(res);
        }

        #endregion

        #region Interface
        public IActionResult GetFlow()
        {
            var allflow = flowBLL.GetList(r => r.is_delete == 0 && r.is_enable == 1);
            var select = allflow.Select(r => new
            {
                name = r.flowname,
                value = r.flowcode
            }).ToList();
            //select.Insert(0, new { name = "全部", value = "ALL" });
            return Success(select);
        }
        #endregion

    }
}
