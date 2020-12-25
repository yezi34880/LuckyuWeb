
using FreeSql.DataAnnotations;
using Luckyu.Log;
using Luckyu.Utility;
using System;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_dataauthorize   
    /// </summary>
    [Table(Name = "SYS_DATAAUTHORIZE")]
    public class sys_dataauthorizeEntity
    {
        #region 属性

        /// <summary>
        ///  auth_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string auth_id { get; set; }

        /// <summary>
        ///  modulename   
        /// </summary>
        public string modulename { get; set; }

        /// <summary>
        ///  objectrange   0自定义1同公司2同部门3同小组9全部
        /// </summary>
        public int objectrange { get; set; }

        /// <summary>
        ///  objecttype   0用户1岗位2角色
        /// </summary>
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        ///  objectname   
        /// </summary>
        public string objectname { get; set; }

        /// <summary>
        ///  edittype   
        /// </summary>
        public int edittype { get; set; }

        /// <summary>
        ///  staterange   
        /// </summary>
        public int staterange { get; set; }

        /// <summary>
        ///  is_enable   
        /// </summary>
        public int is_enable { get; set; }

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

        #endregion

        #region 扩展
        [Column(IsIgnore = true)]
        public int seeobjecttype { get; set; }

        [Column(IsIgnore = true)]
        public string seeobjectnames { get; set; }

        [Column(IsIgnore = true)]
        public string seeobject_ids { get; set; }
        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.auth_id.IsEmpty())
            {
                this.auth_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.auth_id = keyValue;
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
