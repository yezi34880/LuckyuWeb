using Luckyu.Cache;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Luckyu.App.Organization
{
    public class GroupBLL
    {
        #region Var
        private sys_groupService groupService = new sys_groupService();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.GetCurrentDomain() + "luckyu_group_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_groupEntity> Page(JqgridPageRequest jqpage)
        {
            var page = groupService.Page(jqpage);
            return page;
        }
        public List<sys_groupEntity> GetSelect(JqgridPageRequest jqpage)
        {
            var list = groupService.GetSelect(jqpage);
            return list;
        }
        public sys_groupEntity GetEntity(Expression<Func<sys_groupEntity, bool>> condition)
        {
            var entity = groupService.GetEntity(condition);
            return entity;
        }
        public List<sys_groupEntity> GetList(Expression<Func<sys_groupEntity, bool>> condition, Expression<Func<sys_groupEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = groupService.GetList(condition, orderExp, isDesc);
            return list;
        }
        public List<sys_groupEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_groupEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => r.is_delete == 0, r => r.sort);
                cache.Write(cacheKey, list);
            }
            return list;
        }
        public sys_groupEntity GetEntityByCache(Func<sys_groupEntity, bool> condition)
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
                List<sys_groupEntity> list = GetAllByCache();
                foreach (var item in list)
                {
                    var model = new ClientDataMapModel
                    {
                        code = item.groupcode,
                        name = item.groupname
                    };
                    dic.Add(item.group_id, model);
                    cache.Write(cacheKey + "dic", dic);
                }
            }
            return dic;
        }

        #endregion

        #region  Set

        public void DeleteForm(sys_groupEntity entity, UserModel loginInfo)
        {
            groupService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }

        public void SaveForm(string keyValue, sys_groupEntity entity, string strEntity, UserModel loginInfo)
        {
            groupService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }
        #endregion
    }
}
