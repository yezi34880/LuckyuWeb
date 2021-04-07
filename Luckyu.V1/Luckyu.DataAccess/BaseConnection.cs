using Luckyu.Log;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Newtonsoft.Json;
using Luckyu.Utility;

namespace Luckyu.DataAccess
{
    public class BaseConnection
    {
        private static string connectionString = "";
        private static DataType dbType;
        private static sys_logService logService = new sys_logService();

        private static IFreeSql db;

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //connectionString = "server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;";
                throw new Exception("请先调用BaseConnection.SetConnectString方法设置连接字符串");
            }
            return connectionString;
        }

        private static DataType GetDbType()
        {
            dbType = DataType.MySql;
            return dbType;
        }

        public static void SetConnectString(string conString)
        {
            connectionString = conString;
        }
        public static IFreeSql InitDatabase()
        {
            if (db == null)
            {
                db = new FreeSqlBuilder()
                          .UseConnectionString(GetDbType(), GetConnectionString())
                          .UseNameConvert(FreeSql.Internal.NameConvertType.ToLower)
                          .UseAutoSyncStructure(false) //自动同步实体结构到数据库
                                                       //.UseGenerateCommandParameterWithLambda(LuckyuHelper.IsDebug() ? false : true)
                                                       //.UseNoneCommandParameter(LuckyuHelper.IsDebug() ? true : false)
                          .UseGenerateCommandParameterWithLambda(true)
                          .UseNoneCommandParameter(false)
                          .UseMonitorCommand((command) =>
                          {
                              var keywords = new string[] { "insert ", "update ", "delete ", "alter ", "drop " };
                              var sql = command.CommandText;
                              foreach (var keyword in keywords)
                              {
                                  if (sql.ToLower().Trim().StartsWith(keyword))
                                  {
                                      var dic = new Dictionary<string, string>();
                                      foreach (DbParameter para in command.Parameters)
                                      {
                                          dic.Add(para.ParameterName, para.Value.ToString());
                                      }
                                      var log = new sys_logEntity();
                                      log.log_type = (int)LogType.Sql;
                                      log.app_name = LuckyuHelper.AppID;
                                      log.log_content = sql;
                                      log.log_json = JsonConvert.SerializeObject(dic);
                                      logService.Insert(log);
                                      break;
                                  }
                              }

                          }, (command, result) =>
                          {
                              // result 包含 执行 sql语句 和返回结果 执行时间
                              var keywords = new string[] { "insert ", "update ", "delete ", "alter ", "drop " };
                              var sql = command.CommandText;
                              foreach (var keyword in keywords)
                              {
                                  if (sql.ToLower().Trim().StartsWith(keyword))
                                  {
                                      var dic = new Dictionary<string, string>();
                                      foreach (DbParameter para in command.Parameters)
                                      {
                                          dic.Add(para.ParameterName, para.Value.ToString());
                                      }
                                      var log = new sys_logEntity();
                                      log.log_type = (int)LogType.Sql;
                                      log.app_name = LuckyuHelper.AppID;
                                      log.log_content = result;
                                      log.log_json = JsonConvert.SerializeObject(dic);
                                      logService.Insert(log);
                                      break;
                                  }
                              }
                          })
                          .Build(); //请务必定义成 Singleton 单例模式
            }
            return db;
        }

        public static string ParaPre
        {
            get
            {
                switch (GetDbType())
                {
                    case DataType.MySql:
                        return "?";
                    default:
                        return "@";
                }
            }
        }

    }

}
