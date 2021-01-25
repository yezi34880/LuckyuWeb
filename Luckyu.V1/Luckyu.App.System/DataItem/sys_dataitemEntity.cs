using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_dataitem   
    /// </summary>
    [Table(Name = "SYS_DATAITEM")]
    public class sys_dataitemEntity
    {
        #region 属性

        /// <summary>
        ///  dataitem_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string dataitem_id { get; set; }

        /// <summary>
        ///  itemcode   
        /// </summary>
        public string itemcode { get; set; }

        /// <summary>
        ///  itemname   
        /// </summary>
        public string itemname { get; set; }

        /// <summary>
        ///  parent_id   
        /// </summary>
        public string parent_id { get; set; }

        /// <summary>
        ///  sort   
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

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

        /// <summary>
        ///  is_system   
        /// </summary>
        public int is_system { get; set; }

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.dataitem_id.IsEmpty())
            {
                this.dataitem_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";

            this.parent_id = this.parent_id.IsEmpty() ? "0" : this.parent_id;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.dataitem_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = $"{loginInfo.realname}-{loginInfo.loginname}";

            this.parent_id = this.parent_id.IsEmpty() ? "0" : this.parent_id;
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
