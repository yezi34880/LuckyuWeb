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
    /// 人员权限   /OrganizationModule/UserRelation
    /// </summary>
    [Area("OrganizationModule")]
    public class UserRelationController : MvcControllerBase
    {
        #region Var
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        #endregion

        #region Get
        /// <summary>
        /// 获取对应类型用户
        /// </summary>
        public IActionResult GetUsers(int relationType, string objectId)
        {
            var users = userrelationBLL.GetListByType(relationType, objectId);
            return Success(users);
        }
        /// <summary>
        /// 根据用户 类型 获取对应Id
        /// </summary>
        public IActionResult GetRelations(int relationType, string userId)
        {
            var relations = userrelationBLL.GetListByUser(relationType, userId);
            return Success(relations);
        }

        #endregion

        #region Set
        [HttpPost, AjaxOnly]
        public IActionResult SetUsers(int relationType, string objectId, List<string> userIds)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            userrelationBLL.SetRelationByObject(relationType, objectId, userIds, loginInfo);
            return Success();
        }
        [HttpPost, AjaxOnly]
        public IActionResult SetRelations(int relationType, string userId, List<string> objectIds)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            userrelationBLL.SetRelationByUser(relationType, userId, objectIds, loginInfo);
            return Success();
        }

        #endregion

    }
}
