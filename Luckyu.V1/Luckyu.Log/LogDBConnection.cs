using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Luckyu.Log
{
    public class LogDBConnection
    {
        private static string connectionString = "";
        private static DbType dbType;

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //connectionString = "server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;";
                throw new Exception("请先调用LogDBConnection.SetConnectString方法设置连接字符串");
            }
            return connectionString;
        }

        private static DbType GetDbType()
        {
            dbType = DbType.MySql;
            return DbType.MySql;
        }

        public static SqlSugarClient db
        {
            get
            {
                var db = new SqlSugarClient(
                   new ConnectionConfig()
                   {
                       ConnectionString = GetConnectionString(),
                       DbType = GetDbType(),//设置数据库类型
                       IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                       ConfigureExternalServices = new ConfigureExternalServices()
                       {
                           EntityService = (property, column) =>
                           {
                               var attributes = property.GetCustomAttributes(true);//get all attributes     
                               if (attributes.Any(it => it is NotMappedAttribute))//根据自定义属性    
                               {
                                   column.IsIgnore = true;
                               }
                           }
                       },
                   });
                return db;
            }
        }

        public static void SetConnectString(string conString)
        {
            connectionString = conString;
        }

    }
}
