using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Luckyu.App.OA
{
    public class LeaveBLL
    {
        #region Var
        private oa_leaveService leaveService = new oa_leaveService();
        private WFTaskBLL taskBLL = new WFTaskBLL();
        #endregion

        #region Get
        public JqgridPageResponse<oa_leaveEntity> Page(JqgridPageRequest jqpage, UserModel loginInfo)
        {
            var page = leaveService.Page(jqpage, loginInfo);
            return page;
        }

        public oa_leaveEntity GetEntity(Expression<Func<oa_leaveEntity, bool>> condition)
        {
            var entity = leaveService.GetEntity(condition);
            return entity;
        }

        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var dic = new Dictionary<string, object>
            {
                {"Leave",entity }
            };
            return ResponseResult.Success(dic);
        }

        #endregion

        #region Set

        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            if (entity.state != (int)StateEnum.Draft)
            {
                return ResponseResult.Fail("只有起草状态才能删除");
            }
            leaveService.DeleteForm(entity, loginInfo);
            return ResponseResult.Success();
        }

        public ResponseResult<oa_leaveEntity> SaveForm(string keyValue, string strEntity, bool isSubmit, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<oa_leaveEntity>();
            if (!keyValue.IsEmpty())
            {
                var old = GetEntity(r => r.id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<oa_leaveEntity>(MessageString.NoData);
                }
                if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                {
                    return ResponseResult.Fail<oa_leaveEntity>("只有起草状态才能编辑");
                }
            }

            leaveService.SaveForm(keyValue, entity, strEntity, loginInfo);
            if (isSubmit)
            {
                var json = JsonConvert.SerializeObject(entity);
                // 0 起草  1 生效  2 审批中  3 驳回
                var res = taskBLL.Create("Leave", entity.id, $"请假申请 {entity.id}", json, loginInfo);
                if (res.code != 200)
                {
                    return ResponseResult.Fail<oa_leaveEntity>(res.info);
                }
            }
            return ResponseResult.Success(entity);
        }
        #endregion


    }
}
