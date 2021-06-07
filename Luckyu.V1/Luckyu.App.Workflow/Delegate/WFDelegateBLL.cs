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
    public class WFDelegateBLL
    {
        #region Var
        private wf_delegateService delegateService = new wf_delegateService();

        #endregion

        #region Get
        public JqgridPageResponse<wf_delegateEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var page = delegateService.Page(jqPage, loginInfo);
            return page;
        }

        public wf_delegateEntity GetEntity(Expression<Func<wf_delegateEntity, bool>> conditiion)
        {
            var entity = delegateService.GetEntity(conditiion);
            return entity;
        }
        public List<wf_delegateEntity> GetList(Expression<Func<wf_delegateEntity, bool>> conditiion, Expression<Func<wf_delegateEntity, object>> orderby = null, bool isDesc = false)
        {
            var list = delegateService.GetList(conditiion, orderby, isDesc);
            return list;
        }

        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.delegate_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var dic = new Dictionary<string, object>
            {
                {"Delegate",entity }
            };
            return ResponseResult.Success(dic);
        }


        #endregion

        #region Set
        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.delegate_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            delegateService.DeleteForm(entity, loginInfo);
            return ResponseResult.Success();
        }

        public ResponseResult<wf_delegateEntity> SaveForm(string keyValue, string strEntity, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<wf_delegateEntity>();
            if (entity.flowcode.Contains("ALL"))
            {
                entity.flowcode = "ALL";
            }
            delegateService.SaveForm(keyValue, entity, strEntity, loginInfo);
            return ResponseResult.Success(entity);
        }
        #endregion
    }
}
