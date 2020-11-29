using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Luckyu.Log
{
    public class sys_logService
    {
        private string tableSuffix = $"_{DateTime.Today.ToString("yyyyMM")}";

        public void Insert(sys_logEntity entity)
        {
            entity.Create();
            var db = LogDBConnection.InitDatabase();

            var entityInfo = db.CodeFirst.GetTableByEntity(typeof(sys_logEntity));
            var newTableName = entityInfo.DbName + tableSuffix;
            if (!db.DbFirst.ExistsTable(newTableName))
            {
                db.CodeFirst.SyncStructure(typeof(sys_logEntity), newTableName);
            }
            db.Insert(entity).AsTable(tablename => tablename + tableSuffix).ExecuteAffrows();
        }

        public List<sys_logEntity> GetPage(int pageIndex, int paegSize, ISelect<sys_logEntity> query, out long total)
        {
            var list = query.AsTable((_, tablename) => tablename + tableSuffix).Count(out total).Page(pageIndex, paegSize).ToList();
            return list;
        }

        public sys_logEntity GetEntity(Expression<Func<sys_logEntity, bool>> condition)
        {
            var db = LogDBConnection.InitDatabase();
            var query = db.Select<sys_logEntity>().Where(condition);
            var entity = query.AsTable((_, tablename) => tablename + tableSuffix).First();
            return entity;
        }

    }
}
