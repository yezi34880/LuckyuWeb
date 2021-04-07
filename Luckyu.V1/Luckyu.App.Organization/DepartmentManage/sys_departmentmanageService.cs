using FreeSql.Internal.Model;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class sys_departmentmanageService : RepositoryFactory<sys_departmentmanageEntity>
    {
        private DepartmentBLL deptBLL = new DepartmentBLL();

        public void SaveForm(string userId, List<sys_departmentmanageEntity> list, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.db.Delete<sys_departmentmanageEntity>().Where(r => r.user_id == userId).ExecuteAffrows();
                foreach (var item in list)
                {
                    item.Create(loginInfo);
                    item.user_id = userId;
                }
                trans.db.Insert(list).ExecuteAffrows();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

    }
}
