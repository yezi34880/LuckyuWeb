﻿using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class wf_instanceService : RepositoryFactory<wf_instanceEntity>
    {
        /// <summary>
        /// 自己发起
        /// </summary>
        public JqgridPageResponse<wf_instanceEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<wf_instanceEntity, bool>> exp = r => r.submit_userid == loginInfo.user_id;
            if (jqPage.sidx.IsEmpty())
            {
                jqPage.sidx = "createtime";
                jqPage.sord = "DESC";
            }
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

    }
}
