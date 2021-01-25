using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_flow   
    /// </summary>
    [Table(Name = "WF_FLOW")]
    public class wf_flowEntity
    {
        #region 属性

        /// <summary>
        ///  flow_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string flow_id { get; set; }

        /// <summary>
        ///  flowcode   
        /// </summary>
        public string flowcode { get; set; }

        /// <summary>
        ///  flowname   
        /// </summary>
        public string flowname { get; set; }

        /// <summary>
        ///  flowtype   
        /// </summary>
        public string flowtype { get; set; }

        /// <summary>
        ///  flowrange   范围 0全部 1指定
        /// </summary>
        public int flowrange { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///  create_userId   
        /// </summary>
        public string create_userid { get; set; }

        /// <summary>
        ///  create_username   
        /// </summary>
        public string create_username { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        public DateTime? createtime { get; set; }

        /// <summary>
        ///  edittime   
        /// </summary>
        public DateTime? edittime { get; set; }

        /// <summary>
        ///  edit_userid   
        /// </summary>
        public string edit_userid { get; set; }

        /// <summary>
        ///  edit_username   
        /// </summary>
        public string edit_username { get; set; }

        /// <summary>
        ///  is_delete   
        /// </summary>
        public int is_delete { get; set; }

        /// <summary>
        ///  deletetime   
        /// </summary>
        public DateTime? deletetime { get; set; }

        /// <summary>
        ///  delete_userid   
        /// </summary>
        public string delete_userid { get; set; }

        /// <summary>
        ///  delete_username   
        /// </summary>
        public string delete_username { get; set; }

        /// <summary>
        ///  is_enable   
        /// </summary>
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
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.flow_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        public void Remove(UserModel loginInfo)
        {
            this.deletetime = DateTime.Now;
            this.delete_userid = loginInfo.user_id;
            this.delete_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        #endregion

    }
}
