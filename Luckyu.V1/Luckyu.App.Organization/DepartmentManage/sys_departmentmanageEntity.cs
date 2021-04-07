using Luckyu.Log;
using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;
using Luckyu.Utility;

namespace Luckyu.App.Organization
{
    /// <summary>
    /// sys_departmentmanage
    /// </summary>
    [Table(Name = "SYS_DEPARTMENTMANAGE")]
    public class sys_departmentmanageEntity
    {
        #region 属性
        /// <summary>
        /// id
        /// </summary>
        [Column(IsPrimary = true)]
        public string id { get; set; }

        /// <summary>
        /// user_id
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// department_id
        /// </summary>
        public string department_id { get; set; }

        /// <summary>
        /// departmentname
        /// </summary>
        public string departmentname { get; set; }

        /// <summary>
        /// relationtype  1-角色 2-岗位
        /// </summary>
        public int relationtype { get; set; }

        /// <summary>
        /// object_id
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        /// objectname
        /// </summary>
        public string objectname { get; set; }

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
            if (this.id.IsEmpty())
            {
                this.id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        #endregion
    }
}
