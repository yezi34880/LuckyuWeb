using FreeSql;
using FreeSql.Internal.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
