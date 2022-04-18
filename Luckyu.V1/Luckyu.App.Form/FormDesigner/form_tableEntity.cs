
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;

namespace Luckyu.App.Form
{
    /// <summary>
    ///  form_table 
    /// </summary>
    [SugarTable( "form_table")]
    public class form_tableEntity
    {
        #region 属性
        /// <summary>
        ///  form_id 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string form_id { get; set; }

        /// <summary>
        ///  formcode 
        /// </summary>
        public string formcode { get; set; }

        /// <summary>
        ///  formname 
        /// </summary>
        public string formname { get; set; }

        /// <summary>
        ///  dbname 默认为 【cusform_】+formcode
        /// </summary>
        public string dbname { get; set; }

        /// <summary>
        ///  formjson 表单格式
        /// </summary>
        public string formjson { get; set; }

        /// <summary>
        /// formhtml 
        /// </summary>
        public string formhtml { get; set; }

        /// <summary>
        ///  hasdetails 
        /// </summary>
        public int hasdetails { get; set; }

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
        ///  edit_userid 
        /// </summary>
        public string edit_userid { get; set; }

        /// <summary>
        ///  edit_username 
        /// </summary>
        public string edit_username { get; set; }

        /// <summary>
        ///  edittime 
        /// </summary>
        public DateTime? edittime { get; set; }

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

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.form_id.IsEmpty())
            {
                this.form_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.form_id = keyValue;
            this.edittime = DateTime.Now;
            this.edit_userid = loginInfo.user_id;
            this.edit_username = $"{loginInfo.realname}-{loginInfo.loginname}";
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