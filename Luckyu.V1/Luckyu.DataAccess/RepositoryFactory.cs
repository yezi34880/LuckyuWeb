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

        public T GetEntity(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby, bool isDesc = false)
        {
            var entity = BaseRepository().GetEntity<T>(condition, orderby, isDesc);
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
        public List<T> GetList(Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby, bool isDesc = false)
        {
            var entity = BaseRepository().GetList<T>(condition, orderby, isDesc);
            return entity;
        }

        public List<T> GetListTop(int top, Expression<Func<T, bool>> condition, string orderby = "")
        {
            var entity = BaseRepository().GetListTop<T>(top, condition, orderby);
            return entity;
        }
        public List<T> GetListTop(int top, Expression<Func<T, bool>> condition, Expression<Func<T, object>> orderby, bool isDesc = false)
        {
            var entity = BaseRepository().GetListTop<T>(top, condition, orderby, isDesc);
            return entity;
        }
        public T GetEntityTop<T>(int top, Expression<Func<T, bool>> condition, string orderby = "") where T : class, new()
        {
            var entity = BaseRepository().GetEntityTop<T>(top, condition, orderby);
            return entity;
        }

    }
}
