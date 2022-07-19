using IPTools.Core;
using System;
using SqlSugar;

namespace Luckyu.Log
{
    /// <summary>
    ///  sys_log   
    /// </summary>
    [SplitTable(SplitType.Year)]//按年分表 （自带分表支持 年、季、月、周、日）
    [SugarTable("sys_log_{year}{month}{day}")]//生成表名格式 3个变量必须要有
    public class sys_logEntity
    {
        #region 属性

        /// <summary>
        ///  log_id   
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, Length = 50)]
        public string log_id { get; set; }

        /// <summary>
        ///  app_name   项目名
        /// </summary>
        public string app_name { get; set; }

        /// <summary>
        ///  log_type  LogType.Operation枚举  1-登录日志2-操作日志3-异常日志4-调试信息 5-SQL
        /// </summary>
        [SugarColumn(IndexGroupNameList = new string[] { "index1" })]
        public int log_type { get; set; }

        /// <summary>
        ///  log_time   操作时间
        /// </summary>
        [SplitField]
        [SugarColumn(IndexGroupNameList = new string[] { "index1" })]
        public DateTime log_time { get; set; }

        /// <summary>
        ///  process_id  业务主键
        /// </summary>
        [SugarColumn(Length = 50, IndexGroupNameList = new string[] { "index1" })]
        public string process_id { get; set; } = "";

        /// <summary>
        ///  log_content   动作
        /// </summary>
        [SugarColumn(Length = -1)]
        public string log_content { get; set; } = "";

        /// <summary>
        ///  log_json   源数据
        /// </summary>
        [SugarColumn(Length = -1)]
        public string log_json { get; set; } = "";

        /// <summary>
        ///  module   操作模块
        /// </summary>
        public string module { get; set; } = "";

        /// <summary>
        ///  op_type   具体操作类型如登录删除修改
        /// </summary>
        public string op_type { get; set; } = "";

        /// <summary>
        ///  host   主机域名
        /// </summary>
        public string host { get; set; } = "";

        /// <summary>
        ///  ip_address   ip
        /// </summary>
        public string ip_address { get; set; } = "";

        /// <summary>
        ///  ip_location   
        /// </summary>
        public string ip_location { get; set; } = "";

        /// <summary>
        ///  device   根据useragent转信息
        /// </summary>
        public string device { get; set; } = "";

        /// <summary>
        ///  user_id   
        /// </summary>
        [SugarColumn(Length = 50, IndexGroupNameList = new string[] { "index1" })]
        public string user_id { get; set; } = "";

        /// <summary>
        ///  user_name   
        /// </summary>
        public string user_name { get; set; } = "";

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(Length = -1)]
        public string remark { get; set; } = "";

        /// <summary>
        ///  is_enable   
        /// </summary>
        public int is_enable { get; set; } = 1;

        #endregion

        #region 方法
        public void Create()
        {
            this.log_id = SnowflakeHelper.NewCode();
            this.log_time = DateTime.Now;
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
