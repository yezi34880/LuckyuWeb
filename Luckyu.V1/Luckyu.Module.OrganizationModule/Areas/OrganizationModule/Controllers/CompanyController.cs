using System.Collections.Generic;
using System.Linq;
using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 公司  /OrganizationModule/Company
    /// </summary>
    [Area("OrganizationModule")]
    public class CompanyController : MvcControllerBase
    {
        #region Var
        private CompanyBLL companyBLL = new CompanyBLL();

        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var page = companyBLL.Page(jqPage);
            return Json(page.ToTreeGrid(r => r.company_id, r => r.parent_id));
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            companyBLL.DeleteForm(keyValue, loginInfo);
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
            var entity = companyBLL.GetEntity(r => r.company_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var data = new
            {
                Company = entity,
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_companyEntity>();
            var allCompanys = companyBLL.GetAllByCache();
            var already = allCompanys.Exists(r => r.companycode == entity.companycode && r.company_id != keyValue);
            if (already)
            {
                return Fail("公司编码已存在，请重新输入!");
            }

            //公司全名不能重复
            already = allCompanys.Exists(r => r.fullname == entity.fullname && r.company_id != keyValue);
            if (already)
            {
                return Fail("公司名称已经存在,请重新输入!");
            }

            if (entity.parent_id == keyValue)
            {
                return Fail("上级公司不能选择自己");
            }
            if (IsParentContainMyselft(allCompanys, keyValue, entity.parent_id))
            {
                return Fail("上级部门中不能包含自己");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            companyBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }
        #endregion

        #region Select
        [HttpGet]
        public IActionResult CompanySelectForm()
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
        private bool IsParentContainMyselft(List<sys_companyEntity> allCompanys, string Id, string parentId)
        {
            if (Id.IsEmpty())
            {
                return false;
            }
            if (parentId.IsEmpty())
            {
                return false;
            }
            var parent = allCompanys.Where(x => x.company_id == parentId).FirstOrDefault();
            if (parent == null)
            {
                return false;
            }
            if (parent.company_id == Id)
            {
                return true;
            }
            else
            {
                return IsParentContainMyselft(allCompanys, Id, parent.parent_id);
            }
        }

        #endregion

        #region Interface
        /// <summary>
        /// 获取映射数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public IActionResult GetMap(string ver)
        {
            var data = companyBLL.GetModelMap();
            string md5 = EncrypHelper.MD5_Encryp(data.ToJson());
            if (md5 == ver)
            {
                return Fail("no update");
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

        public IActionResult GetTree()
        {
            var treeData = companyBLL.GetTree();
            if (treeData.IsEmpty())
                treeData = new List<eleTree>();

            treeData.Insert(0, new eleTree
            {
                id = "-1",
                label = "全部",
            });
            var data = new
            {
                code = 0,
                data = treeData
            };
            return Json(data);
        }
        public IActionResult GetSelectTree()
        {
            var treeData = companyBLL.GetSelectTree();
            return Json(treeData);
        }
        public IActionResult GetAllCompanyTree()
        {
            var treeData = companyBLL.GetTree();
            if (treeData.IsEmpty())
                treeData = new List<eleTree>();
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
