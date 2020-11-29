using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.App.Organization
{
    public class LoginInfo
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime loginTime { get; set; }
        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 浏览器名称
        /// </summary>
        public string browser { get; set; }
        /// <summary>
        /// 登录者标识
        /// </summary>
        public string device { get; set; }
        /// <summary>
        /// singalir的 connectionid
        /// </summary>
        public string connection_id { get; set; }
        /// <summary>
        /// 票据信息
        /// </summary>
        public string token { get; set; }

    }
}
