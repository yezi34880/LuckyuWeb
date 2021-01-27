using FreeSql.Internal.Model;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class sys_departmentService : RepositoryFactory<sys_departmentEntity>
    {
        public JqgridPageResponse<sys_departmentEntity> Page(string companyId, JqgridPageRequest jqpage)
        {
            Expression<Func<sys_departmentEntity, bool>> expCondition = r => r.is_delete == 0;
            if (!companyId.IsEmpty() && companyId != "-1")
            {
                expCondition = expCondition.LinqAnd(r => r.company_id == companyId);
            }
            var page = BaseRepository().GetPage(jqpage, expCondition);
            return page;
        }

        public void DeleteForm(sys_departmentEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_departmentEntity entity, string strEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var db = trans.db;
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
                    trans.UpdateAppendColumns(entity, strEntity, r => new { r.edittime, r.edit_userid, r.edit_username });
                }
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
