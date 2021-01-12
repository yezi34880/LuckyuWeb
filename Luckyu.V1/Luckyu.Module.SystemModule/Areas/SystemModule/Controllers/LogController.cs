using Luckyu.App.System;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.SystemModule.Controllers
{
    /// <summary>
    /// 日志  /SystemModule/Log
    /// </summary>
    [Area("SystemModule")]
    public class LogController : MvcControllerBase
    {
        #region Var
        private SysLogBLL logBLL = new SysLogBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqpage, int logtype)
        {
            var page = logBLL.Page(jqpage, logtype);
            return Json(page);
        }

        public IActionResult Show()
        {
            return View();
        }

        public IActionResult GetFormData(string keyValue, DateTime date)
        {
            var log = LogBLL.GetLog(keyValue, date);
            var data = new
            {
                Log = log
            };
            return Success(data);
        }
        #endregion


    }
}
