using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    /// <summary>
    /// 构造、分解SQL语句
    /// </summary>
    public class SQLPartsHelper
    {
        public static Regex RegexColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b",
    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex RegexDistinct = new Regex(@"\ADISTINCT\s",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex RegexOrderBy =
            new Regex(
                @"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\[\]`""\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\[\]`""\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*",
                RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex SimpleRegexOrderBy = new Regex(@"\bORDER\s+BY\s+", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        ///     Splits the given <paramref name="sql" /> into <paramref name="parts" />; 
        /// </summary>
        /// <param name="sql">The SQL to split.</param>
        /// <param name="parts">The SQL parts.</param>
        /// <returns><c>True</c> if the SQL could be split; else, <c>False</c>.</returns>
        public static SQLParts SplitSQL(string sql)
        {
            sql = sql.Trim().Trim(';');
            var parts = new SQLParts();
            parts.Sql = sql;
            parts.SqlForm = null;
            parts.SqlOrderBy = null;
            parts.SqlSelectColumn = null;
            parts.SqlSelectOrderRemoved = null;

            // Extract the columns from "SELECT <whatever> FROM"
            var m = RegexColumns.Match(sql);
            if (!m.Success)
            {
                return parts;
            }

            // Save column list and replace with COUNT(1)
            var g = m.Groups[1];
            parts.SqlSelectColumn = g.Value;

            // Look for the last "ORDER BY <whatever>" clause not part of a ROW_NUMBER expression
            var o = SimpleRegexOrderBy.Match(sql);
            if (o.Success)
            {
                parts.SqlSelectOrderRemoved = sql.Substring(g.Index + g.Value.Length, o.Index - g.Index -g.Value.Length);
                parts.SqlOrderBy = sql.Substring(o.Index);
            }
            else
            {
                parts.SqlSelectOrderRemoved = sql.Substring(g.Index + g.Value.Length);
            }

            parts.SqlForm = o.Success ?
                sql.Substring(g.Index + g.Length, o.Index - g.Index - g.Length)
                : sql.Substring(g.Index + g.Length);
            return parts;
        }

        public static string GetCountSql(SQLParts parts)
        {
            var countSql = $@"SELECT COUNT(1) {parts.SqlSelectOrderRemoved}";
            return countSql;
        }

        public static string GetPageSql(SQLParts parts, int pageIndex, int pageSize, DataType dbType)
        {
            string pageSql;
            if (dbType == DataType.SqlServer)
            {
                pageSql = $@"
SELECT * FROM 
(SELECT ROW_NUMBER() OVER ({parts.SqlOrderBy ?? "ORDER BY (SELECT NULL)"}) peta_rn, {(parts.SqlSelectColumn + parts.SqlSelectOrderRemoved)}) peta_paged 
WHERE peta_rn > {((pageIndex - 1) * pageSize).ToString()} 
    AND peta_rn <= {(pageIndex * pageSize).ToString()} ";
            }
            else if (dbType == DataType.MySql)
            {
                var start = (pageIndex - 1) * pageSize;
                pageSql = $@"SELECT * FROM ({parts.Sql}) temp_table LIMIT {start},{pageSize}";
            }
            else
            {
                throw new Exception("暂未适配该类型数据库分页方法");
            }
            return pageSql;
        }

    }

}
