using Luckyu.App.System;
using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using System;
using System.IO;
using System.Reflection;
using SqlSugar;
using System.Collections.Generic;
using System.Linq.Expressions;
using Luckyu.App.Organization;

namespace Luckyu.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestDbTrans();

            //decimal num = 9988.4560M;
            //Console.WriteLine(num);
            //var a = Money.GetCnString(num.ToString());
            //Console.WriteLine(a);
            //var b = Money.GetEnString(num.ToString());
            //Console.WriteLine(b);

            var list = new List<ValueTuple<string, string>>();
            list.Add(new ValueTuple<string, string>("1", "11"));
            list.Add(new ValueTuple<string, string>("2", "22"));

            LogDBConnection.SetConnectString("server=127.0.0.1;port=3306;user id=root;password=admin_root;database=luckyu_log;charset='utf8mb4';");
            BaseConnection.SetConnectString("server=127.0.0.1;port=3306;user id=root;password=admin_root;database=luckyu_core;charset='utf8mb4';");
            var db = new Repository().db;
            //var query = db.Queryable<App.Organization.sys_userEntity>().Where(r => list.Contains(r.company_id, r.department_id));

            Expression<Func<sys_userEntity, bool>> exp = r => list.Contains(r.company_id, r.department_id);
            var query = db.Queryable<App.Organization.sys_userEntity>().Where(exp);
            var sql = query.ToSql();
        }

        /// <summary>
        /// 动态执行方法
        /// </summary>
        static void DynamicRun()
        {
            //var fileNames1 = Directory.GetFiles(AppContext.BaseDirectory, "Luckyu.App.*.dll");
            //foreach (var name in fileNames1)
            //{
            //    //Assembly.LoadFile(name);
            //    System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(name);
            //}

            //Assembly ass = Assembly.Load("Luckyu.App.OA");
            //Type tp = ass.GetType("Luckyu.App.OA.WFLeaveNode");//类名
            //IWFNodeProcess process = Activator.CreateInstance(tp) as IWFNodeProcess;//实例化
            //if (process == null)
            //{
            //    Console.WriteLine("执行程序配置错误");
            //}
            //MethodInfo meth = tp.GetMethod("Approve");//加载方法
            //meth.Invoke(process, new Object[] { 1, "测试" });//执行
        }

        static void TestDbTrans()
        {
            LogDBConnection.SetConnectString("server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_log;");
            BaseConnection.SetConnectString("server=127.0.0.1;port=3306;user id=root;password=admin_root;database=lucky_core;");
            var trans = new Repository();
            trans.BeginTrans();
            try
            {
                trans.Insert<sys_configEntity>(new sys_configEntity { config_id = "111" });
                int.Parse("");
                trans.Delete(new sys_configEntity { config_id = "111" });
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
