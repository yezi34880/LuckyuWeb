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
    }
}
