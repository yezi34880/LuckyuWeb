using Luckyu.App.Form;
using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.FormModule.Controllers
{
    /// <summary>
    /// 表单设计    /FormModule/FormDesigner
    /// </summary>
    [Area("FormModule")]
    public class FormDesignerController : MvcControllerBase
    {
        #region Var
        FormDesignerBLL formBLL = new FormDesignerBLL();
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = formBLL.Page(jqPage, loginInfo);
            return Json(page);
        }


        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        public IActionResult GetFormData(string keyValue)
        {
            return null;
        }

        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var res = formBLL.SaveForm(keyValue, strEntity);
            return Json(res);
        }


        #endregion
    }
}
