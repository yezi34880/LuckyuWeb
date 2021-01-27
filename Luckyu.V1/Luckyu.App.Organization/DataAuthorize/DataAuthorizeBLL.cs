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
    public class DataAuthorizeBLL
    {
        #region Var
        private sys_dataauthorizeService dataService = new sys_dataauthorizeService();
        private sys_dataauthorize_detailService datadetailService = new sys_dataauthorize_detailService();

        #endregion

        #region cache
        private string cacheKey = CacheFactory.GetCurrentDomain() + "luckyu_dataauthorize_";
        private ICache cache = CacheFactory.Create();

        #endregion

        #region Get
        public JqgridPageResponse<sys_dataauthorizeEntity> Page(JqgridPageRequest jqPage)
        {
            var page = dataService.Page(jqPage);
            return page;
        }
        public sys_dataauthorizeEntity GetEntity(Expression<Func<sys_dataauthorizeEntity, bool>> condition)
        {
            var entity = dataService.GetEntity(condition);
            return entity;
        }
        public List<sys_dataauthorizeEntity> GetList(Expression<Func<sys_dataauthorizeEntity, bool>> condition, Expression<Func<sys_dataauthorizeEntity, object>> orderby = null, bool isDesc = false)
        {
            var list = dataService.GetList(condition, orderby, isDesc);
            return list;
        }
        public List<sys_dataauthorizeEntity> GetList(Expression<Func<sys_dataauthorizeEntity, bool>> condition)
        {
            var list = dataService.GetList(condition);
            return list;
        }
        public List<sys_dataauthorize_detailEntity> GetDetailList(Expression<Func<sys_dataauthorize_detailEntity, bool>> condition)
        {
            var list = datadetailService.GetList(condition);
            return list;
        }

        /// <summary>
        /// 获取当前模块配置人员
        /// </summary>
        public DataAuthorizeModel GetDataAuthByUser(string moduleName, UserModel loginInfo)
        {
            var listdata = GetList(r => r.modulename.Contains(moduleName) && (r.object_id == loginInfo.user_id || loginInfo.role_ids.Contains(r.object_id) || loginInfo.post_ids.Contains(r.object_id)));
            if (listdata.IsEmpty())
            {
                return null;
            }
            var data = new DataAuthorizeModel();
            data.staterange = listdata.Exists(r => r.staterange == 1) ? 1 : 0;
            data.edittype = 0;
            if (listdata.Exists(r => r.staterange == 2))
            {
                data.edittype = 2;
            }
            else if (listdata.Exists(r => r.staterange == 1))
            {
                data.edittype = 1;
            }
            foreach (var item in listdata)
            {
                if (item.objectrange == 9)  // 全部
                {
                    data.IsAll = true;
                    break;
                }
                else if (item.objectrange == 2)// 同一公司
                {
                    if (item.objecttype == 2)  // 角色
                    {
                        var roleId = item.object_id;
                        if (loginInfo.role_ids.Contains(roleId))
                        {
                            data.CompanyIds.AddNoRepeat(loginInfo.company_id);
                        }
                    }
                    else if (item.objecttype == 1)  // 岗位
                    {
                        var postId = item.object_id;
                        if (loginInfo.post_ids.Contains(postId))
                        {
                            data.CompanyIds.AddNoRepeat(loginInfo.company_id);
                        }
                    }
                    else if (item.objecttype == 0)  // 用户
                    {
                        var user_id = item.object_id;
                        if (loginInfo.user_id == user_id)
                        {
                            data.CompanyIds.AddNoRepeat(loginInfo.company_id);
                        }
                    }
                }
                else if (item.objectrange == 1)// 同一部门
                {
                    if (item.objecttype == 2)  // 角色
                    {
                        var roleId = item.object_id;
                        if (loginInfo.role_ids.Contains(roleId))
                        {
                            data.DepartmentIds.AddNoRepeat(loginInfo.department_id);
                        }
                    }
                    else if (item.objecttype == 1)  // 岗位
                    {
                        var postId = item.object_id;
                        if (loginInfo.post_ids.Contains(postId))
                        {
                            data.DepartmentIds.AddNoRepeat(loginInfo.department_id);
                        }
                    }
                    else if (item.objecttype == 0)  // 用户
                    {
                        var user_id = item.object_id;
                        if (loginInfo.user_id == user_id)
                        {
                            data.DepartmentIds.AddNoRepeat(loginInfo.department_id);
                        }
                    }
                }
                else if (item.objectrange == 3)// 同一用户组
                {
                    if (item.objecttype == 2)  // 角色
                    {
                        var roleId = item.object_id;
                        if (loginInfo.role_ids.Contains(roleId))
                        {
                            data.GroupIds.AddRangeNoRepeat(loginInfo.group_ids);
                        }
                    }
                    else if (item.objecttype == 1)  // 岗位
                    {
                        var postId = item.object_id;
                        if (loginInfo.post_ids.Contains(postId))
                        {
                            data.GroupIds.AddRangeNoRepeat(loginInfo.group_ids);
                        }
                    }
                    else if (item.objecttype == 0)  // 用户
                    {
                        var user_id = item.object_id;
                        if (loginInfo.user_id == user_id)
                        {
                            data.GroupIds.AddRangeNoRepeat(loginInfo.group_ids);
                        }
                    }

                }
                else if (item.objectrange == 0) // 自定义
                {
                    var listdatadetail = GetDetailList(r => r.auth_id == item.auth_id);
                    var userIds = listdatadetail.Where(r => r.objecttype == 0).Select(r => r.object_id).ToList();
                    var deptIds = listdatadetail.Where(r => r.objecttype == 1).Select(r => r.object_id).ToList();
                    var companyIds = listdatadetail.Where(r => r.objecttype == 2).Select(r => r.object_id).ToList();

                    data.UserIds.AddRangeNoRepeat(userIds);
                    data.DepartmentIds.AddRangeNoRepeat(deptIds);
                    data.CompanyIds.AddRangeNoRepeat(companyIds);
                }
            }
            return data;
        }

        public DataAuthorizeModel GetDataAuthByUser(DataAuthorizeModuleEnum module, UserModel loginInfo)
        {
            var moduleName = module.ToString();
            var dataAuth = GetDataAuthByUser(moduleName, loginInfo);
            return dataAuth;
        }

        public Dictionary<string, object> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.auth_id == keyValue);

            entity.seeobjecttype = -1;// 默认为空
            if (entity != null)
            {
                var list = GetDetailList(r => r.auth_id == keyValue);
                if (!list.IsEmpty())
                {
                    entity.seeobjecttype = list[0].objecttype;
                    entity.seeobject_ids = string.Join(",", list.Select(r => r.object_id));
                    entity.seeobjectnames = string.Join(",", list.Select(r => r.objectname));
                }
            }
            var dic = new Dictionary<string, object>
            {
                {"DataAuthorize",entity }
            };
            return dic;
        }

        /// <summary>
        /// 判断通用权限 0 仅查看 1 可编辑起草 驳回 2 可编辑所有
        /// </summary>
        //public ResponseResult CanOperate(DataAuthorizeModuleEnum module, UserModel loginInfo)
        //{
        //    var dataauth = GetDataAuthByUser(module, loginInfo);
        //    if (dataauth.edittype == 0)  // 0 仅查看 1 可编辑起草 驳回 2 可编辑所有
        //    {
        //        if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
        //        {
        //            return ResponseResult.Fail("只有起草状态才能编辑");
        //        }
        //        if (old.create_userid != loginInfo.user_id)
        //        {
        //            return ResponseResult.Fail("只创建人才能删除");
        //        }
        //    }
        //    else if (dataauth.edittype == 1)
        //    {
        //        if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
        //        {
        //            return ResponseResult.Fail("只有起草状态才能删除");
        //        }
        //    }
        //    return ResponseResult.Success();
        //}

        #endregion

        #region  Set

        public void DeleteForm(sys_dataauthorizeEntity entity, UserModel loginInfo)
        {
            dataService.DeleteForm(entity, loginInfo);
            //cache.Remove(cacheKey + entity.modulename);
        }

        public void SaveForm(string keyValue, sys_dataauthorizeEntity entity, string strEntity, UserModel loginInfo)
        {
            var list = new List<sys_dataauthorize_detailEntity>();
            if (!entity.seeobject_ids.IsEmpty())
            {
                var ids = entity.seeobject_ids.SplitNoEmpty(',');
                var names = entity.seeobjectnames.SplitNoEmpty(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    var name = i < names.Length ? names[i] : "";

                    var detail = new sys_dataauthorize_detailEntity();
                    detail.object_id = id;
                    detail.objectname = name;
                    detail.objecttype = entity.seeobjecttype;

                    list.Add(detail);
                }
            }
            dataService.SaveForm(keyValue, entity, strEntity, list, loginInfo);
            //cache.Remove(cacheKey);
        }
        #endregion

    }
}
