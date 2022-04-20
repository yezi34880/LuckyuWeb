using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SqlSugar;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Luckyu.DataAccess
{
    public static class SqlExtensions
    {
        /// <summary>
        /// 扩展 In 多列的方法
        /// </summary>
        public static bool Contains<T1, T2>(this ICollection<(T1, T2)> that, string exp1, string exp2)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }

    }
}
