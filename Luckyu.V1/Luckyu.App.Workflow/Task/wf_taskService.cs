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
    public class wf_taskService : RepositoryFactory<wf_taskEntity>
    {
        public JqgridPageResponse<wf_taskEntity> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var db = BaseRepository().db;
            var query = db.Select<wf_taskEntity, wf_task_authorizeEntity, wf_flow_instanceEntity>().InnerJoin((t, ta, fi) => t.task_id == ta.task_id && t.instance_id == fi.instance_id)
                .Where((t, ta, fi) => t.is_done == 0 && t.is_finished == 0 &&
                ta.user_id == loginInfo.user_id  // 用户
                || loginInfo.group_ids.Contains(ta.group_id)   // 小组
                || (string.IsNullOrEmpty(ta.post_id) && string.IsNullOrEmpty(ta.role_id) && ta.department_id == loginInfo.department_id)    // 按部门审批
                || (string.IsNullOrEmpty(ta.post_id) && string.IsNullOrEmpty(ta.role_id) && ta.company_id == loginInfo.company_id)  // 按公司审批
                || (loginInfo.post_ids.Contains(ta.post_id) && string.IsNullOrEmpty(ta.department_id) && string.IsNullOrEmpty(ta.company_id) && string.IsNullOrEmpty(ta.manage_dept_id))   // 岗位   不仅判断 下面的人员 还需要判断 同公司 同部门 分管
                || (loginInfo.post_ids.Contains(ta.post_id) && !string.IsNullOrEmpty(ta.department_id) && ta.department_id == loginInfo.department_id)
                || (loginInfo.post_ids.Contains(ta.post_id) && !string.IsNullOrEmpty(ta.company_id) && ta.company_id == loginInfo.company_id)
                || (loginInfo.post_ids.Contains(ta.post_id) && !string.IsNullOrEmpty(ta.manage_dept_id) && loginInfo.manage_dept_ids.Contains(ta.manage_dept_id))
                || (loginInfo.post_ids.Contains(ta.role_id) && string.IsNullOrEmpty(ta.department_id) && string.IsNullOrEmpty(ta.company_id) && string.IsNullOrEmpty(ta.manage_dept_id))   //   角色 不仅判断 下面的人员 还需要判断 同公司 同部门 分管
                || (loginInfo.post_ids.Contains(ta.role_id) && !string.IsNullOrEmpty(ta.department_id) && ta.department_id == loginInfo.department_id)
                || (loginInfo.post_ids.Contains(ta.role_id) && !string.IsNullOrEmpty(ta.company_id) && ta.company_id == loginInfo.company_id)
                || (loginInfo.post_ids.Contains(ta.role_id) && !string.IsNullOrEmpty(ta.manage_dept_id) && loginInfo.manage_dept_ids.Contains(ta.manage_dept_id))
            );

            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
            }
            var list = query.Count(out var total).Page(jqPage.page, jqPage.rows).ToList();
            var page = new JqgridPageResponse<wf_taskEntity>
            {
                count = jqPage.rows,
                page = jqPage.page,
                records = (int)total,
                rows = list,
            };
            return page;
        }

        public JqgridPageResponse<wf_taskEntity> MonitorPage(JqgridPageRequest jqPage)
        {
            Expression<Func<wf_taskEntity, bool>> exp = r => r.is_done == 0;
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void Create(wf_flow_instanceEntity instance, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.Insert(instance);
                if (!listTask.IsEmpty())
                {
                    trans.Insert(listTask);
                    foreach (var task in listTask)
                    {
                        if (!task.authrizes.IsEmpty())
                        {
                            trans.Insert(task.authrizes);
                        }
                    }
                }
                if (!listHistory.IsEmpty())
                {
                    trans.Insert(listHistory);
                }
                if (!listSql.IsEmpty())
                {
                    // 执行 审核 sql
                    foreach (var sql in listSql)
                    {
                        if (!sql.IsEmpty())
                        {
                            var sql1 = sql.Replace("@processId", $"{BaseConnection.ParaPre}processId");
                            trans.db.Ado.ExecuteNonQuery(sql1, new { processId = instance.process_id });
                        }
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void Approve(wf_flow_instanceEntity instance, wf_taskEntity currentTask, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                if (currentTask.is_done == 1)
                {
                    trans.UpdateOnlyColumns(currentTask, r => new { r.is_done, r.is_finished });
                }
                if (currentTask.is_finished == 1)
                {
                    trans.UpdateOnlyColumns(instance, r => new { r.is_finished });
                }
                if (!listTask.IsEmpty())
                {
                    trans.Insert(listTask);
                    foreach (var task in listTask)
                    {
                        if (!task.authrizes.IsEmpty())
                        {
                            trans.Insert(task.authrizes);
                        }
                    }
                }
                if (!listHistory.IsEmpty())
                {
                    trans.Insert(listHistory);
                }
                if (!listSql.IsEmpty())
                {
                    // 执行 审核 sql
                    foreach (var sql in listSql)
                    {
                        if (!sql.IsEmpty())
                        {
                            var sql1 = sql.Replace("@processId", $"{BaseConnection.ParaPre}processId");
                            trans.db.Ado.ExecuteNonQuery(sql1, new { processId = instance.process_id });
                        }
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void AddUser(wf_task_authorizeEntity auth, wf_taskhistoryEntity history)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.Insert(auth);
                trans.Insert(history);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void Finish(wf_flow_instanceEntity instance, List<wf_taskEntity> listTask, wf_taskhistoryEntity history, string sql)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                instance.is_finished = 1;
                trans.UpdateOnlyColumns(instance, r => new { r.is_finished });

                foreach (var task in listTask)
                {
                    task.is_done = 1;
                    task.is_finished = 1;
                    trans.UpdateOnlyColumns(task, r => new { r.is_done, r.is_finished });
                }
                trans.Insert(history);
                if (!sql.IsEmpty())
                {
                    var sql1 = sql.Replace("@processId", $"{BaseConnection.ParaPre}processId");
                    trans.db.Ado.ExecuteNonQuery(sql1, new { processId = instance.process_id });
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
    }
}
