using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_flow_instance   
    /// </summary>
    [Table(Name = "WF_FLOW_INSTANCE")]
    public class wf_flow_instanceEntity
    {
        #region 属性

        /// <summary>
        ///  instance_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string instance_id { get; set; }

        /// <summary>
        ///  flow_id   
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        ///  process_id   单据编码
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        ///  flowcode   
        /// </summary>
        public string flowcode { get; set; }

        /// <summary>
        ///  flowname   
        /// </summary>
        public string flowname { get; set; }

        /// <summary>
        ///  processname   
        /// </summary>
        public string processname { get; set; }

        /// <summary>
        ///  processcontent   表单数据
        /// </summary>
        public string processcontent { get; set; }

        /// <summary>
        ///  submit_user_id    单据提交人 后期 同公司 同部门 根据这个值计算
        /// </summary>
        public string submit_user_id { get; set; }

        /// <summary>
        ///  submit_username    单据提交人 后期 同公司 同部门 根据这个值计算
        /// </summary>
        public string submit_username { get; set; }

        /// <summary>
        ///  is_finished   
        /// </summary>
        public int is_finished { get; set; }

        /// <summary>
        ///  schemejson   
        /// </summary>
        public string schemejson { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///  create_userId   
        /// </summary>
        public string create_userId { get; set; }

        /// <summary>
        ///  create_username   
        /// </summary>
        public string create_username { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        public DateTime? createtime { get; set; }

        /// <summary>
        ///  company_id   
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        ///  department_id   
        /// </summary>
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
