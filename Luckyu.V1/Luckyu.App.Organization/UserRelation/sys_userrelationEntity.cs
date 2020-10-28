using Luckyu.Log;
using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_userrelation   
    /// </summary>
    [Table(Name = "SYS_USERRELATION")]
    public class sys_userrelationEntity
    {
        #region 属性

        /// <summary>
        ///  id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string id { get; set; }

        /// <summary>
        ///  user_id   
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        ///  relationtype   1-角色2-岗位3-用户组4-部门主管
        /// </summary>
        public int relationtype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        public string object_id { get; set; }

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

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            this.id = SnowflakeHelper.NewCode();
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
            this.createtime = DateTime.Now;
        }
        #endregion
    }
}
