using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_delegate_detail   
    /// </summary>
    [SugarTable("WF_DELEGATE_DETAIL", "")]
    public class wf_delegate_detailEntity
    {
        #region 属性
        /// <summary>
        ///  detail_id   
        /// </summary>
        [SugarColumn(ColumnName = "DETAIL_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string detail_id { get; set; }

        /// <summary>
        ///  delegate_id   
        /// </summary>
        [SugarColumn(ColumnName = "DELEGATE_ID", ColumnDescription = "")]
        public string delegate_id { get; set; }

        /// <summary>
        ///  delegatetype   
        /// </summary>
        [SugarColumn(ColumnName = "DELEGATETYPE", ColumnDescription = "")]
        public int delegatetype { get; set; }

        /// <summary>
        ///  flowcode   
        /// </summary>
        [SugarColumn(ColumnName = "FLOWCODE", ColumnDescription = "")]
        public string flowcode { get; set; }

        /// <summary>
        ///  to_user_id   
        /// </summary>
        [SugarColumn(ColumnName = "TO_USER_ID", ColumnDescription = "")]
        public string to_user_id { get; set; }

        /// <summary>
        ///  to_username   
        /// </summary>
        [SugarColumn(ColumnName = "TO_USERNAME", ColumnDescription = "")]
        public string to_username { get; set; }

        #endregion


    }
}
