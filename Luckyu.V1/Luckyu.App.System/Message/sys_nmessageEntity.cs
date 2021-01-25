using System;
using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.System
{
    /// <summary>
    /// sys_message  消息
    /// </summary>
    [Table(Name = "SYS_MESSAGE")]
    public class sys_messageEntity
    {
        #region 属性
        /// <summary>
        /// message_id
        /// </summary>
        [Column(IsPrimary = true)]
        public string message_id { get; set; }

        /// <summary>
        ///  catetory   
        /// </summary>
        public string catetory { get; set; }

        /// <summary>
        ///  contents   
        /// </summary>
        public string contents { get; set; }

        /// <summary>
        /// to_userid
        /// </summary>
        public string to_userid { get; set; }

        /// <summary>
        /// to_username
        /// </summary>
        public string to_username { get; set; }

        /// <summary>
        /// send_userid
        /// </summary>
        public string send_userid { get; set; }

        /// <summary>
        /// send_username
        /// </summary>
        public string send_username { get; set; }

        /// <summary>
        /// sendtime
        /// </summary>
        public DateTime sendtime { get; set; }

        /// <summary>
        /// is_read
        /// </summary>
        public int is_read { get; set; }

        /// <summary>
        /// readtime
        /// </summary>
        public DateTime readtime { get; set; }

        /// <summary>
        ///  create_userid   
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

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.message_id.IsEmpty())
            {
                this.message_id = SnowflakeHelper.NewCode();
            }
            if (this.catetory.IsEmpty())
            {
                this.catetory = "系统通知";
            }

            this.send_userid = loginInfo.user_id;
            this.send_username = $"{loginInfo.realname}-{loginInfo.loginname}";
            this.sendtime = DateTime.Now;

            this.is_read = 0;
            this.readtime = LuckyuHelper.MinDate;

            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }

        #endregion
    }
}
