using Luckyu.Cache;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class CompanyBLL
    {
        #region Var
        private sys_companyService companyService = new sys_companyService();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.GetCurrentDomain() + "luckyu_company_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_companyEntity> Page(JqgridPageRequest jqpage)
        {
            var page = companyService.Page(jqpage);
            return page;
        }
        public sys_companyEntity GetEntity(Expression<Func<sys_companyEntity, bool>> condition)
        {
            var entity = companyService.GetEntity(condition);
            return entity;
        }
        public List<sys_companyEntity> GetList(Expression<Func<sys_companyEntity, bool>> condition, Expression<Func<sys_companyEntity, object>> orderby = null, bool isDesc = false)
        {
            var list = companyService.GetList(condition, orderby, isDesc);
            return list;
        }
        public List<sys_companyEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_companyEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => r.is_delete == 0, r => r.sort);
                cache.Write(cacheKey, list);
            }
            return list;
        }
        public sys_companyEntity GetEntityByCache(Func<sys_companyEntity, bool> condition)
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
                List<sys_companyEntity> list = GetAllByCache();
                foreach (var item in list)
                {
                    var model = new ClientDataMapModel
                    {
                        parentId = item.parent_id,
                        code = item.companycode,
                        name = item.fullname
                    };
                    dic.Add(item.company_id, model);
                    cache.Write(cacheKey + "dic", dic);
                }
            }
            return dic;
        }

        /// <summary>
        /// 树形结构（树形控件）
        /// </summary>
        public List<eleTree> GetTree()
        {
            var allCompany = GetAllByCache();
            var tree = ToTree("0", allCompany);
            return tree;
        }

        /// <summary>
        /// 树形结构（下拉框）
        /// </summary>
        public List<xmSelectTree> GetSelectTree()
        {
            var all = GetAllByCache();
            var tree = ToSelectTree("0", all);
            return tree;
        }
        #endregion

        #region  Set

        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.company_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail("该数据不存在");
            }
            //var user = userBLL.GetEntity(r => r.F_CompanyId == entity.F_CompanyId && r.F_DeleteMark == 0 && r.F_EnabledMark == 1);
            //if (!user.IsEmpty())
            //{
            //    return ResponseResult.Fail("该公司下还有人员，不能删除");
            //}
            companyService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
            return ResponseResult.Success();
        }

        public void SaveForm(string keyValue, sys_companyEntity entity, string strEntity, UserModel loginInfo)
        {
            companyService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + "dic");
        }
        #endregion

        #region Private
        private List<eleTree> ToTree(string parentId, List<sys_companyEntity> listCompany)
        {
            var nodes = listCompany.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new eleTree
            {
                id = r.company_id,
                label = r.fullname,
                children = ToTree(r.company_id, listCompany),
            }).ToList();
            return tree;
        }
        private List<xmSelectTree> ToSelectTree(string parentId, List<sys_companyEntity> listCompany)
        {
            var nodes = listCompany.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new xmSelectTree
            {
                value = r.company_id,
                name = r.fullname,
                children = ToSelectTree(r.company_id, listCompany),
            }).ToList();
            return tree;
        }

        #endregion


    }
}
