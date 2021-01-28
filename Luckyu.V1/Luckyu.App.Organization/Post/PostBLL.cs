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
    public class PostBLL
    {
        #region Var
        private sys_postService postService = new sys_postService();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.CachePrefix() + "post_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_postEntity> Page(JqgridPageRequest jqpage)
        {
            var page = postService.Page(jqpage);
            return page;
        }

        public List<sys_postEntity> GetSelect(JqgridPageRequest jqpage)
        {
            var list = postService.GetSelect(jqpage);
            return list;
        }

        public sys_postEntity GetEntity(Expression<Func<sys_postEntity, bool>> condition, Expression<Func<sys_postEntity, object>> orderExp = null, bool isDesc = false)
        {
            var entity = postService.GetEntity(condition, orderExp, isDesc);
            return entity;
        }

        public List<sys_postEntity> GetList(Expression<Func<sys_postEntity, bool>> condition, Expression<Func<sys_postEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = postService.GetList(condition, orderExp, isDesc);
            return list;
        }

        public List<sys_postEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_postEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => r.is_delete == 0, r => r.sort);
                cache.Write(cacheKey, list);
            }
            return list;
        }
        public sys_postEntity GetEntityByCache(Func<sys_postEntity, bool> condition)
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
                List<sys_postEntity> list = GetAllByCache();
                foreach (var item in list)
                {
                    var model = new ClientDataMapModel
                    {
                        code = item.postcode,
                        name = item.postname
                    };
                    dic.Add(item.post_id, model);
                    cache.Write(cacheKey + "dic", dic);
                }
            }
            return dic;
        }

        #endregion

        #region  Set

        public void DeleteForm(sys_postEntity entity, UserModel loginInfo)
        {
            postService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }

        public void SaveForm(string keyValue, sys_postEntity entity, string strEntity, UserModel loginInfo)
        {
            postService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }
        #endregion

    }
}
