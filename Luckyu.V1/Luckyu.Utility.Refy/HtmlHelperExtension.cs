using Luckyu.Cache;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public delegate void SelfApplicable<T>(SelfApplicable<T> self, T arg);
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// 递归构建HTML
        /// </summary>
        public static void RecursiveRender<T>(this IHtmlHelper helper, T model, SelfApplicable<T> func)
        {
            func(func, model);
        }

        /// <summary>
        /// 自动为 Js 文件添加版本号
        /// </summary>
        /// <param name="contentPath"></param>
        /// <returns></returns>
        public static IHtmlContent AppendJs(this IHtmlHelper html, string contentPath)
        {
            contentPath = contentPath.TrimStart('~');
            if (LuckyuHelper.IsDebug())
            {
                contentPath = FileHelper.MapPath(contentPath);
                var str = $@"
<script type=""text/javascript"">
{FileHelper.GetFileContent(contentPath)}
</script>
";
                return new HtmlString(str);
            }
            else
            {
                var version = GetPathWithVersion(contentPath);
                return new HtmlString($"<script src=\"{version}\" type=\"text/javascript\"></script>");
            }
        }
        /// <summary>
        /// 自动为 css 文件添加版本号
        /// </summary>
        public static IHtmlContent AppendCss(this IHtmlHelper html, string contentPath)
        {
            contentPath = contentPath.TrimStart('~');
            if (LuckyuHelper.IsDebug())
            {
                contentPath = FileHelper.MapPath(contentPath);
                var str = $@"
<style  type=""text/css"">
{FileHelper.GetFileContent(contentPath)}
</style>
";
                return new HtmlString(str);
            }
            else
            {
                var version = GetPathWithVersion(contentPath);
                return new HtmlString($"<link href=\"{version}\" rel=\"stylesheet\"></link>");
            }
        }

        /// <summary>
        /// 自动生成版本号
        /// </summary>
        private static string GetPathWithVersion(string contentPath)
        {
            string hashValue = FileHelper.GetFileVersion(FileHelper.MapPath(contentPath));
            contentPath = contentPath + "?v=" + hashValue;
            return contentPath;
        }
    }

}
