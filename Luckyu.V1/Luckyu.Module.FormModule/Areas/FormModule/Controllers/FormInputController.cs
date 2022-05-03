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
    /// 表单填写    /FormModule/FormInput
    /// </summary>
    [Area("FormModule")]
    public class FormInputController : MvcControllerBase
    {
        #region Var
        FormDesignerBLL formBLL = new FormDesignerBLL();
        #endregion

        #region Index
        public IActionResult Index(string form_id)
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqPage, string form_id)
        {


        }

        #endregion

        #region Form

        public IActionResult Form(string form_id)
        {
            return View();
        }

        #endregion
    }
}
