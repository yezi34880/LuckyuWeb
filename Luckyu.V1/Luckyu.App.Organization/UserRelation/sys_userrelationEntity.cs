using Luckyu.Log;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_userrelation   
    /// </summary>
    [SugarTable("SYS_USERRELATION", "")]
    public class sys_userrelationEntity
    {
        #region 属性

        /// <summary>
        ///  id   
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string id { get; set; }

        /// <summary>
        ///  user_id   
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID", ColumnDescription = "")]
        public string user_id { get; set; }

        /// <summary>
        ///  relationtype   1-角色2-岗位3-用户组4-部门主管
        /// </summary>
        [SugarColumn(ColumnName = "RELATIONTYPE", ColumnDescription = "1-角色2-岗位3-用户组4-部门主管")]
        public int relationtype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECT_ID", ColumnDescription = "")]
        public string object_id { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

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
        ///  createtime   
        /// </summary>
        [SugarColumn(ColumnName = "CREATETIME", ColumnDescription = "")]
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
