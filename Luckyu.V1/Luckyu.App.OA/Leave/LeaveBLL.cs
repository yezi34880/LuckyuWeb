using Luckyu.App.Organization;
using Luckyu.App.Workflow;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public oa_leaveEntity GetEntity(Expression<Func<oa_leaveEntity, bool>> condition, Expression<Func<oa_leaveEntity, object>> orderExp = null, OrderByType orderType = OrderByType.Asc)
        {
            var entity = leaveService.GetEntity(condition, orderExp, orderType);
            return entity;
        }

        public ResponseResult<Dictionary<string, object>> GetFormData(string keyValue)
        {
            var entity = GetEntity(r => r.id == keyValue);
            if (entity == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>("该数据不存在");
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
                return ResponseResult.Fail("该数据不存在");
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
                    return ResponseResult.Fail<oa_leaveEntity>("该数据不存在");
                }
                if (old.state != (int)StateEnum.Draft && old.state != (int)StateEnum.Reject)
                {
                    return ResponseResult.Fail<oa_leaveEntity>("只有起草状态才能编辑");
                }
            }

            leaveService.SaveForm(keyValue, entity, strEntity, loginInfo);
            if (isSubmit)
            {
                var res = taskBLL.Create("Leave", entity.id, $"请假申请 {entity.id}", loginInfo);
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
