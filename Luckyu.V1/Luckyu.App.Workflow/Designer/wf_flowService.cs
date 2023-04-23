
using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using Mapster;
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

            var page = BaseRepository().GetPage(jqPage, exp);
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

        public void CopyForm(wf_flowEntity entity, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var oldFlow = entity;
                var oldAuthorizes = trans.db.Queryable<wf_flow_authorizeEntity>().Where(r => r.flow_id == oldFlow.flow_id).ToList();
                var oldScheme = trans.db.Queryable<wf_flow_schemeEntity>().Where(r => r.flow_id == oldFlow.flow_id).First();

                var newFlow = oldFlow.Adapt<wf_flowEntity>();
                newFlow.Create(loginInfo);
                newFlow.flow_id = SnowflakeHelper.NewCode();
                newFlow.flowcode = "";
                newFlow.flowname = "";
                trans.Insert(newFlow);

                foreach (var oldauth in oldAuthorizes)
                {
                    var newAuth = oldFlow.Adapt<wf_flow_authorizeEntity>();
                    newAuth.Create(newFlow.flow_id);
                    newAuth.auth_id = SnowflakeHelper.NewCode();
                    trans.Insert(newAuth);
                }

                var newScheme = oldScheme.Adapt<wf_flow_schemeEntity>();

                #region 序列化里的ID也要排重下
                var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(newScheme.schemejson);
                var nodes = jsonObject.GetValue("nodes");
                var dirNodeChange = new Dictionary<string, string>();
                if (nodes != null)
                {
                    foreach (var node in nodes.Children())
                    {
                        var oldid = node["id"].ToString();
                        node["id"] = Guid.NewGuid().ToString();

                        dirNodeChange.Add(oldid, node["id"].ToString());

                        if (node["authusers"] != null)
                        {
                            foreach (var au in node["authusers"].Children())
                            {
                                au["id"] = Guid.NewGuid().ToString();
                            }
                        }
                        if (node["forms"] != null)
                        {
                            foreach (var form in node["forms"].Children())
                            {
                                form["id"] = Guid.NewGuid().ToString();
                            }
                        }
                    }
                }
                var lines = jsonObject.GetValue("lines");
                if (lines != null)
                {
                    foreach (var line in lines.Children())
                    {
                        line["id"] = Guid.NewGuid().ToString();
                        if (dirNodeChange.ContainsKey(line["from"].ToString()))
                        {
                            line["from"] = dirNodeChange[line["from"].ToString()];
                        }
                        if (dirNodeChange.ContainsKey(line["to"].ToString()))
                        {
                            line["to"] = dirNodeChange[line["to"].ToString()];
                        }
                    }
                }

                newScheme.schemejson = jsonObject.ToString();

                #endregion

                newScheme.Create(newFlow.flow_id);
                newScheme.flow_id = SnowflakeHelper.NewCode();
                trans.Insert(newScheme);

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
