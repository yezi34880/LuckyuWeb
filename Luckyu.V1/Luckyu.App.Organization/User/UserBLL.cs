﻿using Luckyu.Cache;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class UserBLL
    {
        #region Var
        private sys_userService userService = new sys_userService();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private DepartmentManageBLL manageBLL = new DepartmentManageBLL();
        private PostBLL postBLL = new PostBLL();
        private GroupBLL groupBLL = new GroupBLL();
        private RoleBLL roleBLL = new RoleBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.CachePrefix() + "user_";
        private string cacheKeyLoginname = CacheFactory.CachePrefix() + "user_loginname_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_userEntity> Page(JqgridPageRequest jqPage, string organizationTag, string organizationId)
        {
            var page = userService.Page(jqPage, organizationTag, organizationId);
            if (!page.rows.IsEmpty())
            {
                var rolws = roleBLL.GetAllByCache();
                var posts = postBLL.GetAllByCache();
                var groups = groupBLL.GetAllByCache();
                var depts = deptBLL.GetAllByCache();
                var allmanages = manageBLL.GetAllByCache();
                foreach (var row in page.rows)
                {
                    var relations = userrelationBLL.GetListByUser(row.user_id);
                    row.np_roles = string.Join(",", rolws.Where(t => relations.Where(r => r.relationtype == (int)UserRelationEnum.Role).Select(r => r.object_id).Contains(t.role_id)).Select(t => t.rolename));
                    row.np_posts = string.Join(",", posts.Where(t => relations.Where(r => r.relationtype == (int)UserRelationEnum.Post).Select(r => r.object_id).Contains(t.post_id)).Select(t => t.postname));
                    row.np_groups = string.Join(",", groups.Where(t => relations.Where(r => r.relationtype == (int)UserRelationEnum.Group).Select(r => r.object_id).Contains(t.group_id)).Select(t => t.groupname));
                    row.np_depts = string.Join(",", depts.Where(t => allmanages.Where(r => r.user_id == row.user_id).Select(r => r.department_id).Contains(t.department_id)).Select(t => t.fullname));
                }
            }
            return page;
        }

        public sys_userEntity GetEntity(Expression<Func<sys_userEntity, bool>> condition, Expression<Func<sys_userEntity, object>> orderExp = null, bool isDesc = false)
        {
            var entity = userService.GetEntity(condition, orderExp, isDesc);
            return entity;
        }
        public List<sys_userEntity> GetList(Expression<Func<sys_userEntity, bool>> condition, Expression<Func<sys_userEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = userService.GetList(condition, orderExp, isDesc);
            return list;
        }
        public List<sys_userEntity> GetAllByCache(string companyId = "")
        {
            var list = cache.Read<List<sys_userEntity>>(cacheKey + companyId);
            if (list.IsEmpty())
            {
                if (companyId.IsEmpty() || companyId == "-1" || companyId.ToLower() == "all")
                {
                    list = GetList(r => r.is_delete == 0, r => r.sort);
                }
                else
                {
                    list = GetList(r => r.is_delete == 0 && r.company_id == companyId);
                }
                cache.Write(cacheKey + companyId, list);
            }
            return list;
        }
        public sys_userEntity GetEntityByCache(Func<sys_userEntity, bool> condition)
        {
            var list = GetAllByCache();
            var entity = list.Where(condition).FirstOrDefault();
            return entity;
        }

        public sys_userEntity GetEntityByLoginName(string loginname)
        {
            var entity = cache.Read<sys_userEntity>(cacheKeyLoginname + loginname);
            if (entity == null)
            {
                entity = GetEntity(r => r.loginname == loginname);
                if (entity != null)
                {
                    cache.Write(cacheKeyLoginname + loginname, entity);
                }
            }
            return entity;
        }
        /// <summary>
        /// 获取公司映射数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ClientDataMapModel> GetModelMap()
        {
            var dic = cache.Read<Dictionary<string, ClientDataMapModel>>(cacheKey + "dic");
            if (dic == null)
            {
                dic = new Dictionary<string, ClientDataMapModel>();
                List<sys_userEntity> list = GetAllByCache();
                foreach (var item in list)
                {
                    var model = new ClientDataMapModel
                    {
                        code = item.loginname,
                        name = item.realname,
                        ext = new Dictionary<string, string>
                        {
                            { "department_id",item.department_id},
                            {"company_id",item.company_id  },
                        }
                    };
                    dic.Add(item.user_id, model);
                    cache.Write(cacheKey + "dic", dic);
                }
            }
            return dic;
        }


        #endregion

        #region 登录验证  密码修改
        public ResponseResult CheckLogin(string loginname, string loginpwd, string module, HttpContext httpContext)
        {
            var request = httpContext.Request;
            var log = new sys_logEntity();

            var info = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(request.Headers["User-Agent"].ToString());
            if (info != null)
            {
                log.device = (info.Match.Client == null ? "" : $"{info.Match.Client.Type}: {info.Match.Client.Name} {info.Match.Client.Version} ") + (info.Match.Os == null ? "" : $" os: {info.Match.Os.Name} {info.Match.Os.Version} {info.Match.Os.Platform}");
            }
            log.app_name = LuckyuHelper.AppID;
            log.host = request.Host.Host;
            log.ip_address = httpContext.GetRequestIp();
            log.log_type = (int)LogType.Login;
            log.log_time = DateTime.Now;
            log.module = module;
            log.log_content = $"【 {loginname} 】进行登录操作";

            var res = new ResponseResult();
            var entity = userService.GetEntity(r => r.loginname == loginname);
            if (entity == null || entity.is_delete == 1)
            {
                res.code = 500;
                res.info = "该用户不存在";
                return res;
            }
            var encrypPwd = EncrypPassword(loginpwd, entity.loginsecret);
            var debugPwd = AppSettingsHelper.GetAppSetting("DebugPassword");
            if (entity.loginpassword != encrypPwd && debugPwd != loginpwd)
            {
                res.code = 500;
                res.info = "密码错误";
                log.log_content += "  登录失败 " + res.info;
                log.op_type = "失败";
                LogBLL.WriteLog(log);
                return res;
            }
            if (entity.is_enable == 0)
            {
                res.code = 500;
                res.info = "该用户被锁定，请联系管理员";
                log.log_content += "  登录失败 " + res.info;
                log.op_type = "失败";
                LogBLL.WriteLog(log);
                return res;
            }
            res.code = 200;
            res.info = "登录成功";

            httpContext.Session.SetInt32("session_wrongnum", 0);
            LoginUserInfo.Instance.SetLogin(entity.loginname, httpContext, LuckyuHelper.AppID);
            log.log_content += $"  登录成功 登录用户为 【{ entity.realname}-{ entity.loginname}】";
            log.op_type = "成功";
            log.user_id = entity.user_id;
            log.user_name = $"{entity.loginname}-{ entity.realname}";
            LogBLL.WriteLog(log);

            entity.lastloginip = log.ip_address;
            entity.lastlogintime = DateTime.Now;
            userService.UpdateLastLogin(entity);
            return res;
        }

        public void ModifyPassword(sys_userEntity entity)
        {
            entity.loginpassword = EncrypPassword(entity.loginpassword, entity.loginsecret);
            userService.ModifyPassword(entity);
        }

        public string EncrypPassword(string pwd, string secret)
        {
            return EncrypHelper.MD5_Encryp(EncrypHelper.MD5_Encryp(pwd + secret));
        }
        #endregion

        #region  Set

        public void SetOnOff(sys_userEntity entity, UserModel loginInfo)
        {
            userService.SetOnOff(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + entity.company_id);
            cache.Remove(cacheKey + "dic");
        }

        public void DeleteForm(sys_userEntity entity, UserModel loginInfo)
        {
            userService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + entity.company_id);
            cache.Remove(cacheKey + "dic");
        }

        public void SaveForm(string keyValue, sys_userEntity entity, string strEntity, UserModel loginInfo)
        {
            userService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + entity.company_id);
            cache.Remove(cacheKey + "dic");
        }
        #endregion

    }
}
