using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Luckyu.WebTest.Models;
using Microsoft.AspNetCore.SignalR;

namespace Luckyu.WebTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHubContext<MessageHub> hub;

        public HomeController(ILogger<HomeController> logger, IHubContext<MessageHub> _hub)
        {
            _logger = logger;
            hub = _hub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void Send()
        {
            hub.Clients.All.SendAsync("ReceiveMessage", "后台发送");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region 测试程序热更新



        public IActionResult GetMsg()
        {
            return null;
        }

        public IActionResult DynamicLoad()
        {
            var assemblyDynamic = new System.Runtime.Loader.AssemblyLoadContext("AppDynamicLoca", true);

            //var folder = @"D:\MyProject\OpenSource\LuckyuWeb\Luckyu.V1\Luckyu.WebTest\bin\Debug\netcoreapp3.1\UpdateFolder";
            //var fileNames1 = System.IO.Directory.GetFiles(folder, "*.dll");
            //foreach (var name in fileNames1)
            //{
            //    //Assembly.LoadFile(name);
            //    var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(name);
            //    System.IO.File.Delete(name);
            //}
            return Content("");
        }

        #endregion

    }
}
