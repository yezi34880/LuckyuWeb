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
                                                       // 参数化时候有问题，超长字符串会背截断，还没找到原因，有空来找
                                                       // 找到原因了 使用参数时，默认长度255 ，除非手动指定   [Column(Name = "Name", StringLength = -1)]
                          .UseGenerateCommandParameterWithLambda(false)
                          .UseNoneCommandParameter(true)
                          .UseMonitorCommand((command) =>
                          {
                          }, (command, result) =>
                          {
                              //// result 包含 执行 sql语句 和返回结果 执行时间
                              //var keywords = new string[] { "insert ", "update ", "delete ", "alter ", "drop " };
                              //var sql = command.CommandText;
                              //foreach (var keyword in keywords)
                              //{
                              //    if (sql.ToLower().Trim().StartsWith(keyword))
                              //    {
                              //        var dic = new Dictionary<string, string>();
                              //        foreach (DbParameter para in command.Parameters)
                              //        {
                              //            dic.Add(para.ParameterName, para.Value.ToString());
                              //        }
                              //        var log = new sys_logEntity();
                              //        log.log_type = (int)LogType.Sql;
                              //        log.app_name = LuckyuHelper.AppID;
                              //        log.log_content = result;
                              //        log.log_json = JsonConvert.SerializeObject(dic);
                              //        logService.Insert(log);
                              //        break;
                              //    }
                              //}
                          })
                          .Build(); //请务必定义成 Singleton 单例模式

                db.Aop.CurdAfter += (s, e) =>
                {
                    if (e.CurdType != FreeSql.Aop.CurdType.Select)
                    {
                        var log = new sys_logEntity();
                        log.log_type = (int)LogType.Sql;
                        log.op_type = e.CurdType.ToString();
                        log.app_name = LuckyuHelper.AppID;
                        log.log_content = $"【SQL】{e.Sql}\r\n【PARAMS】{e.DbParms.ToJson()}\r\n【RESULT】{e.ExecuteResult.ToJson()}\r\n";
                        log.log_json = "";
                        log.module = e.Table.DbName;

                        //var strkeyvalues = new System.Text.StringBuilder();
                        //if (!e.ExecuteResult.IsEmpty())
                        //{
                        //    var t = e.ExecuteResult.GetType();
                        //    IEnumerable result = Convert.ChangeType(e.ExecuteResult, t);
                        //    foreach (var row in result)
                        //    {
                        //        var entity = Convert.ChangeType(row, e.EntityType);
                        //        foreach (var pri in e.Table.Primarys)
                        //        {
                        //            var keyvalue = pri.GetDbValue(entity);
                        //            strkeyvalues.Append($"{keyvalue},");
                        //        }
                        //    }
                        //}
                        //log.process_id = strkeyvalues.ToString().Trim(',');
                        logService.Insert(log);
                    }
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
