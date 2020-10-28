using FreeSql.DataAnnotations;
using Luckyu.Log;
using Luckyu.Utility;
using System;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_company   
    /// </summary>
    [Table(Name = "SYS_COMPANY")]
    public class sys_companyEntity : ExtensionEntityBase
    {
        #region 属性

        /// <summary>
        ///  company_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string company_id { get; set; }

        /// <summary>
        ///  parent_id   
        /// </summary>
        public string parent_id { get; set; }

        /// <summary>
        ///  companycode   
        /// </summary>
        public string companycode { get; set; }

        /// <summary>
        ///  fullname   
        /// </summary>
        public string fullname { get; set; }

        /// <summary>
        ///  shortname   
        /// </summary>
        public string shortname { get; set; }

        /// <summary>
        ///  legalperson   
        /// </summary>
        public string legalperson { get; set; }

        /// <summary>
        ///  email   
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///  phone   
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        ///  country   
        /// </summary>
        public string country { get; set; }

        /// <summary>
        ///  province   
        /// </summary>
        public string province { get; set; }

        /// <summary>
        ///  city   
        /// </summary>
        public string city { get; set; }

        /// <summary>
        ///  counties   县区
        /// </summary>
        public string counties { get; set; }

        /// <summary>
        ///  address   
        /// </summary>
        public string address { get; set; }

        /// <summary>
        ///  foundeddate   成立时间
        /// </summary>
        public DateTime? foundeddate { get; set; }

        /// <summary>
        ///  website   主页
        /// </summary>
        public string website { get; set; }

        /// <summary>
        ///  sort   
        /// </summary>
        public int sort { get; set; }

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
