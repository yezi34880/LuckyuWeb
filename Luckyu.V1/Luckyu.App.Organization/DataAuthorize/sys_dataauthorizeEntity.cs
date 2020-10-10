
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_dataauthorize   
    /// </summary>
    [SugarTable("SYS_DATAAUTHORIZE", "")]
    public class sys_dataauthorizeEntity
    {
        #region 属性

        /// <summary>
        ///  auth_id   
        /// </summary>
        [SugarColumn(ColumnName = "AUTH_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string auth_id { get; set; }

        /// <summary>
        ///  modulename   
        /// </summary>
        [SugarColumn(ColumnName = "MODULENAME", ColumnDescription = "")]
        public string modulename { get; set; }

        /// <summary>
        ///  objectrange   0自定义1同公司2同部门3同小组9全部
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTRANGE", ColumnDescription = "0自定义1同公司2同部门3同小组9全部")]
        public int objectrange { get; set; }

        /// <summary>
        ///  objecttype   0用户1岗位2角色
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTTYPE", ColumnDescription = "0用户1岗位2角色")]
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECT_ID", ColumnDescription = "")]
        public string object_id { get; set; }

        /// <summary>
        ///  objectname   
        /// </summary>
        [SugarColumn(ColumnName = "OBJECTNAME", ColumnDescription = "")]
        public string objectname { get; set; }

        /// <summary>
        ///  is_enable   
        /// </summary>
        [SugarColumn(ColumnName = "IS_ENABLE", ColumnDescription = "")]
        public int is_enable { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

        /// <summary>
        ///  create_userId   
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

        /// <summary>
        ///  edittime   
        /// </summary>
        [SugarColumn(ColumnName = "EDITTIME", ColumnDescription = "")]
        public DateTime? edittime { get; set; }

        /// <summary>
        ///  edit_userid   
        /// </summary>
        [SugarColumn(ColumnName = "EDIT_USERID", ColumnDescription = "")]
        public string edit_userid { get; set; }

        /// <summary>
        ///  edit_username   
        /// </summary>
        [SugarColumn(ColumnName = "EDIT_USERNAME", ColumnDescription = "")]
        public string edit_username { get; set; }

        /// <summary>
        ///  is_delete   
        /// </summary>
        [SugarColumn(ColumnName = "IS_DELETE", ColumnDescription = "")]
        public int is_delete { get; set; }

        /// <summary>
        ///  delete_userid   
        /// </summary>
        [SugarColumn(ColumnName = "DELETE_USERID", ColumnDescription = "")]
        public string delete_userid { get; set; }

        /// <summary>
        ///  delete_username   
        /// </summary>
        [SugarColumn(ColumnName = "DELETE_USERNAME", ColumnDescription = "")]
        public string delete_username { get; set; }

        /// <summary>
        ///  deletetime   
        /// </summary>
        [SugarColumn(ColumnName = "DELETETIME", ColumnDescription = "")]
        public DateTime? deletetime { get; set; }

        #endregion

        #region 扩展
        [SugarColumn(IsIgnore = true)]
        public int seeobjecttype { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string seeobjectnames { get; set; }

        [SugarColumn(IsIgnore = true)]
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
