using DeviceDetectorNET;
using Luckyu.Cache;
using Luckyu.Utility;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luckyu.App.Organization
{
    public class LoginUserInfo
    {
        #region var
        /// <summary>
        /// 秘钥
        /// </summary>
        private string cookieKeyToken = "luckyu_login_token";

        private UserBLL userBLL = new UserBLL();
        private CompanyBLL companyBLL = new CompanyBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private UserRelationBLL relationBLL = new UserRelationBLL();
        #endregion

        #region cache
        private string cacheKeyToken = CacheFactory.GetCurrentDomain() + "luckyu_login_token";
        private ICache cache = CacheFactory.Create();

        #endregion

        public static LoginUserInfo Instance
        {
            get
            {
                return new LoginUserInfo();
            }
        }

        public void SetLogin(string loginname, HttpContext httpContext, string appId)
        {
            var loginInfo = new LoginInfo
            {
                appId = appId,
                loginname = loginname,
                loginTime = DateTime.Now,
                ip = httpContext.GetRequestIp(),
            };
            var info = DeviceDetector.GetInfoFromUserAgent(httpContext.Request.Headers["User-Agent"].ToString());
            if (info != null)
            {
                loginInfo.browser = (info.Match.Client == null ? "" : $"{info.Match.Client.Type}: {info.Match.Client.Name} {info.Match.Client.Version} ");
                loginInfo.device = (info.Match.Os == null ? "" : $" os: {info.Match.Os.Name} {info.Match.Os.Version} {info.Match.Os.Platform}");
            }

            // 写入 Cookie
            string cookie = httpContext.GetCookie(cookieKeyToken);
            if (cookie.IsEmpty())
            {
                loginInfo.token = Guid.NewGuid().ToString();
                httpContext.SetCookie(cookieKeyToken, loginInfo.token);
            }
            else
            {
                loginInfo.token = cookie;
            }

            // 缓存 登录信息
            var loginList = cache.Read<List<LoginInfo>>(cacheKeyToken + loginname);
            if (loginList == null)// 此账号第一次登录
            {
                loginList = new List<LoginInfo>() { loginInfo };
            }
            else // 不是第一次登录 ,记录多台设备
            {
                var that = loginList.Where(r => r.token == loginInfo.token).FirstOrDefault();
                if (that != null)
                {
                    that = loginInfo;
                }
                else
                {
                    loginList.Add(loginInfo);
                }
            }
            cache.Write(cacheKeyToken + loginname, loginList);

            // 缓存帐号信息
            var token = cache.Read<string>(cacheKeyToken + loginInfo.token);
            if (token.IsEmpty())
            {
                cache.Write(cacheKeyToken + loginInfo.token, loginInfo.loginname);
            }
        }

        public bool IsOnLine(HttpContext httpContext)
        {
            try
            {
                string token = httpContext.GetCookie(cookieKeyToken);
                if (token.IsEmpty())
                {
                    return false;
                }
                var loginname = cache.Read<string>(cacheKeyToken + token);
                if (loginname.IsEmpty())
                {
                    return false;
                }
                var loginList = cache.Read<List<LoginInfo>>(cacheKeyToken + loginname);
                if (loginList.IsEmpty())
                {
                    return false;
                }
                var thislogin = loginList.Where(r => r.token == token).FirstOrDefault();
                if (thislogin == null)
                {
                    return false;
                }
                TimeSpan span = DateTime.Now - thislogin.loginTime;
                if (span.TotalHours >= 24)   // 登录操作过24小时移除
                {
                    loginList.Remove(thislogin);
                    cache.Write(cacheKeyToken + thislogin.loginname, loginList);
                    return false;
                }
                else
                {
                    UserModel userInfo = new UserModel
                    {
                        loginname = thislogin.loginname,
                    };

                    var user = userBLL.GetEntityByLoginName(userInfo.loginname);
                    if (user != null)
                    {
                        userInfo = user.Adapt<UserModel>();

                        var relations = relationBLL.GetListByUser(user.user_id);
                        userInfo.post_ids = relations.Where(r => r.relationtype == (int)UserRelationType.Post).Select(r => r.object_id).ToList();
                        userInfo.role_ids = relations.Where(r => r.relationtype == (int)UserRelationType.Role).Select(r => r.object_id).ToList();
                        userInfo.group_ids = relations.Where(r => r.relationtype == (int)UserRelationType.Group).Select(r => r.object_id).ToList();
                        userInfo.manage_dept_ids = relations.Where(r => r.relationtype == (int)UserRelationType.DeptManager).Select(r => r.object_id).ToList();

                        var company = companyBLL.GetEntityByCache(r => r.company_id == user.company_id);
                        userInfo.companyname = company.IsEmpty() ? "未设公司" : (company.shortname.Trim().IsEmpty() ? company.fullname : company.shortname);

                        var companyid = company.IsEmpty() ? "" : company.company_id;
                        var dept = deptBLL.GetEntityByCache(r => r.department_id == user.department_id, companyid);
                        userInfo.departmentname = dept.IsEmpty() ? "未设部门" : (dept.shortname.Trim().IsEmpty() ? dept.fullname : dept.shortname);

                        if (httpContext != null && !httpContext.Items.ContainsKey("LoginUserInfo"))
                        {
                            httpContext.Items.Add("LoginUserInfo", userInfo);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Logout(HttpContext httpContext)
        {
            string token = httpContext.GetCookie(cookieKeyToken);
            httpContext.Session.Clear();
            httpContext.RemoveAllCookie();
            if (!token.IsEmpty())
            {
                var loginname = cache.Read<string>(cacheKeyToken + token);
                if (!loginname.IsEmpty())
                {
                    var loginList = cache.Read<List<LoginInfo>>(cacheKeyToken + loginname);
                    var thislogin = loginList.Where(r => r.token == token).FirstOrDefault();
                    loginList.Remove(thislogin);
                    cache.Write(cacheKeyToken + thislogin.loginname, loginList);
                }
            }
        }

        public UserModel GetLoginUser(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }
            if (httpContext.Items["LoginUserInfo"] != null)
            {
                return (UserModel)httpContext.Items["LoginUserInfo"];
            }
            else
            {
                return null;
            }
        }

        public bool UpdateLoginUser(HttpContext httpContext, UserModel updateLoginInfo)
        {
            if (httpContext == null)
            {
                return false;
            }
            if (httpContext.Items["LoginUserInfo"] == null)
            {
                return false;
            }
            else
            {
                //var loginInfo = (UserModel)httpContext.Items["LoginUserInfo"];
                var loginInfo = updateLoginInfo.Adapt<UserModel>();
                httpContext.Items["LoginUserInfo"] = loginInfo;
                return true;
            }
        }

        public void AddSingalIRConnection(HttpContext httpContext, string connectionId)
        {
            var loginInfo = GetLoginUser(httpContext);
            var loginList = cache.Read<List<LoginInfo>>(cacheKeyToken + loginInfo.loginname);
            if (!loginList.Exists(r => r.connection_id == connectionId))
            {

            }
        }

        public void RemoveSingalIRConnection(HttpContext httpContext, string connectionId)
        {
            var loginInfo = GetLoginUser(httpContext);
            var loginList = cache.Read<List<LoginInfo>>(cacheKeyToken + loginInfo.loginname);
            var that = loginList.Where(r => r.connection_id == connectionId).FirstOrDefault();
            if (that != null)
            {
                that.connection_id = "";
                cache.Remove(cacheKeyToken + loginInfo.loginname);
                cache.Write(cacheKeyToken + loginInfo.loginname, loginList);
            }
        }

    }
}
