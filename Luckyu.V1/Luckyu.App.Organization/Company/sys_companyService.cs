using Luckyu.DataAccess;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.App.Organization
{
    public class sys_companyService : RepositoryFactory<sys_companyEntity>
    {
        public JqgridPageResponse<sys_companyEntity> Page(JqgridPageRequest jqpage)
        {
            Expression<Func<sys_companyEntity, bool>> expCondition = r => r.is_delete == 0;
            var dicCondition = new Dictionary<string, Func<string, string, List<IConditionalModel>>>();
            dicCondition.Add("is_enable",
                (field, data) => new List<IConditionalModel> { SearchConditionHelper.GetStringEqualCondition(field, data, "-1") }
                );
            dicCondition.Add("foundeddate",
                (field, data) => SearchConditionHelper.GetDateCondition(field, data)
                );
            var page = BaseRepository().GetPage(jqpage, expCondition, dicCondition);
            return page;
        }

        public void DeleteForm(sys_companyEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }

        public void SaveForm(string keyValue, sys_companyEntity entity, string strEntity, UserModel loginInfo)
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
