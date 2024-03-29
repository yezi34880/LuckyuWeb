﻿
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

namespace Luckyu.App.OA
{
    public class oa_leaveService : RepositoryFactory<oa_leaveEntity>
    {
        #region Var
        private DataAuthorizeBLL dataBLL = new DataAuthorizeBLL();
        private DataBaseBLL dbBLL = new DataBaseBLL();
        #endregion

        public JqgridPageResponse<oa_leaveEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<oa_leaveEntity, bool>> exp = r => r.is_delete == 0;

            #region 数据权限
            var dataauth = dataBLL.GetDataAuthByUser(DataAuthorizeModuleEnum.Leave, loginInfo);
            if (dataauth != null)
            {
                Expression<Func<oa_leaveEntity, bool>> expSelf = r => r.user_id == loginInfo.user_id;
                if (!dataauth.IsAll)
                {
                    var authusers = dataauth.AllUserIds;
                    Expression<Func<oa_leaveEntity, bool>> exp1 = r => r.state == 1 && authusers.Contains(r.user_id);
                    if (dataauth.staterange == 1)
                    {
                        exp1 = r => authusers.Contains(r.user_id);
                    }
                    exp = exp.LinqAnd(expSelf.LinqOr(exp1));
                }
                else
                {
                    Expression<Func<oa_leaveEntity, bool>> exp1 = r => r.state == 1;
                    if (dataauth.staterange == 1)
                    {
                        exp1 = r => true;
                    }
                    exp = exp.LinqAnd(expSelf.LinqOr(exp1));
                }
            }
            else
            {
                exp = exp.LinqAnd(r => r.user_id == loginInfo.user_id);
            }

            #endregion

            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void DeleteForm(oa_leaveEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
            dbBLL.LogDelete(entity, loginInfo);
        }

        public void SaveForm(string keyValue, oa_leaveEntity entity, string strEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);
                    dbBLL.LogInsert(entity, loginInfo);
                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
                    dbBLL.LogUpdate(entity, strEntity, loginInfo);
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
