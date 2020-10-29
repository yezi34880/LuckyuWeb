using System.Collections.Generic;
using System.Linq;
using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 数据权限  /OrganizationModule/DataAuthorize
    /// </summary>
    [Area("OrganizationModule")]
    public class DataAuthorizeController : MvcControllerBase
    {
        #region Var
        private DataAuthorizeBLL dataBLL = new DataAuthorizeBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var page = dataBLL.Page(jqPage);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = dataBLL.GetEntity(r => r.auth_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            dataBLL.DeleteForm(entity, loginInfo);
            return Success();
        }

        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        public IActionResult GetFormData(string keyValue)
        {
            var entity = dataBLL.GetEntity(r => r.auth_id == keyValue);
            if (entity != null)
            {
                var list = dataBLL.GetDetailList(r => r.auth_id == keyValue);
                if (!list.IsEmpty())
                {
                    entity.seeobjecttype = list[0].objecttype;
                    entity.seeobject_ids = string.Join(",", list.Select(r => r.object_id));
                    entity.seeobjectnames = string.Join(",", list.Select(r => r.objectname));
                }
            }
            var dic = new Dictionary<string, object>
            {
                {"DataAuthorize",entity }
            };
            return Success(dic);
        }
        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_dataauthorizeEntity>();
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            dataBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }
        #endregion


    }
}
