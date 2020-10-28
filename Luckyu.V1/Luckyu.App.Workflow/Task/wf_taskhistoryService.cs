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
            var query = db.Select<wf_taskhistoryEntity, wf_flow_instanceEntity>()
                .InnerJoin((th, fi) => th.instance_id == fi.instance_id)
                .Where((th, fi) => th.authorize_user_id == loginInfo.user_id);

            //var querySelect = query.AsType(typeof(wf_taskhistoryEntity));
            //var page = BaseRepository().GetPage(jqPage, querySelect);
            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<wf_taskhistoryEntity>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }

    }
}
