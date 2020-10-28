using FreeSql.DataAnnotations;
using System;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_delegate   
    /// </summary>
    [Table(Name ="WF_DELEGATE")]
    public class wf_delegateEntity
    {
        #region 属性
        /// <summary>
        ///  delegate_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string delegate_id { get; set; }

        /// <summary>
        ///  user_id   
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        ///  username   
        /// </summary>
        public string username { get; set; }

        /// <summary>
        ///  department_id   
        /// </summary>
        public string department_id { get; set; }

        /// <summary>
        ///  company_id   
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        ///  starttime   
        /// </summary>
        public DateTime starttime { get; set; }

        /// <summary>
        ///  endtime   
        /// </summary>
        public DateTime endtime { get; set; }

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

        #endregion
    }
}
