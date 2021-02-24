using Luckyu.App.Organization;
using Luckyu.Log;
using Luckyu.Utility;
using FreeSql.DataAnnotations;
using System;

namespace Luckyu.App.OA
{
    /// <summary>
    ///  oa_carorder   
    /// </summary>
    [Table(Name = "OA_CARORDER")]
    public class oa_carorderEntity : ExtensionEntityBase
    {
        #region 属性
        /// <summary>
        ///  order_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string order_id { get; set; }

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
        ///  begintime   
        /// </summary>
        public DateTime begintime { get; set; }

        /// <summary>
        ///  endtime   
        /// </summary>
        public DateTime endtime { get; set; }

        /// <summary>
        ///  place   
        /// </summary>
        public string place { get; set; }

        /// <summary>
        ///  reason   
        /// </summary>
        public string reason { get; set; }

        /// <summary>
        ///  carno   车牌号
        /// </summary>
        public string carno { get; set; }

        /// <summary>
        ///  isselfdrive   是否自驾
        /// </summary>
        public int isselfdrive { get; set; }

        /// <summary>
        ///  driver_id   驾驶员id
        /// </summary>
        public string driver_id { get; set; }

        /// <summary>
        ///  drivername   驾驶员姓名
        /// </summary>
        public string drivername { get; set; }

        /// <summary>
        ///  cost   费用
        /// </summary>
        public decimal cost { get; set; }

        /// <summary>
        ///  state   0 起草 1 生效 2 报批 -1 驳回
        /// </summary>
        public int state { get; set; }

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
            if (this.order_id.IsEmpty())
            {
                this.order_id = SnowflakeHelper.NewCode();
            }
            this.createtime = DateTime.Now;
            this.create_userid = loginInfo.user_id;
            this.create_username = $"{loginInfo.realname}-{loginInfo.loginname}";

            this.user_id = loginInfo.user_id;
            this.department_id = loginInfo.department_id;
            this.company_id = loginInfo.company_id;
        }
        public void Modify(string keyValue, UserModel loginInfo)
        {
            this.order_id = keyValue;
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
