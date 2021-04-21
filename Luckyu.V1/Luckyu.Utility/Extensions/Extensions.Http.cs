using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Luckyu.Utility
{
    public static partial class Extensions
    {
        #region Cookie
        public static void SetCookie(this HttpContext httpContext, string key, string value, DateTimeOffset offset)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Path = "/";
            cookieOptions.Expires = offset;
            httpContext.Response.Cookies.Append(key, value, cookieOptions);
        }

        public static void SetCookie(this HttpContext httpContext, string key, string value)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Path = "/";
            cookieOptions.Expires = DateTimeOffset.Now.AddDays(1);
            httpContext.Response.Cookies.Append(key, value, cookieOptions);
        }

        public static string GetCookie(this HttpContext httpContext, string key)
        {
            string cookie = httpContext.Request.Cookies[key];
            return cookie;
        }

        public static void RemoveCookie(this HttpContext httpContext, string key)
        {
            string cookie = httpContext.Request.Cookies[key];
            if (cookie != null)
            {
                httpContext.Response.Cookies.Append(key, "", new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddMinutes(-1)
                });
            }
        }
        public static void RemoveAllCookie(this HttpContext httpContext)
        {
            var allKey = httpContext.Request.Cookies.Keys;
            foreach (var key in allKey)
            {
                httpContext.RemoveCookie(key);
            }
        }

        #endregion

        #region Session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        #endregion

        /// <summary>
        /// 获取请求IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

    }
}
