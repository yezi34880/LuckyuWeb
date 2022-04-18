using Luckyu.DataAccess;
using Luckyu.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class wf_task_authorizeService : RepositoryFactory<wf_task_authorizeEntity>
    {
        public void DeleteTaskAuth(string authId)
        {
            var db = BaseRepository().db;
            db.Deleteable<wf_task_authorizeEntity>().Where(r => r.auth_id == authId).ExecuteCommand();
        }

        public List<WFTaskAuthModel> GetAddUserPage(JqgridPageRequest jqPage, string instanceId)
        {
            var db = BaseRepository().db;
            var query = db.Queryable<wf_task_authorizeEntity, wf_taskEntity>((ta, t) => ta.task_id == t.task_id)
                .Where((ta, t) => t.instance_id == instanceId && t.is_done == 0 && ta.is_add == 1);

            var list = query.Select<WFTaskAuthModel>().ToList();
            return list;
        }

    }
}
