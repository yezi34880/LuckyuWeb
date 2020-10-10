using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 权限验证  /OrganizationModule/Authorize
    /// </summary>
    [Area("OrganizationModule")]
    public class AuthorizeController : MvcControllerBase
    {
        #region Var
        private AuthorizeBLL authorizeBLL = new AuthorizeBLL();
        private ModuleBLL moduleBLL = new ModuleBLL();
        #endregion

        #region Form
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }

        [HttpGet, AjaxOnly]
        public IActionResult GetFormData(string objectId)
        {
            var list = authorizeBLL.GetList(r => r.object_id == objectId);
            var allModule = moduleBLL.GetAllByCache();
            for (var i = list.Count - 1; i > -1; i--)
            {
                var item = list[i];
                if (item.itemtype == 1)
                {
                    // 菜单只保留叶子节点，否则选中父节点子节点会全选
                    var mod = allModule.Where(r => r.module_id == item.item_id).FirstOrDefault();
                    if (mod != null)
                    {
                        if (allModule.Exists(r => r.parent_id == mod.module_id))
                        {
                            list.RemoveAt(i);
                        }
                    }
                }
            }
            return Success(list);
        }

        [HttpPost, AjaxOnly]
        public IActionResult SaveForm(int objectType, string objectId, List<string> modules)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            authorizeBLL.SaveForm(objectType, objectId, modules, loginInfo);
            return Success("操作成功");
        }
        #endregion

    }
}
