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
        public JqgridPageResponse<WFTaskModel> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var db = BaseRepository().db;
            var query = db.Select<wf_flow_instanceEntity, wf_taskhistoryEntity>()
                .InnerJoin((fi, th) => th.instance_id == fi.instance_id)
                .Where((fi, th) => th.authorize_user_id == loginInfo.user_id);

            if (!jqPage.sidx.IsEmpty())
            {
                switch (jqPage.sidx)
                {
                    case "createtime":
                        jqPage.sidx = "a.createtime";
                        break;
                }
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            else
            {
                jqPage.sidx = "b.createtime";
                jqPage.sord = "DESC";
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList<WFTaskModel>();
            var page = new JqgridPageResponse<WFTaskModel>
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
