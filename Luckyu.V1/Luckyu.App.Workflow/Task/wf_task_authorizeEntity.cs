using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;

namespace Luckyu.App.Workflow
{
    /// <summary>
    ///  wf_task_authorize   
    /// </summary>
    [Table(Name = "WF_TASK_AUTHORIZE")]
    public class wf_task_authorizeEntity
    {
        #region 属性

        /// <summary>
        ///  auth_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string auth_id { get; set; }

        /// <summary>
        ///  task_id   
        /// </summary>
        public string task_id { get; set; }

        /// <summary>
        ///  user_id   
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        ///  department_id   
        /// </summary>
        public string department_id { get; set; }

        /// <summary>
        ///  company_id   
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        ///  role_id   
        /// </summary>
        public string role_id { get; set; }

        /// <summary>
        ///  post_id   
        /// </summary>
        public string post_id { get; set; }

        /// <summary>
        ///  group_id   
        /// </summary>
        public string group_id { get; set; }

        /// <summary>
        ///  manage_dept_id   
        /// </summary>
        public string manage_dept_id { get; set; }

        /// <summary>
        ///  is_add   1 加签  3  协办
        /// </summary>
        public int is_add { get; set; }

        /// <summary>
        ///  create_userid   
        /// </summary>
        public string create_userid { get; set; }
        #endregion

        #region 扩展

        #endregion

        #region 方法
        public void Create(UserModel loginInfo)
        {
            if (this.auth_id.IsEmpty())
            {
                this.auth_id = SnowflakeHelper.NewCode();
            }
            this.create_userid = loginInfo.user_id;
        }
        #endregion

    }
}
