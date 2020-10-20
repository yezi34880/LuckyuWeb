using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_delegate   
    /// </summary>
    [SugarTable("WF_DELEGATE", "")]
    public class wf_delegateEntity
    {
        #region 属性
        /// <summary>
        ///  delegate_id   
        /// </summary>
        [SugarColumn(ColumnName = "DELEGATE_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string delegate_id { get; set; }

        /// <summary>
        ///  user_id   
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID", ColumnDescription = "")]
        public string user_id { get; set; }

        /// <summary>
        ///  username   
        /// </summary>
        [SugarColumn(ColumnName = "USERNAME", ColumnDescription = "")]
        public string username { get; set; }

        /// <summary>
        ///  department_id   
        /// </summary>
        [SugarColumn(ColumnName = "DEPARTMENT_ID", ColumnDescription = "")]
        public string department_id { get; set; }

        /// <summary>
        ///  company_id   
        /// </summary>
        [SugarColumn(ColumnName = "COMPANY_ID", ColumnDescription = "")]
        public string company_id { get; set; }

        /// <summary>
        ///  starttime   
        /// </summary>
        [SugarColumn(ColumnName = "STARTTIME", ColumnDescription = "")]
        public DateTime starttime { get; set; }

        /// <summary>
        ///  endtime   
        /// </summary>
        [SugarColumn(ColumnName = "ENDTIME", ColumnDescription = "")]
        public DateTime endtime { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

        /// <summary>
        ///  create_userId   
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_USERID", ColumnDescription = "")]
        public string create_userid { get; set; }

        /// <summary>
        ///  create_username   
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_USERNAME", ColumnDescription = "")]
        public string create_username { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        [SugarColumn(ColumnName = "CREATETIME", ColumnDescription = "")]
        public DateTime? createtime { get; set; }

        /// <summary>
        ///  edittime   
        /// </summary>
        [SugarColumn(ColumnName = "EDITTIME", ColumnDescription = "")]
        public DateTime? edittime { get; set; }

        /// <summary>
        ///  edit_userid   
        /// </summary>
        [SugarColumn(ColumnName = "EDIT_USERID", ColumnDescription = "")]
        public string edit_userid { get; set; }

        /// <summary>
        ///  edit_username   
        /// </summary>
        [SugarColumn(ColumnName = "EDIT_USERNAME", ColumnDescription = "")]
        public string edit_username { get; set; }

        /// <summary>
        ///  is_delete   
        /// </summary>
        [SugarColumn(ColumnName = "IS_DELETE", ColumnDescription = "")]
        public int is_delete { get; set; }

        /// <summary>
        ///  deletetime   
        /// </summary>
        [SugarColumn(ColumnName = "DELETETIME", ColumnDescription = "")]
        public DateTime? deletetime { get; set; }

        /// <summary>
        ///  delete_userid   
        /// </summary>
        [SugarColumn(ColumnName = "DELETE_USERID", ColumnDescription = "")]
        public string delete_userid { get; set; }

        /// <summary>
        ///  delete_username   
        /// </summary>
        [SugarColumn(ColumnName = "DELETE_USERNAME", ColumnDescription = "")]
        public string delete_username { get; set; }

        /// <summary>
        ///  is_enable   
        /// </summary>
        [SugarColumn(ColumnName = "IS_ENABLE", ColumnDescription = "")]
        public int is_enable { get; set; }
        #endregion

        #region 方法

        #endregion
    }
}
