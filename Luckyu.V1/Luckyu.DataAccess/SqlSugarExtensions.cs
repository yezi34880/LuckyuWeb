using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    public static class SqlSugarExtensions
    {
        public static void Add(this List<SugarParameter> dp, string name, object value)
        {
            string cname = name.StartsWith("@") ? name : "@" + name;
            dp.Add(new SugarParameter(cname, value));
        }

        public static void Add(this List<SugarParameter> dp, string name, object value, System.Data.DbType type)
        {
            string cname = name.StartsWith("@") ? name : "@" + name;
            dp.Add(new SugarParameter(cname, value, type));
        }
    }
}
