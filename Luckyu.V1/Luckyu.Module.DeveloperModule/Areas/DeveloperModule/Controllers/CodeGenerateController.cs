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
    /// 代码生成    /DeveloperModule/CodeGenerate
    /// </summary>
    [Area("DeveloperModule")]
    public class CodeGenerateController : MvcControllerBase
    {
        #region Var
        private DataBaseBLL databaseBLL = new DataBaseBLL();
        #endregion

        #region Form
        public IActionResult Form()
        {
            return View();
        }

        public IActionResult GetAllDbTableSelect()
        {
            var list = databaseBLL.GetAllDBTable();
            var xmselect = list.Select(r => new xmSelectTree
            {
                value = r.Key,
                name = r.Value,
            }).ToList();
            return Success(xmselect);
        }

        #endregion

        #region 生成
        public async Task<IActionResult> GenerateEntity(string dbtablename)
        {
            var dbtable = databaseBLL.GetTableInfoByName(dbtablename);
            var partialViewHtml = await this.RenderViewAsync("Templete_Entity", dbtable, true);
            return Success(partialViewHtml);
        }
        public async Task<IActionResult> GenerateService(string dbtablename)
        {
            var dbtable = databaseBLL.GetTableInfoByName(dbtablename);
            var partialViewHtml = await this.RenderViewAsync("Templete_Service", dbtable, true);
            return Success(partialViewHtml);
        }
        public async Task<IActionResult> GenerateBLL(string dbtablename)
        {
            var dbtable = databaseBLL.GetTableInfoByName(dbtablename);
            var partialViewHtml = await this.RenderViewAsync("Templete_BLL", dbtable, true);
            return Success(partialViewHtml);
        }

        public async Task<IActionResult> DownloadTemplete(string dbtablename)
        {
            var dbtable = databaseBLL.GetTableInfoByName(dbtablename);
            var tempDir = FileHelper.MapDirectoryPath("/File/Temp");
            var tempCodeDir = FileHelper.MapDirectoryPath("/File/Temp/GenerateCode");

            var fileEntity = FileHelper.Combine(tempCodeDir, $"/{dbtablename}Entity.cs");
            var strEntity = await this.RenderViewAsync("Templete_Entity", dbtable, true);
            await System.IO.File.WriteAllTextAsync(fileEntity, strEntity, Encoding.UTF8);

            var fileService = FileHelper.Combine(tempCodeDir, $"/{dbtablename}Service.cs");
            var strService = await this.RenderViewAsync("Templete_Service", dbtable, true);
            await System.IO.File.WriteAllTextAsync(fileService, strService, Encoding.UTF8);

            var fileBLL = FileHelper.Combine(tempCodeDir, $"/{dbtablename}BLL.cs");
            var strBLL = await this.RenderViewAsync("Templete_BLL", dbtable, true);
            await System.IO.File.WriteAllTextAsync(fileBLL, strBLL, Encoding.UTF8);

            var fileFinal = FileHelper.Combine(tempDir, "/GenerateCode.zip");
            if (System.IO.File.Exists(fileFinal))
            {
                System.IO.File.Delete(fileFinal);
            }
            System.IO.Compression.ZipFile.CreateFromDirectory(tempCodeDir, fileFinal);

            var btyes = System.IO.File.ReadAllBytes(fileFinal);

            // 删除临时文件
            if (System.IO.File.Exists(fileFinal))
            {
                System.IO.File.Delete(fileFinal);
            }
            if (System.IO.Directory.Exists(tempCodeDir))
            {
                System.IO.Directory.Delete(tempCodeDir, true);
            }

            return File(btyes, "application/zip");

        }
        #endregion
    }
}
