using Luckyu.Log;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Luckyu.DataAccess
{
    public class BaseConnection
    {
        private static string connectionString = "";
        private static DbType dbType = DbType.MySql;
        private sys_logService logService = new sys_logService();

        public SqlSugarClient Instance
        {
            get
            {
                var db = new SqlSugarClient(
                   new ConnectionConfig()
                   {
                       ConnectionString = GetConnectionString(),
                       DbType = GetDbType(),
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
                       }
                   });
                //用来打印Sql方便你调式    
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    var keywords = new string[] { "insert", "update", "delete", "alter", "drop" };
                    foreach (var keyword in keywords)
                    {
                        if (sql.ToLower().Contains(keyword))
                        {
                            var log = new sys_logEntity();
                            log.log_type = (int)LogType.Sql;
                            log.log_content = sql;
                            log.log_json = db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                            log.log_time = DateTime.Now;
                            logService.Insert(log);
                            break;
                        }
                    }
                };
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
                throw new Exception("请先调用BaseConnection.SetConnectString方法设置连接字符串");
            }
            return connectionString;
        }

        private DbType GetDbType()
        {
            return dbType;
        }
    }

}
