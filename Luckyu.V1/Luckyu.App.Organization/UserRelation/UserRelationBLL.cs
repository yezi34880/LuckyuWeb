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
    public class UserRelationBLL
    {
        #region Var
        private sys_userrelationService relationService = new sys_userrelationService();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.GetCurrentDomain() + "luckyu_userrelation_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public List<sys_userrelationEntity> GetList(Expression<Func<sys_userrelationEntity, bool>> condition)
        {
            var list = relationService.GetList(condition);
            return list;
        }

        public List<sys_userrelationEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_userrelationEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => true);
                cache.Write(cacheKey, list);
            }
            return list;
        }

        public List<sys_userrelationEntity> GetListByType(int relationType, string objectId)
        {
            var all = GetAllByCache();
            var list = all.Where(r => r.relationtype == relationType && r.object_id == objectId).ToList();
            return list;
        }

        public List<sys_userrelationEntity> GetListByUser(string userId)
        {
            var all = GetAllByCache();
            var list = all.Where(r => r.user_id == userId).ToList();
            return list;
        }
        public List<sys_userrelationEntity> GetListByUser(int relationType, string userId)
        {
            var all = GetListByUser(userId);
            var list = all.Where(r => r.relationtype == relationType).ToList();
            return list;
        }

        #endregion
        public void SetRelationByObject(int objectType, string objectId, List<string> userIds, UserModel loginInfo)
        {
            relationService.SetRelationByObject(objectType, objectId, userIds, loginInfo);
            cache.Remove(cacheKey);
        }

        public void SetRelationByUser(int relationType, string userId, List<string> objectIds, UserModel loginInfo)
        {
            relationService.SetRelationByUser(relationType, userId, objectIds, loginInfo);
            cache.Remove(cacheKey);
        }

    }
}
