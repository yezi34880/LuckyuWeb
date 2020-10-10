
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_dataauthorize_detail   
    /// </summary>
    [SugarTable("SYS_DATAAUTHORIZE_DETAIL", "")]
    public class sys_dataauthorize_detailEntity
    {
        #region 属性

        /// <summary>
        ///  detail_id   
        /// </summary>
        [SugarColumn(ColumnName = "DETAIL_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string detail_id { get; set; }

        /// <summary>
        ///  auth_id   
        /// </summary>
        [SugarColumn(ColumnName = "AUTH_ID", ColumnDescription = "")]
        public string auth_id { get; set; }

        /// <summary>
        ///  objecttype   0用户1公司2部门
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTTYPE", ColumnDescription = "0用户1公司2部门")]
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECT_ID", ColumnDescription = "")]
        public string object_id { get; set; }

        /// <summary>
        ///  objectname   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTNAME", ColumnDescription = "")]
        public string objectname { get; set; }

        #endregion

        #region 方法
        public void Create(string authId)
        {
            if (this.detail_id.IsEmpty())
            {
                this.detail_id = SnowflakeHelper.NewCode();
            }
            this.auth_id = authId;
        }
        #endregion
    }
}
