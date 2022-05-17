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
        FormDesignerBLL designerBLL = new FormDesignerBLL();
        FormInputBLL formBLL = new FormInputBLL();
        #endregion

        #region Index
        public IActionResult Index(string form_id)
        {
            return View();
        }

        public IActionResult Page(JqgridPageRequest jqPage, string form_id)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var page = formBLL.Page(jqPage, form_id, loginInfo);
            return Json(page);
        }

        public IActionResult DeleteForm(string form_id, string keyValue)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            var res = formBLL.DeleteForm(form_id, keyValue, loginInfo);
            return Json(res);
        }

        #endregion

        #region Form

        public IActionResult Form(string form_id)
        {
            var table = designerBLL.GetTableEntity(r => r.form_id == form_id);
            var columns = designerBLL.GetColumnList(r => r.form_id == form_id);
            ViewBag.FormTable = table;
            ViewBag.FormColumns = columns;
            return View();
        }

        public IActionResult GetFormData(string form_id, string keyValue)
        {
            var data = formBLL.GetFormData(form_id, keyValue);
            return Success(data);
        }

        public IActionResult SaveForm(string form_id, string keyValue, string strEntity)
        {
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            Dictionary<string, object> dicEntity = strEntity.ToObject<Dictionary<string, object>>();
            var res = formBLL.SaveForm(form_id, keyValue, dicEntity, loginInfo);
            return Json(res);
        }

        #endregion

        #region Interface
        /// <summary>
        /// 通用数据源
        /// </summary>
        /// <param name="formcode">自定义表单名</param>
        /// <param name="columncode">自定义字段名</param>
        /// <returns></returns>
        public IActionResult GetDataSource(string formcode, string columncode)
        {
            var xmselect = formBLL.GetDataSource(formcode, columncode);
            return Success(xmselect);
        }
        #endregion
    }
}
