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
    public class ModuleBLL
    {
        #region Var
        private sys_moduleService moduleService = new sys_moduleService();
        private sys_authorizeService authorizeService = new sys_authorizeService();

        #endregion

        #region Cache
        private string cacheKey = CacheFactory.CachePrefix() + "module_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_moduleEntity> Page(JqgridPageRequest jqPage)
        {
            var page = moduleService.Page(jqPage);
            return page;
        }

        public sys_moduleEntity GetEntity(Expression<Func<sys_moduleEntity, bool>> condition, Expression<Func<sys_moduleEntity, object>> orderExp = null, bool isDesc = false)
        {
            var entity = moduleService.GetEntity(condition, orderExp, isDesc);
            return entity;
        }
        public List<sys_moduleEntity> GetList(Expression<Func<sys_moduleEntity, bool>> condition, Expression<Func<sys_moduleEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = moduleService.GetList(condition, orderExp, isDesc);
            return list;
        }
        public List<sys_moduleEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_moduleEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => r.is_delete == 0, r => r.sort);
                cache.Write(cacheKey, list);
            }
            return list;
        }

        public List<xmSelectTree> GetSelectTree()
        {
            var all = GetAllByCache();
            var tree = ToSelectTree("0", all);
            return tree;
        }
        public List<eleTree> GetTree()
        {
            var all = GetAllByCache();
            var tree = ToTree("0", all);
            return tree;
        }


        public List<CommonTree<sys_moduleEntity>> GetModuleTreeByUser(UserModel user, int moduletype, out List<sys_moduleEntity> listSelfModule)
        {
            var listModule = GetAllByCache();
            listModule = listModule.Where(r => r.is_enable == 1 && r.moduletype == moduletype).ToList();
            listSelfModule = new List<sys_moduleEntity>();
            if (user.level == 99)
            {
                listSelfModule = listModule;
            }
            else
            {
                List<string> objectIds = new List<string>
                {
                     user.user_id
                };
                if (!user.role_ids.IsEmpty())
                {
                    objectIds.AddRange(user.role_ids);
                }

                List<string> itemIdList = authorizeService.GetList(r => objectIds.Contains(r.object_id) && r.itemtype == 1).Select(r => r.item_id).ToList();
                listSelfModule = listModule.Where(r => itemIdList.Contains(r.module_id)).OrderBy(r => r.sort).ToList();
            }
            var tree = ToCommonTree("0", listSelfModule);
            return tree;
        }
        #endregion

        #region  Set

        public void DeleteForm(sys_moduleEntity entity, UserModel loginInfo)
        {
            moduleService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }

        public void SaveForm(string keyValue, sys_moduleEntity entity, string strEntity, UserModel loginInfo)
        {
            moduleService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }
        #endregion

        #region privite
        private List<xmSelectTree> ToSelectTree(string parentId, List<sys_moduleEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new xmSelectTree
            {
                value = r.module_id,
                name = r.modulename,
                children = ToSelectTree(r.module_id, list),
            }).ToList();
            return tree;
        }
        private List<eleTree> ToTree(string parentId, List<sys_moduleEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new eleTree
            {
                id = r.module_id,
                label = $"<i class=\"{r.moduleicon}\"></i> " + r.modulename + $"  {r.remark}",
                children = ToTree(r.module_id, list),
            }).ToList();
            return tree;
        }

        /// <summary>
        /// 递归构造通用树形结构（用于菜单）
        /// </summary>
        private List<CommonTree<sys_moduleEntity>> ToCommonTree(string parentId, List<sys_moduleEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new CommonTree<sys_moduleEntity>
            {
                Main = r,
                Children = ToCommonTree(r.module_id, list),
            }).ToList();
            return tree;
        }

        #endregion

    }
}
