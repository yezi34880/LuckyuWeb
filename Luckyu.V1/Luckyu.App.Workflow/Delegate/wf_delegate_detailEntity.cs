using FreeSql.DataAnnotations;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_delegate_detail   
    /// </summary>
    [Table(Name ="WF_DELEGATE_DETAIL")]
    public class wf_delegate_detailEntity
    {
        #region 属性
        /// <summary>
        ///  detail_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string detail_id { get; set; }

        /// <summary>
        ///  delegate_id   
        /// </summary>
        public string delegate_id { get; set; }

        /// <summary>
        ///  delegatetype   
        /// </summary>
        public int delegatetype { get; set; }

        /// <summary>
        ///  flowcode   
        /// </summary>
        public string flowcode { get; set; }

        /// <summary>
        ///  to_user_id   
        /// </summary>
        public string to_user_id { get; set; }

        /// <summary>
        ///  to_username   
        /// </summary>
        public string to_username { get; set; }

        #endregion


    }
}
