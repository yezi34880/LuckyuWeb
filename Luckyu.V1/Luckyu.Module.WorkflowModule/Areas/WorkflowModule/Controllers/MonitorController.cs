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

        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var page = taskBLL.MonitorPage(jqPage);
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

        #endregion

    }
}
