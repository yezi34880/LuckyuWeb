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
        /// <summary>
        /// 待办任务
        /// </summary>
        public JqgridPageResponse<WFTaskModel> Page(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var db = BaseRepository().db;
            var query = db.Select<wf_flow_instanceEntity, wf_taskEntity, wf_task_authorizeEntity>().InnerJoin((fi, t, ta) => t.task_id == ta.task_id && t.instance_id == fi.instance_id)
                .Where((fi, t, ta) => t.is_done == 0 &&
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
                switch (jqPage.sidx)
                {
                    case "createtime":
                        jqPage.sidx = "a.createtime";
                        break;
                }
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
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

        public JqgridPageResponse<WFTaskModel> MonitorPage(JqgridPageRequest jqPage, int is_finished)
        {
            //Expression<Func<wf_flow_instanceEntity, bool>> exp = r => r.is_finished == is_finished;
            //var page = BaseRepository().GetPage(jqPage, exp);
            //return page;

            var db = BaseRepository().db;
            var query = db.Select<wf_flow_instanceEntity, wf_taskEntity>()
                .InnerJoin((fi, t) => t.instance_id == fi.instance_id)
                .Where((fi, t) => fi.is_finished == is_finished);

            if (!string.IsNullOrEmpty(jqPage.sidx))
            {
                switch (jqPage.sidx)
                {
                    case "createtime":
                        jqPage.sidx = "a.createtime";
                        break;
                }
                query = query.OrderBy($" {jqPage.sidx} {jqPage.sord} ");
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
                    trans.UpdateOnlyColumns(currentTask, r => new { r.is_done });
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
                if (instance.is_finished == 1)
                {
                    trans.UpdateOnlyColumns(instance, r => new { r.is_finished });
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
                foreach (var task in listTask)
                {
                    task.is_done = 1;
                    trans.UpdateOnlyColumns(task, r => new { r.is_done });
                }
                trans.Insert(history);
                if (!sql.IsEmpty())
                {
                    var sql1 = sql.Replace("@processId", $"{BaseConnection.ParaPre}processId");
                    trans.db.Ado.ExecuteNonQuery(sql1, new { processId = instance.process_id });
                }
                instance.is_finished = 1;
                trans.UpdateOnlyColumns(instance, r => new { r.is_finished });

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void Modify(wf_flow_instanceEntity instance, List<wf_taskEntity> oldTasks, wf_taskEntity newTask, wf_taskhistoryEntity history)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                if (instance != null)
                {
                    trans.UpdateOnlyColumns(instance, r => r.schemejson);
                }
                foreach (var task in oldTasks)
                {
                    trans.Delete<wf_task_authorizeEntity>(r => r.task_id == task.task_id);
                    trans.Delete(task);
                }
                trans.Insert(newTask);
                if (!newTask.authrizes.IsEmpty())
                {
                    trans.Insert(newTask.authrizes);
                }
                trans.Insert(history);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void Complete(wf_flow_instanceEntity instance, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                if (!listTask.IsEmpty())
                {
                    foreach (var task in listTask)
                    {
                        trans.InsertOrUpdate(listTask);
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
                if (instance != null)
                {
                    trans.UpdateOnlyColumns(instance, r => r.is_finished);
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
