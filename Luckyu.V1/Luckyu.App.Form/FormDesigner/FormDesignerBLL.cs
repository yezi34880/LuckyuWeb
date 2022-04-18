using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Luckyu.App.Organization;
using Luckyu.Utility;

namespace Luckyu.App.Form
{
    public class FormDesignerBLL
    {
        #region Var
        form_tableService tableService = new form_tableService();
        form_columnService colService = new form_columnService();

        #endregion

        #region Get
        public JqgridPageResponse<form_tableEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var page = tableService.Page(jqPage, loginInfo);
            return page;
        }

        public form_tableEntity GetTableEntity(Expression<Func<form_tableEntity, bool>> condition)
        {
            var entity = tableService.GetEntity(condition);
            return entity;
        }

        #endregion

        #region Set
        public ResponseResult<form_tableEntity> SaveForm(string keyValue, string strEntity)
        {
            var formEntity = strEntity.ToObject<form_tableEntity>();
            if (keyValue.IsEmpty())
            {


            }
            else
            {
                var old = GetTableEntity(r => r.form_id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<form_tableEntity>(MessageString.NoData);
                }



            }
            return ResponseResult.Success<form_tableEntity>();
        }
        #endregion

    }
}
