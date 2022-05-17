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
        FormDesignerBLL designerBLL = new FormDesignerBLL();
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Page(JqgridPageRequest jqPage)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = designerBLL.Page(jqPage, loginInfo);
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
            var form = designerBLL.GetTableEntity(r => r.form_id == keyValue);
            var dic = new Dictionary<string, object>();
            dic.Add("Form", form);
            return Success(dic);
        }

        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = designerBLL.SaveForm(keyValue, strEntity, loginInfo);
            return Json(res);
        }
        #endregion

        #region Interface
        /// <summary>
        /// 获取所有自定义表单
        /// </summary>
        public IActionResult GetSelect()
        {
            var data = designerBLL.GetTableSelectList(r => true);
            return Success(data);
        }

        public IActionResult GetTableInfo(string form_id)
        {
            var table = designerBLL.GetTableEntity(r => r.form_id == form_id);
            var cols = designerBLL.GetColumnList(r => r.form_id == form_id);
            var data = new
            {
                FormTable = table,
                FormColumns = cols
            };
            return Success(data);
        }
        #endregion
    }
}
