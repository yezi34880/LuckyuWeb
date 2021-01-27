using FreeSql.Internal.Model;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class sys_dataauthorizeService : RepositoryFactory<sys_dataauthorizeEntity>
    {
        public JqgridPageResponse<sys_dataauthorizeEntity> Page(JqgridPageRequest jqPage)
        {
            Expression<Func<sys_dataauthorizeEntity, bool>> exp = r => r.is_delete == 0;
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void DeleteForm(sys_dataauthorizeEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_dataauthorizeEntity entity, string strEntity, List<sys_dataauthorize_detailEntity> list, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var db = trans.db;
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);
                    foreach (var item in list)
                    {
                        item.Create(item.auth_id);
                    }
                    trans.Insert(list);
                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
                    trans.UpdateAppendColumns(entity, strEntity, r => new { r.edittime, r.edit_userid, r.edit_username });

                    trans.Delete<sys_dataauthorize_detailEntity>(r => r.auth_id == entity.auth_id);
                    foreach (var item in list)
                    {
                        item.Create(item.auth_id);
                    }
                    trans.Insert(list);
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
