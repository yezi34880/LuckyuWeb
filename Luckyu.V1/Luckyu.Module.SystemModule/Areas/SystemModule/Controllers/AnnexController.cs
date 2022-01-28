using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Module.SystemModule.Controllers
{
    /// <summary>
    /// 附件  /SystemModule/Annex
    /// </summary>
    [Area("SystemModule")]
    public class AnnexController : MvcControllerBase
    {
        #region Var
        private AnnexFileBLL annexBLL = new AnnexFileBLL();
        private ConfigBLL configBLL = new ConfigBLL();
        #endregion

        public IActionResult ShowFile(string keyValue)
        {
            var annex = annexBLL.GetEntity(r => r.annex_id == keyValue, r => r.createtime);
            if (annex == null)
            {
                return new EmptyResult();
            }
            var basepath = configBLL.GetValueByCache(annex.basepath);
            var filePath = FileHelper.Combine(basepath, annex.filepath);
            if (!System.IO.File.Exists(filePath))
            {
                return new EmptyResult();
            }
            Response.Headers.Append("Content-Disposition", "inline; filename=" + System.Web.HttpUtility.UrlEncode(annex.filename, Encoding.UTF8));
            Response.ContentType = annex.contexttype;
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, annex.contexttype);
        }

        public IActionResult ShowFileThumb(string keyValue)
        {
            //var annex = annexBLL.GetEntity(r => r.annex_id == keyValue, r => r.createtime);
            //if (annex == null)
            //{
            //    return new EmptyResult();
            //}
            var thumb = annexBLL.GetEntity(r => r.externalcode == "[thumb]" && r.external_id == keyValue, r => r.createtime);
            if (thumb == null)
            {
                return new EmptyResult();
            }

            var basepath = configBLL.GetValueByCache(thumb.basepath);
            var filePath = FileHelper.Combine(basepath, thumb.filepath);
            if (!System.IO.File.Exists(filePath))
            {
                return new EmptyResult();
            }
            Response.Headers.Append("Content-Disposition", "inline; filename=" + System.Web.HttpUtility.UrlEncode(thumb.filename, Encoding.UTF8));
            Response.ContentType = thumb.contexttype;
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, thumb.contexttype);
        }

        public IActionResult DownloadFile(string keyValue)
        {
            var annex = annexBLL.GetEntity(r => r.annex_id == keyValue, r => r.createtime);
            if (annex == null)
            {
                return Redirect("/Error/NotFoundPage");
            }
            var basepath = configBLL.GetValueByCache(annex.basepath);
            var filePath = FileHelper.Combine(basepath, annex.filepath);
            if (!System.IO.File.Exists(filePath))
            {
                return Redirect("/Error/NotFoundPage");
            }
            annexBLL.DownLoad(annex);
            Response.Headers.Append("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(annex.filename, Encoding.UTF8));
            Response.ContentType = annex.contexttype;
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, annex.contexttype, annex.filename);
        }

        public IActionResult ShowFileByExid(string exId, string exCode)
        {
            Expression<Func<sys_annexfileEntity, bool>> exp = r => r.external_id == exId;
            if (!exCode.IsEmpty())
            {
                exp = exp.LinqAnd(r => r.externalcode == exCode);
            }
            var annex = annexBLL.GetEntity(exp, r => r.createtime, true);
            var basepath = configBLL.GetValueByCache(annex.basepath);
            var filePath = FileHelper.Combine(basepath, annex.filepath);
            if (!System.IO.File.Exists(filePath))
            {
                return new EmptyResult();
            }
            Response.Headers.Append("Content-Disposition", "inline; filename=" + System.Web.HttpUtility.UrlEncode(annex.filename, Encoding.UTF8));
            Response.ContentType = annex.contexttype;
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, annex.contexttype);
        }

        public IActionResult ShowFileByExidSort(string exId, int sort)
        {
            Expression<Func<sys_annexfileEntity, bool>> exp = r => r.external_id == exId;
            var annex = annexBLL.GetEntityTop(sort, exp, "sort,createtime");
            if (annex == null)
            {
                return new EmptyResult();
            }
            var basepath = configBLL.GetValueByCache(annex.basepath);
            var filePath = FileHelper.Combine(basepath, annex.filepath);
            if (!System.IO.File.Exists(filePath))
            {
                return new EmptyResult();
            }
            Response.Headers.Append("Content-Disposition", "inline; filename=" + System.Web.HttpUtility.UrlEncode(annex.filename, Encoding.UTF8));
            Response.ContentType = annex.contexttype;
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, annex.contexttype);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        [ValidateAntiForgeryToken]
        public IActionResult UploadAnnex(string exId, string exCode, string folderPre)
        {
            var res = new FileInputResponse();
            if (exId.IsEmpty())
            {
                res.error = "关联字段不能为空";
                return Json(res);
            }
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
                res.initialPreview = new List<string>();
                res.initialPreviewConfig = new List<initialPreviewConfig>();
                var files = HttpContext.Request.Form.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    annexBLL.SaveAnnex(exId, exCode, folderPre, file.FileName, file.ContentType, file.OpenReadStream(), loginInfo);
                }
                res = annexBLL.GetPreviewList(r => r.external_id == exId);
            }
            return Json(res);
        }

        /// <summary>
        /// 虚拟删除,否则前台404不触发
        /// </summary>
        /// <returns></returns>
        public IActionResult VirtualDeleteAnnex(string fileId)
        {
            return Success();
        }

    }
}
