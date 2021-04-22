using Luckyu.App.OA;
using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.PrintModule.Controllers
{
    /// <summary>
    /// 打印 /PrintModule/Print
    /// </summary>
    [Area("PrintModule")]
    public class PrintController : MvcControllerBase
    {
        public IActionResult Leave()
        {
            return View();
        }
    }
}
