using System;
using FreeSql.DataAnnotations;

namespace Luckyu.App.System
{
    /// <summary>
    ///  sys_dbcolumn   数据库列
    /// </summary>
    [Table(Name = "SYS_DBCOLUMN")]
    public class sys_dbcolumnEntity
    {
        /// <summary>
        /// column_id
        /// </summary>
        [Column(IsPrimary = true)]
        public string column_id { get; set; }

        /// <summary>
        /// table_id
        /// </summary>
        public string table_id { get; set; }

        /// <summary>
        /// dbcolumnname
        /// </summary>
        public string dbcolumnname { get; set; }

        /// <summary>
        /// showcolumnname
        /// </summary>
        public string showcolumnname { get; set; }

        /// <summary>
        /// dbtype
        /// </summary>
        public string dbtype { get; set; }

        /// <summary>
        /// showtype
        /// </summary>
        public string showtype { get; set; }

        /// <summary>
        /// showformat
        /// </summary>
        public string showformat { get; set; }

        /// <summary>
        /// layverify
        /// </summary>
        public string layverify { get; set; }

        /// <summary>
        /// laylength
        /// </summary>
        public string laylength { get; set; }

        /// <summary>
        /// is_hidden
        /// </summary>
        public string is_hidden { get; set; }

    }
}
