using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_flow_instance   
    /// </summary>
    [SugarTable("WF_FLOW_INSTANCE", "")]
    public class wf_flow_instanceEntity
    {
        #region 属性

        /// <summary>
        ///  instance_id   
        /// </summary>
        [SugarColumn(ColumnName = "INSTANCE_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string instance_id { get; set; }

        /// <summary>
        ///  flow_id   
        /// </summary>
        [SugarColumn(ColumnName = "FLOW_ID", ColumnDescription = "")]
        public string flow_id { get; set; }

        /// <summary>
        ///  process_id   单据编码
        /// </summary>
        [SugarColumn(ColumnName = "PROCESS_ID", ColumnDescription = "单据编码")]
        public string process_id { get; set; }

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
        ///  processname   
        /// </summary>
        [SugarColumn(ColumnName = "PROCESSNAME", ColumnDescription = "")]
        public string processname { get; set; }

        /// <summary>
        ///  processcontent   表单数据
        /// </summary>
        [SugarColumn(ColumnName = "PROCESSCONTENT", ColumnDescription = "")]
        public string processcontent { get; set; }

       /// <summary>
        ///  submit_user_id    单据提交人 后期 同公司 同部门 根据这个值计算
        /// </summary>
        [SugarColumn(ColumnName = "SUBMIT_USER_ID", ColumnDescription = "")]
        public string submit_user_id { get; set; }

        /// <summary>
        ///  submit_username    单据提交人 后期 同公司 同部门 根据这个值计算
        /// </summary>
        [SugarColumn(ColumnName = "SUBMIT_USERNAME", ColumnDescription = "")]
        public string submit_username { get; set; }


        /// <summary>
        ///  is_finished   
        /// </summary>
        [SugarColumn(ColumnName = "IS_FINISHED", ColumnDescription = "")]
        public int is_finished { get; set; }

        /// <summary>
        ///  schemejson   
        /// </summary>
        [SugarColumn(ColumnName = "SCHEMEJSON", ColumnDescription = "")]
        public string schemejson { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

        /// <summary>
        ///  create_userId   
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_USERID", ColumnDescription = "")]
        public string create_userId { get; set; }

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
        ///  company_id   
        /// </summary>
        [SugarColumn(ColumnName = "COMPANY_ID", ColumnDescription = "")]
        public string company_id { get; set; }

        /// <summary>
        ///  department_id   
        /// </summary>
        [SugarColumn(ColumnName = "DEPARTMENT_ID", ColumnDescription = "")]
        public string department_id { get; set; }

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.instance_id.IsEmpty())
            {
                this.instance_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userId = loginInfo.user_id;
            this.create_username = loginInfo.realname;
            this.department_id = loginInfo.department_id;
            this.company_id = loginInfo.company_id;
            this.is_finished = 0;
        }
        #endregion
    }
}
