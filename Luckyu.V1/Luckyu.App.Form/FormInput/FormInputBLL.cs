using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luckyu.App.Organization;
using Luckyu.Utility;

namespace Luckyu.App.Form
{
    public class FormInputBLL
    {
        #region Var
        FormBaseService formService = new FormBaseService();

        #endregion

        public JqgridDatatablePageResponse Page(JqgridPageRequest jqPage, string form_id, UserModel loginInfo)
        {
            var page = formService.Page(jqPage, form_id, loginInfo);
            return page;
        }

        public Dictionary<string, object> GetEntity(string form_id, string keyValue)
        {
            var entity = formService.GetEntity(form_id, keyValue);
            return entity;
        }

        public Dictionary<string, object> GetFormData(string form_id, string keyValue)
        {
            var data = new Dictionary<string, object>();
            var entity = GetEntity(form_id, keyValue);
            data.Add("Main", entity);
            return data;
        }

        /// <summary>
        /// 获取自定义下拉框数据源
        /// </summary>
        public List<xmSelectTree> GetDataSource(string formcode, string columncode)
        {
            var dt = formService.GetDataSource(formcode, columncode);
            var xmdata = new List<xmSelectTree>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    xmdata.Add(new xmSelectTree
                    {
                        name = row["name"].ToString(),
                        value = row["value"].ToString()
                    });
                }
            }
            return xmdata;
        }

        public ResponseResult DeleteForm(string form_id, string keyValue, UserModel loginInfo)
        {
            formService.DeleteForm(form_id, keyValue, loginInfo);
            return ResponseResult.Success();
        }

        public ResponseResult SaveForm(string form_id, string keyValue, Dictionary<string, object> dicEntity, UserModel loginInfo)
        {
            if (!keyValue.IsEmpty())
            {
                var old = GetEntity(form_id, keyValue);
                if (old == null)
                {
                    return ResponseResult.Fail(MessageString.NoData);
                }
            }
            keyValue = formService.SaveForm(form_id, keyValue, dicEntity, loginInfo);
            return ResponseResult.Success();
        }

    }
}
