using Luckyu.App.Organization;
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

        #endregion

        #region Set

        public void DeleteForm(oa_leaveEntity entity, UserModel loginInfo)
        {
            leaveService.DeleteForm(entity, loginInfo);
        }

        public void SaveForm(string keyValue, oa_leaveEntity entity, string strEntity, UserModel loginInfo)
        {
            leaveService.SaveForm(keyValue, entity, strEntity, loginInfo);
        }
        #endregion


    }
}
