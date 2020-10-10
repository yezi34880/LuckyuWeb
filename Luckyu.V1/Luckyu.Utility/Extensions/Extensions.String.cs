using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public static partial class Extensions
    {
        /// <summary>
        /// 获取字符长度 中文两个字符
        /// </summary>
        public static int GetASCIILength(this string str)
        {
            if (str.Length == 0)
                return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        /// <summary>
        /// length大于字符串长度，截取；小于，返回字符串而不抛出异常
        /// </summary>
        public static string Substr(this string str, int startIndex, int length)
        {
            string str1;
            if (string.IsNullOrEmpty(str))
            {
                str1 = "";
            }
            else
            {
                str1 = (str.Length < startIndex + length ? str.Substring(startIndex) : str.Substring(startIndex, length));
            }
            return str1;
        }

        /// <summary>
        /// html 字符串 取出中间纯文本
        /// </summary>
        public static string InnerText(this string str)
        {
            str = str.Replace("</p>", "/n</p>");
            var reg = new System.Text.RegularExpressions.Regex(@"(?<=>)[^<>]+(?=<)");
            var texts = reg.Matches(str);
            str = "";
            for (int i = 0; i < texts.Count; i++)
            {
                str += texts[i].Value;
            }
            str = str.Replace("&nbsp;", " ");
            return str;
        }


    }
}
