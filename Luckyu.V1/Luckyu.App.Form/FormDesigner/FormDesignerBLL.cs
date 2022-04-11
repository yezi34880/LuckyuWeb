using System;
using System.Collections.Generic;
using System.Text;
using Luckyu.App.Organization;
using Luckyu.Utility;

namespace Luckyu.App.Form
{
    public class FormDesignerBLL
    {
        #region Var
        form_tableService tableService = new form_tableService();

        #endregion

        #region Get
        public JqgridPageResponse<form_tableEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var page = tableService.Page(jqPage, loginInfo);
            return page;
        }


        #endregion

    }
}
