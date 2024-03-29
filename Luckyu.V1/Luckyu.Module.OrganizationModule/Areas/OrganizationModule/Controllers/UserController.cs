﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Luckyu.App.Organization;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Module.OrganizationModule.Controllers
{
    /// <summary>
    /// 用户    /OrganizationModule/User
    /// </summary>
    [Area("OrganizationModule")]
    public class UserController : MvcControllerBase
    {
        #region Var
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqPage, string organizationTag, string organizationId)
        {
            var page = userBLL.Page(jqPage, organizationTag, organizationId);
            return Json(page);
        }

        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = userBLL.GetEntity(r => r.user_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            userBLL.DeleteForm(entity, loginInfo);
            return Success();
        }

        [HttpPost, AjaxOnly]
        public IActionResult SetOnOff(string keyValue)
        {
            var entity = userBLL.GetEntity(r => r.user_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            userBLL.SetOnOff(entity, loginInfo);
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
            var entity = userBLL.GetEntity(r => r.user_id == keyValue);
            var data = new
            {
                User = entity
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_userEntity>();
            var already = userBLL.GetEntity(r => r.loginname == entity.loginname && r.user_id != keyValue);
            if (already != null)
            {
                return Fail("用户名已存在，请重新输入!");
            }

            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            userBLL.SaveForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }

        #endregion

        #region 修改密码（管理员，不用输入原密码）
        public ActionResult ModifyPwd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ModifyPassword(string keyValue, string password)
        {
            var entity = userBLL.GetEntity(r => r.user_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            entity.loginpassword = password;
            userBLL.ModifyPassword(entity);
            return Success("操作成功");
        }

        #endregion

        #region UserSelect
        [HttpGet]
        public IActionResult UserSelectForm()
        {
            return View();
        }
        [HttpGet, AjaxOnly]
        public IActionResult GetUserByCondition(string companyId, string departmentId, string postId, string roleId)
        {
            var list = new List<sys_userEntity>();
            list = userBLL.GetAllByCache(companyId);
            if (!departmentId.IsEmpty())
            {
                list = list.Where(r => r.department_id == departmentId).ToList();
            }
            if (!postId.IsEmpty())
            {
                var relations = userrelationBLL.GetListByType(UserRelationEnum.Post, postId);
                var userids = relations.Select(r => r.user_id).ToList();
                list = list.Where(r => userids.Contains(r.user_id)).ToList();
            }
            if (!roleId.IsEmpty())
            {
                var relations = userrelationBLL.GetListByType(UserRelationEnum.Role, roleId);
                var userids = relations.Select(r => r.user_id).ToList();
                list = list.Where(r => userids.Contains(r.user_id)).ToList();
            }

            return Success(list);
        }

        public IActionResult GetDepartmentUsers(string companyId)
        {
            var listUers = userBLL.GetAllByCache(companyId);
            var deptids = listUers.Select(r => r.department_id).Distinct().ToList();
            var listDept = deptBLL.GetAllByCache(companyId);
            listDept = listDept.Where(r => deptids.Contains(r.department_id)).ToList();

            var tree = new List<KeyValueList<sys_departmentEntity, sys_userEntity>>();
            foreach (var dept in listDept)
            {
                var d = new KeyValueList<sys_departmentEntity, sys_userEntity>();
                d.Key = dept;

                var users = listUers.Where(r => r.department_id == dept.department_id).ToList();
                d.ValueList = users;

                tree.Add(d);
            }
            return Success(tree);
        }
        #endregion

        #region Interface
        [HttpGet, AjaxOnly]
        public IActionResult GetMap(string ver)
        {
            var data = userBLL.GetModelMap();
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

        #endregion

    }
}
