using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 小组  /OrganizationModule/Group
    /// </summary>
    [Area("OrganizationModule")]
    public class GroupController : MvcControllerBase
    {
        #region Var
        private GroupBLL groupBLL = new GroupBLL();
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
            var page = groupBLL.Page(jqpage);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = groupBLL.GetEntity(r => r.group_id == keyValue);
            if (entity == null)
            {
                return Fail("该数据不存在");
            }
            //var user = userBLL.GetEntity(r => r.group_id == entity.group_id && r.is_delete == 0 && r.is_enable == 1);
            //if (!user.IsEmpty())
            //{
            //    return Fail("该部门下还有人员，不能删除");
            //}

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            groupBLL.DeleteForm(entity, loginInfo);
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
            var entity = groupBLL.GetEntity(r => r.group_id == keyValue);
            var data = new
            {
                Group = entity
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_groupEntity>();
            var already = groupBLL.GetList(r => r.groupcode == entity.groupcode && r.group_id != keyValue);
            if (already == null)
            {
                return Fail("岗位编码已存在，请重新输入!");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            groupBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }

        #endregion

        #region Select
        [HttpGet]
        public IActionResult GroupSelectForm()
        {
            return View();
        }

        [HttpGet, AjaxOnly]
        public IActionResult GetGroupList(JqgridPageRequest jqPage)
        {
            var list = groupBLL.GetSelect(jqPage);
            return Json(list);
        }
        #endregion

        #region  Interface
        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public IActionResult GetMap(string ver)
        {
            var data = groupBLL.GetModelMap();
            string md5 = EncrypHelper.MD5_Encryp(data.ToJson());
            if (md5 == ver)
            {
                return Success("no update");
            }
            else
            {
                var jsondata = new
                {
                    data = data,
                    ver = md5
                };
                return Success(jsondata);
            }
        }

        #endregion

    }
}
