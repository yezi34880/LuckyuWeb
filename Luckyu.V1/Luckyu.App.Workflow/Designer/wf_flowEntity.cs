using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_flow   
    /// </summary>
    [SugarTable("WF_FLOW", "")]
    public class wf_flowEntity
    {
        #region 属性

        /// <summary>
        ///  flow_id   
        /// </summary>
        [SugarColumn(ColumnName = "FLOW_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string flow_id { get; set; }

        /// <summary>
        ///  flowcode   
        /// </summary>
        [SugarColumn(ColumnName = "FLOWCODE", ColumnDescription = "")]
        public string flowcode { get; set; }

        /// <summary>
        ///  flowname   
        /// </summary>
        [SugarColumn(ColumnName = "FLOWNAME", ColumnDescription = "")]
        public string flowname { get; set; }

        /// <summary>
        ///  flowtype   
        /// </summary>
        [SugarColumn(ColumnName = "FLOWTYPE", ColumnDescription = "")]
        public string flowtype { get; set; }

        /// <summary>
        ///  flowrange   范围 0全部 1指定
        /// </summary>
        [SugarColumn(ColumnName = "FLOWRANGE", ColumnDescription = "")]
        public int flowrange { get; set; }

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
        public void Create(UserModel loginInfo)
        {
            if (this.flow_id.IsEmpty())
            {
                this.flow_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.flow_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = loginInfo.realname;
        }
        public void Remove(UserModel loginInfo)
        {
            this.deletetime = DateTime.Now;
            this.delete_userid = loginInfo.user_id;
            this.delete_username = loginInfo.realname;
        }
        #endregion

    }
}
