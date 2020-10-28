using Luckyu.App.System;
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
            BaseConnection.SetConnectString("server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_core;");
            var trans = new Repository();
            trans.BeginTrans();
            try
            {
                trans.Insert<sys_configEntity>(new sys_configEntity { id = "111" });
                int.Parse("");
                trans.Delete(new sys_configEntity { id = "111" });
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
    }
}
