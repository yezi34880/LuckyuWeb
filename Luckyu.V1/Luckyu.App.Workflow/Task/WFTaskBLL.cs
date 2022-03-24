using Luckyu.App.Organization;
using Luckyu.App.System;
using Luckyu.Log;
using Luckyu.Utility;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFTaskBLL
    {
        #region Var
        private wf_flowService flowService = new wf_flowService();
        private wf_flow_authorizeService authService = new wf_flow_authorizeService();
        private wf_flow_shemeService schemeService = new wf_flow_shemeService();
        private wf_instanceService instanceService = new wf_instanceService();
        private wf_taskhistoryService taskhistoryService = new wf_taskhistoryService();
        private wf_taskService taskService = new wf_taskService();
        private wf_task_authorizeService taskauthService = new wf_task_authorizeService();
        private WFDelegateBLL delegateBLL = new WFDelegateBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL deptBLL = new DepartmentBLL();
        private CompanyBLL companyBLL = new CompanyBLL();
        private RoleBLL roleBLL = new RoleBLL();
        private PostBLL postBLL = new PostBLL();
        private GroupBLL groupBLL = new GroupBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        private DepartmentManageBLL manageBLL = new DepartmentManageBLL();
        private MessageBLL messageBLL = new MessageBLL();
        private AnnexFileBLL annexBLL = new AnnexFileBLL();
        #endregion

        #region Get
        public JqgridPageResponse<WFTaskModel> Page(JqgridPageRequest jqPage, int tasktype, UserModel loginInfo)
        {
            var page = new JqgridPageResponse<WFTaskModel>();
            if (tasktype == 1)  // 待办
            {
                page = taskService.Page(jqPage, loginInfo);
            }
            else if (tasktype == 2) // 已办
            {
                page = taskhistoryService.Page(jqPage, loginInfo);
            }
            else if (tasktype == 3)  // 自己发起
            {
                var page1 = instanceService.Page(jqPage, loginInfo);
                page = page1.Adapt<JqgridPageResponse<WFTaskModel>>();
            }
            else if (tasktype == 4)  // 委托
            {
                page = taskService.DelegatePage(jqPage, loginInfo);
            }
            return page;
        }

        public wf_taskEntity GetTaskEntitty(Expression<Func<wf_taskEntity, bool>> condition, string orderby = "")
        {
            var entity = taskService.GetEntity(condition, orderby);
            return entity;
        }
        public wf_taskEntity GetTaskEntitty(Expression<Func<wf_taskEntity, bool>> condition, Expression<Func<wf_taskEntity, object>> orderby, bool isDesc = false)
        {
            var entity = taskService.GetEntity(condition, orderby, isDesc);
            return entity;
        }
        public List<wf_taskEntity> GetTaskList(Expression<Func<wf_taskEntity, bool>> condition)
        {
            var list = taskService.GetList(condition);
            return list;
        }
        public wf_taskhistoryEntity GetHistoryEnttity(Expression<Func<wf_taskhistoryEntity, bool>> condition)
        {
            var entity = taskhistoryService.GetEntity(condition);
            return entity;
        }
        public List<wf_taskhistoryEntity> GetHistoryEnttity(Expression<Func<wf_taskhistoryEntity, bool>> condition, Expression<Func<wf_taskhistoryEntity, object>> orderby)
        {
            var list = taskhistoryService.GetList(condition, orderby);
            return list;
        }

        public List<wf_task_authorizeEntity> GetTaskAuthorizeList(Expression<Func<wf_task_authorizeEntity, bool>> condition)
        {
            var list = taskauthService.GetList(condition);
            return list;
        }
        public wf_instanceEntity GetInstanceEnttity(Expression<Func<wf_instanceEntity, bool>> condition)
        {
            var entity = instanceService.GetEntity(condition);
            return entity;
        }
        public List<wf_instanceEntity> GetInstanceList(Expression<Func<wf_instanceEntity, bool>> condition)
        {
            var list = instanceService.GetList(condition, r => r.createtime);
            return list;
        }

        public List<wf_taskhistoryEntity> GetHistoryTaskList(string processId, string instanceId = "")
        {
            Expression<Func<wf_taskhistoryEntity, bool>> exp = r => true;
            if (!processId.IsEmpty())
            {
                exp = exp.LinqAnd(r => r.process_id == processId);
            }
            if (!instanceId.IsEmpty())
            {
                exp = exp.LinqAnd(r => r.instance_id == instanceId);
            }
            var list = taskhistoryService.GetList(exp, r => r.createtime);
            foreach (var his in list)
            {
                var annex = annexBLL.GetList(r => r.external_id == his.history_id);
                his.annex = Newtonsoft.Json.JsonConvert.SerializeObject(annex.Select(r => new KeyValue
                {
                    Key = r.annex_id,
                    Value = r.filename
                }).ToList());
            }

            Expression<Func<wf_taskEntity, bool>> exp1 = r => r.is_done == 0 && r.process_id == processId;
            if (!instanceId.IsEmpty())
            {
                exp1 = exp1.LinqAnd(r => r.instance_id == instanceId);
            }
            var listTask = taskService.GetList(exp1, r => r.createtime);

            foreach (var item in listTask)
            {
                var item1 = item.Adapt<wf_taskhistoryEntity>();
                var auths = taskauthService.GetList(r => r.task_id == item.task_id);
                var users = GetUserByAuth(auths);
                item1.history_id = "";
                item1.opinion = string.Join(",", users.Select(r => $"{r.realname}-{ r.loginname}"));
                item1.result = 100;
                item1.createtime = null;
                item1.create_username = "";
                list.Add(item1);
            }

            return list;
        }

        public JqgridPageResponse<wf_instanceEntity> MonitorPage(JqgridPageRequest jqPage, int is_finished)
        {
            var page = taskService.MonitorPage(jqPage, is_finished);
            return page;
        }

        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="instanceId">自己发起的,只有instanceId</param>
        /// <param name="taskId">如果taskId不为空, 在表示待审批</param>
        /// <param name="historyId">如果historyId不为空, 则表示已审批</param>
        /// <returns></returns>
        public ResponseResult<Dictionary<string, object>> GetFormData(string instanceId, string taskId, string historyId, UserModel loginInfo)
        {
            var instance = GetInstanceEnttity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var scheme = instance.schemejson.ToObject<WFSchemeModel>();
            // 展示节点 如 自己提交 就是startround 已办历史 就是自己批的那个
            WFSchemeNodeModel showNode = new WFSchemeNodeModel();
            // 当前待批节点
            WFSchemeNodeModel currentNode = new WFSchemeNodeModel();
            // 如果是待办，还要查出下一步
            List<WFSchemeNodeModel> nextNodes = new List<WFSchemeNodeModel>();
            if (!historyId.IsEmpty())
            {
                // 已办历史
                var history = GetHistoryEnttity(r => r.history_id == historyId);
                if (history == null)
                {
                    return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
                }
                showNode = scheme.nodes.Where(r => r.id == history.node_id).FirstOrDefault();

                var task = GetTaskEntitty(r => r.instance_id == instance.instance_id && r.is_done == 0);
                if (task != null)
                {
                    currentNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                }
            }
            else if (!taskId.IsEmpty())
            {
                // 待办
                var task = GetTaskEntitty(r => r.task_id == taskId);
                if (task == null)
                {
                    return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
                }
                showNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                currentNode = showNode;

                // 查出下一步节点（可能多个）
                var lines = scheme.lines.Where(r => r.from == currentNode.id).ToList();
                var to_nodeids = lines.Where(r => r.wftype == 1 || r.wftype == 0).Select(r => r.to).ToList();
                nextNodes = scheme.nodes.Where(r => (r.type == "stepnode" || r.type == "auditornode") && to_nodeids.Contains(r.id)).ToList();
                // 查出下一步节点中 具体的审批人（具体到人）
                if (!nextNodes.IsEmpty())
                {
                    foreach (var nextnode in nextNodes)
                    {
                        var users = GetUserByNode(nextnode, instance);
                        nextnode.authusers = users.Select(r => new WFAuthorizeModel
                        {
                            objectids = r.user_id,
                            objectnames = $"{r.realname}-{r.loginname}"
                        }).ToList();
                    }
                }
            }
            else
            {
                // 自己发起的
                showNode = scheme.nodes.Where(r => r.type == "startround").FirstOrDefault();
                var task = GetTaskEntitty(r => r.instance_id == instance.instance_id && r.is_done == 0);
                if (task != null)
                {
                    currentNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                }

            }
            var historys = GetHistoryTaskList(instance.process_id, instance.instance_id);
            var dic = new Dictionary<string, object>
            {
                {"Instance",instance },
                {"ShowNode",showNode },
                {"CurrentNode",currentNode },
                {"NextNodes",nextNodes },
                { "History",historys}
            };
            return ResponseResult.Success(dic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public ResponseResult<Dictionary<string, object>> GetTaskScheme(string instanceId)
        {
            var instance = GetInstanceEnttity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
            }
            var lastScheme = schemeService.GetEntity(r => r.flow_id == instance.flow_id);
            var scheme = instance.schemejson.ToObject<WFSchemeModel>();
            // 当前待批节点
            WFSchemeNodeModel currentNode = new WFSchemeNodeModel();
            var task = GetTaskEntitty(r => r.instance_id == instance.instance_id && r.is_done == 0);
            if (task != null)
            {
                currentNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
            }
            var dic = new Dictionary<string, object>
            {
                {"Instance",instance },
                {"CurrentNode",currentNode },
                {"LastScheme",lastScheme },
            };
            return ResponseResult.Success(dic);

        }

        /// <summary>
        /// 获取当前流程所有转发查看步骤
        /// </summary>
        public List<wf_taskEntity> GetHelpMePage(JqgridPageRequest jqPage, string instanceId)
        {
            var list = GetTaskList(r => r.instance_id == instanceId && r.is_done == 0 && r.nodetype == "helpme");
            foreach (var task in list)
            {
                var auths = GetTaskAuthorizeList(r => r.task_id == task.task_id);
                foreach (var auth in auths)
                {
                    var user = userBLL.GetEntityByCache(r => r.user_id == auth.user_id);
                    task.create_username = string.Join(",", user.realname);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取当前流程所有会签办理、会签办理用户
        /// </summary>
        public List<WFTaskAuthModel> GetAddUserPage(JqgridPageRequest jqPage, string instanceId)
        {
            var list = taskauthService.GetAddUserPage(jqPage, instanceId);
            foreach (var auth in list)
            {
                var user = userBLL.GetEntityByCache(r => r.user_id == auth.user_id);
                auth.username = string.Join(",", user.realname);
            }
            return list;
        }

        /// <summary>
        /// 该单据是否在流程中
        /// </summary>
        public bool IsInWorkflow(string processId)
        {
            var instance = GetInstanceEnttity(r => r.process_id == processId && r.is_finished == 0);
            return instance != null;
        }
        #endregion

        #region Set

        #region Create
        /// <summary>
        /// 创建流程
        /// </summary>
        /// <param name="flowCode">流程编码</param>
        /// <param name="processId">单据主键</param>
        /// <param name="processName">流程名</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult Create(string flowCode, string processId, string processName, string processContent, UserModel loginInfo)
        {
            if (flowCode.IsEmpty())
            {
                return ResponseResult.Fail("流程编码不存在，请检查");
            }
            if (processId.IsEmpty())
            {
                return ResponseResult.Fail("单据主键不存在，请检查");
            }
            var flow = flowService.GetEntity(r => r.is_delete == 0 && r.is_enable == 1 && r.flowcode == flowCode);
            if (flow == null)
            {
                return ResponseResult.Fail("该流程不存在，请联系管理员");
            }
            if (processName.IsEmpty())
            {
                processName = flow.flowname;
            }
            if (!processName.Contains(flow.flowname))
            {
                processName = $"【{flow.flowname}】" + processName;
            }

            var instanceAlready = instanceService.GetEntity(r => r.is_finished == 0 && r.flow_id == flow.flow_id && r.process_id == processId);
            if (instanceAlready != null)
            {
                return ResponseResult.Fail("该流程已有运行中的实例，请勿重复提交");
            }

            #region 范围 Range
            var isContain = false;
            if (flow.flowrange == 0)
            {
                isContain = true;
            }
            else
            {
                var auths = authService.GetList(r => r.flow_id == flow.flow_id);
                foreach (var auth in auths)
                {
                    switch (auth.objecttype)  // 1 用户  2 部门 3 公司 4 岗位 5  角色 6 小组
                    {
                        case 1:
                            if (auth.object_id == loginInfo.user_id)
                            {
                                isContain = true;
                            }
                            break;
                        case 2:
                            if (auth.object_id == loginInfo.department_id)
                            {
                                isContain = true;
                            }
                            break;
                        case 3:
                            if (auth.object_id == loginInfo.company_id)
                            {
                                isContain = true;
                            }
                            break;
                        case 4:
                            if (loginInfo.post_ids.Contains(auth.object_id))
                            {
                                isContain = true;
                            }
                            break;
                        case 5:
                            if (loginInfo.role_ids.Contains(auth.object_id))
                            {
                                isContain = true;
                            }
                            break;
                        case 6:
                            if (loginInfo.group_ids.Contains(auth.object_id))
                            {
                                isContain = true;
                            }
                            break;
                    }
                    if (isContain)
                    {
                        break;
                    }
                }
            }
            if (!isContain)
            {
                return ResponseResult.Fail("该流程不对当前用户开放，请联系管理员");
            }

            #endregion

            var scheme = schemeService.GetEntity(r => r.flow_id == flow.flow_id);

            var instance = new wf_instanceEntity();
            instance = flow.Adapt<wf_instanceEntity>();

            instance.process_id = processId;
            instance.processname = processName;
            instance.processcontent = processContent;

            // 单据提交人 后期 同公司 同部门 根据这个值计算 后期考虑作为入参 代为提交别人单据的情况 先不管
            instance.submit_userid = loginInfo.user_id;
            instance.submit_username = $"{loginInfo.realname}-{loginInfo.loginname}";

            instance.Create(loginInfo);
            instance.schemejson = scheme.schemejson;

            var nodeModel = instance.schemejson.ToObject<WFSchemeModel>();
            if (nodeModel.nodes.IsEmpty())
            {
                return ResponseResult.Fail("该流程的流程图没有节点，请联系管理员");
            }
            if (nodeModel.lines.IsEmpty())
            {
                return ResponseResult.Fail("该流程的流程图没有没有连线，请联系管理员");
            }
            var listTask = new List<wf_taskEntity>();
            var listHistory = new List<wf_taskhistoryEntity>();
            var listSql = new List<string>();

            var nodeStart = nodeModel.nodes.Where(r => r.type == "startround").FirstOrDefault();
            if (nodeStart == null)
            {
                return ResponseResult.Fail("该流程的流程图没有开始节点，请联系管理员");
            }
            // history插入开始
            var historyStart = new wf_taskhistoryEntity();
            historyStart = instance.Adapt<wf_taskhistoryEntity>();
            historyStart.result = 0;
            historyStart.node_id = nodeStart.id;
            historyStart.nodename = nodeStart.name;
            historyStart.nodetype = nodeStart.type;
            historyStart.previous_id = "0";
            historyStart.previousname = "";
            historyStart.appremark = "【流程开始】";
            historyStart.Create(loginInfo);

            historyStart.tasktime = DateTime.Now;
            historyStart.apptime = DateTime.Now;

            listHistory.Add(historyStart);
            if (!nodeStart.sqlsuccess.IsEmpty())
            {
                listSql.Add(nodeStart.sqlsuccess);
            }
            var res = ProcessNodeInject(nodeStart, instance.instance_id, instance.process_id, 1, "", listTask, listHistory, listSql);
            if (res.code == (int)ResponseCode.Fail)
            {
                return ResponseResult.Fail(res.info, res.data);
            }
            var lines = nodeModel.lines.Where(r => r.from == nodeStart.id && (r.wftype == 1 || r.wftype == 0)).ToList();
            if (lines.IsEmpty())
            {
                return ResponseResult.Fail("该流程的流程图开始节点没有连线，请联系管理员");
            }
            var turple = FindNextNode(lines, nodeModel.lines, nodeModel.nodes, instance, nodeStart, loginInfo);
            if (!turple.Item1.IsEmpty())
            {
                listTask = turple.Item1;
            }
            if (!turple.Item2.IsEmpty())
            {
                listHistory.AddRange(turple.Item2);
            }
            if (!turple.Item3.IsEmpty())
            {
                listSql.AddRange(turple.Item3);
            }
            taskService.Create(instance, listTask, listHistory, listSql);

            var data = new Tuple<wf_instanceEntity, List<wf_taskEntity>>(instance, listTask);
            return ResponseResult.Success((object)data);
        }
        public ResponseResult Create(FlowEnum flow, string processId, string processName, string processContent, UserModel loginInfo)
        {
            var flowCode = flow.ToString();
            var res = Create(flowCode, processId, processName, processContent, loginInfo);
            return res;
        }

        public async Task<ResponseResult> Create(string flowCode, string processId, string processName, string processContent, UserModel loginInfo, IHubContext<MessageHub> hubContext)
        {
            var res = Create(flowCode, processId, processName, processContent, loginInfo);
            if (res.code == (int)ResponseCode.Success)
            {
                var data = res.data as Tuple<wf_instanceEntity, List<wf_taskEntity>>;
                foreach (var task in data.Item2)
                {
                    var instance = data.Item1;
                    var msg = $"{instance.submit_username} 提交了【{instance.flowname}】{task.processname}审批流程";
                    var authrizes = task.authrizes;
                    var nextUsers = GetUserByAuth(authrizes);
                    foreach (var user in nextUsers)
                    {
                        var sendRes = new ResponseResult();
                        sendRes.info = res.info;
                        sendRes.data = "审批通知";
                        await SignalRHelper.SendMessageToUser(hubContext, user.loginname, sendRes);
                    }
                }
            }
            return res;
        }

        public async Task<ResponseResult> Create(FlowEnum flow, string processId, string processName, string processContent, UserModel loginInfo, IHubContext<MessageHub> hubContext)
        {
            var res = Create(flow, processId, processName, processContent, loginInfo);
            if (res.code == (int)ResponseCode.Success)
            {
                var data = res.data as Tuple<wf_instanceEntity, List<wf_taskEntity>>;
                foreach (var task in data.Item2)
                {
                    var instance = data.Item1;
                    var msg = $"{instance.submit_username} 提交了【{instance.flowname}】{task.processname}审批流程";
                    var authrizes = task.authrizes;
                    var nextUsers = GetUserByAuth(authrizes);
                    foreach (var user in nextUsers)
                    {
                        var sendRes = new ResponseResult();
                        sendRes.info = res.info;
                        sendRes.data = "审批通知";
                        await SignalRHelper.SendMessageToUser(hubContext, user.loginname, sendRes);
                    }
                }
            }
            return res;
        }

        #endregion

        #region Approval
        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="taskId">当前任务</param>
        /// <param name="result">1同意 2拒绝</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult Approve(string taskId, int result, string opinion, int returnType, Dictionary<string, List<KeyValue>> authors, UserModel loginInfo)
        {
            var task = taskService.GetEntity(r => r.task_id == taskId);
            if (task == null)
            {
                return ResponseResult.Fail("该任务不存在");
            }
            if (task.is_done == 1)
            {
                return ResponseResult.Fail("该任务已被其他人审批，请关闭当前页");
            }
            var instance = instanceService.GetEntity(r => r.instance_id == task.instance_id);
            if (instance == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }
            var auths = taskauthService.GetList(r => r.task_id == task.task_id);
            if (auths.IsEmpty())
            {
                return ResponseResult.Fail("没有审批人");
            }
            var users = GetUserByAuth(auths);
            if (users.IsEmpty() || !users.Exists(r => r.user_id == loginInfo.user_id))
            {
                return ResponseResult.Fail("当前用户不是审批人");
            }

            var scheme = instance.schemejson.ToObject<WFSchemeModel>();

            var listTask = new List<wf_taskEntity>();
            var listHistory = new List<wf_taskhistoryEntity>();
            var listSql = new List<string>();

            var nodeCurrent = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
            var historyCurrent = new wf_taskhistoryEntity();
            historyCurrent = task.Adapt<wf_taskhistoryEntity>();
            historyCurrent.result = result;
            historyCurrent.nodetype = nodeCurrent.type;

            if (result == 2 && task.nodetype != "helpme")
            {
                if (returnType == 1)
                {
                    historyCurrent.appremark = "退回上一步";
                }
                else
                {
                    historyCurrent.appremark = "退回";
                }
            }

            var currentAuth = auths.Where(r => r.user_id == loginInfo.user_id).FirstOrDefault();
            if (currentAuth != null && currentAuth.is_add == 1)
            {
                historyCurrent.appremark = "【会签办理】 ";
            }
            else if (currentAuth != null && currentAuth.is_add == 3)
            {
                historyCurrent.appremark = "【转发查看】 ";
            }

            if (nodeCurrent.type == "confluencenode")
            {
                historyCurrent.appremark = "【会签】 ";
            }
            else if (nodeCurrent.type == "auditornode")
            {
                historyCurrent.appremark = "【传阅】 ";
            }
            else
            {
                historyCurrent.appremark = "【审批】";
            }
            historyCurrent.opinion += opinion;
            historyCurrent.app_userid = loginInfo.user_id;
            historyCurrent.app_username = $"{loginInfo.realname}-{loginInfo.loginname}";

            historyCurrent.tasktime = task.createtime.Value;
            historyCurrent.apptime = DateTime.Now;
            historyCurrent.Create(loginInfo);
            listHistory.Add(historyCurrent);

            task.is_done = 1;
            task.result = result;
            task.app_userid = loginInfo.user_id;
            task.app_username = $"{loginInfo.realname}-{loginInfo.loginname}";
            task.opinion = opinion;
            if (task.nodetype == "helpme") // 转发查看 不往下进行
            {
                taskService.Approve(instance, task, listTask, listHistory, listSql);
                var data1 = new Tuple<wf_instanceEntity, List<wf_taskEntity>, List<wf_taskhistoryEntity>>(instance, listTask, listHistory);
                return ResponseResult.Success((object)data1);
            }
            //  会签办理 任务，查询是否为多人会签办理，如果是多人等同于会签，必须所有通过才能往下走
            if (task.nodetype == "adduser")
            {
                // 会签办理 只有一种情况需要中断，即，多人会签办理，且只有部分人批完，这时候节点停住不动
                // 如果拒绝，则相当于原节点拒绝，根据选择回到上一步或是起始
                // 如果所有人都同意，在继续往下走
                if (result == 1)
                {
                    var countTaskAdd = taskService.GetCount(r => r.instance_id == instance.instance_id && r.nodetype == "adduser" && r.node_id == task.node_id);
                    var countHistoryAdd = taskhistoryService.GetCount(r => r.instance_id == instance.instance_id && r.nodetype == "adduser" && r.node_id == task.node_id);
                    if (countHistoryAdd < countTaskAdd)
                    {
                        var data1 = new Tuple<wf_instanceEntity, List<wf_taskEntity>, List<wf_taskhistoryEntity>>(instance, listTask, listHistory);
                        return ResponseResult.Success((object)data1);
                    }
                }
            }

            var res = ProcessNodeInject(nodeCurrent, instance.instance_id, instance.process_id, result, opinion, listTask, listHistory, listSql);
            if (res.code == (int)ResponseCode.Fail)
            {
                return ResponseResult.Fail(res.info, res.data);
            }

            var lines = scheme.lines.Where(r => r.from == nodeCurrent.id).ToList();
            if (result == 2) // 拒绝
            {
                // 这里有问题，退回上一步其实有两种可能，如果上一步是其他审批，不用执行退回SQL与程序，否则直接就是起草了，而且这里后期可能需要针对这里把 退回SQL 再次拆分为两个，暂时先不弄
                // 还有一个问题，如果上一步为开始，就要执行SQL，相当于退回起草状态了

                if (returnType == 1) // 退回至上一步 
                {
                    var preTasks = GetTaskList(r => r.instance_id == instance.instance_id && r.node_id == task.previous_id);
                    if (!preTasks.IsEmpty())
                    {
                        if (preTasks.Exists(r => r.nodetype == "startround"))
                        {
                            listSql.Add(nodeCurrent.sqlfail);
                            var historyEnd = new wf_taskhistoryEntity();
                            historyEnd = instance.Adapt<wf_taskhistoryEntity>();
                            historyEnd.result = 2;
                            historyEnd.node_id = "";
                            historyEnd.nodename = "";
                            historyEnd.nodetype = "startround";
                            historyEnd.previous_id = nodeCurrent.id;
                            historyEnd.previousname = nodeCurrent.name;
                            historyEnd.appremark = "【流程结束】退回上一步，上一步为开始节点";
                            historyEnd.tasktime = DateTime.Now;
                            historyEnd.apptime = DateTime.Now;
                            historyEnd.Create(loginInfo);
                            listHistory.Add(historyEnd);

                            instance.is_finished = 1;
                        }
                        else
                        {
                            foreach (var preTask in preTasks)
                            {
                                var taskAuths = GetTaskAuthorizeList(r => r.task_id == preTask.task_id);
                                preTask.Create(loginInfo);
                                foreach (var au in taskAuths)
                                {
                                    au.Create(loginInfo);
                                    au.task_id = preTask.task_id;
                                }
                                listTask.Add(preTask);
                            }
                        }
                        taskService.Approve(instance, task, listTask, listHistory, listSql);
                        var data1 = new Tuple<wf_instanceEntity, List<wf_taskEntity>, List<wf_taskhistoryEntity>>(instance, listTask, listHistory);
                        return ResponseResult.Success((object)data1);
                    }
                }
                else  // 走到 箭头为【否】连接的下一步，如果没有下一步则回到起始，这里也有问题，其实应该调用 GetNextNode 方法，而不是只是单纯找拒绝的下一步，因为下一步可能会是执行或者会签等按钮，需要递归
                {
                    lines = lines.Where(r => r.wftype == 2 || r.wftype == 0).ToList();
                    if (!nodeCurrent.sqlfail.IsEmpty())
                    {
                        listSql.Add(nodeCurrent.sqlfail);
                    }
                    if (lines.IsEmpty())  // 如果没有为【否】连线的节点，则回到起始
                    {
                        var historyEnd = new wf_taskhistoryEntity();
                        historyEnd = instance.Adapt<wf_taskhistoryEntity>();
                        historyEnd.result = 2;
                        historyEnd.node_id = "";
                        historyEnd.nodename = "";
                        historyEnd.nodetype = "endround";
                        historyEnd.previous_id = nodeCurrent.id;
                        historyEnd.previousname = nodeCurrent.name;
                        historyEnd.appremark = "【流程结束】";
                        historyEnd.tasktime = DateTime.Now;
                        historyEnd.apptime = DateTime.Now;
                        historyEnd.Create(loginInfo);
                        listHistory.Add(historyEnd);

                        instance.is_finished = 1;

                        taskService.Approve(instance, task, listTask, listHistory, listSql);
                        var data1 = new Tuple<wf_instanceEntity, List<wf_taskEntity>, List<wf_taskhistoryEntity>>(instance, listTask, listHistory);
                        return ResponseResult.Success((object)data1);
                    }
                }
            }
            else  // 同意
            {
                lines = lines.Where(r => r.wftype == 1 || r.wftype == 0).ToList();
                if (!nodeCurrent.sqlsuccess.IsEmpty())
                {
                    listSql.Add(nodeCurrent.sqlsuccess);
                }
            }
            if (!lines.IsEmpty())
            {
                if (!authors.IsEmpty()) // 如果有选择节点，则从下一步所有节点中删除未选择的节点
                {
                    lines.RemoveAll(r => !authors.ContainsKey(r.to));
                }

                var turple = FindNextNode(lines, scheme.lines, scheme.nodes, instance, nodeCurrent, loginInfo);
                if (!turple.Item1.IsEmpty()) // 下步任务
                {
                    listTask = turple.Item1;

                    if (!authors.IsEmpty()) // 如果有选择节点或用户，则从下一步所有节点中删除未选择的用户，并设置多选用户会签
                    {
                        listTask.RemoveAll(r => !authors.ContainsKey(r.node_id));
                        foreach (var ntask in listTask)
                        {
                            ntask.authrizes = new List<wf_task_authorizeEntity>();
                            var chkAuths = authors[ntask.node_id];
                            foreach (var chkAuth in chkAuths)
                            {
                                var au = new wf_task_authorizeEntity();
                                au.task_id = ntask.task_id;
                                au.is_add = 1;
                                au.user_id = chkAuth.Key;
                                au.Create(loginInfo);
                                ntask.authrizes.Add(au);
                            }
                        }
                    }
                }
                if (!turple.Item2.IsEmpty())  // 历史记录
                {
                    listHistory.AddRange(turple.Item2);
                }
                if (!turple.Item3.IsEmpty())   // 执行sql
                {
                    listSql.AddRange(turple.Item3);
                }
            }
            //else  // 不用判断，有可能会叉两条线，其中一条没有收尾了
            //{
            //    return ResponseResult.Fail("该流程节点没有下一步连线，请联系管理员");
            //}
            taskService.Approve(instance, task, listTask, listHistory, listSql);
            var data = new Tuple<wf_instanceEntity, List<wf_taskEntity>, List<wf_taskhistoryEntity>>(instance, listTask, listHistory);
            return ResponseResult.Success((object)data);
        }

        public async Task<ResponseResult> Approve(string taskId, int result, string opinion, int returnType, Dictionary<string, List<KeyValue>> authors, UserModel loginInfo, IHubContext<MessageHub> hubContext)
        {
            var res = Approve(taskId, result, opinion, returnType, authors, loginInfo);
            if (res.code == (int)ResponseCode.Success)
            {
                var data = res.data as Tuple<wf_instanceEntity, List<wf_taskEntity>, List<wf_taskhistoryEntity>>;
                if (data != null)
                {
                    foreach (var task in data.Item2)
                    {
                        var instance = data.Item1;
                        var msg = $"{instance.submit_username} 提交了【{instance.flowname}】{task.processname}审批流程";
                        var authrizes = task.authrizes;
                        var nextUsers = GetUserByAuth(authrizes);
                        foreach (var user in nextUsers)
                        {
                            var sendRes = new ResponseResult();
                            sendRes.info = res.info;
                            sendRes.data = "审批通知";
                            await SignalRHelper.SendMessageToUser(hubContext, user.loginname, sendRes);
                        }
                    }
                }
            }
            return res;
        }

        #endregion

        /// <summary>
        /// 会签办理（注意 ：如果会签办理选择多人，必须多人全部同意才会继续）
        /// </summary>
        /// <param name="taskId">当前流程</param>
        /// <param name="userId">会签办理人员</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult AddUser(string taskId, List<string> userIds, string remark, UserModel loginInfo)
        {
            var task = taskService.GetEntity(r => r.task_id == taskId);
            if (task == null)
            {
                return ResponseResult.Fail("该任务不存在");
            }
            var instance = instanceService.GetEntity(r => r.instance_id == task.instance_id);
            if (task == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }

            var listTask = new List<wf_taskEntity>();
            var users = new List<sys_userEntity>();
            foreach (var userId in userIds)
            {
                var taskHelp = task.Adapt<wf_taskEntity>();
                taskHelp.Create(loginInfo);
                taskHelp.task_id = SnowflakeHelper.NewCode();
                taskHelp.previous_id = task.previous_id;
                taskHelp.previousname = task.previousname;
                taskHelp.node_id = task.node_id;
                taskHelp.nodetype = "adduser";
                taskHelp.nodename = task.nodename + "【会签办理】";
                var auths = new List<wf_task_authorizeEntity>();

                var auth = new wf_task_authorizeEntity();
                auth.task_id = taskHelp.task_id;
                auth.user_id = userId;
                auth.is_add = 1;  // 会签办理
                auth.Create(loginInfo);
                auths.Add(auth);

                var user = userBLL.GetEntityByCache(r => r.user_id == userId);
                users.Add(user);

                taskHelp.authrizes = auths;
                listTask.Add(taskHelp);
            }

            var history = new wf_taskhistoryEntity();
            history = task.Adapt<wf_taskhistoryEntity>();
            var struser = string.Join(",", users.Select(r => $"{r.realname}-{r.loginname}"));
            history.appremark = $"{loginInfo.realname}-{loginInfo.loginname}  申请 【会签办理】，用户： {struser} ";
            if (!remark.IsEmpty())
            {
                history.opinion = remark;
            }
            history.result = 3;
            history.Create(loginInfo);

            task.is_done = 1;
            taskService.AddUser(task, listTask, history);
            var ohistory_id = (object)history.history_id;
            return ResponseResult.Success(ohistory_id);
        }

        /// <summary>
        /// 转发查看（邀请别人协助，之后再回到自己）
        /// </summary>
        /// <param name="taskId">当前流程</param>
        /// <param name="userId">会签办理人员</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult HelpMe(string taskId, List<string> userIds, string remark, UserModel loginInfo)
        {
            var task = taskService.GetEntity(r => r.task_id == taskId);
            if (task == null)
            {
                return ResponseResult.Fail("该任务不存在");
            }
            var instance = instanceService.GetEntity(r => r.instance_id == task.instance_id);
            if (task == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }

            var listTask = new List<wf_taskEntity>();
            var users = new List<sys_userEntity>();
            foreach (var userId in userIds)
            {
                var taskHelp = task.Adapt<wf_taskEntity>();
                taskHelp.Create(loginInfo);
                taskHelp.task_id = SnowflakeHelper.NewCode();
                taskHelp.previous_id = "";
                taskHelp.previousname = "";
                taskHelp.node_id = task.node_id;
                taskHelp.nodetype = "helpme";
                taskHelp.nodename = task.nodename + "【转发查看】";
                var auths = new List<wf_task_authorizeEntity>();

                var auth = new wf_task_authorizeEntity();
                auth.task_id = taskHelp.task_id;
                auth.user_id = userId;
                auth.is_add = 3;  // 转发查看
                auth.Create(loginInfo);
                auths.Add(auth);

                var user = userBLL.GetEntityByCache(r => r.user_id == userId);
                users.Add(user);

                taskHelp.authrizes = auths;
                listTask.Add(taskHelp);
            }

            var history = new wf_taskhistoryEntity();
            history = task.Adapt<wf_taskhistoryEntity>();
            var struser = string.Join(",", users.Select(r => $"{r.realname}-{r.loginname}"));
            history.appremark = $"{loginInfo.realname}-{loginInfo.loginname}  申请【转发查看】，用户：  {struser} ";
            if (!remark.IsEmpty())
            {
                history.opinion = remark;
            }
            history.result = 6;
            history.Create(loginInfo);

            taskService.HelpMe(listTask, history);
            var ohistory_id = (object)history.history_id;
            return ResponseResult.Success(ohistory_id);
        }

        /// <summary>
        /// 强制结束流程
        /// </summary>
        public ResponseResult Finish(string instanceId, UserModel loginInfo)
        {
            //if (loginInfo.modules.IsEmpty() || !loginInfo.modules.Exists(r => r.moduleurl.Contains("/WorkflowModule/Monitor/Index")))
            //{
            //    return ResponseResult.Fail("当前账户没有终止流程的权限");
            //}
            var instance = instanceService.GetEntity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }
            if (instance.is_finished == 1)
            {
                return ResponseResult.Fail("该流程已结束");
            }
            var tasks = taskService.GetList(r => r.instance_id == instance.instance_id && r.is_done == 0);
            if (tasks.IsEmpty())
            {
                return ResponseResult.Fail("该流程没有活动中的任务");
            }

            var history = new wf_taskhistoryEntity();
            history.instance_id = instance.instance_id;
            history.flow_id = instance.flow_id;
            history.process_id = instance.process_id;
            history.result = 5;
            history.appremark = $"【流程监控】{loginInfo.realname} 强制结束流程";
            history.Create(loginInfo);

            taskService.Finish(instance, tasks, history);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 挂起（流程实例还在，但是删除所有待办，以后激活时在调整）
        /// </summary>
        public ResponseResult Sleep(string instanceId, UserModel loginInfo)
        {
            var instance = instanceService.GetEntity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }
            if (instance.is_finished == 1)
            {
                return ResponseResult.Fail("该流程已结束");
            }

            var history = new wf_taskhistoryEntity();
            history.instance_id = instanceId;
            history.flow_id = instance.flow_id;
            history.process_id = instance.process_id;
            history.result = 5;
            history.appremark = $"【流程监控】{loginInfo.realname} 挂起流程";
            history.Create(loginInfo);

            taskService.Sleep(instanceId, history);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 调整流程, 设置运行中流程到任意节点
        /// </summary>
        /// <returns></returns>
        public ResponseResult Modify(string instanceId, string schemeId, string nodeId, List<string> userIds, UserModel loginInfo)
        {
            var instance = instanceService.GetEntity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }
            //if (instance.is_finished == 1)
            //{
            //    return ResponseResult.Fail("该流程已结束, 不能调整");
            //}

            var oldTasks = taskService.GetList(r => r.instance_id == instanceId && r.is_done == 0);
            if (oldTasks.IsEmpty())
            {
                return ResponseResult.Fail("当前任务不存在");
            }
            if (!schemeId.IsEmpty())
            {
                var scheme = schemeService.GetEntity(r => r.scheme_id == schemeId);
                if (scheme != null)
                {
                    instance.schemejson = scheme.schemejson;
                }
            }
            var nodeModel = instance.schemejson.ToObject<WFSchemeModel>();
            var nodeNext = nodeModel.nodes.Where(r => r.id == nodeId).FirstOrDefault();
            if (nodeNext == null)
            {
                return ResponseResult.Fail("调整节点不存在");
            }
            var newTask = new wf_taskEntity();
            newTask = instance.Adapt<wf_taskEntity>();
            newTask.node_id = nodeNext.id;
            newTask.nodename = nodeNext.name;
            newTask.nodetype = nodeNext.type;
            newTask.previous_id = oldTasks[0].node_id;
            newTask.previousname = oldTasks[0].nodename;
            newTask.Create(loginInfo);

            if (userIds.IsEmpty())
            {
                var turple = GetNextAuth(nodeNext, instance, newTask.task_id, loginInfo);
                newTask.authrizes = turple.Item1;
            }
            else
            {
                foreach (var userid in userIds)
                {
                    var au = new wf_task_authorizeEntity();
                    au.task_id = newTask.task_id;
                    au.is_add = 1;
                    au.user_id = userid;
                    au.Create(loginInfo);
                    newTask.authrizes.Add(au);
                }
            }

            var history = new wf_taskhistoryEntity();
            history = oldTasks[0].Adapt<wf_taskhistoryEntity>();
            history.apptime = DateTime.Now;
            history.tasktime = DateTime.Now;
            history.result = 5;
            history.appremark = !schemeId.IsEmpty() ? $"{loginInfo.realname} 调整该流程至最新版本 调整后为【{nodeNext.name}】" : $"{loginInfo.realname} 调整 当前待办任务为【{ oldTasks[0].nodename}】 调整后为【{nodeNext.name}】";
            history.Create(loginInfo);

            taskService.Modify(instance.instance_id, (schemeId.IsEmpty() ? "" : instance.schemejson), oldTasks, newTask, history);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public ResponseResult DeleteTask(string taskId)
        {
            var task = taskService.GetEntity(r => r.task_id == taskId);
            if (task == null)
            {
                return ResponseResult.Fail("该任务不存在");
            }
            if (task.is_done == 1)
            {
                return ResponseResult.Fail("该任务已被其他人审批，不能删除");
            }
            taskService.DeleteTask(task);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 删除任务中的用户
        /// </summary>
        public ResponseResult DeleteTaskAuth(string authId)
        {
            var auth = taskauthService.GetEntity(r => r.auth_id == authId);
            if (auth == null)
            {
                return ResponseResult.Fail("该用户审批不存在");
            }
            var task = taskService.GetEntity(r => r.task_id == auth.task_id);
            if (task == null)
            {
                return ResponseResult.Fail("该任务不存在");
            }
            taskauthService.DeleteTaskAuth(authId);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 删除审批历史
        /// </summary>
        public ResponseResult DeleteHistory(string historyId)
        {
            var his = taskhistoryService.GetEntity(r => r.history_id == historyId);
            if (his == null)
            {
                return ResponseResult.Fail("该审批记录不存在");
            }
            taskhistoryService.DeleteHistory(historyId);
            var annexs = annexBLL.GetList(r => r.external_id == historyId);
            foreach (var annex in annexs)
            {
                annex.externalcode = his.task_id;
                annexBLL.Update(annex);
            }
            return ResponseResult.Success();
        }
        #endregion

        #region Private
        /// <summary>
        /// 动态执行注入程序
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="result">审批结果 1 通过 2 拒绝</param>
        /// <param name="opinion">审批意见</param>
        private ResponseResult ProcessNodeInject(WFSchemeNodeModel node, string instanceId, string processId, int result, string opinion, List<wf_taskEntity> listTask, List<wf_taskhistoryEntity> listHistory, List<string> listSql)
        {
            if (!node.injectassembly.IsEmpty() && !node.injectclass.IsEmpty())
            {
                Assembly ass = Assembly.Load(node.injectassembly);
                Type tp = ass.GetType(node.injectclass);//类名
                IWFNodeProcess process = Activator.CreateInstance(tp) as IWFNodeProcess;//实例化
                if (process == null)
                {
                    throw new Exception($"{node.name}执行程序配置错误，请联系管理员");
                }
                MethodInfo methCheck = tp.GetMethod("CheckApprove");//加载方法
                var res = methCheck.Invoke(process, new object[] { instanceId, processId, result, opinion }) as ResponseResult;//执行
                if (res == null)
                {
                    throw new Exception($"{node.name}执行程序配置错误，验证出错，请联系管理员");
                }
                if (res.code == (int)ResponseCode.Success)
                {
                    MethodInfo meth = tp.GetMethod("Approve");//加载方法
                    meth.Invoke(process, new object[] { instanceId, processId, result, opinion, listTask, listHistory, listSql });//执行
                    return ResponseResult.Success();
                }
                else
                {
                    return res;
                }
            }
            return ResponseResult.Success();
        }

        /// <summary>
        /// 递归判断下一节点, 
        /// 如果下一节点是 结束  生成 history 并标记结束
        /// 如果下一节点是 判断 则判断之后再找 下下个 并生成 history
        /// 如果下一节点是 执行 则执行后找 下下个  并生成 history
        /// 如果下一节点 普通审批 (包括 会签 传阅 ) 生成 task 并且 如果下一节点审批人包含自己 则 自动通过
        /// </summary>
        /// <param name="nextlines">下一节点连线, 下一节点可能有多个</param>
        /// <param name="alllines">所有连线</param>
        /// <param name="allnodes">所有节点</param>
        /// <param name="instance">流程实例</param>
        /// <param name="nodeCurrent">当前节点</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private Tuple<List<wf_taskEntity>, List<wf_taskhistoryEntity>, List<string>> FindNextNode(List<WFSchemeLineModel> nextlines, List<WFSchemeLineModel> alllines, List<WFSchemeNodeModel> allnodes, wf_instanceEntity instance, WFSchemeNodeModel nodeCurrent, UserModel loginInfo)
        {
            var listTask = new List<wf_taskEntity>();
            var listHistory = new List<wf_taskhistoryEntity>();
            var listSql = new List<string>();
            foreach (var line in nextlines)
            {
                var currentLine = line;
                var nextNodes = allnodes.Where(r => r.id == currentLine.to).ToList();
                foreach (var nodeNext in nextNodes)
                {
                    if (nodeNext.type == "endround" || nodeNext.type == "startround") //结束
                    {     // 之后直接结束  task 表不插值  history插入结束
                        var historyEnd = new wf_taskhistoryEntity();
                        historyEnd = instance.Adapt<wf_taskhistoryEntity>();
                        historyEnd.result = 0;
                        historyEnd.node_id = nodeNext.id;
                        historyEnd.nodename = nodeNext.name;
                        historyEnd.nodetype = nodeNext.type;
                        historyEnd.previous_id = nodeCurrent.id;
                        historyEnd.previousname = nodeCurrent.name;

                        historyEnd.tasktime = DateTime.Now;
                        historyEnd.apptime = DateTime.Now;

                        historyEnd.appremark = "【流程结束】";
                        historyEnd.Create(loginInfo);
                        listHistory.Add(historyEnd);

                        instance.is_finished = 1;
                        instance.finishtime = DateTime.Now;
                        instance.finish_userid = loginInfo.user_id;
                        instance.finish_username = $"{loginInfo.realname}-{loginInfo.loginname}";
                        break;
                    }
                    else if (nodeNext.type == "conditionnode") // 条件判断
                    {  // 之后为条件判断节点  task 表不插值  history插入条件判断  需要递归 
                        var historyCondition = new wf_taskhistoryEntity();
                        historyCondition = instance.Adapt<wf_taskhistoryEntity>();
                        historyCondition.result = 0;
                        historyCondition.node_id = nodeNext.id;
                        historyCondition.nodename = nodeNext.name;
                        historyCondition.nodetype = nodeNext.type;
                        historyCondition.previous_id = nodeCurrent.id;
                        historyCondition.previousname = nodeCurrent.name;

                        historyCondition.tasktime = DateTime.Now;
                        historyCondition.apptime = DateTime.Now;

                        historyCondition.Create(loginInfo);

                        var db = new DataAccess.Repository().db;
                        var sql = nodeNext.sqlcondition.Replace("@processId", $"{DataAccess.BaseConnection.ParaPre}processId");
                        var resultCondition = db.Ado.ExecuteNonQuery(sql, new { processId = instance.process_id });
                        if (resultCondition > 0)
                        {
                            historyCondition.appremark = "判断结果为【是】";
                            currentLine = alllines.Where(r => r.from == nodeNext.id && r.wftype == 1).FirstOrDefault();
                        }
                        else
                        {
                            historyCondition.appremark = "判断结果为【否】";
                            currentLine = alllines.Where(r => r.from == nodeNext.id && r.wftype == 2).FirstOrDefault();
                        }
                        listHistory.Add(historyCondition);
                    }
                    else if (nodeNext.type == "processnode") // 执行
                    {   // 之后为执行节点  task 表不插值  history插入执行  需要递归 , 可能后面都是执行 直到结束    
                        listSql.Add(nodeNext.sqlsuccess);
                        ProcessNodeInject(nodeNext, instance.instance_id, instance.process_id, 1, "", listTask, listHistory, listSql);

                        currentLine = alllines.Where(r => r.from == nodeNext.id).FirstOrDefault();

                        var historyProcess = new wf_taskhistoryEntity();
                        historyProcess = instance.Adapt<wf_taskhistoryEntity>();
                        historyProcess.result = 0;
                        historyProcess.node_id = nodeNext.id;
                        historyProcess.nodename = nodeNext.name;
                        historyProcess.nodetype = nodeNext.type;
                        historyProcess.previous_id = nodeCurrent.id;
                        historyProcess.previousname = nodeCurrent.name;

                        historyProcess.tasktime = DateTime.Now;
                        historyProcess.apptime = DateTime.Now;

                        historyProcess.Create(loginInfo);
                        listHistory.Add(historyProcess);
                    }
                    else if (nodeNext.type == "confluencenode") // 会签
                    {
                        var preLines = alllines.Where(r => r.to == nodeNext.id).ToList();
                        var preNodes = allnodes.Where(r => preLines.Select(t => t.from).Contains(r.id)).ToList();
                        var preNodeIDs = preNodes.Select(r => r.id).ToList();

                        var countAll = preNodes.Count;

                        var countAgree = taskhistoryService.GetCount(r => r.instance_id == instance.instance_id && preNodeIDs.Contains(r.node_id) && r.result == 1);

                        var flag = false;  // 会签成功
                        if (nodeNext.confluence_type == 1 && countAll == countAgree + 1)  // 全部
                        {
                            flag = true;
                        }
                        if (nodeNext.confluence_type == 2 && countAll == countAgree + 1)  //  任意一个
                        {
                            flag = true;
                        }
                        if (nodeNext.confluence_type == 3 && (countAgree + 1) / countAll * 100 > nodeNext.confluence_rate)  // 按比例
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            var nextLines = alllines.Where(r => r.from == nodeNext.id).ToList();

                            var result = FindNextNode(nextLines, alllines, allnodes, instance, nodeNext, loginInfo);

                            listTask.AddRange(result.Item1);
                            listHistory.AddRange(result.Item2);
                            listSql.AddRange(result.Item3);
                        }
                    }
                    else  // 传阅  一般审批 如果包含自己  自动通过
                    {
                        var taskEntity = new wf_taskEntity();
                        taskEntity = instance.Adapt<wf_taskEntity>();
                        taskEntity.node_id = nodeNext.id;
                        taskEntity.nodename = nodeNext.name;
                        taskEntity.previous_id = nodeCurrent.id;
                        taskEntity.nodetype = nodeNext.type;
                        taskEntity.previousname = nodeCurrent.name;
                        taskEntity.Create(loginInfo);

                        var turple = GetNextAuth(nodeNext, instance, taskEntity.task_id, loginInfo);
                        taskEntity.authrizes = turple.Item1; // 下一步审批人

                        bool isContainSelf = turple.Item2;  // 下一步审批人是否包含自己
                        if (isContainSelf && nodeCurrent.autoskip == 1 && !nodeNext.forms.Exists(r => r.canedit == 1))  // 如果下一步包含自己 则 递归  必须表单不可编辑，且没有设置该步骤不需要自动跳过
                        {
                            // 如果包含自己，则刚刚的待审批置为已完成
                            taskEntity.is_done = 1;

                            listSql.Add(nodeNext.sqlsuccess);
                            ProcessNodeInject(nodeNext, instance.instance_id, instance.process_id, 1, "", listTask, listHistory, listSql);

                            var historySelf = new wf_taskhistoryEntity();
                            historySelf = taskEntity.Adapt<wf_taskhistoryEntity>();
                            historySelf.result = 1;
                            historySelf.node_id = nodeNext.id;
                            historySelf.nodename = nodeNext.name;
                            historySelf.previous_id = nodeCurrent.id;
                            historySelf.previousname = nodeCurrent.name;
                            historySelf.app_userid = loginInfo.user_id;
                            historySelf.app_username = $"{loginInfo.realname}-{loginInfo.loginname}";
                            historySelf.appremark = "【系统自动通过】与上一节点用户相同";
                            historySelf.tasktime = DateTime.Now;
                            historySelf.apptime = DateTime.Now;
                            historySelf.Create(loginInfo);
                            listHistory.Add(historySelf);

                            var nextLines = alllines.Where(r => r.from == nodeNext.id && (r.wftype == 1 || r.wftype == 0)).ToList();

                            var result = FindNextNode(nextLines, alllines, allnodes, instance, nodeNext, loginInfo);

                            listTask.AddRange(result.Item1);
                            listHistory.AddRange(result.Item2);
                            listSql.AddRange(result.Item3);
                        }

                        listTask.Add(taskEntity);

                    }
                }
            }
            return new Tuple<List<wf_taskEntity>, List<wf_taskhistoryEntity>, List<string>>(listTask, listHistory, listSql);
        }

        /// <summary>
        /// 判断下一步审批 哪些人可以看到 并且判断其中有没有包含自己
        /// </summary>
        /// <param name="nodeNext"></param>
        /// <param name="instance"></param>
        /// <param name="task_id"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private Tuple<List<wf_task_authorizeEntity>, bool> GetNextAuth(WFSchemeNodeModel nodeNext, wf_instanceEntity instance, string task_id, UserModel loginInfo)
        {
            // 生成 哪些人可以审批该条目
            bool isContainSelf = false;  // 下一步审批人是否包含自己
            var listAuth = new List<wf_task_authorizeEntity>();
            foreach (var nodeAuth in nodeNext.authusers)
            {
                var objects = nodeAuth.objectids.SplitNoEmpty(',');
                foreach (var objectId in objects)
                {
                    var auth = new wf_task_authorizeEntity();
                    // 1 用户 2 部门 3 公司 4 岗位 5 角色 6 小组
                    switch (nodeAuth.objecttype)
                    {
                        case 1:
                            auth.user_id = objectId;
                            if (objectId == loginInfo.user_id)
                            {
                                isContainSelf = true;
                            }
                            break;
                        case 2:
                            auth.department_id = objectId;
                            if (objectId == loginInfo.department_id)
                            {
                                isContainSelf = true;
                            }
                            break;
                        case 3:
                            auth.company_id = objectId;
                            if (objectId == loginInfo.company_id)
                            {
                                isContainSelf = true;
                            }
                            break;
                        case 4:
                            auth.post_id = objectId;
                            if (nodeAuth.objectrange == 1)
                            {
                                auth.company_id = instance.company_id;
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.company_id == instance.company_id)
                                {
                                    isContainSelf = true;
                                }
                            }
                            else if (nodeAuth.objectrange == 2)
                            {
                                auth.department_id = instance.department_id;
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.department_id == instance.department_id)
                                {
                                    isContainSelf = true;
                                }
                            }
                            else if (nodeAuth.objectrange == 3)
                            {
                                auth.manage_dept_id = instance.department_id;
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.managedepartments.Exists(r => r.relationtype == 2 && r.object_id == objectId && r.department_id == instance.department_id))
                                {
                                    isContainSelf = true;
                                }
                            }
                            break;
                        case 5:
                            auth.role_id = objectId;
                            if (nodeAuth.objectrange == 1)
                            {
                                auth.company_id = instance.company_id;
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.company_id == instance.company_id)
                                {
                                    isContainSelf = true;
                                }

                            }
                            else if (nodeAuth.objectrange == 2)
                            {
                                auth.department_id = instance.department_id;
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.department_id == instance.department_id)
                                {
                                    isContainSelf = true;
                                }
                            }
                            else if (nodeAuth.objectrange == 3)
                            {
                                auth.manage_dept_id = instance.department_id;
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.managedepartments.Exists(r => r.relationtype == 1 && r.object_id == objectId && r.department_id == instance.department_id))
                                {
                                    isContainSelf = true;
                                }
                            }
                            break;
                        case 6:
                            auth.group_id = objectId;
                            if (loginInfo.group_ids.Contains(objectId))
                            {
                                isContainSelf = true;
                            }
                            break;
                        case 9:   // 提交人自己
                            auth.user_id = instance.create_userId;
                            if (loginInfo.user_id == auth.user_id)
                            {
                                isContainSelf = true;
                            }
                            break;
                    }
                    auth.task_id = task_id;
                    auth.Create(loginInfo);
                    listAuth.Add(auth);
                }
            }

            // 添加 委托任务
            // 委托会签办理不要放在这里面，通过查询来，写成记录很不灵活
            //var allAuthUsers = GetUserByAuth(listAuth);
            //var dateNow = DateTime.Now;
            //foreach (var user in allAuthUsers)
            //{
            //    var delegates = delegateBLL.GetList(r => r.user_id == loginInfo.user_id && r.starttime >= dateNow && r.endtime < dateNow);
            //    foreach (var dele in delegates)
            //    {
            //        if (dele.flowcode == "ALL" && dele.flowcode.SplitNoEmpty(",").Contains(instance.flowcode))
            //        {
            //            listAuth.Add(new wf_task_authorizeEntity
            //            {
            //                user_id = dele.to_user_id,
            //                is_add = 2,  // 委托任务
            //            });
            //            if (dele.to_user_id == loginInfo.user_id)
            //            {
            //                isContainSelf = true;
            //            }
            //        }
            //    }
            //}

            return new Tuple<List<wf_task_authorizeEntity>, bool>(listAuth, isContainSelf);
        }

        /// <summary>
        /// 获取能看到下一步审批的用户
        /// </summary>
        /// <param name="auths"></param>
        /// <returns></returns>
        private List<sys_userEntity> GetUserByAuth(List<wf_task_authorizeEntity> auths)
        {
            var alluser = userBLL.GetAllByCache();
            var allrelation = userrelationBLL.GetAllByCache();
            var listUser = new List<sys_userEntity>();
            foreach (var auth in auths)
            {
                if (!auth.user_id.IsEmpty())
                {
                    var user = alluser.Where(r => r.user_id == auth.user_id).FirstOrDefault();
                    if (user != null)
                    {
                        listUser.Add(user);
                    }
                }
                if (!auth.company_id.IsEmpty() && auth.post_id.IsEmpty() && auth.role_id.IsEmpty())
                {
                    var users = alluser.Where(r => r.company_id == auth.company_id).ToList();
                    if (!users.IsEmpty())
                    {
                        listUser.AddRange(users);
                    }
                }
                if (!auth.department_id.IsEmpty() && auth.post_id.IsEmpty() && auth.role_id.IsEmpty())
                {
                    var users = alluser.Where(r => r.department_id == auth.department_id).ToList();
                    if (!users.IsEmpty())
                    {
                        listUser.AddRange(users);
                    }
                }
                if (!auth.post_id.IsEmpty())
                {
                    var userIds = allrelation.Where(r => r.relationtype == (int)UserRelationEnum.Post && r.object_id == auth.post_id).Select(r => r.user_id).ToList();
                    if (!auth.company_id.IsEmpty())
                    {
                        var users = alluser.Where(r => userIds.Contains(r.user_id) && r.company_id == auth.company_id).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                    if (!auth.department_id.IsEmpty())
                    {
                        var users = alluser.Where(r => userIds.Contains(r.user_id) && r.department_id == auth.department_id).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                    if (!auth.manage_dept_id.IsEmpty())
                    {
                        var magener_ids = manageBLL.GetDepartmentManagers(auth.manage_dept_id, auth.post_id, DepartmentManageRelationEnum.Post);
                        magener_ids = userIds.Intersect(magener_ids).ToList();
                        var users = alluser.Where(r => magener_ids.Contains(r.user_id)).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                }
                if (!auth.role_id.IsEmpty())
                {
                    var userIds = allrelation.Where(r => r.relationtype == (int)UserRelationEnum.Role && r.object_id == auth.role_id).Select(r => r.user_id).ToList();
                    if (!auth.company_id.IsEmpty())
                    {
                        var users = alluser.Where(r => userIds.Contains(r.user_id) && r.company_id == auth.company_id).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                    if (!auth.department_id.IsEmpty())
                    {
                        var users = alluser.Where(r => userIds.Contains(r.user_id) && r.department_id == auth.department_id).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                    if (!auth.manage_dept_id.IsEmpty())
                    {
                        var magener_ids = manageBLL.GetDepartmentManagers(auth.manage_dept_id, auth.post_id, DepartmentManageRelationEnum.Role);
                        magener_ids = userIds.Intersect(magener_ids).ToList();
                        var users = alluser.Where(r => magener_ids.Contains(r.user_id)).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                }
                if (!auth.group_id.IsEmpty())
                {
                    var userIds = allrelation.Where(r => r.relationtype == (int)UserRelationEnum.Group && r.object_id == auth.group_id).Select(r => r.user_id).ToList();
                    var users = alluser.Where(r => userIds.Contains(r.user_id)).ToList();
                    if (users != null)
                    {
                        listUser.AddRange(users);
                    }
                }
            }
            return listUser;
        }

        /// <summary>
        /// 查出节点下配置的具体审批人员
        /// </summary>
        private List<sys_userEntity> GetUserByNode(WFSchemeNodeModel node, wf_instanceEntity instance)
        {
            var listUser = new List<sys_userEntity>();
            if (!node.authusers.IsEmpty())
            {
                var alluser = userBLL.GetAllByCache();
                var alldept = deptBLL.GetAllByCache();
                var allcompany = companyBLL.GetAllByCache();
                var allrolw = roleBLL.GetAllByCache();
                var allpost = postBLL.GetAllByCache();
                var allgroup = groupBLL.GetAllByCache();

                var allrelation = userrelationBLL.GetAllByCache();
                foreach (var auth in node.authusers)
                {
                    switch (auth.objecttype)
                    {
                        case 1:  // 用户
                            var users = alluser.Where(r => auth.objectids.Contains(r.user_id)).ToList();
                            if (!users.IsEmpty())
                            {
                                users.RemoveAll(r => listUser.Exists(t => t.user_id == r.user_id));
                                listUser.AddRange(users);
                            }
                            break;
                        case 2:  // 部门
                            var depts = alldept.Where(r => auth.objectids.Contains(r.department_id)).ToList();
                            var users2 = alluser.Where(r => depts.Select(t => t.department_id).Contains(r.department_id)).ToList();
                            if (!users2.IsEmpty())
                            {
                                users2.RemoveAll(r => listUser.Exists(t => t.user_id == r.user_id));
                                listUser.AddRange(users2);
                            }
                            break;
                        case 3:  // 公司
                            var companys = allcompany.Where(r => auth.objectids.Contains(r.company_id)).ToList();
                            var users3 = alluser.Where(r => companys.Select(t => t.company_id).Contains(r.company_id)).ToList();
                            if (!users3.IsEmpty())
                            {
                                users3.RemoveAll(r => listUser.Exists(t => t.user_id == r.user_id));
                                listUser.AddRange(users3);
                            }
                            break;
                        case 4:  // 岗位
                            var posts = allpost.Where(r => auth.objectids.Contains(r.post_id)).ToList();
                            var userids4 = allrelation.Where(r => r.relationtype == 2 && posts.Select(t => t.post_id).Contains(r.object_id)).Select(r => r.user_id).ToList();
                            var users4 = alluser.Where(r => userids4.Contains(r.user_id)).ToList();
                            if (!users4.IsEmpty())
                            {
                                users4.RemoveAll(r => listUser.Exists(t => t.user_id == r.user_id));
                                listUser.AddRange(users4);
                            }
                            break;
                        case 5:  // 角色
                            var roles = allrolw.Where(r => auth.objectids.Contains(r.role_id)).ToList();
                            var userids5 = allrelation.Where(r => r.relationtype == 1 && roles.Select(t => t.role_id).Contains(r.object_id)).Select(r => r.user_id).ToList();
                            var users5 = alluser.Where(r => userids5.Contains(r.user_id)).ToList();
                            if (!users5.IsEmpty())
                            {
                                users5.RemoveAll(r => listUser.Exists(t => t.user_id == r.user_id));
                                listUser.AddRange(users5);
                            }
                            break;
                        case 6:  // 小组
                            var groups = allgroup.Where(r => auth.objectids.Contains(r.group_id)).ToList();
                            var userids6 = allrelation.Where(r => r.relationtype == 3 && groups.Select(t => t.group_id).Contains(r.object_id)).Select(r => r.user_id).ToList();
                            var users6 = alluser.Where(r => userids6.Contains(r.user_id)).ToList();
                            if (!users6.IsEmpty())
                            {
                                users6.RemoveAll(r => listUser.Exists(t => t.user_id == r.user_id));
                                listUser.AddRange(users6);
                            }
                            break;
                        case 9:  // 提交人自己
                            var user = alluser.Where(r => r.user_id == instance.create_userId).FirstOrDefault();
                            if (user != null && !listUser.Exists(t => t.user_id == user.user_id))
                            {
                                listUser.Add(user);
                            }
                            break;
                    }
                }
            }
            listUser = listUser.OrderBy(r => r.sort).ToList();
            return listUser;
        }
        #endregion

        #region 翻译Result
        public static Dictionary<int, string> ApproveResult = new Dictionary<int, string>
        {
            {1,"通过" },
            {2,"退回" },
            {3,"申请会签办理" },
            {4,"已阅" },
            {5,"调整" },
            {6,"申请转发查看" },
            {100,"当前待办" },
        };

        public static string ApproveResultName(int result)
        {
            if (ApproveResult.ContainsKey(result))
            {
                return $"【{ApproveResult[result]}】"; ;
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}
