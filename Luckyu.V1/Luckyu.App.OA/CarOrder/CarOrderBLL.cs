using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Luckyu.App.OA
{
    public class CarOrderBLL
    {
        #region Var
        private oa_carorderService carorderService = new oa_carorderService();
        private WFTaskBLL taskBLL = new WFTaskBLL();
        private DataAuthorizeBLL dataBLL = new DataAuthorizeBLL();
        private AnnexFileBLL fileBLL = new AnnexFileBLL();
        private DataBaseBLL dbBLL = new DataBaseBLL();
        #endregion

        #region Get
        public JqgridPageResponse<oa_carorderEntity> Page(JqgridPageRequest jqpage, UserModel loginInfo)
        {
            var page = carorderService.Page(jqpage, loginInfo);
            return page;
        }

        public oa_carorderEntity GetEntity(Expression<Func<oa_carorderEntity, bool>> condition)
        {
            var entity = carorderService.GetEntity(condition);
            return entity;
        }

        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.order_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var annex = fileBLL.GetPreviewList(r => r.external_id == keyValue);
            var dic = new Dictionary<string, object>
            {
                {"CarOrder",entity },
                {"Annex",annex }
            };
            return ResponseResult.Success(dic);
        }

        #endregion

        #region Set
        /// <summary>
        /// 生效撤回
        /// </summary>
        public async Task<ResponseResult> Revoke(string keyValue, UserModel loginInfo, IHubContext<MessageHub> hubContext)
        {
            var entity = GetEntity(r => r.order_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            if (entity.state != (int)StateEnum.Effect)
            {
                return ResponseResult.Fail("只有生效状态才能请求生效撤回");
            }
            var json = JsonConvert.SerializeObject(entity);
            var res = await taskBLL.Create(FlowEnum.Car_Revoke, entity.order_id, $"约车-生效撤回 {entity.username}", json, loginInfo, hubContext);
            if (res.code != 200)
            {
                return ResponseResult.Fail(res.info);
            }
            return ResponseResult.Success();
        }

        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.order_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            var dataauth = dataBLL.GetDataAuthByUser(DataAuthorizeModuleEnum.Leave, loginInfo);
            if (dataauth != null)
            {
                if (dataauth.edittype == 0)
                {
                    if (entity.state != (int)StateEnum.Draft)
                    {
                        return ResponseResult.Fail("只有起草状态才能删除");
                    }
                    if (entity.create_userid != loginInfo.user_id)
                    {
                        return ResponseResult.Fail("只创建人才能删除");
                    }
                }
                else if (dataauth.edittype == 1)
                {
                    if (entity.state != (int)StateEnum.Draft)
                    {
                        return ResponseResult.Fail("只有起草状态才能删除");
                    }
                }

            }
            carorderService.DeleteForm(entity, loginInfo);
            return ResponseResult.Success();
        }

        public async Task<ResponseResult<oa_carorderEntity>> SaveForm(string keyValue, string strEntity, List<string> deleteAnnex, int isSubmit, UserModel loginInfo, IHubContext<MessageHub> messageHubContext)
        {
            var entity = strEntity.ToObject<oa_carorderEntity>();
            // 修改 权限判断
            if (!keyValue.IsEmpty())
            {
                var old = GetEntity(r => r.order_id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<oa_carorderEntity>(MessageString.NoData);
                }
                var dataauth = dataBLL.GetDataAuthByUser(DataAuthorizeModuleEnum.Leave, loginInfo);
                if (dataauth != null)
                {
                    if (dataauth.edittype == 0)
                    {
                        if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                        {
                            return ResponseResult.Fail<oa_carorderEntity>("只有起草状态才能编辑");
                        }
                        if (old.create_userid != loginInfo.user_id)
                        {
                            return ResponseResult.Fail<oa_carorderEntity>("只创建人才能删除");
                        }
                    }
                    else if (dataauth.edittype == 1)
                    {
                        if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                        {
                            return ResponseResult.Fail<oa_carorderEntity>("只有起草状态才能删除");
                        }
                    }

                }

            }

            // 统一的后台验证
            if (isSubmit > 0)
            {
                var res = dbBLL.CheckEntity(entity);
                if (res.code == (int)ResponseCode.Fail)
                {
                    return ResponseResult.Fail<oa_carorderEntity>(res.info);
                }
            }

            carorderService.SaveForm(keyValue, entity, strEntity, loginInfo);
            fileBLL.DeleteAnnexs(deleteAnnex);

            // 提交审批
            if (isSubmit > 0)
            {
                var json = JsonConvert.SerializeObject(entity);
                // 0 起草  1 生效  2 审批中  3 退回
                var res = await taskBLL.Create(FlowEnum.CarOrder, entity.order_id, $"{entity.username}", json, loginInfo, messageHubContext);
                if (res.code != 200)
                {
                    return ResponseResult.Fail<oa_carorderEntity>(res.info);
                }
            }
            return ResponseResult.Success(entity);
        }

        public ResponseResult<oa_carorderEntity> ApproveSave(string keyValue, string strEntity, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<oa_carorderEntity>();
            carorderService.SaveForm(keyValue, entity, strEntity, loginInfo);
            return ResponseResult.Success(entity);
        }

        #endregion


    }

}
