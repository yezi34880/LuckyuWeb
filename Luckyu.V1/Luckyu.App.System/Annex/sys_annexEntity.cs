using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_annex   
    /// </summary>
    [SugarTable("SYS_ANNEX", "")]
    public class sys_annexEntity
    {
        #region 属性

        /// <summary>
        ///  annex_id   
        /// </summary>
        [SugarColumn(ColumnName = "ANNEX_ID", IsPrimaryKey = true, ColumnDescription = "")]
        public string annex_id { get; set; }

        /// <summary>
        ///  folder_id   
        /// </summary>
        [SugarColumn(ColumnName = "FOLDER_ID", ColumnDescription = "")]
        public string folder_id { get; set; }

        /// <summary>
        ///  filename   
        /// </summary>
        [SugarColumn(ColumnName = "FILENAME", ColumnDescription = "")]
        public string filename { get; set; }

        /// <summary>
        ///  filepath   
        /// </summary>
        [SugarColumn(ColumnName = "FILEPATH", ColumnDescription = "")]
        public string filepath { get; set; }

        /// <summary>
        ///  filesize   
        /// </summary>
        [SugarColumn(ColumnName = "FILESIZE", ColumnDescription = "")]
        public int filesize { get; set; }

        /// <summary>
        ///  fileextenssion   
        /// </summary>
        [SugarColumn(ColumnName = "FILEEXTENSSION", ColumnDescription = "")]
        public string fileextenssion { get; set; }

        /// <summary>
        ///  contexttype   
        /// </summary>
        [SugarColumn(ColumnName = "CONTEXTTYPE", ColumnDescription = "")]
        public string contexttype { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", ColumnDescription = "")]
        public string remark { get; set; }

        /// <summary>
        ///  downloadcount   
        /// </summary>
        [SugarColumn(ColumnName = "DOWNLOADCOUNT", ColumnDescription = "")]
        public int downloadcount { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        [SugarColumn(ColumnName = "CREATETIME", ColumnDescription = "")]
        public DateTime? createtime { get; set; }

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
