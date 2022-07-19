using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Luckyu.Log
{
    public class sys_logService
    {
        public void Insert(sys_logEntity entity)
        {
            entity.Create();
            var db = LogDBConnection.db;
            db.Insertable(entity).SplitTable().ExecuteCommand();
        }

        public List<sys_logEntity> GetPage(int pageIndex, int paegSize, Expression<Func<sys_logEntity, bool>> condition, DateTime date, ref int total)
        {
            var db = LogDBConnection.db;
            var name = db.SplitHelper<sys_logEntity>().GetTableName(date);//根据时间获取表名
            var list = db.Queryable<sys_logEntity>().Where(condition).AS(name).ToPageList(pageIndex, paegSize, ref total);
            return list;
        }

        public sys_logEntity GetEntity(Expression<Func<sys_logEntity, bool>> condition, DateTime date)
        {
            var db = LogDBConnection.db;
            var name = db.SplitHelper<sys_logEntity>().GetTableName(date);//根据时间获取表名
            var entity = db.Queryable<sys_logEntity>().Where(condition).AS(name).First();
            return entity;
        }

    }
}
