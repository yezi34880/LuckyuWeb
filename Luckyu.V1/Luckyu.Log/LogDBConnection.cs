using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.Log
{
    public class LogDBConnection
    {
        private static string connectionString = "";
        private static DataType dbType;
        private static IFreeSql db;

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //connectionString = "server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;";
                throw new Exception("请先调用LogDBConnection.SetConnectString方法设置连接字符串");
            }
            return connectionString;
        }

        private static DataType GetDbType()
        {
            return DataType.MySql;
        }
        public static IFreeSql InitDatabase()
        {
            if (db == null)
            {
                db = new FreeSqlBuilder()
                          .UseConnectionString(GetDbType(), GetConnectionString())
                          .UseNameConvert(FreeSql.Internal.NameConvertType.ToUpper)
                          .UseAutoSyncStructure(false) //自动同步实体结构到数据库
                          .Build(); //请务必定义成 Singleton 单例模式
            }
            return db;
        }

        public static void SetConnectString(string conString)
        {
            connectionString = conString;
        }

    }
}
