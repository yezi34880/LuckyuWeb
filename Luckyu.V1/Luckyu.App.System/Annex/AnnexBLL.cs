using Luckyu.App.Organization;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class AnnexBLL
    {
        #region Var
        private sys_annexService annexService = new sys_annexService();

        #endregion

        #region Get
        public sys_annexEntity GetEntity(Expression<Func<sys_annexEntity, bool>> condition, string orderby = "")
        {
            var entity = annexService.GetEntity(condition, orderby);
            return entity;
        }
        public List<sys_annexEntity> GetList(Expression<Func<sys_annexEntity, bool>> condition, string orderby = "")
        {
            var list = annexService.GetList(condition, orderby);
            return list;
        }

        #endregion

        #region Set
        public sys_annexEntity SaveAnnex(string folderId, string preFolderName, string fileName, string contentType, Stream fileStream, UserModel userInfo)
        {
            if (fileStream == null || fileStream.Length < 1)
            {
                return null;
            }

            var annex = new sys_annexEntity();
            annex.Create(userInfo);
            annex.folder_id = folderId;

            string basePath = AppSettingsHelper.GetAppSetting("AnnexPath");
            string FileEextension = Path.GetExtension(fileName);
            string virtualPath = $"{userInfo.user_id}\\{DateTime.Now.ToString("yyyyMMdd")}\\{annex.annex_id}{FileEextension}";
            if (!preFolderName.IsEmpty())
            {
                virtualPath = FileHelper.Combine(preFolderName, virtualPath);
            }
            string realPath = FileHelper.Combine(basePath, virtualPath);
            //创建文件夹
            string path = Path.GetDirectoryName(realPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            byte[] buffer = new Byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Seek(0, SeekOrigin.Begin);
            using (FileStream fs = new FileStream(realPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
            annex.downloadcount = 0;
            annex.fileextenssion = FileEextension;
            annex.filename = fileName;
            annex.filepath = virtualPath;
            annex.filesize = buffer.Length;
            annex.contexttype = contentType;

            annexService.Insert(annex);
            return annex;
        }

        public void DeleteAnnex(string fileId)
        {
            var entity = GetEntity(r => r.annex_id == fileId);
            if (entity != null)
            {
                annexService.Delete(entity);
                string basePath = AppSettingsHelper.GetAppSetting("AnnexPath");
                string realPath = FileHelper.Combine(basePath, entity.filepath);
                if (File.Exists(realPath))
                {
                    File.Delete(realPath);
                }
            }
        }

        #endregion

        #region bootstrap fileinput 预览
        // bootstrap-fileinput 预览类型
        //private static readonly string[] previewTypes = new string[] { "image", "text", "html", "video", "audio", "flash", "object", "office", "pdf", "gdocs", "other" };
        private static readonly string[] previewTypes = new string[] { "image", "pdf" };

        /// <summary>
        /// 转换 附件记录列表 至 可预览数据
        /// </summary>
        /// <returns></returns>
        public FileInputResponse ChangeListToPreview(List<sys_annexEntity> list)
        {
            if (list.IsEmpty())
            {
                return null;
            }
            var previewsConfig = list.Select(r =>
            {
                var type = "object";
                if (!r.contexttype.IsEmpty())
                {
                    foreach (var preType in previewTypes)
                    {
                        if (r.contexttype.Contains(preType))
                        {
                            type = preType;
                            break;
                        }
                    }
                }

                var config = new initialPreviewConfig
                {
                    type = type,
                    size = r.filesize.ToInt(),
                    downloadUrl = "/BaseModule/Annexes/ShowAnnexesFile?fileId=" + r.annex_id,
                    caption = r.filename,
                    key = r.annex_id
                };
                return config;
            }).ToList();
            var annex = new FileInputResponse
            {
                initialPreview = previewsConfig.Select(r => r.downloadUrl).ToList(),
                initialPreviewConfig = previewsConfig,
            };
            return annex;
        }

        /// <summary>
        /// 获取附件记录 并转换为 页面预览数据
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public FileInputResponse GetPreviewList(Expression<Func<sys_annexEntity, bool>> condition, string orderby = "")
        {
            var list = GetList(condition, orderby);
            var res = ChangeListToPreview(list);
            return res;
        }
        #endregion

    }
}
