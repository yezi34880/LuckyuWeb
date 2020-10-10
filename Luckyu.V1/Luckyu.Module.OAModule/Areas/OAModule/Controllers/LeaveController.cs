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
            var page = leaveBLL.Page(jqPage, loginInfo);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = leaveBLL.GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return Fail("该数据不存在");
            }
            if (entity.state != (int)StateEnum.Draft)
            {
                return Fail("只有起草状态才能删除");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            leaveBLL.DeleteForm(entity, loginInfo);
            return Success();
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
            var entity = leaveBLL.GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return Fail("该数据不存在");
            }
            var dic = new Dictionary<string, object>
            {
                {"Leave",entity }
            };
            return Success(dic);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity, bool isSubmit)
        {
            var entity = strEntity.ToObject<oa_leaveEntity>();
            if (!keyValue.IsEmpty())
            {
                var old = leaveBLL.GetEntity(r => r.id == keyValue);
                if (old == null)
                {
                    return Fail("该数据不存在");
                }
                if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                {
                    return Fail("只有起草状态才能编辑");
                }
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            leaveBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            if (isSubmit)
            {
                taskBLL.Create("Leave", entity.id, $"请假申请 {entity.id}", loginInfo);
            }
            return Success(entity);
        }

        #endregion

    }
}
