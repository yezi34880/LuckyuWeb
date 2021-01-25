﻿using Luckyu.App.Organization;
using Luckyu.App.System;
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
        private DataAuthorizeBLL dataBLL = new DataAuthorizeBLL();
        private AnnexFileBLL fileBLL = new AnnexFileBLL();
        private DataBaseBLL dbBLL = new DataBaseBLL();
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
            var entity = GetEntity(r => r.leave_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var annex = fileBLL.GetPreviewList(r => r.external_id == keyValue);
            var dic = new Dictionary<string, object>
            {
                {"Leave",entity },
                {"Annex",annex }
            };
            return ResponseResult.Success(dic);
        }

        #endregion

        #region Set

        public ResponseResult DeleteForm(string keyValue, UserModel loginInfo)
        {
            var entity = GetEntity(r => r.leave_id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail(MessageString.NoData);
            }
            var dataauth = dataBLL.GetDataAuthByUser(DataAuthorizeModuleEnum.Leave, loginInfo);
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
            leaveService.DeleteForm(entity, loginInfo);
            return ResponseResult.Success();
        }

        public ResponseResult<oa_leaveEntity> SaveForm(string keyValue, string strEntity, int isSubmit, UserModel loginInfo)
        {
            var entity = strEntity.ToObject<oa_leaveEntity>();
            if (!keyValue.IsEmpty()) // 修改
            {
                var old = GetEntity(r => r.leave_id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<oa_leaveEntity>(MessageString.NoData);
                }
                var dataauth = dataBLL.GetDataAuthByUser(DataAuthorizeModuleEnum.Leave, loginInfo);
                if (dataauth.edittype == 0)
                {
                    if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                    {
                        return ResponseResult.Fail<oa_leaveEntity>("只有起草状态才能编辑");
                    }
                    if (old.create_userid != loginInfo.user_id)
                    {
                        return ResponseResult.Fail<oa_leaveEntity>("只创建人才能删除");
                    }
                }
                else if (dataauth.edittype == 1)
                {
                    if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                    {
                        return ResponseResult.Fail<oa_leaveEntity>("只有起草状态才能删除");
                    }
                }
            }

            // 统一的后台验证
            if (isSubmit > 0)
            {
                var res = dbBLL.CheckEntity(entity);
                if (res.code == (int)ResponseCode.Fail)
                {
                    return ResponseResult.Fail<oa_leaveEntity>(res.info);
                }
            }

            leaveService.SaveForm(keyValue, entity, strEntity, loginInfo);
            if (isSubmit > 0)
            {
                var json = JsonConvert.SerializeObject(entity);
                // 0 起草  1 生效  2 审批中  3 驳回
                var res = taskBLL.Create(FlowEnum.Leave, entity.leave_id, $"请假申请 {entity.username}", json, loginInfo);
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
