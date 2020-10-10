using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_company   
    /// </summary>
    [SugarTable("SYS_COMPANY", "")]
    public class sys_companyEntity : ExtensionEntityBase
    {
        #region 属性

        /// <summary>
        ///  company_id   
        /// </summary>
        [SugarColumn(ColumnName = "COMPANY_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string company_id { get; set; }

        /// <summary>
        ///  parent_id   
        /// </summary>
        [SugarColumn(ColumnName = "PARENT_ID", ColumnDescription = "")]
        public string parent_id { get; set; }

        /// <summary>
        ///  companycode   
        /// </summary>
        [SugarColumn(ColumnName = "COMPANYCODE", ColumnDescription = "")]
        public string companycode { get; set; }

        /// <summary>
        ///  fullname   
        /// </summary>
        [SugarColumn(ColumnName = "FULLNAME", ColumnDescription = "")]
        public string fullname { get; set; }

        /// <summary>
        ///  shortname   
        /// </summary>
        [SugarColumn(ColumnName = "SHORTNAME", ColumnDescription = "")]
        public string shortname { get; set; }

        /// <summary>
        ///  legalperson   
        /// </summary>
        [SugarColumn(ColumnName = "LEGALPERSON", ColumnDescription = "")]
        public string legalperson { get; set; }

        /// <summary>
        ///  email   
        /// </summary>
        [SugarColumn(ColumnName = "EMAIL", ColumnDescription = "")]
        public string email { get; set; }

        /// <summary>
        ///  phone   
        /// </summary>
        [SugarColumn(ColumnName = "PHONE", ColumnDescription = "")]
        public string phone { get; set; }

        /// <summary>
        ///  country   
        /// </summary>
        [SugarColumn(ColumnName = "COUNTRY", ColumnDescription = "")]
        public string country { get; set; }

        /// <summary>
        ///  province   
        /// </summary>
        [SugarColumn(ColumnName = "PROVINCE", ColumnDescription = "")]
        public string province { get; set; }

        /// <summary>
        ///  city   
        /// </summary>
        [SugarColumn(ColumnName = "CITY", ColumnDescription = "")]
        public string city { get; set; }

        /// <summary>
        ///  counties   县区
        /// </summary>
        [SugarColumn(ColumnName = "COUNTIES", ColumnDescription = "县区")]
        public string counties { get; set; }

        /// <summary>
        ///  address   
        /// </summary>
        [SugarColumn(ColumnName = "ADDRESS", ColumnDescription = "")]
        public string address { get; set; }

        /// <summary>
        ///  foundeddate   成立时间
        /// </summary>
        [SugarColumn(ColumnName = "FOUNDEDDATE", ColumnDescription = "成立时间")]
        public DateTime? foundeddate { get; set; }

        /// <summary>
        ///  website   主页
        /// </summary>
        [SugarColumn(ColumnName = "WEBSITE", ColumnDescription = "主页")]
        public string website { get; set; }

        /// <summary>
        ///  sort   
        /// </summary>
        [SugarColumn(ColumnName = "SORT", ColumnDescription = "")]
        public int sort { get; set; }

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
        ///  deletetime   
        /// </summary>
        [SugarColumn(ColumnName = "DELETETIME", ColumnDescription = "")]
        public DateTime? deletetime { get; set; }

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
        ///  is_enable   
        /// </summary>
        [SugarColumn(ColumnName = "IS_ENABLE", ColumnDescription = "")]
        public int is_enable { get; set; }

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.company_id.IsEmpty())
            {
                this.company_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;

            this.parent_id = this.parent_id.IsEmpty() ? "0" : this.parent_id;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.company_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = loginInfo.realname;

            this.parent_id = this.parent_id.IsEmpty() ? "0" : this.parent_id;
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
