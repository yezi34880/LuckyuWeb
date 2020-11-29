using System;
using FreeSql.DataAnnotations;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_dbtable   
    /// </summary>
    [Table(Name = "SYS_DBTABLE")]
    public class sys_dbtableEntity
    {
        /// <summary>
        /// table_id
        /// </summary>
        [Column(IsPrimary = true)]
        public string table_id { get; set; }

        /// <summary>
        /// dbname
        /// </summary>
        public string dbname { get; set; }

        /// <summary>
        /// showname
        /// </summary>
        public string showname { get; set; }

        /// <summary>
        /// remark
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// create_userId
        /// </summary>
        public string create_userId { get; set; }

        /// <summary>
        /// create_username
        /// </summary>
        public string create_username { get; set; }

        /// <summary>
        /// createtime
        /// </summary>
        public DateTime? createtime { get; set; }

    }
}
