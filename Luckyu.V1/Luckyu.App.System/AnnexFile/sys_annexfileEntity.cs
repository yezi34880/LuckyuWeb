using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.Collections.Generic;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_annexfile   
    /// </summary>
    [Table(Name = "SYS_ANNEXFILE")]
    public class sys_annexfileEntity
    {
        #region 属性

        /// <summary>
        ///  annex_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string annex_id { get; set; }

        /// <summary>
        ///  external_id   
        /// </summary>
        public string external_id { get; set; }

        /// <summary>
        ///  filename   
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        ///  basepath   
        /// </summary>
        public string basepath { get; set; }

        /// <summary>
        ///  filepath   
        /// </summary>
        public string filepath { get; set; }

        /// <summary>
        ///  filesize   
        /// </summary>
        public int filesize { get; set; }

        /// <summary>
        ///  fileextenssion   
        /// </summary>
        public string fileextenssion { get; set; }

        /// <summary>
        ///  contexttype   
        /// </summary>
        public string contexttype { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///  downloadcount   
        /// </summary>
        public int downloadcount { get; set; }

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

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.annex_id.IsEmpty())
            {
                this.annex_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        #endregion
    }
}
