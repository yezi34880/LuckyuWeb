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
    public class DepartmentBLL
    {
        #region Var
        private sys_departmentService deptService = new sys_departmentService();
        private CompanyBLL companyBLL = new CompanyBLL();
        #endregion

        #region cache
        private string cacheKey = CacheFactory.GetCurrentDomain() + "luckyu_department_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_departmentEntity> Page(string companyId, JqgridPageRequest jqpage)
        {
            var page = deptService.Page(companyId, jqpage);
            return page;
        }
        public sys_departmentEntity GetEntity(Expression<Func<sys_departmentEntity, bool>> condition)
        {
            var entity = deptService.GetEntity(condition);
            return entity;
        }
        public List<sys_departmentEntity> GetList(Expression<Func<sys_departmentEntity, bool>> condition, Expression<Func<sys_departmentEntity, object>> orderby = null, bool isDesc = false)
        {
            var list = deptService.GetList(condition, orderby, isDesc);
            return list;
        }
        public List<sys_departmentEntity> GetAllByCache(string companyId = "")
        {
            var list = cache.Read<List<sys_departmentEntity>>(cacheKey + companyId);
            if (list.IsEmpty())
            {
                if (companyId == "" || companyId == "-1" || companyId.ToLower() == "all")
                {
                    list = GetList(r => r.is_delete == 0, r => r.sort);
                }
                else
                {
                    list = GetList(r => r.is_delete == 0 && r.company_id == companyId, r => r.sort);
                }
                cache.Write(cacheKey + companyId, list);
            }
            return list;
        }
        public sys_departmentEntity GetEntityByCache(Func<sys_departmentEntity, bool> condition, string companyId = "")
        {
            var list = GetAllByCache(companyId);
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
                List<sys_departmentEntity> list = GetAllByCache();
                foreach (var item in list)
                {
                    var model = new ClientDataMapModel
                    {
                        parentId = item.parent_id,
                        code = item.departmentcode,
                        name = item.fullname
                    };
                    dic.Add(item.department_id, model);
                    cache.Write(cacheKey + "dic", dic);
                }
            }
            return dic;
        }

        public List<eleTree> GetAllTree(string companyId, bool multiple)
        {
            var tree = new List<eleTree>();
            var allcompany = companyBLL.GetAllByCache();
            if (!allcompany.IsEmpty())
            {
                if (companyId.IsEmpty() || companyId.ToLower() == "all" || companyId == "-1")
                {
                    tree.AddRange(allcompany.Select(r => new eleTree
                    {
                        id = r.company_id,
                        label = r.fullname,
                        disabled = multiple ? false : true,
                        ext = new Dictionary<string, string> { { "tag", "company" } }
                    }));
                }
                else
                {
                    tree.AddRange(allcompany.Where(r => r.company_id == companyId)
                        .Select(r => new eleTree
                        {
                            id = r.company_id,
                            label = r.fullname,
                            disabled = multiple ? false : true,
                            ext = new Dictionary<string, string> { { "tag", "company" } }
                        }));
                }
            }
            else
            {
                return tree;
            }
            foreach (var item in tree)
            {
                item.children = GetTree(item.id);
            }
            return tree;
        }

        public List<eleTree> GetTree(string companyId)
        {
            var all = GetAllByCache(companyId);
            var tree = ToTree("0", all);
            return tree;
        }

        /// <summary>
        /// 树形结构（下拉框）
        /// </summary>
        public List<xmSelectTree> GetSelectTree(string companyId)
        {
            var all = GetAllByCache(companyId);
            var tree = ToSelectTree("0", all);
            return tree;
        }

        #endregion

        #region  Set

        public void DeleteForm(sys_departmentEntity entity, UserModel loginInfo)
        {
            deptService.DeleteForm(entity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + entity.company_id);
            cache.Remove(cacheKey + "dic");
        }

        public void SaveForm(string keyValue, sys_departmentEntity entity, string strEntity, UserModel loginInfo)
        {
            deptService.SaveForm(keyValue, entity, strEntity, loginInfo);
            cache.Remove(cacheKey);
            cache.Remove(cacheKey + entity.company_id);
            cache.Remove(cacheKey + "dic");
        }
        #endregion

        #region Private
        private List<eleTree> ToTree(string parentId, List<sys_departmentEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new eleTree
            {
                id = r.department_id,
                label = r.fullname,
                ext = new Dictionary<string, string> { { "tag", "department" } },
                children = ToTree(r.department_id, list),
            }).ToList();
            return tree;
        }
        private List<xmSelectTree> ToSelectTree(string parentId, List<sys_departmentEntity> list)
        {
            var nodes = list.Where(r => r.parent_id == parentId).ToList();
            if (nodes.IsEmpty())
            {
                return null;
            }
            var tree = nodes.Select(r => new xmSelectTree
            {
                value = r.department_id,
                name = r.fullname,
                children = ToSelectTree(r.department_id, list),
            }).ToList();
            return tree;
        }

        #endregion

    }
}
