using FreeSql.Internal.Model;
using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Luckyu.App.OA
{
    public class oa_newsService : RepositoryFactory<oa_newsEntity>
    {
        #region Var
        private DataAuthorizeBLL dataBLL = new DataAuthorizeBLL();
        private DataBaseBLL dbBLL = new DataBaseBLL();
        #endregion

        public JqgridPageResponse<oa_newsEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<oa_newsEntity, bool>> exp = r => r.is_delete == 0;

            if (jqPage.sidx.IsEmpty())
            {
                jqPage.sord = "";
                jqPage.sidx = "sort DESC,publishtime DESC";
            }
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }
        public JqgridPageResponse<oa_newsEntity> ShowPage(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<oa_newsEntity, bool>> exp = r => r.is_delete == 0 && r.is_publish == 1;

            if (jqPage.sidx.IsEmpty())
            {
                jqPage.sord = "";
                jqPage.sidx = "sort DESC,publishtime DESC";
            }
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void DeleteForm(oa_newsEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
            dbBLL.LogDelete(entity, loginInfo);
        }

        public void Publish(oa_newsEntity entity, UserModel loginInfo)
        {
            entity.is_publish = 1 - entity.is_publish;
            entity.edittime = DateTime.Now;
            entity.edit_userid = loginInfo.user_id;
            entity.edit_username = loginInfo.realname;
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_publish, r.edit_userid, r.edit_username, r.edittime });
        }
        public void SetTop(oa_newsEntity entity, UserModel loginInfo)
        {
            entity.sort = 99 - entity.sort;
            entity.edittime = DateTime.Now;
            entity.edit_userid = loginInfo.user_id;
            entity.edit_username = loginInfo.realname;
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.sort, r.edit_userid, r.edit_username, r.edittime });
        }

        public void SaveForm(string keyValue, oa_newsEntity entity, string strEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var db = trans.db;
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);
                    dbBLL.LogInsert(entity, loginInfo);
                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
                    trans.UpdateAppendColumns(entity, strEntity, r => new { r.edittime, r.edit_userid, r.edit_username });
                    dbBLL.LogUpdate(entity, strEntity, loginInfo);
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
