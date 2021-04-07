using FreeSql;
using FreeSql.Internal.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using System.Threading;

namespace Luckyu.DataAccess
{
    public static class SqlExtensions
    {
        public static TableInfo GetTableByEntity<T>(this ICodeFirst codefirst)
        {
            var t = typeof(T);
            var tableInfo = codefirst.GetTableByEntity(t);
            return tableInfo;
        }

    }

    [ExpressionCall]
    public static class MyFreeSqlExpressionCall
    {
        public static ThreadLocal<ExpressionCallContext> expContext = new ThreadLocal<ExpressionCallContext>();
        /// <summary>
        /// C#：从元组集合中查找 exp1, exp2 是否存在<para></para>
        /// SQL： <para></para>
        /// exp1 = that[0].Item1 and exp2 = that[0].Item2 OR <para></para>
        /// exp1 = that[1].Item1 and exp2 = that[1].Item2 OR <para></para>
        /// ... <para></para>
        /// 注意：当 that 为 null 或 empty 时，返回 1=0
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="that"></param>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static bool Contains<T1, T2>([RawValue] this IEnumerable<(T1, T2)> that, T1 exp1, T2 exp2)
        {
            if (expContext.IsValueCreated == false || expContext.Value == null || expContext.Value.ParsedContent == null)
                return that?.Any(a => a.Item1.Equals(exp1) && a.Item2.Equals(exp2)) == true;
            if (that?.Any() != true)
            {
                expContext.Value.Result = "1=0";
                return false;
            }
            var sb = new StringBuilder();
            var idx = 0;
            foreach (var item in that)
            {
                if (idx++ > 0) sb.Append(" OR \r\n");
                sb
                    .Append(expContext.Value.ParsedContent["exp1"]).Append(" = ").Append(expContext.Value.FormatSql(FreeSql.Internal.Utils.GetDataReaderValue(typeof(T1), item.Item1)))
                    .Append(" AND ")
                    .Append(expContext.Value.ParsedContent["exp2"]).Append(" = ").Append(expContext.Value.FormatSql(FreeSql.Internal.Utils.GetDataReaderValue(typeof(T2), item.Item2)));
            }
            expContext.Value.Result = sb.ToString();
            return true;
        }
        /// <summary>
        /// C#：从元组集合中查找 exp1, exp2, exp2 是否存在<para></para>
        /// SQL： <para></para>
        /// exp1 = that[0].Item1 and exp2 = that[0].Item2 and exp3 = that[0].Item3 OR <para></para>
        /// exp1 = that[1].Item1 and exp2 = that[1].Item2 and exp3 = that[1].Item3 OR <para></para>
        /// ... <para></para>
        /// 注意：当 that 为 null 或 empty 时，返回 1=0
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="that"></param>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <param name="exp3"></param>
        /// <returns></returns>
        public static bool Contains<T1, T2, T3>([RawValue] this IEnumerable<(T1, T2, T3)> that, T1 exp1, T2 exp2, T3 exp3)
        {
            if (expContext.IsValueCreated == false || expContext.Value == null || expContext.Value.ParsedContent == null)
                return that.Any(a => a.Item1.Equals(exp1) && a.Item2.Equals(exp2) && a.Item3.Equals(exp3));
            if (that.Any() == false)
            {
                expContext.Value.Result = "1=0";
                return false;
            }
            var sb = new StringBuilder();
            var idx = 0;
            foreach (var item in that)
            {
                if (idx++ > 0) sb.Append(" OR \r\n");
                sb
                    .Append(expContext.Value.ParsedContent["exp1"]).Append(" = ").Append(expContext.Value.FormatSql(FreeSql.Internal.Utils.GetDataReaderValue(typeof(T1), item.Item1)))
                    .Append(" AND ")
                    .Append(expContext.Value.ParsedContent["exp2"]).Append(" = ").Append(expContext.Value.FormatSql(FreeSql.Internal.Utils.GetDataReaderValue(typeof(T2), item.Item2)))
                    .Append(" AND ")
                    .Append(expContext.Value.ParsedContent["exp3"]).Append(" = ").Append(expContext.Value.FormatSql(FreeSql.Internal.Utils.GetDataReaderValue(typeof(T3), item.Item3)));
            }
            expContext.Value.Result = sb.ToString();
            return true;
        }
    }

}
