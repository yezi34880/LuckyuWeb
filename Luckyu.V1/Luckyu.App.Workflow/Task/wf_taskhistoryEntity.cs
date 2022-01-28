using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_taskhistory   
    /// </summary>
    [Table(Name ="WF_TASKHISTORY")]
    public class wf_taskhistoryEntity
    {
        #region 属性

        /// <summary>
        ///  history_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string history_id { get; set; }

        /// <summary>
        ///  task_id   
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        ///  flow_id   
        /// </summary>
        public string flow_id { get; set; }

        /// <summary>
        ///  instance_id   
        /// </summary>
        public string instance_id { get; set; }

        /// <summary>
        ///  process_id   
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        ///  node_id   
        /// </summary>
        public string node_id { get; set; }

        /// <summary>
        ///  nodename   
        /// </summary>
        public string nodename { get; set; }

        /// <summary>
        ///  nodetype  
        /// </summary>
        public string nodetype { get; set; }

        /// <summary>
        ///  previous_id   
        /// </summary>
        public string previous_id { get; set; }

        /// <summary>
        ///  previousname   
        /// </summary>
        public string previousname { get; set; }

        /// <summary>
        ///  result   1同意 2拒绝 3会签办理
        /// </summary>
        public int result { get; set; }

        /// <summary>
        ///  opinion   
        /// </summary>
        public string opinion { get; set; }

        /// <summary>
        /// tasktime 任务时间
        /// </summary>
        public DateTime tasktime { get; set; }

        /// <summary>
        ///  app_userid   
        /// </summary>
        public string app_userid { get; set; }

        /// <summary>
        ///  app_username   
        /// </summary>
        public string app_username { get; set; }

        /// <summary>
        ///  appremark   审批备注
        /// </summary>
        public string appremark { get; set; }

        /// <summary>
        ///  processjson   表单数据Json
        /// </summary>
        public string processjson { get; set; }

        /// <summary>
        /// apptime 审批时间
        /// </summary>
        public DateTime apptime { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        public DateTime? createtime { get; set; }

        /// <summary>
        ///  create_userid   
        /// </summary>
        public string create_userid { get; set; }

        /// <summary>
        ///  create_username   
        /// </summary>
        public string create_username { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        #endregion

        #region 扩展
        [Column(IsIgnore = true)]
        public string annex { get; set; }
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
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        #endregion

    }
}
