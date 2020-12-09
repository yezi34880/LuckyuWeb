using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;


namespace Luckyu.Module.WorkflowModule.Controllers
{
    /// <summary>
    /// 流程监控    /WorkflowModule/Monitor
    /// </summary>
    [Area("WorkflowModule")]
    public class MonitorController : MvcControllerBase
    {
        #region Var
        private WFTaskBLL taskBLL = new WFTaskBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqPage, int is_finished)
        {
            var page = taskBLL.MonitorPage(jqPage, is_finished);
            return Json(page);
        }

        #endregion

        #region Interface
        [AjaxOnly, HttpPost]
        public IActionResult Finish(string instanceId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Finish(instanceId, loginInfo);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult Modify(string instanceId, string schemeId, string nodeId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Modify(instanceId, schemeId, nodeId, loginInfo);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult Complete(string instanceId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Complete(instanceId, loginInfo);
            return Json(res);
        }
        #endregion

    }
}
