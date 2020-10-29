using Luckyu.App.Organization;
using Luckyu.App.System;
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
    /// 数据字典  /SystemModule/Dataitem
    /// </summary>
    [Area("SystemModule")]
    public class DataitemController : MvcControllerBase
    {
        #region Var
        private DataitemBLL dataitemBLL = new DataitemBLL();

        #endregion

        #region SystemIndex
        [HttpGet]
        public IActionResult SystemIndex()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SystemPage(JqgridPageRequest jqpage, string classifyId)
        {
            var page = dataitemBLL.DetailPage(jqpage, classifyId, true);
            return Json(page);
        }
        [HttpPost, AjaxOnly]
        public IActionResult DeleteForm(string keyValue)
        {
            var entity = dataitemBLL.GetDetailEntity(r => r.detail_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            dataitemBLL.DeleteDetailForm(entity, loginInfo);
            return Success();
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Page(JqgridPageRequest jqpage, string classifyId)
        {
            var page = dataitemBLL.DetailPage(jqpage, classifyId, false);
            return Json(page);
        }

        #endregion

        #region Form
        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetFormData(string keyValue)
        {
            var entity = dataitemBLL.GetDetailEntity(r => r.detail_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var data = new
            {
                DataitemDetail = entity,
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_dataitem_detailEntity>();
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            dataitemBLL.SaveDetailForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }

        #endregion

        #region ClassifyIndex
        [HttpGet]
        public IActionResult ClassifyIndex()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ClassifyPage(JqgridPageRequest jqpage)
        {
            var page = dataitemBLL.ClassifyPage(jqpage);
            return Json(page.ToTreeGrid(r => r.dataitem_id, r => r.parent_id));
        }
        [HttpPost, AjaxOnly]
        public IActionResult DeleteClassifyForm(string keyValue)
        {
            var entity = dataitemBLL.GetClassifyEntity(r => r.dataitem_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            dataitemBLL.DeleteClassifyForm(entity, loginInfo);
            return Success();
        }

        #endregion

        #region ClassifyForm
        [HttpGet]
        public IActionResult ClassifyForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetClassifyFormData(string keyValue)
        {
            var entity = dataitemBLL.GetClassifyEntity(r => r.dataitem_id == keyValue);
            if (entity == null)
            {
                return Fail(MessageString.NoData);
            }
            var data = new
            {
                DataitemClassify = entity,
            };
            return Success(data);
        }

        [HttpPost, AjaxOnly]
        [ValidateAntiForgeryToken]
        public IActionResult SaveClassifyForm(string keyValue, string strEntity)
        {
            var entity = strEntity.ToObject<sys_dataitemEntity>();
            var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
            dataitemBLL.SaveClassifyForm(keyValue, entity, strEntity, loginInfo);
            return Success(entity);
        }

        #endregion

        #region Interface
        [HttpGet, AjaxOnly]
        public IActionResult GetMap(string ver)
        {
            var data = dataitemBLL.GetModelMap();
            string md5 = EncrypHelper.MD5_Encryp(data.ToJson());
            if (md5 == ver)
            {
                return Fail("no update");
            }
            else
            {
                var jsondata = new
                {
                    data = data,
                    ver = md5
                };
                return Success(jsondata);
            }
        }

        [HttpGet, AjaxOnly]
        public IActionResult GetDataItemDatailsByCode(string code)
        {
            var data = dataitemBLL.GetDetailList(r => r.itemcode == code);
            return Success(data);
        }
        [HttpGet, AjaxOnly]
        public IActionResult GetSelectTree()
        {
            var treeData = dataitemBLL.GetSelectTree();
            return Json(treeData);
        }
        [HttpGet, AjaxOnly]
        public IActionResult GetTree()
        {
            var treeData = dataitemBLL.GetTree();
            if (treeData.IsEmpty())
                treeData = new List<eleTree>();
            var data = new
            {
                code = 0,
                data = treeData
            };
            return Json(data);
        }

        #endregion
    }
}
