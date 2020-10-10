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
    public class wf_taskhistoryService : RepositoryFactory<wf_taskhistoryEntity>
    {

        /// <summary>
        /// 已办
        /// </summary>
        public JqgridPageResponse<wf_taskhistoryEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var db = BaseRepository().db;
            var query = db.Queryable<wf_taskhistoryEntity, wf_flow_instanceEntity>((th, fi) => th.instance_id == fi.instance_id)
                .Where((th, fi) => th.authorize_user_id == loginInfo.user_id);

            var querySelect = query.Select<wf_taskhistoryEntity>();
            var page = BaseRepository().GetPage(jqPage, querySelect);
            return page;
        }

    }
}
