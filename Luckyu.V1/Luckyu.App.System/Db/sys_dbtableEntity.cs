using System;
using SqlSugar;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_dbtable   数据库 表
    /// </summary>
    [SugarTable( "sys_dbtable")]
    public class sys_dbtableEntity
    {
        /// <summary>
        /// table_id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
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
