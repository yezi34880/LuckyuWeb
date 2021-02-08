using Luckyu.App.Organization;
using Luckyu.Cache;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.System
{
    public class ConfigBLL
    {
        #region Var
        private sys_configService configService = new sys_configService();

        #endregion

        #region Cache
        private string cacheKey = CacheFactory.CachePrefix() + "config_";
        private ICache cache = CacheFactory.Create();

        #endregion


        #region Get
        public List<sys_configEntity> GetList(Expression<Func<sys_configEntity, bool>> condition, string orderby = "")
        {
            var list = configService.GetList(condition, orderby);
            return list;
        }
        public sys_configEntity GetEntity(Expression<Func<sys_configEntity, bool>> condition, string orderby = "")
        {
            var entity = configService.GetEntity(condition, orderby);
            return entity;
        }

        public List<sys_configEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_configEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => r.is_delete == 0);
                cache.Write(cacheKey, list);
            }
            return list;
        }
        public sys_configEntity GetEntityByCache(string configCode)
        {
            var list = GetAllByCache();
            var entity = list.Where(r => r.configcode == configCode).FirstOrDefault();
            return entity;
        }

        public string GetValueByCache(string configCode)
        {
            var entity = GetEntityByCache(configCode);
            if (entity == null)
            {
                throw new Exception($"{configCode}配置项不存在");
            }
            else
            {
                return entity.configvalue;
            }
        }
        public JqgridPageResponse<sys_configEntity> Page(JqgridPageRequest jqpage)
        {
            var page = configService.Page(jqpage);
            return page;
        }

        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.config_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var dic = new Dictionary<string, object>
            {
                {"Config",entity }
            };
            return ResponseResult.Success(dic);
        }

        #endregion

        #region Set

        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.config_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            configService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            return ResponseResult.Success();
        }

        public ResponseResult<sys_configEntity> SaveForm(string keyValue, string strEntity, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<sys_configEntity>();
            if (!keyValue.IsEmpty())
            {
                var old = GetEntity(r => r.config_id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<sys_configEntity>(MessageString.NoData);
                }
            }

            configService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            return ResponseResult.Success(entity);
        }
        #endregion
    }
}
