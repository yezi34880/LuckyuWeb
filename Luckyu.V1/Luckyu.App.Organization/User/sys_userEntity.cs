
using Luckyu.Log;
using FreeSql;
using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;
using Luckyu.Utility;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_user   
    /// </summary>
    [Table(Name = "SYS_USER")]
    public class sys_userEntity
    {
        #region 属性

        /// <summary>
        ///  user_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string user_id { get; set; }

        /// <summary>
        ///  company_id   
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        ///  department_id   
        /// </summary>
        public string department_id { get; set; }

        /// <summary>
        ///  usercode   
        /// </summary>
        public string usercode { get; set; }

        /// <summary>
        ///  realname   
        /// </summary>
        public string realname { get; set; }

        /// <summary>
        ///  nickname   
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        ///  loginname   
        /// </summary>
        public string loginname { get; set; }

        /// <summary>
        ///  loginpassword   
        /// </summary>
        public string loginpassword { get; set; }

        /// <summary>
        ///  loginsecret   
        /// </summary>
        public string loginsecret { get; set; }

        /// <summary>
        ///  mobile   
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        ///  email   
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///  sex   1男 2女
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        ///  birthday   
        /// </summary>
        public DateTime? birthday { get; set; }

        /// <summary>
        ///  qq   
        /// </summary>
        public string qq { get; set; }

        /// <summary>
        ///  wechat   
        /// </summary>
        public string wechat { get; set; }

        /// <summary>
        ///  level   0-一般用户1-管理员2-超级管理员
        /// </summary>
        public int level { get; set; }

        /// <summary>
        ///  sort   
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

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
        ///  delete_userid   
        /// </summary>
        public string delete_userid { get; set; }

        /// <summary>
        ///  delete_username   
        /// </summary>
        public string delete_username { get; set; }

        /// <summary>
        ///  deletetime   
        /// </summary>
        public DateTime? deletetime { get; set; }

        /// <summary>
        ///  is_enable   
        /// </summary>
        public int is_enable { get; set; }

        #endregion

        #region 扩展
        [Column(IsIgnore = true)]
        public string np_roles { get; set; }

        [Column(IsIgnore = true)]
        public string np_posts { get; set; }

        [Column(IsIgnore = true)]
        public string np_groups { get; set; }

        [Column(IsIgnore = true)]
        public string np_depts { get; set; }
        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.user_id.IsEmpty())
            {
                this.user_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.user_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        public void Remove(UserModel loginInfo)
        {
            this.is_delete = 1;
            this.deletetime = DateTime.Now;
            this.delete_userid = loginInfo.user_id;
            this.delete_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        #endregion

    }
}
