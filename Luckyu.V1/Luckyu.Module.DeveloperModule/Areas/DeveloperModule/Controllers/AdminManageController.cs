using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.DeveloperModule.Controllers
{
    /// <summary>
    /// 开发人员管理配置    /DeveloperModule/AdminManage
    /// </summary>
    [Area("DeveloperModule")]
    public class AdminManageController : MvcControllerBase
    {
        #region Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DynamicLoadDLL()
        {
            var folder = AppSettingsHelper.GetAppSetting("UpdateDLLFolder");

            var fileNames = System.IO.Directory.GetFiles(folder, "*.dll");
            foreach (var name in fileNames)
            {
                var bytes = System.IO.File.ReadAllBytes(name);
                using (var stream = new System.IO.MemoryStream(bytes))
                {
                    System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(stream);
                }
                var filename = name.Substring(name.LastIndexOf('\\') + 1);
                System.IO.File.Copy(name, FileHelper.Combine(AppContext.BaseDirectory, filename), true);
            }

            return Success();
        }
        #endregion
    }
}
