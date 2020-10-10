using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.IO;
using System.Reflection;

namespace Luckyu.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDbTrans();
            Console.ReadLine();
        }

        /// <summary>
        /// 动态执行方法
        /// </summary>
        static void DynamicRun()
        {
            var fileNames1 = Directory.GetFiles(AppContext.BaseDirectory, "Luckyu.App.*.dll");
            foreach (var name in fileNames1)
            {
                //Assembly.LoadFile(name);
                System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(name);
            }

            Assembly ass = Assembly.Load("Luckyu.App.OA");
            Type tp = ass.GetType("Luckyu.App.OA.WFLeaveNode");//类名
            IWFNodeProcess process = Activator.CreateInstance(tp) as IWFNodeProcess;//实例化
            if (process == null)
            {
                Console.WriteLine("执行程序配置错误");
            }
            MethodInfo meth = tp.GetMethod("Approve");//加载方法
            meth.Invoke(process, new Object[] { 1, "测试" });//执行


        }

        static void TestDbTrans()
        {
            LogDBConnection.SetConnectString("server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;");
            var db = new LogDBConnection().dbClient;
            db.BeginTran();
            try
            {
                db.Insertable(new sys_logEntity
                {
                    log_id = SnowflakeHelper.NewCode(),
                    log_type = 4,
                    log_content = "一开始",
                    log_time = DateTime.Now
                }).ExecuteCommand();

                db.UseTran(() =>
                {
                    db.Insertable(new sys_logEntity
                    {
                        log_id = SnowflakeHelper.NewCode(),
                        log_type = 4,
                        log_content = "内部事务当中",
                        log_time = DateTime.Now
                    }).ExecuteCommand();


                    db.Insertable(new sys_logEntity
                    {
                        log_id = SnowflakeHelper.NewCode(),
                        log_type = 4,
                        log_content = "内部事务当异常后",
                        log_time = DateTime.Now
                    }).ExecuteCommand();

                }, (ex) =>
                {
                    throw ex;
                });
                    int.Parse("");

                db.Insertable(new sys_logEntity
                {
                    log_id = SnowflakeHelper.NewCode(),
                    log_type = 4,
                    log_content = "最后",
                    log_time = DateTime.Now
                }).ExecuteCommand();

                db.CommitTran();
            }
            catch (Exception)
            {
                db.RollbackTran();
                throw;
            }
        }
    }
}
