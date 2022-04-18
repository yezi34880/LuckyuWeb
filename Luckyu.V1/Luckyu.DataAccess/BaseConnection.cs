using Luckyu.Log;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Newtonsoft.Json;
using Luckyu.Utility;
using SqlSugar;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Luckyu.DataAccess
{
    public class BaseConnection
    {
        private static string connectionString = "";
        private static DbType dbType;
        private static sys_logService logService = new sys_logService();

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //connectionString = "server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;";
                throw new Exception("请先调用BaseConnection.SetConnectString方法设置连接字符串");
            }
            return connectionString;
        }

        private static DbType GetDbType()
        {
            dbType = DbType.MySql;
            return dbType;
        }

        public static void SetConnectString(string conString)
        {
            connectionString = conString;
        }

        public static SqlSugarClient db
        {
            get
            {
                /// 扩展 In 多列的方法
                var expMethods = new List<SqlFuncExternal>();
                expMethods.Add(new SqlFuncExternal()
                {
                    UniqueMethodName = "Contains",
                    MethodValue = (expInfo, dbType, expContext) =>
                    {
                        var str = new StringBuilder();
                        //var listType = expInfo.Args[0].MemberValue.GetType();
                        //var tupleType = listType.GetGenericArguments()[0];
                        //var itemType1 = tupleType.GetGenericArguments()[0];
                        //var itemType2 = tupleType.GetGenericArguments()[1];
                        var list = expInfo.Args[0].MemberValue as ICollection<ValueTuple<string, string>>;
                        if (list.IsEmpty())
                        {
                            str.Append(" 1=0 ");
                            return str.ToString();
                        }
                        var idx = 0;
                        foreach (var item in list)
                        {
                            if (idx++ > 0)
                            {
                                str.Append(" OR ");
                            }
                            str.Append($"( {item.Item1}={expInfo.Args[1].MemberName}")
                            .Append(" AND ")
                            .Append($"{item.Item2}={expInfo.Args[2].MemberName} )");
                        }
                        return str.ToString();
                    }
                });

                var db = new SqlSugarClient(
                   new ConnectionConfig()
                   {
                       ConnectionString = GetConnectionString(),
                       DbType = GetDbType(),//设置数据库类型
                       IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                       ConfigureExternalServices = new ConfigureExternalServices()
                       {
                           SqlFuncServices = expMethods,//set ext method
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
                db.Aop.OnLogExecuted = (sql, pars) =>
                {
                    sql = sql.ToLower().Trim();
                    if (sql.StartsWith("update") || sql.StartsWith("insert") || sql.StartsWith("delete")|| sql.StartsWith("drop")|| sql.StartsWith("create")|| sql.StartsWith("alert"))
                    {
                        var log = new sys_logEntity();
                        log.log_type = (int)LogType.Sql;
                        // log.op_type = e.CurdType.ToString();
                        log.app_name = LuckyuHelper.AppID;
                        log.log_content = $"【SQL】{sql}\r\n【PARAMS】{pars.ToDictionary(r => r.ParameterName, t => t.Value.ToString()).ToJson()}\r\n【RESULT】{db.Ado.SqlExecutionTime}\r\n";
                        log.log_json = "";
                        //log.module = e.Table.DbName;
                        logService.Insert(log);
                    }
                };

                return db;
            }
        }

        public static string ParaPre
        {
            get
            {
                return db.Ado.SqlParameterKeyWord;
                //switch (GetDbType())
                //{
                //    case DbType.MySql:
                //        return "?";
                //    default:
                //        return "@";
                //}
            }
        }

    }

}
