using Luckyu.App.Organization;
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
    public class wf_flow_instanceService : RepositoryFactory<wf_flow_instanceEntity>
    {
        /// <summary>
        /// 自己发起
        /// </summary>
        public JqgridPageResponse<wf_flow_instanceEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            Expression<Func<wf_flow_instanceEntity, bool>> exp = r => r.submit_user_id == loginInfo.user_id;
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

    }
}
