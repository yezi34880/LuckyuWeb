using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;

namespace Luckyu.App.OA
{
    /// <summary>
    ///  oa_news   
    /// </summary>
    [SugarTable("OA_NEWS", "")]
    public class oa_newsEntity : ExtensionEntityBase
    {
        #region 属性
        /// <summary>
        ///  id   
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string id { get; set; }

        /// <summary>
        ///  title   
        /// </summary>
        [SugarColumn(ColumnName = "TITLE", ColumnDescription = "")]
        public string title { get; set; }

        /// <summary>
        ///  catetory   
        /// </summary>
        [SugarColumn(ColumnName = "CATETORY", ColumnDescription = "")]
        public string catetory { get; set; }

        /// <summary>
        ///  keywords   
        /// </summary>
        [SugarColumn(ColumnName = "KEYWORDS", ColumnDescription = "")]
        public string keywords { get; set; }

        /// <summary>
        ///  source   
        /// </summary>
        [SugarColumn(ColumnName = "SOURCE", ColumnDescription = "")]
        public string source { get; set; }

        /// <summary>
        ///  publishtime   
        /// </summary>
        [SugarColumn(ColumnName = "PUBLISHTIME", ColumnDescription = "")]
        public DateTime publishtime { get; set; }

        /// <summary>
        ///  contents   
        /// </summary>
        [SugarColumn(ColumnName = "CONTENTS", ColumnDescription = "")]
        public string contents { get; set; }

        /// <summary>
        ///  is_publish
        /// </summary>
        [SugarColumn(ColumnName = "IS_PUBLISH", ColumnDescription = "")]
        public int is_publish { get; set; }

        /// <summary>
        ///  state   0 起草 1 生效 2 报批 -1 驳回
        /// </summary>
        [SugarColumn(ColumnName = "STATE", ColumnDescription = "")]
        public int state { get; set; }

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

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            this.id = SnowflakeHelper.NewCode();
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
            this.state = 1;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.id = keyValue;
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
