using Luckyu.Cache;
using Luckyu.Utility;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class DepartmentManageBLL
    {
        #region Var
        private sys_departmentmanageService manageService = new sys_departmentmanageService();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        #endregion

        #region cache
        private string cacheKey = CacheFactory.CachePrefix() + "departmentmanage_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public List<sys_departmentmanageEntity> GetList(Expression<Func<sys_departmentmanageEntity, bool>> exp)
        {
            var list = manageService.GetList(exp);
            return list;
        }

        public List<sys_departmentmanageEntity> GetAllByCache()
        {
            var list = cache.Read<List<sys_departmentmanageEntity>>(cacheKey);
            if (list.IsEmpty())
            {
                list = GetList(r => true);
                cache.Write(cacheKey, list);
            }
            return list;
        }

        public List<string> GetManageDepartmentIds(string user_id, string object_id, DepartmentManageRelationEnum relation)
        {
            var list = GetAllByCache();
            var list1 = list.Where(r => r.user_id == user_id && r.object_id == object_id && r.relationtype == (int)relation).Select(r => r.department_id).ToList();
            return list1;
        }

        public List<string> GetDepartmentManagers(string department_id, string object_id, DepartmentManageRelationEnum relation)
        {
            var list = GetAllByCache();
            var list1 = list.Where(r => r.user_id == department_id && r.object_id == object_id && r.relationtype == (int)relation).Select(r => r.user_id).ToList();
            return list1;
        }

        public Dictionary<string, object> GetFormData(string userId)
        {
            var list = manageService.GetList(r => r.user_id == userId);
            //  合并记录
            var listNew = (
                from l in list
                group l by new { l.relationtype, l.object_id, l.objectname } into g
                select new sys_departmentmanageEntity
                {
                    id = Guid.NewGuid().ToString(),
                    relationtype = g.Key.relationtype,
                    object_id = g.Key.object_id,
                    objectname = g.Key.objectname,
                    department_id = string.Join(",", list.Where(r => r.relationtype == g.Key.relationtype && r.object_id == g.Key.object_id).Select(r => r.department_id)),
                    departmentname = string.Join(",", list.Where(r => r.relationtype == g.Key.relationtype && r.object_id == g.Key.object_id).Select(r => r.departmentname))
                }
                ).ToList();
            var dic = new Dictionary<string, object>();
            dic.Add("DepartmentManage", listNew);
            return dic;
        }
        #endregion

        #region Set
        public void SaveForm(string userId, List<sys_departmentmanageEntity> list, UserModel loginInfo)
        {
            for (int i = list.Count - 1; i > -1; i--)
            {
                var item = list[i];
                if (item.object_id.IsEmpty() || item.relationtype == 0 || item.department_id.IsEmpty())
                {
                    list.RemoveAt(i);
                }
            }
            var listNew = new List<sys_departmentmanageEntity>();
            foreach (var item in list)  // 拆分每一条记录
            {
                var deptids = item.department_id.SplitNoEmpty(',');
                foreach (var deptid in deptids)
                {
                    var itemNew = item.Adapt<sys_departmentmanageEntity>();
                    itemNew.department_id = deptid;
                    var dept = deptBLL.GetEntityByCache(r => r.department_id == itemNew.department_id);
                    itemNew.departmentname = dept == null ? "" : dept.fullname;
                    listNew.Add(itemNew);
                }
            }
            manageService.SaveForm(userId, listNew, loginInfo);
        }
        #endregion
    }
}
