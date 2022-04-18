
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class sys_departmentmanageService : RepositoryFactory<sys_departmentmanageEntity>
    {
        public void SaveForm(string userId, List<sys_departmentmanageEntity> list, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.db.Deleteable<sys_departmentmanageEntity>().Where(r => r.user_id == userId).ExecuteCommand();
                foreach (var item in list)
                {
                    item.Create(loginInfo);
                    item.user_id = userId;
                }
                trans.db.Insertable(list).ExecuteCommand();
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
