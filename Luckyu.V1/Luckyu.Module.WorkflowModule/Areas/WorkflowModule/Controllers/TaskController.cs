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
    /// 任务  /WorkflowModule/Task
    /// </summary>
    [Area("WorkflowModule")]
    public class TaskController : MvcControllerBase
    {
        #region Var
        private WFTaskBLL taskBLL = new WFTaskBLL();
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqPage, int tasktype)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = taskBLL.Page(jqPage, tasktype, loginInfo);
            return Json(page);
        }
        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetFormData(string instanceId, string taskId, string historyId)
        {
            var instance = taskBLL.GetInstanceEnttity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return Fail("该数据不存在");
            }
            var scheme = instance.schemejson.ToObject<WFSchemeModel>();
            // 展示节点 如 自己提交 就是startround 已办历史 就是自己批的那个
            WFSchemeNodeModel showNode = new WFSchemeNodeModel();
            // 当前待批节点
            WFSchemeNodeModel currentNode = new WFSchemeNodeModel();
            if (!historyId.IsEmpty())
            {
                // 已办历史
                var history = taskBLL.GetHistoryEnttity(r => r.history_id == historyId);
                if (history == null)
                {
                    return Fail("该数据不存在");
                }
                showNode = scheme.nodes.Where(r => r.id == history.node_id).FirstOrDefault();

                var task = taskBLL.GetTaskEnttity(r => r.instance_id == instance.instance_id && r.is_done == 0);
                if (task != null)
                {
                    currentNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                }
            }
            else if (!taskId.IsEmpty())
            {
                // 待办
                var task = taskBLL.GetTaskEnttity(r => r.task_id == taskId);
                if (task == null)
                {
                    return Fail("该数据不存在");
                }
                showNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                currentNode = showNode;
            }
            else
            {
                // 自己发起的
                showNode = scheme.nodes.Where(r => r.type == "startround").FirstOrDefault();
                var task = taskBLL.GetTaskEnttity(r => r.instance_id == instance.instance_id && r.is_done == 0);
                if (task != null)
                {
                    currentNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                }

            }
            var historys = taskBLL.GetHistoryTaskList(instance.process_id, instance.instance_id);
            var dic = new Dictionary<string, object>
            {
                {"Instance",instance },
                {"ShowNode",showNode },
                {"CurrentNode",currentNode },
                {"Scheme",  instance.schemejson },
                { "History",historys}
            };
            return Success(dic);
        }

        [HttpGet]
        public IActionResult GetTaskLog(string processId, string instanceId)
        {
            var historys = taskBLL.GetHistoryTaskList(processId, instanceId);
            return Json(historys);
        }
        #endregion

        #region 对话框
        public IActionResult ApproveForm()
        {
            return View();
        }
        #endregion

        #region Interface
        [AjaxOnly, HttpPost]
        public IActionResult Create(string flowCode, string processId, string processName)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Create(flowCode, processId, processName, loginInfo);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult Approve(string taskId, int result, string opinion)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.Approve(taskId, result, opinion, loginInfo);
            return Json(res);
        }

        [AjaxOnly, HttpPost]
        public IActionResult AddUser(string taskId, string userId)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = taskBLL.AddUser(taskId, userId, loginInfo);
            return Json(res);
        }
        #endregion

    }
}
