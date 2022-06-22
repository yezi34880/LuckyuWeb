using Luckyu.App.Organization;
using Luckyu.DataAccess;
using Luckyu.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            var query = GetActiveQueryable(loginInfo);
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }
            if (jqPage.sidx.IsEmpty())
            {
                jqPage.sidx = "fi.createtime";
                jqPage.sord = "DESC";
            }
            int total = 0;
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

        private ISugarQueryable<wf_instanceEntity, wf_taskEntity, wf_task_authorizeEntity> GetActiveQueryable(UserModel loginInfo)
        {
            var db = BaseRepository().db;

            var roledepts = loginInfo.managedepartments.Where(r => r.relationtype == 1).Select(r => new ValueTuple<string, string>(r.object_id, r.department_id)).ToList();
            var postdepts = loginInfo.managedepartments.Where(r => r.relationtype == 2).Select(r => new ValueTuple<string, string>(r.object_id, r.department_id)).ToList();

            //var expPost = Expressionable.Create<wf_task_authorizeEntity>();
            //foreach (var item in postdepts)
            //{
            //    expPost.Or(it => it.post_id == item.Item1 && it.manage_dept_id == item.Item2);
            //}

            var query = db.Queryable<wf_instanceEntity, wf_taskEntity, wf_task_authorizeEntity>((fi, t, ta) => t.task_id == ta.task_id && t.instance_id == fi.instance_id)
                .Where((fi, t, ta) => fi.is_finished == 0 && t.is_done == 0 && (
                ta.user_id == loginInfo.user_id  // 用户
                || loginInfo.group_ids.Contains(ta.group_id)   // 小组
                || (string.IsNullOrEmpty(ta.post_id) && string.IsNullOrEmpty(ta.role_id) && ta.department_id == loginInfo.department_id)    // 按部门审批
                || (string.IsNullOrEmpty(ta.post_id) && string.IsNullOrEmpty(ta.role_id) && ta.company_id == loginInfo.company_id)  // 按公司审批

                || (loginInfo.post_ids.Contains(ta.post_id) && string.IsNullOrEmpty(ta.department_id) && string.IsNullOrEmpty(ta.company_id) && string.IsNullOrEmpty(ta.manage_dept_id))   // 岗位   不仅判断 下面的人员 还需要判断 同公司 同部门 分管
                || (loginInfo.post_ids.Contains(ta.post_id) && !string.IsNullOrEmpty(ta.department_id) && ta.department_id == loginInfo.department_id)
                || (loginInfo.post_ids.Contains(ta.post_id) && !string.IsNullOrEmpty(ta.company_id) && ta.company_id == loginInfo.company_id)
                || (loginInfo.post_ids.Contains(ta.post_id) && !string.IsNullOrEmpty(ta.manage_dept_id) && postdepts.Contains(ta.post_id, ta.manage_dept_id))

                || (loginInfo.role_ids.Contains(ta.role_id) && string.IsNullOrEmpty(ta.department_id) && string.IsNullOrEmpty(ta.company_id) && string.IsNullOrEmpty(ta.manage_dept_id))   //   角色 不仅判断 下面的人员 还需要判断 同公司 同部门 分管
                || (loginInfo.role_ids.Contains(ta.role_id) && !string.IsNullOrEmpty(ta.department_id) && ta.department_id == loginInfo.department_id)
                || (loginInfo.role_ids.Contains(ta.role_id) && !string.IsNullOrEmpty(ta.company_id) && ta.company_id == loginInfo.company_id)
                || (loginInfo.role_ids.Contains(ta.role_id) && !string.IsNullOrEmpty(ta.manage_dept_id) && roledepts.Contains(ta.role_id, ta.manage_dept_id))
                )
            );

            return query;
        }

        public int GetActiveCount(UserModel loginInfo)
        {
            var query = GetActiveQueryable(loginInfo);
            var count = query.Count();
            return count;
        }

        /// <summary>
        /// 委托待办 列表
        /// </summary>
        public JqgridPageResponse<WFTaskModel> DelegatePage(JqgridPageRequest jqPage, UserModel loginInfo)
        {
            var query = GetDelegateQueryable(loginInfo);
            var filters = SearchConditionHelper.ContructJQCondition(jqPage);
            if (!filters.IsEmpty())
            {
                query = query.Where(filters);
            }

            if (jqPage.sidx.IsEmpty())
            {
                jqPage.sidx = "fi.createtime";
                jqPage.sord = "DESC";
            }

            int total = 0;
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

        private ISugarQueryable<wf_instanceEntity, wf_taskEntity, wf_task_authorizeEntity> GetDelegateQueryable(UserModel loginInfo)
        {
            var db = BaseRepository().db;
            var now = DateTime.Now;
            var delegates = db.Queryable<wf_delegateEntity>().Where(r => r.is_delete == 0 && r.is_enable == 1 && now >= r.starttime && now < r.endtime && r.to_user_id == loginInfo.user_id).ToList();

            var delegate_userIds = delegates.Select(r => r.user_id).ToList();
            var roledepts = db.Queryable<sys_departmentmanageEntity>().Where(r => delegate_userIds.Contains(r.user_id) && r.relationtype == 1).ToList().Select(r => new ValueTuple<string, string>(r.object_id, r.department_id)).ToList();
            var postdepts = db.Queryable<sys_departmentmanageEntity>().Where(r => delegate_userIds.Contains(r.user_id) && r.relationtype == 2).ToList().Select(r => new ValueTuple<string, string>(r.object_id, r.department_id)).ToList();

            // 分流程的委托好难写 要考虑许多
            //  比如 两个流程 分别分给两个人，这两个人对应的 公司 部门 角色 岗位 组 都要和 流程关联起来，好好理一理
            var delegate_flow_user = delegates.Select(r => new ValueTuple<string, string>(r.flowcode.Trim(','), r.user_id)).ToList();
            var grouprelations = db.Queryable<sys_userrelationEntity>().Where(r => r.relationtype == 3 && delegate_userIds.Contains(r.user_id)).ToList();
            var delegate_flow_group = grouprelations.Select(r =>
            {
                var value = new ValueTuple<string, string>();
                var allcodeStr = string.Join(",", delegates.Where(t => t.user_id == r.user_id).Select(t => t.flowcode).ToList());
                var allcodeList = allcodeStr.SplitNoEmpty(',').Distinct();
                value.Item1 = string.Join(",", allcodeList);
                value.Item2 = r.object_id;
                return value;
            }).ToList();
            var rolerelations = db.Queryable<sys_userrelationEntity>().Where(r => r.relationtype == 1 && delegate_userIds.Contains(r.user_id)).ToList();
            var delegate_flow_role = rolerelations.Select(r =>
            {
                var value = new ValueTuple<string, string>();
                var allcodeStr = string.Join(",", delegates.Where(t => t.user_id == r.user_id).Select(t => t.flowcode).ToList());
                var allcodeList = allcodeStr.SplitNoEmpty(',').Distinct();
                value.Item1 = string.Join(",", allcodeList);
                value.Item2 = r.object_id;
                return value;
            }).ToList();
            var postrelations = db.Queryable<sys_userrelationEntity>().Where(r => r.relationtype == 2 && delegate_userIds.Contains(r.user_id)).ToList();
            var delegate_flow_post = postrelations.Select(r =>
            {
                var value = new ValueTuple<string, string>();
                var allcodeStr = string.Join(",", delegates.Where(t => t.user_id == r.user_id).Select(t => t.flowcode).ToList());
                var allcodeList = allcodeStr.SplitNoEmpty(',').Distinct();
                value.Item1 = string.Join(",", allcodeList);
                value.Item2 = r.object_id;
                return value;
            }).ToList();

            var users = db.Queryable<sys_userEntity>().Where(r => delegate_userIds.Contains(r.user_id)).ToList();
            var delegate_flow_dept = delegates.Select(r =>
            {
                var value = new ValueTuple<string, string>();
                value.Item1 = r.flowcode.Trim(',');
                value.Item2 = users.Where(t => t.user_id == r.user_id).Select(t => t.department_id).FirstOrDefault();
                return value;
            }).ToList();
            var delegate_flow_company = delegates.Select(r =>
            {
                var value = new ValueTuple<string, string>();
                value.Item1 = r.flowcode.Trim(',');
                value.Item2 = users.Where(t => t.user_id == r.user_id).Select(t => t.company_id).FirstOrDefault();
                return value;
            }).ToList();



            var query = db.Queryable<wf_instanceEntity, wf_taskEntity, wf_task_authorizeEntity>((fi, t, ta) => t.task_id == ta.task_id && t.instance_id == fi.instance_id)
                .Where((fi, t, ta) => fi.is_finished == 0 && t.is_done == 0 && (
                delegate_flow_user.Contains(fi.flowcode, ta.user_id)

                || delegate_flow_group.Contains(fi.flowcode, ta.group_id)   // 小组
                || (string.IsNullOrEmpty(ta.post_id) && string.IsNullOrEmpty(ta.role_id) && delegate_flow_dept.Contains(fi.flowcode, ta.department_id))    // 按部门审批
                || (string.IsNullOrEmpty(ta.post_id) && string.IsNullOrEmpty(ta.role_id) && delegate_flow_company.Contains(fi.flowcode, ta.company_id))    // 按公司审批

                || (delegate_flow_post.Contains(fi.flowcode, ta.post_id) && string.IsNullOrEmpty(ta.department_id) && string.IsNullOrEmpty(ta.company_id) && string.IsNullOrEmpty(ta.manage_dept_id))   // 岗位   不仅判断 下面的人员 还需要判断 同公司 同部门 分管
                || (delegate_flow_post.Contains(fi.flowcode, ta.post_id) && !string.IsNullOrEmpty(ta.department_id) && delegate_flow_dept.Contains(fi.flowcode, ta.department_id))
                || (delegate_flow_post.Contains(fi.flowcode, ta.post_id) && !string.IsNullOrEmpty(ta.company_id) && delegate_flow_company.Contains(fi.flowcode, ta.company_id))
                || (delegate_flow_post.Contains(fi.flowcode, ta.post_id) && !string.IsNullOrEmpty(ta.manage_dept_id) && postdepts.Contains(ta.post_id, ta.manage_dept_id))

                || (delegate_flow_role.Contains(fi.flowcode, ta.role_id) && string.IsNullOrEmpty(ta.department_id) && string.IsNullOrEmpty(ta.company_id) && string.IsNullOrEmpty(ta.manage_dept_id))   //   角色 不仅判断 下面的人员 还需要判断 同公司 同部门 分管
                || (delegate_flow_role.Contains(fi.flowcode, ta.role_id) && !string.IsNullOrEmpty(ta.department_id) && delegate_flow_dept.Contains(fi.flowcode, ta.department_id))
                || (delegate_flow_role.Contains(fi.flowcode, ta.role_id) && !string.IsNullOrEmpty(ta.company_id) && delegate_flow_company.Contains(fi.flowcode, ta.company_id))
                || (delegate_flow_role.Contains(fi.flowcode, ta.role_id) && !string.IsNullOrEmpty(ta.manage_dept_id) && roledepts.Contains(ta.role_id, ta.manage_dept_id))
                )
            );
            return query;
        }

        public int GetDelegateCount(UserModel loginInfo)
        {
            var query = GetDelegateQueryable(loginInfo);
            var count = query.Count();
            return count;
        }


        /// <summary>
        /// 流程监控 列表
        /// </summary>
        public JqgridPageResponse<wf_instanceEntity> MonitorPage(JqgridPageRequest jqPage, int is_finished)
        {
            Expression<Func<wf_instanceEntity, bool>> exp = r => r.is_finished == is_finished;
            if (jqPage.sidx.IsEmpty())
            {
                jqPage.sidx = "createtime";
                jqPage.sord = "DESC";
            }
            var page = BaseRepository().GetPage(jqPage, exp);
            return page;
        }

        public void Create(wf_instanceEntity instance, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql, UserModel loginInfo)
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
                            var tuple = BuildSql(sql, instance.process_id, loginInfo);
                            trans.db.Ado.ExecuteCommand(tuple.Item1, tuple.Item2);
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

        public void Approve(wf_instanceEntity instance, wf_taskEntity currentTask, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql, UserModel loginInfo)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                if (instance.is_finished == 1)
                {
                    trans.UpdateOnlyColumns(instance, r => new { r.is_finished, r.finishtime, r.finish_userid, r.finish_username });
                    trans.db.Updateable<wf_taskEntity>().Where(r => r.instance_id == instance.instance_id).SetColumns(r => r.is_done == 1).ExecuteCommand();
                }
                else
                {
                    if (currentTask.is_done == 1)
                    {
                        trans.UpdateOnlyColumns(currentTask, r => new { r.is_done });
                    }
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
                            var tuple = BuildSql(sql, instance.process_id, loginInfo);
                            trans.db.Ado.ExecuteCommand(tuple.Item1, tuple.Item2);
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

        public void AddUser(wf_taskEntity currentTask, List<wf_taskEntity> listTask, wf_taskhistoryEntity history)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.db.Updateable<wf_taskEntity>(currentTask).ExecuteCommand();
                foreach (var task in listTask)
                {
                    trans.Insert(task);
                    if (!task.authrizes.IsEmpty())
                    {
                        trans.Insert(task.authrizes);
                    }
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

        public void HelpMe(List<wf_taskEntity> listTask, wf_taskhistoryEntity history)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                foreach (var task in listTask)
                {
                    trans.Insert(task);
                    if (!task.authrizes.IsEmpty())
                    {
                        trans.Insert(task.authrizes);
                    }
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

        public void Finish(wf_instanceEntity instance, List<wf_taskEntity> listTask, wf_taskhistoryEntity history)
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

        public void Modify(string instanceId, string schemejson, List<wf_taskEntity> oldTasks, wf_taskEntity newTask, wf_taskhistoryEntity history)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                var query = trans.db.Updateable<wf_instanceEntity>().Where(r => r.instance_id == instanceId).SetColumns(r => r.is_finished == 0);
                if (!schemejson.IsEmpty())
                {
                    query = query.SetColumns(r => r.schemejson == schemejson);
                }
                query.ExecuteCommand();
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

        public void Complete(wf_instanceEntity instance, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql, UserModel loginInfo)
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
                            var tuple = BuildSql(sql, instance.process_id, loginInfo);
                            trans.db.Ado.ExecuteCommand(tuple.Item1, tuple.Item2);
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

        public void Sleep(string instanceId, wf_taskhistoryEntity history)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.db.Updateable<wf_instanceEntity>().Where(r => r.instance_id == instanceId).SetColumns(r => r.is_finished == 2).ExecuteCommand();
                trans.db.Deleteable<wf_taskEntity>().Where(r => r.is_done == 0 && r.instance_id == instanceId).ExecuteCommand();
                trans.db.Insertable(history).ExecuteCommand();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void DeleteTask(wf_taskEntity task)
        {
            var trans = BaseRepository().BeginTrans();
            try
            {
                trans.db.Deleteable<wf_taskEntity>().Where(r => r.task_id == task.task_id).ExecuteCommand();
                trans.db.Deleteable<wf_task_authorizeEntity>().Where(r => r.task_id == task.task_id).ExecuteCommand();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 统一组装sql
        /// </summary>
        private Tuple<string, Dictionary<string, object>> BuildSql(string sql, string processId, UserModel loginInfo)
        {
            var sqlparams = new Dictionary<string, object>();
            var sql1 = sql;
            if (sql1.Contains("@processId"))
            {
                sql1 = sql1.Replace("@processId", $"{BaseConnection.ParaPre}processId");
                sqlparams.Add("processId", processId);
            }
            if (sql1.Contains("@userId"))
            {
                sql1 = sql1.Replace("@proceuserIdssId", $"{BaseConnection.ParaPre}userId");
                sqlparams.Add("userId", loginInfo.user_id);
            }
            if (sql1.Contains("@userCode"))
            {
                sql1 = sql1.Replace("@userCode", $"{BaseConnection.ParaPre}userCode");
                sqlparams.Add("userCode", loginInfo.loginname);
            }
            if (sql1.Contains("@userName"))
            {
                sql1 = sql1.Replace("@userName", $"{BaseConnection.ParaPre}userName");
                sqlparams.Add("userName", loginInfo.realname);
            }
            return new Tuple<string, Dictionary<string, object>>(sql1, sqlparams);
        }
    }
}
