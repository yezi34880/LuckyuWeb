using System;
using System.Collections.Generic;
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



    }
}
