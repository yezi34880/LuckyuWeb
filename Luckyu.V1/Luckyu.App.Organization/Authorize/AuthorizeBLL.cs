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
    public class AuthorizeBLL
    {
        #region Var
        private sys_authorizeService authorizeService = new sys_authorizeService();

        #endregion

        #region cache
        private string cacheKey = LuckyuHelper.AppID + "authorize_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public List<sys_authorizeEntity> GetList(Expression<Func<sys_authorizeEntity, bool>> condition)
        {
            var list = authorizeService.GetList(condition);
            return list;
        }
        public List<sys_authorizeEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_authorizeEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => true);
                cache.Write(cacheKey, list);
            }
            return list;
        }
        public List<sys_authorizeEntity> GetListByCache(Func<sys_authorizeEntity, bool> condition)
        {
            var all = GetAllByCache();
            var list = all.Where(condition).ToList();
            return list;
        }

        #endregion

        #region Set
        public void SaveForm(int objectType, string objectId, List<string> modules, UserModel loginInfo)
        {
            var list = new List<sys_authorizeEntity>();
            if (!modules.IsEmpty())
            {
                foreach (var moduleid in modules)
                {
                    var entity = new sys_authorizeEntity();
                    entity.objecttype = objectType;
                    entity.object_id = objectId;
                    entity.itemtype = 1;
                    entity.item_id = moduleid;
                    entity.Create(loginInfo);
                    list.Add(entity);
                }
            }
            authorizeService.SaveForm(objectType, objectId, list);
            cache.Remove(cacheKey);
        }
        #endregion

    }
}
