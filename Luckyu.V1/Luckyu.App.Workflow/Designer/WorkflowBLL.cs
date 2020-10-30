using Luckyu.App.Organization;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WorkflowBLL
    {
        #region Var
        private wf_flowService flowService = new wf_flowService();
        private wf_flow_authorizeService flowauthService = new wf_flow_authorizeService();
        private wf_flow_shemeService flowshemeService = new wf_flow_shemeService();

        #endregion

        #region Get
        public JqgridPageResponse<wf_flowEntity> Page(JqgridPageRequest jqPage)
        {
            var page = flowService.Page(jqPage);
            return page;
        }

        public wf_flowEntity GetEntity(Expression<Func<wf_flowEntity, bool>> condition, Expression<Func<wf_flowEntity, object>> orderExp = null, bool isDesc = false)
        {
            var entity = flowService.GetEntity(condition, orderExp, isDesc);
            return entity;
        }
        public List<wf_flowEntity> GetList(Expression<Func<wf_flowEntity, bool>> condition, Expression<Func<wf_flowEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = flowService.GetList(condition, orderExp, isDesc);
            return list;
        }

        public List<wf_flow_authorizeEntity> GetAuthorizeList(Expression<Func<wf_flow_authorizeEntity, bool>> condition, Expression<Func<wf_flow_authorizeEntity, object>> orderExp = null, bool isDesc = false)
        {
            var list = flowauthService.GetList(condition, orderExp, isDesc);
            return list;
        }

        public wf_flow_schemeEntity GetSchemeEntity(Expression<Func<wf_flow_schemeEntity, bool>> condition, Expression<Func<wf_flow_schemeEntity, object>> orderExp = null, bool isDesc = false)
        {
            var entity = flowshemeService.GetEntity(condition, orderExp, isDesc);
            return entity;
        }

        #endregion

        #region Set
        public void DeleteForm(wf_flowEntity entity, UserModel loginInfo)
        {
            flowService.DeleteForm(entity, loginInfo);
        }
        public void SaveForm(string keyValue, wf_flowEntity entity, string strEntity, List<WFAuthorizeModel> listAuthModel, string schemejson, UserModel loginInfo)
        {
            var listAuthorize = new List<wf_flow_authorizeEntity>();
            if (!listAuthModel.IsEmpty())
            {
                foreach (var model in listAuthModel)
                {
                    var objectids = model.objectids.SplitWithoutEmpty(',');
                    foreach (var objectid in objectids)
                    {
                        var auth = new wf_flow_authorizeEntity
                        {
                            objecttype = model.objecttype,
                            object_id = objectid
                        };
                        listAuthorize.Add(auth);
                    }
                }
            }
            var schemeEntity = new wf_flow_schemeEntity();
            schemeEntity.schemejson = schemejson;
            flowService.SaveForm(keyValue, entity, strEntity, listAuthorize, schemeEntity, loginInfo);
        }
        #endregion

    }
}
