using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_task   
    /// </summary>
    [SugarTable("WF_TASK", "")]
    public class wf_taskEntity
    {
        #region 属性

        /// <summary>
        ///  task_id   
        /// </summary>
        [SugarColumn(ColumnName = "TASK_ID", IsPrimaryKey = true, ColumnDescription = "")]
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
        ///  processname   
        /// </summary>
        [SugarColumn(ColumnName = "PROCESSNAME", ColumnDescription = "")]
        public string processname { get; set; }

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
        ///  is_done   1生成历史
        /// </summary>
        [SugarColumn(ColumnName = "IS_DONE", ColumnDescription = "")]
        public int is_done { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

        #endregion

        #region 扩展
        /// <summary>
        /// 待审批 能看到该步骤的用户
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<wf_task_authorizeEntity> authrizes { get; set; }

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
            if (this.task_id.IsEmpty())
            {
                this.task_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        #endregion
    }
}
