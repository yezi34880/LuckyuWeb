using Luckyu.Cache;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class RoleBLL
    {
        #region Var
        private sys_roleService roleService = new sys_roleService();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.CachePrefix() + "role_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_roleEntity> Page(JqgridPageRequest jqpage)
        {
            var page = roleService.Page(jqpage);
            return page;
        }

        public List<sys_roleEntity> GetSelect(JqgridPageRequest jqpage)
        {
            var list = roleService.GetSelect(jqpage);
            return list;
        }

        public sys_roleEntity GetEntity(Expression<Func<sys_roleEntity, bool>> condition)
        {
            var entity = roleService.GetEntity(condition);
            return entity;
        }
        public List<sys_roleEntity> GetList(Expression<Func<sys_roleEntity, bool>> condition)
        {
            var list = roleService.GetList(condition);
            return list;
        }
        public List<sys_roleEntity> GetList(Expression<Func<sys_roleEntity, bool>> condition, Expression<Func<sys_roleEntity, object>> orderby = null, bool isDesc = false)
        {
            var list = roleService.GetList(condition, orderby, isDesc);
            return list;
        }
        public List<sys_roleEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_roleEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => r.is_delete == 0, r => r.sort);
                cache.Write(cacheKey, list);
            }
            return list;
        }
        public sys_roleEntity GetEntityByCache(Func<sys_roleEntity, bool> condition)
        {
            var list = GetAllByCache();
            var entity = list.Where(condition).FirstOrDefault();
            return entity;
        }
        /// <summary>
        /// 获取公司映射数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ClientDataMapModel> GetModelMap()
        {
            var dic = cache.Read<Dictionary<string, ClientDataMapModel>>(cacheKey + "dic");
            if (dic == null)
            {
                dic = new Dictionary<string, ClientDataMapModel>();
                List<sys_roleEntity> list = GetAllByCache();
                foreach (var item in list)
                {
                    var model = new ClientDataMapModel
                    {
                        code = item.rolecode,
                        name = item.rolename
                    };
                    dic.Add(item.role_id, model);
                    cache.Write(cacheKey + "dic", dic);
                }
            }
            return dic;
        }

        #endregion

        #region  Set

        public void DeleteForm(sys_roleEntity entity, UserModel loginInfo)
        {
            roleService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }

        public void SaveForm(string keyValue, sys_roleEntity entity, string strEntity, UserModel loginInfo)
        {
            roleService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }
        #endregion

    }
}
