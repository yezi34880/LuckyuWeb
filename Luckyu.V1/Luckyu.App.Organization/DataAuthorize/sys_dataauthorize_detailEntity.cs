
using FreeSql.DataAnnotations;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.Organization
{
    /// <summary>
    ///  sys_dataauthorize_detail   
    /// </summary>
    [Table(Name = "SYS_DATAAUTHORIZE_DETAIL")]
    public class sys_dataauthorize_detailEntity
    {
        #region 属性

        /// <summary>
        ///  detail_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string detail_id { get; set; }

        /// <summary>
        ///  auth_id   
        /// </summary>
        public string auth_id { get; set; }

        /// <summary>
        ///  objecttype   0用户1公司2部门
        /// </summary>
        public int objecttype { get; set; }

        /// <summary>
        ///  object_id   
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        ///  objectname   
        /// </summary>
        public string objectname { get; set; }

        #endregion

        #region 方法
        public void Create(string authId)
        {
            if (this.detail_id.IsEmpty())
            {
                this.detail_id = SnowflakeHelper.NewCode();
            }
            this.auth_id = authId;
        }
        #endregion
    }
}
