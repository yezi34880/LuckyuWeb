using FreeSql.Internal.Model;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Organization
{
    public class sys_postService : RepositoryFactory<sys_postEntity>
    {
        public JqgridPageResponse<sys_postEntity> Page(JqgridPageRequest jqpage)
        {
            Expression<Func<sys_postEntity, bool>> expCondition = r => r.is_delete == 0;

            var page = BaseRepository().GetPage(jqpage, expCondition);
            return page;
        }
        public List<sys_postEntity> GetSelect(JqgridPageRequest jqpage)
        {
            Expression<Func<sys_postEntity, bool>> expCondition = r => r.is_delete == 0 && r.is_enable == 1;
            var list = BaseRepository().GetList(jqpage, expCondition);
            return list;
        }

        public void DeleteForm(sys_postEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_postEntity entity, string strEntity, UserModel loginInfo)
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
