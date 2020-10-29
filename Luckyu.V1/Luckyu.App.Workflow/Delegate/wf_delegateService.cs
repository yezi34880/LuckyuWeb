using FreeSql.Internal.Model;
using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class wf_delegateService : RepositoryFactory<wf_delegateEntity>
    {
        public JqgridPageResponse<wf_delegateEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<wf_delegateEntity, bool>> exp = r => r.create_userid == loginInfo.user_id;
            var dicCondition = new Dictionary<string, Func<string, string, DynamicFilterInfo>>();
            dicCondition.Add("starttime",
                (field, data) => SearchConditionHelper.GetDateCondition(field, data)
                );
            dicCondition.Add("endtime",
                (field, data) => SearchConditionHelper.GetDateCondition(field, data)
                );
            var page = BaseRepository().GetPage(jqPage, exp, dicCondition);
            return page;
        }

        public void DeleteForm(wf_delegateEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.deletetime, r.delete_userid, r.delete_username });
        }

        public void SaveForm(string keyValue, wf_delegateEntity entity, string strEntity, UserModel loginInfo)
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
