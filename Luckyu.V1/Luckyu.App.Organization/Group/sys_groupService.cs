
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class sys_groupService : RepositoryFactory<sys_groupEntity>
    {
        public JqgridPageResponse<sys_groupEntity> Page(JqgridPageRequest jqpage)
        {
            Expression<Func<sys_groupEntity, bool>> expCondition = r => r.is_delete == 0;

            var page = BaseRepository().GetPage(jqpage, expCondition);
            return page;
        }

        public List<sys_groupEntity> GetSelect(JqgridPageRequest jqpage)
        {
            Expression<Func<sys_groupEntity, bool>> expCondition = r => r.is_delete == 0 && r.is_enable == 1;
            var list = BaseRepository().GetList(jqpage, expCondition);
            return list;
        }

        public void DeleteForm(sys_groupEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_groupEntity entity, string strEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
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
