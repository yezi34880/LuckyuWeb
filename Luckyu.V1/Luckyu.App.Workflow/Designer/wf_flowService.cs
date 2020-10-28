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
    public class wf_flowService : RepositoryFactory<wf_flowEntity>
    {
        public JqgridPageResponse<wf_flowEntity> Page(JqgridPageRequest jqPage)
        {
            Expression<Func<wf_flowEntity, bool>> exp = r => r.is_delete == 0;
            var dicCondition = new Dictionary<string, Func<string, string, DynamicFilterInfo>>();
            dicCondition.Add("flowtype",
                (field, data) => SearchConditionHelper.GetStringEqualCondition(field, data, "-1")
                );
            dicCondition.Add("is_enable",
                (field, data) => SearchConditionHelper.GetStringEqualCondition(field, data, "-1")
                );

            var page = BaseRepository().GetPage(jqPage, exp, dicCondition);
            return page;
        }

        public void SaveForm(string keyValue, wf_flowEntity entity, string strEntity, List<wf_flow_authorizeEntity> listAuthorize, wf_flow_schemeEntity schemeEntity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var db = trans.db;
                if (keyValue.IsEmpty())
                {
                    entity.Create(loginInfo);
                    trans.Insert(entity);
                    foreach (var item in listAuthorize)
                    {
                        item.Create(entity.flow_id);
                    }
                    trans.Insert(listAuthorize);

                    schemeEntity.Create(entity.flow_id);
                    trans.Insert(schemeEntity);
                }
                else
                {
                    entity.Modify(keyValue, loginInfo);
                    trans.UpdateAppendColumns(entity, strEntity, r => new { r.edittime, r.edit_userid, r.edit_username });

                    trans.Delete<wf_flow_authorizeEntity>(r => r.flow_id == keyValue);
                    foreach (var item in listAuthorize)
                    {
                        item.Create(entity.flow_id);
                    }
                    trans.Insert(listAuthorize);

                    trans.Delete<wf_flow_schemeEntity>(r => r.flow_id == keyValue);
                    schemeEntity.Create(entity.flow_id);
                    trans.Insert(schemeEntity);
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void DeleteForm(wf_flowEntity entity, UserModel loginInfo)
        {
            entity.Remove(loginInfo);
            BaseRepository().UpdateOnlyColumns(entity, r => new { r.is_delete, r.delete_userid, r.delete_username, r.deletetime });
        }


    }
}
