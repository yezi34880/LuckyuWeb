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
    /// 约车  /OAModule/CarOrder
    /// </summary>
    [Area("OAModule")]
    public class CarOrderController : MvcControllerBase
    {
        #region Var
        private CarOrderBLL carorderBLL = new CarOrderBLL();

        private readonly IHubContext<MessageHub> _hubContext;
        public CarOrderController(IHubContext<MessageHub> messageHubContext)
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
            var page = carorderBLL.Page(jqPage, loginInfo);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = carorderBLL.DeleteForm(keyValue, loginInfo);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        public async Task<IActionResult> Revoke(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = carorderBLL.Revoke(keyValue, loginInfo, _hubContext);
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
            var res = carorderBLL.GetFormData(keyValue);
            return Json(res);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForm(string keyValue, string strEntity, List<string> deleteAnnex, int isSubmit)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = await carorderBLL.SaveForm(keyValue, strEntity, deleteAnnex, isSubmit, loginInfo, _hubContext);
            return Json(res);
        }

        #endregion

        #region ApproveForm 
        // 审批展示页面 由于审批时需要编辑 则 单独起一个页面 且页面前台save方法必须为同步方法，返回true/false以说明保存是否成功，后台单独save方法以确保验证
        public IActionResult ApproveForm()
        {
            return View();
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveSave(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = carorderBLL.ApproveSave(keyValue, strEntity, loginInfo);
            return Json(res);
        }
        #endregion

    }

}
