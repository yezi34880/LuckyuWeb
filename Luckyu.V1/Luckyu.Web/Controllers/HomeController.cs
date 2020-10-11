using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceDetectorNET;
using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.App.Workflow;
using Luckyu.Cache;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Luckyu.Web.Controllers
{
    public class HomeController : MvcControllerBase
    {
        #region Var
        private ModuleBLL moduleBLL = new ModuleBLL();
        private UserBLL userBLL = new UserBLL();
        private WFTaskBLL taskBLL = new WFTaskBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            ViewBag.UserName = loginInfo.realname;
            ViewBag.DepartmentName = loginInfo.departmentname;
            ViewBag.CompanyName = loginInfo.companyname;

            List<sys_moduleEntity> listSelfModule;
            var modules = moduleBLL.GetModuleTreeByUser(loginInfo, out listSelfModule);
            loginInfo.modules = listSelfModule;
            LoginUserInfo.Instance.UpdateLoginUser(HttpContext, loginInfo);

            ViewBag.Modules = modules;
            return View();
        }

        [HttpPost]
        public IActionResult VisitModule(string moduleName, string moduleUrl)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var log = new sys_logEntity
            {
                log_type = (int)LogType.Operation,
                op_type = "访问菜单",
                host = HttpContext.Request.Host.Host,
                ip_address = HttpContext.GetRequestIp(),
                log_time = DateTime.Now,
                module = moduleUrl,
                log_content = moduleName + " " + moduleUrl,
                user_id = loginInfo.user_id,
                user_name = loginInfo.realname
            };
            var info = DeviceDetector.GetInfoFromUserAgent(HttpContext.Request.Headers["User-Agent"].ToString());
            if (info != null)
            {
                log.device = (info.Match.Client == null ? "" : $"{info.Match.Client.Type}: {info.Match.Client.Name} {info.Match.Client.Version} ") + (info.Match.Os == null ? "" : $" os: {info.Match.Os.Name} {info.Match.Os.Version} {info.Match.Os.Platform}");
            }
            LogBLL.WriteLog(log);
            return Success();
        }
        #endregion

        #region 修改密码（用户自己，需要输入原密码）
        public ActionResult ModifyPwdSelf()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ModifyPasswordSelf(string oldPassword, string newPassword)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var entity = userBLL.GetEntity(r => r.user_id == loginInfo.user_id);
            if (entity == null)
            {
                return Fail("该数据不存在");
            }
            if (entity.loginpassword != userBLL.EncrypPassword(oldPassword, entity.loginsecret))
            {
                return Fail("原密码输入有误，请检查");
            }
            entity.loginpassword = newPassword;
            userBLL.ModifyPassword(entity);
            return Success();
        }

        #endregion

        #region 清除缓存
        public IActionResult ClearCache()
        {
            //var loginInfo = LoginUserInfo.Get();
            //if (loginInfo.isSystem)
            //{
            //    var cache = new CacheByMemory();
            //    cache.RemoveAllwithoutLogin();
            //}
            ICache cache = CacheFactory.Create();
            cache.RemoveAllwithoutLogin();
            return Success();
        }
        #endregion

        #region Get
        [HttpGet, AjaxOnly]
        public IActionResult GetUserInfo()
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            return Success(loginInfo);
        }
        #endregion

        #region Home
        /// <summary>
        /// 首页
        /// </summary>
        public IActionResult Home()
        {
            return View();
        }
        /// <summary>
        /// 首页统计
        /// </summary>
        public IActionResult Refrash()
        {
            var jqPage = new JqgridPageRequest
            {
                page = 1,
                rows = 5,
                sidx = "createtime",
                sord = "DESC"
            };
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = taskBLL.Page(jqPage, 1, loginInfo);
            var dic = new Dictionary<string, object>();
            dic.Add("Task", page.rows);

            return Success(dic);
        }
        #endregion
    }
}
