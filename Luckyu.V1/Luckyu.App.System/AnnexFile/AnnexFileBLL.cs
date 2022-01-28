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
    public class AnnexFileBLL
    {
        #region Var
        private sys_annexfileService annexService = new sys_annexfileService();
        private ConfigBLL configBLL = new ConfigBLL();

        #endregion

        #region Get
        public sys_annexfileEntity GetEntity(Expression<Func<sys_annexfileEntity, bool>> condition, string orderby = "")
        {
            var entity = annexService.GetEntity(condition, orderby);
            return entity;
        }
        public sys_annexfileEntity GetEntity(Expression<Func<sys_annexfileEntity, bool>> condition, Expression<Func<sys_annexfileEntity, object>> orderby, bool isDesc = false)
        {
            var entity = annexService.GetEntity(condition, orderby, isDesc);
            return entity;
        }

        public sys_annexfileEntity GetEntityTop(int top, Expression<Func<sys_annexfileEntity, bool>> condition, string orderby = "")
        {
            var entity = annexService.GetEntityTop(top, condition, orderby);
            return entity;
        }

        public List<sys_annexfileEntity> GetList(Expression<Func<sys_annexfileEntity, bool>> condition, string orderby = "")
        {
            condition = condition.And(r => r.is_delete == 0);
            var list = annexService.GetList(condition, orderby);
            return list;
        }
        public List<sys_annexfileEntity> GetList(Expression<Func<sys_annexfileEntity, bool>> condition, Expression<Func<sys_annexfileEntity, object>> orderby, bool isDesc = false)
        {
            condition = condition.And(r => r.is_delete == 0);
            var list = annexService.GetList(condition, orderby, isDesc);
            return list;
        }
        #endregion

        #region Set
        public sys_annexfileEntity SaveAnnex(string exId, string exCode, string folderPre, string fileName, string contentType, Stream fileStream, UserModel userInfo)
        {
            if (fileStream == null || fileStream.Length < 1)
            {
                return null;
            }

            var annex = new sys_annexfileEntity();
            annex.Create(userInfo);
            annex.external_id = exId;
            annex.externalcode = exCode;

            var basepath = GetBasePath();
            string FileEextension = Path.GetExtension(fileName);
            string onlyfilename = Path.GetFileNameWithoutExtension(fileName);
            string virtualPath = $"{userInfo.user_id}\\{DateTime.Now.ToString("yyyyMMdd")}\\{onlyfilename}_{annex.annex_id}{FileEextension}";
            if (!folderPre.IsEmpty())
            {
                virtualPath = FileHelper.Combine(folderPre, virtualPath);
            }
            string realPath = FileHelper.Combine(basepath.Item2, virtualPath);
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
            annex.basepath = basepath.Item1;
            annex.fileextenssion = FileEextension;
            annex.filename = fileName;
            annex.filepath = virtualPath;
            annex.filesize = buffer.Length;
            annex.contexttype = contentType;

            annexService.Insert(annex);

            // 如果是图片类型，自动生成缩略图
            var images = new List<string> { ".jpg", ".jpeg", ".png", ".ico", ".bmp" };
            if (images.Contains(annex.fileextenssion.ToLower()))
            {
                var virtualPaththumb = virtualPath.Replace(FileEextension, $"_thumb{FileEextension}");
                string thumbpaht = FileHelper.Combine(basepath.Item2, virtualPaththumb);
                var thumbWidth = AppSettingsHelper.GetAppSetting("ThumbWidth").ToInt();
                FileHelper.ThumbImage(realPath, thumbpaht, thumbWidth);

                // 缩略图 关联ID 为 主图 ID,缩略图 附加字段  [thumb]
                var thumb = new sys_annexfileEntity();
                thumb.Create(userInfo);
                thumb.external_id = annex.annex_id;
                thumb.externalcode = "[thumb]";

                annex.downloadcount = 0;
                annex.basepath = basepath.Item1;
                annex.fileextenssion = FileEextension;
                annex.filename = fileName;
                annex.filepath = virtualPaththumb;
                annex.filesize = buffer.Length;
                annex.contexttype = contentType;

                annexService.Insert(annex);
            }
            return annex;
        }

        public void DeleteAnnex(string fileId)
        {
            var entity = GetEntity(r => r.annex_id == fileId);
            if (entity != null)
            {
                // 逻辑删除
                annexService.Delete(entity);
                //var basepath = GetBasePath();
                //string realPath = FileHelper.Combine(basepath.Item2, entity.filepath);
                //if (File.Exists(realPath))
                //{
                //    File.Delete(realPath);
                //}
            }
        }
        public void DeleteAnnexs(List<string> fileIds)
        {
            if (!fileIds.IsEmpty())
            {
                foreach (var fileId in fileIds)
                {
                    DeleteAnnex(fileId);
                }
            }
        }

        public void UpdateAnnexSort(List<initialPreviewConfig> previewAnnex)
        {
            var list = new List<sys_annexfileEntity>();
            for (int i = 0; i < previewAnnex.Count; i++)
            {
                var item = previewAnnex[i];
                list.Add(new sys_annexfileEntity
                {
                    annex_id = item.key,
                    sort = (i + 1)
                });
            }
            annexService.UpdateSort(list);
        }
        public void Update(sys_annexfileEntity entity)
        {
            annexService.Update(entity);
        }
        public void DownLoad(sys_annexfileEntity entity)
        {
            annexService.DownLoad(entity);
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
        public FileInputResponse ChangeListToPreview(List<sys_annexfileEntity> list)
        {
            if (list.IsEmpty())
            {
                return new FileInputResponse
                {
                    initialPreview = null,
                    initialPreviewConfig = null,
                };
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
                    downloadUrl = "/SystemModule/Annex/ShowFile?keyValue=" + r.annex_id,
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
        public FileInputResponse GetPreviewList(Expression<Func<sys_annexfileEntity, bool>> condition, string orderby = "")
        {
            var list = GetList(condition, orderby);
            var res = ChangeListToPreview(list);
            return res;
        }
        #endregion

        #region private
        private (string, string) GetBasePath()
        {
            var configCode = $"annexbasepath_{DateTime.Today.ToString("yyyy")}";
            var config = configBLL.GetEntityByCache(configCode);
            if (config != null)
            {
                return (configCode, config.configvalue);
            }
            else
            {
                configCode = "annexbasepath";
                string basePath = configBLL.GetValueByCache(configCode);
                return (configCode, basePath);
            }
        }
        #endregion

    }
}
