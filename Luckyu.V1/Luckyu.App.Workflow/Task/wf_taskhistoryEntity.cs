using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_taskhistory   
    /// </summary>
    [SugarTable("WF_TASKHISTORY", "")]
    public class wf_taskhistoryEntity
    {
        #region 属性

        /// <summary>
        ///  history_id   
        /// </summary>
        [SugarColumn(ColumnName = "HISTORY_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string history_id { get; set; }

        /// <summary>
        ///  task_id   
        /// </summary>
        [SugarColumn(ColumnName = "TASK_ID", ColumnDescription = "")]
        public string task_id { get; set; }

        /// <summary>
        ///  flow_id   
        /// </summary>
        [SugarColumn(ColumnName = "FLOW_ID", ColumnDescription = "")]
        public string flow_id { get; set; }

        /// <summary>
        ///  instance_id   
        /// </summary>
        [SugarColumn(ColumnName = "INSTANCE_ID", ColumnDescription = "")]
        public string instance_id { get; set; }

        /// <summary>
        ///  process_id   
        /// </summary>
        [SugarColumn(ColumnName = "PROCESS_ID", ColumnDescription = "")]
        public string process_id { get; set; }

        /// <summary>
        ///  node_id   
        /// </summary>
        [SugarColumn(ColumnName = "NODE_ID", ColumnDescription = "")]
        public string node_id { get; set; }

        /// <summary>
        ///  nodename   
        /// </summary>
        [SugarColumn(ColumnName = "NODENAME", ColumnDescription = "")]
        public string nodename { get; set; }

        /// <summary>
        ///  nodetype  
        /// </summary>
        [SugarColumn(ColumnName = "NODETYPE", ColumnDescription = "")]
        public string nodetype { get; set; }

        /// <summary>
        ///  is_finished   
        /// </summary>
        [SugarColumn(ColumnName = "IS_FINISHED", ColumnDescription = "")]
        public int is_finished { get; set; }

        /// <summary>
        ///  previous_id   
        /// </summary>
        [SugarColumn(ColumnName = "PREVIOUS_ID", ColumnDescription = "")]
        public string previous_id { get; set; }

        /// <summary>
        ///  previousname   
        /// </summary>
        [SugarColumn(ColumnName = "PREVIOUSNAME", ColumnDescription = "")]
        public string previousname { get; set; }

        /// <summary>
        ///  result   1同意2拒绝
        /// </summary>
        [SugarColumn(ColumnName = "RESULT", ColumnDescription = "1同意2拒绝")]
        public int result { get; set; }

        /// <summary>
        ///  opinion   
        /// </summary>
        [SugarColumn(ColumnName = "OPINION", ColumnDescription = "")]
        public string opinion { get; set; }

        /// <summary>
        ///  authorize_user_id   
        /// </summary>
        [SugarColumn(ColumnName = "AUTHORIZE_USER_ID", ColumnDescription = "")]
        public string authorize_user_id { get; set; }

        /// <summary>
        ///  authorizen_userame   
        /// </summary>
        [SugarColumn(ColumnName = "AUTHORIZEN_USERAME", ColumnDescription = "")]
        public string authorizen_userame { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        [SugarColumn(ColumnName = "CREATETIME", ColumnDescription = "")]
        public DateTime? createtime { get; set; }

        /// <summary>
        ///  create_userid   
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_USERID", ColumnDescription = "")]
        public string create_userid { get; set; }

        /// <summary>
        ///  create_username   
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_USERNAME", ColumnDescription = "")]
        public string create_username { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

        #endregion

        #region 扩展
        [SugarColumn(IsIgnore = true)]
        public string submit_user_id { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string submit_username { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string flowname { get; set; }
        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.history_id.IsEmpty())
            {
                this.history_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        #endregion

    }
}
