

using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Form
{
    public class form_tableService : RepositoryFactory<form_tableEntity>
    {
        #region Var

        #endregion

        public JqgridPageResponse<form_tableEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<form_tableEntity, bool>> exp = r => r.is_delete == 0;
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void DeleteForm(form_tableEntity entity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                entity.Remove(loginInfo);
                trans.UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public ResponseResult InsertTable(string keyValue, form_tableEntity entity, UserModel loginInfo)
        {
            var repo = BaseRepository();
            var dbtableName = $"cusform_{entity.formcode}";
            var isExists = repo.db.DbMaintenance.IsAnyTable(dbtableName);
            if (isExists)
            {
                return ResponseResult.Fail("数据库表已存在，请联系管理员确认");
            }

            var trans = repo.BeginTrans();
            try
            {
                entity.Create(loginInfo);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            return ResponseResult.Success();
        }

    }
}