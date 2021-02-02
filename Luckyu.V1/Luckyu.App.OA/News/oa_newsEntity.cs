using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;

namespace Luckyu.App.OA
{
    /// <summary>
    ///  oa_news   
    /// </summary>
    [Table(Name = "OA_NEWS")]
    public class oa_newsEntity : ExtensionEntityBase
    {
        #region 属性
        /// <summary>
        ///  news_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string news_id { get; set; }

        /// <summary>
        ///  title   
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///  category   
        /// </summary>
        public string category { get; set; }

        /// <summary>
        ///  keywords   
        /// </summary>
        public string keywords { get; set; }

        /// <summary>
        ///  source   
        /// </summary>
        public string source { get; set; }

        /// <summary>
        ///  publishtime   
        /// </summary>
        public DateTime publishtime { get; set; }

        /// <summary>
        ///  contents   
        /// </summary>
        public string contents { get; set; }

        /// <summary>
        ///  is_publish
        /// </summary>
        public int is_publish { get; set; }

        /// <summary>
        ///  state   0 起草 1 生效 2 报批 -1 驳回
        /// </summary>
        public int state { get; set; }

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

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.news_id.IsEmpty())
            {
                this.news_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";
            this.state = 1;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.news_id = keyValue;
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
