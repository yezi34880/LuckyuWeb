using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_dataitem_detail   
    /// </summary>
    [Table(Name = "SYS_DATAITEM_DETAIL")]
    public class sys_dataitem_detailEntity
    {
        #region 属性

        /// <summary>
        ///  detail_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string detail_id { get; set; }

        /// <summary>
        ///  dataitem_id   
        /// </summary>
        public string dataitem_id { get; set; }

        /// <summary>
        ///  itemcode   
        /// </summary>
        public string itemcode { get; set; }

        /// <summary>
        ///  showname   
        /// </summary>
        public string showname { get; set; }

        /// <summary>
        ///  itemvalue   
        /// </summary>
        public string itemvalue { get; set; }

        /// <summary>
        ///  itemvalue2   
        /// </summary>
        public string itemvalue2 { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///  sort   
        /// </summary>
        public int sort { get; set; }

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
            if (this.detail_id.IsEmpty())
            {
                this.detail_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.detail_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = loginInfo.realname;
        }
        public void Remove(UserModel loginInfo)
        {
            this.is_delete = 1;
            this.deletetime = DateTime.Now;
            this.delete_userid = loginInfo.user_id;
            this.delete_username = loginInfo.realname;
        }
        #endregion

    }

}
