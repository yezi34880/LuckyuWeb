using SqlSugar;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_authorize   
    /// </summary>
    [SugarTable("sys_authorize")]
    public class sys_authorizeEntity
    {
        #region 属性

        /// <summary>
        ///  id   
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string id { get; set; }

        /// <summary>
        ///  objecttype   1-角色2-用户
        /// </summary>
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        ///  itemtype   1-菜单2-按钮
        /// </summary>
        public int itemtype { get; set; }

        /// <summary>
        ///  item_id   
        /// </summary>
        public string item_id { get; set; }

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

            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
            this.createtime = DateTime.Now;
        }
        #endregion
    }
}
