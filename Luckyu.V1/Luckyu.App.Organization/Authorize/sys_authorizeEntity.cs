
using Luckyu.Log;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_authorize   
    /// </summary>
    [SugarTable("SYS_AUTHORIZE", "")]
    public class sys_authorizeEntity
    {
        #region 属性

        /// <summary>
        ///  id   
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string id { get; set; }

        /// <summary>
        ///  objecttype   1-角色2-用户
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTTYPE", ColumnDescription = "1-角色2-用户")]
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECT_ID", ColumnDescription = "")]
        public string object_id { get; set; }

        /// <summary>
        ///  itemtype   1-菜单2-按钮
        /// </summary>
        [SugarColumn(ColumnName = "ITEMTYPE", ColumnDescription = "1-菜单2-按钮")]
        public int itemtype { get; set; }

        /// <summary>
        ///  item_id   
        /// </summary>
        [SugarColumn(ColumnName = "ITEM_ID", ColumnDescription = "")]
        public string item_id { get; set; }

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
