using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 部门分管 /OrganizationModule/DepartmentManage
    /// </summary>
    [Area("OrganizationModule")]
    public class DepartmentManageController : MvcControllerBase
    {
        #region Var
        private DepartmentManageBLL manageBLL = new DepartmentManageBLL();

        #endregion

        #region Form
        public IActionResult ManageForm()
        {
            return View();
        }

        public IActionResult GetFormData(string userId)
        {
            var dic = manageBLL.GetFormData(userId);
            return Success(dic);
        }

        public IActionResult SaveForm(string userId, string strList)
        {
            var list = strList.ToObject<List<sys_departmentmanageEntity>>();

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            manageBLL.SaveForm(userId, list, loginInfo);
            return Success();
        }
        #endregion

    }
}
