using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<xmSelectTree> GetTableSelectList(Expression<Func<form_tableEntity, bool>> condition)
        {
            var list = tableService.GetList(condition);
            var selectList = list.Select(r => new xmSelectTree
            {
                name = r.formname,
                value = r.form_id
            }).ToList();
            return selectList;
        }

        public List<form_columnEntity> GetColumnList(Expression<Func<form_columnEntity, bool>> condition)
        {
            var list = colService.GetList(condition);
            return list;
        }

        #endregion

        #region Set
        public ResponseResult<form_tableEntity> SaveForm(string keyValue, string strEntity, UserModel loginInfo)
        {
            var formEntity = strEntity.ToObject<form_tableEntity>();
            var list = formEntity.formjson.ToObject<List<form_columnEntity>>();
            if (keyValue.IsEmpty())
            {
                tableService.InsertTable(formEntity, list, loginInfo);
            }
            else
            {
                var old = GetTableEntity(r => r.form_id == keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail<form_tableEntity>(MessageString.NoData);
                }
                tableService.UpdateTable(keyValue, formEntity, list, loginInfo);
            }
            return ResponseResult.Success<form_tableEntity>();
        }
        #endregion

    }
}
