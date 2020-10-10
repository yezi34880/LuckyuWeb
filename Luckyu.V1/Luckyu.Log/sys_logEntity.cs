
using DeviceDetectorNET;
using IPTools.Core;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.Log
{
    /// <summary>
    ///  sys_log   
    /// </summary>
    [SugarTable("SYS_LOG", "")]
    public class sys_logEntity
    {
        #region 属性

        /// <summary>
        ///  log_id   
        /// </summary>
        [SugarColumn(ColumnName = "LOG_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string log_id { get; set; }

        /// <summary>
        ///  app_name   项目名
        /// </summary>
        [SugarColumn(ColumnName = "APP_NAME", ColumnDescription = "项目名")]
        public string app_name { get; set; } = "Luckyu.Web";

        /// <summary>
        ///  log_type   1-登录日志2-操作日志3-异常日志4-调试信息 5-SQL
        /// </summary>
        [SugarColumn(ColumnName = "LOG_TYPE", ColumnDescription = "1-登录日志2-操作日志3-异常日志4-调试信息 5-SQL")]
        public int log_type { get; set; } = (int)LogType.Debug;

        /// <summary>
        ///  log_time   操作时间
        /// </summary>
        [SugarColumn(ColumnName = "LOG_TIME", ColumnDescription = "操作时间")]
        public DateTime log_time { get; set; }

        /// <summary>
        ///  log_content   动作
        /// </summary>
        [SugarColumn(ColumnName = "LOG_CONTENT", ColumnDescription = "动作")]
        public string log_content { get; set; } = "";

        /// <summary>
        ///  log_json   源数据
        /// </summary>
        [SugarColumn(ColumnName = "LOG_JSON", ColumnDescription = "源数据")]
        public string log_json { get; set; } = "";

        /// <summary>
        ///  module   操作模块
        /// </summary>
        [SugarColumn(ColumnName = "MODULE", ColumnDescription = "操作模块")]
        public string module { get; set; } = "";

        /// <summary>
        ///  op_type   具体操作类型如登录删除修改
        /// </summary>
        [SugarColumn(ColumnName = "OP_TYPE", ColumnDescription = "具体操作类型如登录删除修改")]
        public string op_type { get; set; } = "";

        /// <summary>
        ///  host   主机域名
        /// </summary>
        [SugarColumn(ColumnName = "HOST", ColumnDescription = "主机域名")]
        public string host { get; set; } = "";

        /// <summary>
        ///  ip_address   ip
        /// </summary>
        [SugarColumn(ColumnName = "IP_ADDRESS", ColumnDescription = "ip")]
        public string ip_address { get; set; } = "";

        /// <summary>
        ///  ip_location   
        /// </summary>
        [SugarColumn(ColumnName = "IP_LOCATION", ColumnDescription = "")]
        public string ip_location { get; set; } = "";

        /// <summary>
        ///  device   根据useragent转信息
        /// </summary>
        [SugarColumn(ColumnName = "DEVICE", ColumnDescription = "")]
        public string device { get; set; } = "";

        /// <summary>
        ///  user_id   
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID", ColumnDescription = "")]
        public string user_id { get; set; } = "";

        /// <summary>
        ///  user_name   
        /// </summary>
        [SugarColumn(ColumnName = "USER_NAME", ColumnDescription = "")]
        public string user_name { get; set; } = "";

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; } = "";

        /// <summary>
        ///  is_enable   
        /// </summary>
        [SugarColumn(ColumnName = "IS_ENABLE", ColumnDescription = "")]
        public int is_enable { get; set; } = 1;

        #endregion

        #region 方法
        public void Create()
        {
            this.log_id = SnowflakeHelper.NewCode();
            if (!string.IsNullOrEmpty(this.ip_address) && string.IsNullOrEmpty(this.ip_location))
            {
                var ipinfo = IpTool.SearchWithI18N(this.ip_address);
                if (ipinfo != null)
                {
                    this.ip_location = $"{ipinfo.Country} {ipinfo.Province} {ipinfo.City} {ipinfo.PostCode} ({ipinfo.Longitude},{ipinfo.Latitude})";
                }
            }
        }
        #endregion
    }
}
