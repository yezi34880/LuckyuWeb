using FreeSql.DataAnnotations;
using Luckyu.App.Organization;
using Luckyu.Log;
using System;
using System.Collections.Generic;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_coderule_seed   
    /// </summary>
    [Table(Name = "SYS_CODERULE_SEED")]
    public class sys_coderule_seedEntity
    {
        #region 属性

        /// <summary>
        ///  seed_id   
        /// </summary>
        [Column(IsPrimary = true)]
        public string seed_id { get; set; }

        /// <summary>
        ///  rule_id   
        /// </summary>
        public string rule_id { get; set; }

        /// <summary>
        ///  seedprename   前缀
        /// </summary>
        public string seedprename { get; set; }

        /// <summary>
        ///  seedvalue   流水号
        /// </summary>
        public int seedvalue { get; set; }

        /// <summary>
        ///  remark   
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///  create_userId   
        /// </summary>
        public string create_userId { get; set; }

        /// <summary>
        ///  create_username   
        /// </summary>
        public string create_username { get; set; }

        /// <summary>
        ///  createtime   
        /// </summary>
        public DateTime? createtime { get; set; }

        #endregion

        #region 方法
        public sys_coderule_seedEntity()
        {
        }
        public sys_coderule_seedEntity(string ruleId, string preNumber)
        {
            this.rule_id = ruleId;
            this.seedprename = preNumber;
        }
        public void Create(UserModel loginInfo)
        {
            this.seed_id = SnowflakeHelper.NewCode();
            this.seedvalue = 1;
            this.createtime = DateTime.Now;

            this.create_userId = loginInfo.user_id;
            this.create_username = loginInfo.realname;
        }
        public void Modify(string keyValue)
        {
            this.seed_id = keyValue;
        }

        #endregion

    }
}
