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
        private string cacheKey = CacheFactory.GetCurrentDomain() + "luckyu_config_";
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
                throw new Exception("该配置项不存在");
            }
            else
            {
                return entity.configvalue;
            }
        }

        #endregion
    }
}
