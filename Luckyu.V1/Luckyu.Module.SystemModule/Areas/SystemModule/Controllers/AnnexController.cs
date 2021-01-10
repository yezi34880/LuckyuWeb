﻿using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult ShowFile(string fileId)
        {
            var annex = annexBLL.GetEntity(r => r.annex_id == fileId);
            if (annex == null)
            {
                return null;
            }
            var basepath = configBLL.GetValueByCache(annex.basepath);
            var filePath = FileHelper.Combine(basepath, annex.filepath);
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            Response.Headers.Append("Content-Disposition", "inline; filename=" + System.Web.HttpUtility.UrlEncode(annex.filename, Encoding.UTF8));
            return File(filePath, annex.contexttype);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        [ValidateAntiForgeryToken]
        public IActionResult UploadAnnex(string keyValue, string preFolderName, List<IFormFile> files)
        {
            var res = new FileInputResponse();
            if (files.Count > 0)
            {
                var loginInfo = LoginUserInfo.Instance.GetLoginUser(HttpContext);
                res.initialPreview = new List<string>();
                res.initialPreviewConfig = new List<initialPreviewConfig>();
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    annexBLL.SaveAnnex(keyValue, preFolderName, file.FileName, file.ContentType, file.OpenReadStream(), loginInfo);
                }
                res = annexBLL.GetPreviewList(r => r.external_id == keyValue);
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