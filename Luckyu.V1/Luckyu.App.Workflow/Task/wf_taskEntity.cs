using SqlSugar;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_task   
    /// </summary>
    [SugarTable( "wf_task")]
    public class wf_taskEntity
    {
        #region 属性

        /// <summary>
        ///  task_id   
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
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
        ///  processname   
        /// </summary>
        public string processname { get; set; }

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
        ///  app_userid   
        /// </summary>
        public string app_userid { get; set; }

        /// <summary>
        ///  app_username   
        /// </summary>
        public string app_username { get; set; }

        /// <summary>
        /// 结果 1-同意 2-拒绝
        /// </summary>
        public int result { get; set; }

        /// <summary>
        ///  opinion   
        /// </summary>
        public string opinion { get; set; }

        /// <summary>
        ///  previous_id   
        /// </summary>
        public string previous_id { get; set; }

        /// <summary>
        ///  previousname   
        /// </summary>
        public string previousname { get; set; }

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
        ///  is_done   1生成历史
        /// </summary>
        public int is_done { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        #endregion

        #region 扩展
        /// <summary>
        /// 待审批 能看到该步骤的用户
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<wf_task_authorizeEntity> authrizes { get; set; }
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
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        #endregion
    }
}
