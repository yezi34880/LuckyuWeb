using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.DataAccess
{
    public class RepositoryFactory<T> where T : class, new()
    {
        public Repository BaseRepository()
        {
            return new Repository();
        }

        public T GetEntity(string keyValue)
        {
            var entity = BaseRepository().GetEntity<T>(keyValue);
            return entity;
        }

        public T GetEntity(Expression<Func<T, bool>> condition, string orderby = "")
        {
            var entity = BaseRepository().GetEntity<T>(condition, orderby);
            return entity;
        }

        public T GetEntity(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderExp, OrderByType orderType = OrderByType.Asc)
        {
            var entity = BaseRepository().GetEntity<T>(condition, orderExp, orderType);
            return entity;
        }

        public int GetCount(Expression<Func<T, bool>> condition)
        {
            var count = BaseRepository().Count<T>(condition);
            return count;
        }
        public List<T> GetList(Expression<Func<T, bool>> condition, string orderby = "")
        {
            var entity = BaseRepository().GetList<T>(condition, orderby);
            return entity;
        }
        public List<T> GetList(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderExp, OrderByType orderType = OrderByType.Asc)
        {
            var entity = BaseRepository().GetList<T>(condition, orderExp, orderType);
            return entity;
        }

        public List<T> GetListTop(int top, Expression<Func<T, bool>> condition, string orderby = "")
        {
            var entity = BaseRepository().GetListTop<T>(top, condition, orderby);
            return entity;
        }
        public List<T> GetListTop(int top, Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderExp, OrderByType orderType = OrderByType.Asc)
        {
            var entity = BaseRepository().GetListTop<T>(top, condition, orderExp, orderType);
            return entity;
        }

    }
}
