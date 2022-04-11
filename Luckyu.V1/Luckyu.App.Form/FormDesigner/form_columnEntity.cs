
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using FreeSql.DataAnnotations;
using System;

namespace Luckyu.App.Form
{
    /// <summary>
    ///  form_column 
    /// </summary>
    [Table(Name = "FORM_COLUMN")]
    public class form_columnEntity
    {
        #region 属性
        /// <summary>
        ///  column_id 
        /// </summary>
        [Column(IsPrimary = true)]
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
        ///  remark 备注
        /// </summary>
        public string remark { get; set; }

        #endregion

        #region 扩展

        #endregion

        #region 方法

        #endregion

    }
}