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
    /// 菜单  /OrganizationModule/Module
    /// </summary>
    [Area("OrganizationModule")]
    public class ModuleController : MvcControllerBase
    {
        #region Var
        private ModuleBLL moduleBLL = new ModuleBLL();

        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqpage)
        {
            var page = moduleBLL.Page(jqpage);
            return Json(page.ToTreeGrid(r => r.module_id, r => r.parent_id));
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = moduleBLL.GetEntity(r => r.module_id == keyValue);
            if (entity == null)
            {
                return Fail("该数据不存在");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            moduleBLL.DeleteForm(entity, loginInfo);
            return Success();
        }

        #endregion

        #region Form
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var entity = moduleBLL.GetEntity(r => r.module_id == keyValue);
            var data = new
            {
                Module = entity
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_moduleEntity>();
            var already = moduleBLL.GetList(r => r.modulecode == entity.modulecode && r.module_id != keyValue);
            if (already == null)
            {
                return Fail("岗位编码已存在，请重新输入!");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            moduleBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }
        #endregion

        #region Interface
        public IActionResult GetSelectTree()
        {
            var treeData = moduleBLL.GetSelectTree();
            return Json(treeData);
        }
        public IActionResult GetTree()
        {
            var treeData = moduleBLL.GetTree();
            var data = new
            {
                code = 0,
                data = treeData
            };
            return Json(data);
        }

        #endregion
    }
}
