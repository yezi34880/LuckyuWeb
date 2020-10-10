using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.Log
{
    public class LogDBConnection
    {
        private static string connectionString = "";
        private static DbType dbType = DbType.MySql;

        public SqlSugarClient dbClient
        {
            get
            {
                connectionString = GetConnectionString();
                dbType = GetDbType();
                var db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = DbType.MySql,//设置数据库类型
                    IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                    InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                });
                return db;
            }
        }

        public static void SetConnectString(string conString)
        {
            connectionString = conString;
        }

        private string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //connectionString = "server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;";
                throw new Exception("请先调用LogDBConnection.SetConnectString方法设置连接字符串");
            }
            return connectionString;
        }

        private DbType GetDbType()
        {
            return dbType;
        }
    }
}
