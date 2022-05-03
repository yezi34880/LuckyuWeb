
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;

namespace Luckyu.App.Form
{
    /// <summary>
    ///  form_column 
    /// </summary>
    [SugarTable("form_column")]
    public class form_columnEntity
    {
        #region 属性
        /// <summary>
        ///  column_id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string column_id { get; set; }

        /// <summary>
        /// form_id
        /// </summary>
        public string form_id { get; set; }

        /// <summary>
        /// columntype
        /// </summary>
        public string columntype { get; set; }

        /// <summary>
        ///  columncode 数据库字段名(同表不能重复)
        /// </summary>
        public string columncode { get; set; }

        /// <summary>
        ///  columnname 标签显示名
        /// </summary>
        public string columnname { get; set; }

        /// <summary>
        ///  columnconfig Json配置 dataitem { code:"" }
        /// </summary>
        public string columnconfig { get; set; }

        /// <summary>
        ///  placeholder 标签显示名
        /// </summary>
        public string placeholder { get; set; }

        /// <summary>
        ///  formlength 页面占位 3 4 6 12
        /// </summary>
        public int formlength { get; set; }

        /// <summary>
        ///  dbdigits 精度（小数位数）
        /// </summary>
        public int dbdigits { get; set; }

        /// <summary>
        ///  dblength 长度
        /// </summary>
        public int dblength { get; set; }

        /// <summary>
        ///  dbtype 数据库类型
        /// </summary>
        public string dbtype { get; set; }

        /// <summary>
        ///  defaultvalue 默认值
        /// </summary>
        public string defaultvalue { get; set; }

        /// <summary>
        /// is_visible 是否显示
        /// </summary>
        public int is_visible { get; set; }

       /// <summary>
        ///  remark 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///  create_userid 
        /// </summary>
        public string create_userid { get; set; }

        /// <summary>
        ///  create_username 
        /// </summary>
        public string create_username { get; set; }

        /// <summary>
        ///  createtime 
        /// </summary>
        public DateTime? createtime { get; set; }

        /// <summary>
        /// is_delete 是否删除
        /// </summary>
        public int is_delete { get; set; }

        /// <summary>
        ///  delete_userid 
        /// </summary>
        public string delete_userid { get; set; }

        /// <summary>
        ///  delete_username 
        /// </summary>
        public string delete_username { get; set; }

        /// <summary>
        ///  deletetime 
        /// </summary>
        public DateTime? deletetime { get; set; }

        #endregion

        #region 扩展

        #endregion

        #region 方法

        #endregion

    }
}