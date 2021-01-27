using System.Collections.Generic;
using System.Linq;
using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 部门  /OrganizationModule/Department
    /// </summary>
    [Area("OrganizationModule")]
    public class DepartmentController : MvcControllerBase
    {
        #region Var
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private UserBLL userBLL = new UserBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page(string companyId, JqgridPageRequest jqpage)
        {
            var page = deptBLL.Page(companyId, jqpage);
            return Json(page.ToTreeGrid(r => r.department_id, r => r.parent_id));
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = deptBLL.GetEntity(r => r.department_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var user = userBLL.GetEntity(r => r.department_id == entity.department_id && r.is_delete == 0 && r.is_enable == 1);
            if (!user.IsEmpty())
            {
                return Fail("该部门下还有人员，不能删除");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            deptBLL.DeleteForm(entity, loginInfo);
            return Success();
        }
        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var entity = deptBLL.GetEntity(r => r.department_id == keyValue);
            var data = new
            {
                Department = entity
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var deptEntity = strEntity.ToObject<sys_departmentEntity>();
            var allDepartments = deptBLL.GetList(r => r.is_delete == 0 && r.company_id == deptEntity.company_id);
            var already = allDepartments.Exists(r => r.departmentcode == deptEntity.departmentcode && r.department_id != keyValue);
            if (already)
            {
                return Fail("部门编码已存在，请重新输入!");
            }

            //部门全名不能重复
            already = allDepartments.Exists(r => r.fullname == deptEntity.fullname && r.department_id != keyValue);
            if (already)
            {
                return Fail("部门名称已经存在,请重新输入!");
            }
            if (deptEntity.parent_id == keyValue)
            {
                return Fail("上级部门不能选择自己");
            }
            if (IsParentContainMyselft(allDepartments, keyValue, deptEntity.parent_id))
            {
                return Fail("上级部门中不能包含自己");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            deptBLL.SaveForm(keyValue, deptEntity, strEntity, loginInfo);
            return Success(deptEntity);
        }

        #endregion

        #region Select
        [HttpGet]
        public IActionResult DepartmentSelectForm()
        {
            return View();
        }

        #endregion

        #region Private
        /// <summary>
        /// 递归判断 上级节点中有么有包含自己本身（避免死循环）
        /// </summary>
        /// <param name="Id">自己Id</param>
        /// <param name="parentId">父Id</param>
        /// <returns>true：包含自身；false：不包含</returns>
        private bool IsParentContainMyselft(List<sys_departmentEntity> allDepartments, string Id, string parentId)
        {
            if (Id.IsEmpty())
            {
                return false;
            }
            if (parentId.IsEmpty())
            {
                return false;
            }
            var parent = allDepartments.Where(x => x.department_id == parentId).FirstOrDefault();
            if (parent == null)
            {
                return false;
            }
            if (parent.department_id == Id)
            {
                return true;
            }
            else
            {
                return IsParentContainMyselft(allDepartments, Id, parent.parent_id);
            }
        }
        #endregion

        #region 数据接口 
        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public IActionResult GetMap(string ver)
        {
            var data = deptBLL.GetModelMap();
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

        public IActionResult GetTree(string companyId, bool multiple = false)
        {
            var treeData = deptBLL.GetAllTree(companyId, multiple);
            if (multiple)
            {
                treeData.Insert(0, new eleTree
                {
                    id = "-1",
                    label = "全部",
                    ext = new Dictionary<string, string> { { "tag", "all" } }
                });
            }
            var data = new
            {
                code = 0,
                data = treeData,
            };
            return Json(data);
        }

        public IActionResult GetSelectTree(string companyId)
        {
            var treeData = deptBLL.GetSelectTree(companyId);
            return Json(treeData);
        }


        #endregion

    }
}
