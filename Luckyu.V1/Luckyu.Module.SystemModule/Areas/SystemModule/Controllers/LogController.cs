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
    public class LogController : MvcControllerBase
    {
        #region Var
        private LogBLL logBLL = new LogBLL();

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Page(JqgridPageRequest jqpage)
        //{
        //    var page = logBLL.Page(jqpage);
        //    return Json(page);
        //}
        #endregion

        public IActionResult Show()
        {
            return View();
        }

    }
}
