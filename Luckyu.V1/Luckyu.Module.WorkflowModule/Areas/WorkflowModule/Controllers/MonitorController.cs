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

        #region 调整流程位置
        public IActionResult ModifyForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetTaskScheme(string instanceId)
        {
            var res = taskBLL.GetTaskScheme(instanceId);
            return Json(res);
        }

        public IActionResult GetHelpMePage(JqgridPageRequest jqPage, string instanceId)
        {
            var res = taskBLL.GetHelpMePage(jqPage, instanceId);
            return Json(res);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public IActionResult DeleteTask(string taskId)
        {
            var res = taskBLL.DeleteTask(taskId);
            return Json(res);
        }

        public IActionResult GetAddUserPage(JqgridPageRequest jqPage, string instanceId)
        {
            var res = taskBLL.GetAddUserPage(jqPage, instanceId);
            return Json(res);
        }
        /// <summary>
        /// 删除任务下用户
        /// </summary>
        public IActionResult DeleteTaskAuth(string authId)
        {
            var res = taskBLL.DeleteTaskAuth(authId);
            return Json(res);
        }

        /// <summary>
        /// 删除审批历史
        /// </summary>
        /// <param name="authId"></param>
        /// <returns></returns>
        public IActionResult DeleteHistory(string historyId)
        {
            var res = taskBLL.DeleteHistory(historyId);
            return Json(res);
        }
        #endregion

        #region Interface
        [AjaxOnly, HttpPost]
        public IActionResult Sleep(string instanceId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Sleep(instanceId, loginInfo);
            return Json(res);
        }

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
        #endregion

    }
}
