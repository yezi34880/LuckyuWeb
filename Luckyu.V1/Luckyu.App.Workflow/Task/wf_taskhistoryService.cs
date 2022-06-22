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
            var query = db.Queryable<wf_instanceEntity, wf_taskhistoryEntity>((fi, th) => th.instance_id == fi.instance_id)
                .Where((fi, th) => th.app_userid == loginInfo.user_id);

            if (!jqPage.sidx.IsEmpty())
            {
                switch (jqPage.sidx)
                {
                    case "createtime":
                        jqPage.sidx = "fi.createtime";
                        break;
                }
            }
            else
            {
                jqPage.sidx = "fi.createtime";
                jqPage.sord = "DESC";
            }
            query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            var total = 0;
            var list = query.Select<WFTaskModel>().ToPageList(jqPage.page, jqPage.rows, ref total);
            var page = new JqgridPageResponse<WFTaskModel>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = total,
                rows = list,
            };
            return page;
        }

        public void DeleteHistory(string historyId)
        {
            var db = BaseRepository().db;
            db.Deleteable<wf_taskhistoryEntity>().Where(r => r.history_id == historyId).ExecuteCommand();
        }

    }
}
