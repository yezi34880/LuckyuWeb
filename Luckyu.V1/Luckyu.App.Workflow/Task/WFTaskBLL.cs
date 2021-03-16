using Luckyu.App.Organization;
using Luckyu.App.System;
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
        private wf_flow_instanceService instanceService = new wf_flow_instanceService();
        private wf_taskhistoryService taskhistoryService = new wf_taskhistoryService();
        private wf_taskService taskService = new wf_taskService();
        private wf_task_authorizeService taskauthService = new wf_task_authorizeService();
        private WFDelegateBLL delegateBLL = new WFDelegateBLL();
        private UserBLL userBLL = new UserBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        private MessageBLL messageBLL = new MessageBLL();
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
            return page;
        }

        public wf_taskEntity GetTaskEnttity(Expression<Func<wf_taskEntity, bool>> condition)
        {
            var entity = taskService.GetEntity(condition);
            return entity;
        }
        public wf_taskhistoryEntity GetHistoryEnttity(Expression<Func<wf_taskhistoryEntity, bool>> condition)
        {
            var entity = taskhistoryService.GetEntity(condition);
            return entity;
        }
        public wf_flow_instanceEntity GetInstanceEnttity(Expression<Func<wf_flow_instanceEntity, bool>> condition)
        {
            var entity = instanceService.GetEntity(condition);
            return entity;
        }
        public List<wf_flow_instanceEntity> GetInstanceList(Expression<Func<wf_flow_instanceEntity, bool>> condition)
        {
            var list = instanceService.GetList(condition, r => r.createtime);
            return list;
        }

        public List<wf_taskhistoryEntity> GetHistoryTaskList(string processId, string instanceId = "")
        {
            Expression<Func<wf_taskhistoryEntity, bool>> exp = r => r.process_id == processId;
            if (!instanceId.IsEmpty())
            {
                exp = exp.LinqAnd(r => r.instance_id == instanceId);
            }
            var list = taskhistoryService.GetList(exp, r => r.createtime);

            Expression<Func<wf_taskEntity, bool>> exp1 = r => r.is_done == 0 && r.process_id == processId;
            if (!instanceId.IsEmpty())
            {
                exp1 = exp1.LinqAnd(r => r.instance_id == instanceId);
            }
            var listTask = taskService.GetList(exp1);

            foreach (var item in listTask)
            {
                var item1 = item.Adapt<wf_taskhistoryEntity>();
                var auths = taskauthService.GetList(r => r.task_id == item.task_id);
                var users = GetUserByAuth(auths);
                item1.opinion = string.Join(",", users.Select(r => $"{r.realname} { r.loginname}"));
                item1.result = -1;
                item1.createtime = null;
                item1.create_username = "";
                list.Add(item1);
            }
            return list;
        }

        public JqgridPageResponse<WFTaskModel> MonitorPage(JqgridPageRequest jqPage, int is_finished)
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
        public ResponseResult<Dictionary<string, object>> GetFormData(string instanceId, string taskId, string historyId)
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
            if (!historyId.IsEmpty())
            {
                // 已办历史
                var history = GetHistoryEnttity(r => r.history_id == historyId);
                if (history == null)
                {
                    return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
                }
                showNode = scheme.nodes.Where(r => r.id == history.node_id).FirstOrDefault();

                var task = GetTaskEnttity(r => r.instance_id == instance.instance_id && r.is_done == 0);
                if (task != null)
                {
                    currentNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                }
            }
            else if (!taskId.IsEmpty())
            {
                // 待办
                var task = GetTaskEnttity(r => r.task_id == taskId);
                if (task == null)
                {
                    return ResponseResult.Fail<Dictionary<string, object>>(MessageString.NoData);
                }
                showNode = scheme.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                currentNode = showNode;
            }
            else
            {
                // 自己发起的
                showNode = scheme.nodes.Where(r => r.type == "startround").FirstOrDefault();
                var task = GetTaskEnttity(r => r.instance_id == instance.instance_id && r.is_done == 0);
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
            var task = GetTaskEnttity(r => r.instance_id == instance.instance_id && r.is_done == 0);
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

            var instance = new wf_flow_instanceEntity();
            instance = flow.Adapt<wf_flow_instanceEntity>();

            instance.process_id = processId;
            instance.processname = processName;
            instance.processcontent = processContent;

            // 单据提交人 后期 同公司 同部门 根据这个值计算 后期考虑作为入参 代为提交别人单据的情况 先不管
            instance.submit_user_id = loginInfo.user_id;
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
            historyStart.opinion = "【发起流程】";
            historyStart.Create(loginInfo);
            listHistory.Add(historyStart);
            if (!nodeStart.sqlsuccess.IsEmpty())
            {
                listSql.Add(nodeStart.sqlsuccess);
            }
            var res = ProcessNodeInject(nodeStart, 1, "");
            if (res.code == (int)ResponseCode.Fail)
            {
                return ResponseResult.Fail(res.info, res.data);
            }
            var lines = nodeModel.lines.Where(r => r.from == nodeStart.id).ToList();
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

            var data = new Tuple<wf_flow_instanceEntity, List<wf_taskEntity>>(instance, listTask);
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
                var data = res.data as Tuple<wf_flow_instanceEntity, List<wf_taskEntity>>;
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
                var data = res.data as Tuple<wf_flow_instanceEntity, List<wf_taskEntity>>;
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
        public ResponseResult Approve(string taskId, int result, string opinion, UserModel loginInfo)
        {
            var task = taskService.GetEntity(r => r.task_id == taskId);
            if (task == null)
            {
                return ResponseResult.Fail("该任务不存在");
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

            var nodeModel = instance.schemejson.ToObject<WFSchemeModel>();

            var listTask = new List<wf_taskEntity>();
            var listHistory = new List<wf_taskhistoryEntity>();
            var listSql = new List<string>();

            var nodeCurrent = nodeModel.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
            var historyCurrent = new wf_taskhistoryEntity();
            historyCurrent = task.Adapt<wf_taskhistoryEntity>();
            historyCurrent.result = result;
            historyCurrent.nodetype = nodeCurrent.type;

            var currentAuth = auths.Where(r => r.user_id == loginInfo.user_id).FirstOrDefault();
            if (currentAuth != null && currentAuth.is_add == 1)
            {
                historyCurrent.opinion = "【加签】 ";
            }
            else if (currentAuth != null && currentAuth.is_add == 2)
            {
                historyCurrent.opinion = "【任务委托】 ";
            }
            if (nodeCurrent.type == "confluencenode")
            {
                historyCurrent.opinion += "【会签】 " + opinion;
            }
            else if (nodeCurrent.type == "auditornode")
            {
                historyCurrent.opinion += "【传阅】 " + opinion;
            }
            else
            {
                historyCurrent.opinion += "【审批】" + opinion;
            }
            historyCurrent.authorize_user_id = loginInfo.user_id;
            historyCurrent.authorizen_userame = loginInfo.realname;
            historyCurrent.Create(loginInfo);
            listHistory.Add(historyCurrent);
            var res = ProcessNodeInject(nodeCurrent, result, opinion);
            if (res.code == (int)ResponseCode.Fail)
            {
                return ResponseResult.Fail(res.info, res.data);
            }

            task.is_done = 1;
            var lines = nodeModel.lines.Where(r => r.from == nodeCurrent.id).ToList();
            if (result == 1)
            {
                lines = lines.Where(r => r.wftype == 1 || r.wftype == 0).ToList();
                if (!nodeCurrent.sqlsuccess.IsEmpty())
                {
                    listSql.Add(nodeCurrent.sqlsuccess);
                }
            }
            else if (result == 2)
            {
                lines = lines.Where(r => r.wftype == 2 || r.wftype == 0).ToList();
                if (!nodeCurrent.sqlfail.IsEmpty())
                {
                    listSql.Add(nodeCurrent.sqlfail);
                }
            }
            if (lines.IsEmpty())
            {
                return ResponseResult.Fail("该流程节点没有下一步连线，请联系管理员");
            }
            var turple = FindNextNode(lines, nodeModel.lines, nodeModel.nodes, instance, nodeCurrent, loginInfo);
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

            taskService.Approve(instance, task, listTask, listHistory, listSql);
            var data = new Tuple<wf_flow_instanceEntity, List<wf_taskEntity>>(instance, listTask);
            return ResponseResult.Success((object)data);
        }

        public async Task<ResponseResult> Approve(string taskId, int result, string opinion, UserModel loginInfo, IHubContext<MessageHub> hubContext)
        {
            var res = Approve(taskId, result, opinion, loginInfo);
            if (res.code == (int)ResponseCode.Success)
            {
                var data = res.data as Tuple<wf_flow_instanceEntity, List<wf_taskEntity>>;
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

        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="taskId">当前流程</param>
        /// <param name="userId">加签人员</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult AddUser(string taskId, List<string> userIds, UserModel loginInfo)
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

            var auths = new List<wf_task_authorizeEntity>();
            var users = new List<sys_userEntity>();
            foreach (var userId in userIds)
            {
                var auth = new wf_task_authorizeEntity();
                auth.task_id = task.task_id;
                auth.user_id = userId;
                auth.is_add = 1;  // 加签审批
                auth.Create(loginInfo);
                auths.Add(auth);

                var user = userBLL.GetEntityByCache(r => r.user_id == userId);
                users.Add(user);
            }

            var history = new wf_taskhistoryEntity();
            history = task.Adapt<wf_taskhistoryEntity>();
            var struser = string.Join(",", users.Select(r => $"{r.realname}-{r.loginname}"));
            history.opinion = $"{loginInfo.realname}-{loginInfo.loginname}  申请  {struser} 【加签】审批";
            history.result = 3;
            history.Create(loginInfo);

            taskService.AddUser(auths, history);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 强制结束流程
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
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
            var schemeModel = instance.schemejson.ToObject<WFSchemeModel>();
            var firsttask = tasks.Select(r => r).FirstOrDefault();
            var firstnode = schemeModel.nodes.Where(r => r.id == firsttask.node_id).Select(r => r).FirstOrDefault();
            var res = ProcessNodeInject(firstnode, 2, "");
            if (res.code == (int)ResponseCode.Fail)
            {
                return ResponseResult.Fail(res.info, res.data);
            }

            var history = new wf_taskhistoryEntity();
            history = firsttask.Adapt<wf_taskhistoryEntity>();
            history.result = 2;
            history.opinion = $"【流程监控】{loginInfo.realname} 强制结束流程";
            history.Create(loginInfo);

            taskService.Finish(instance, tasks, history, firstnode.sqlfail);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 模拟完成审批
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult Complete(string instanceId, UserModel loginInfo)
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
            var tasks = taskService.GetList(r => r.instance_id == instanceId && r.is_done == 0);
            if (tasks.IsEmpty())
            {
                return ResponseResult.Fail("当前任务不存在");
            }
            var listTask = new List<wf_taskEntity>();
            var listHistory = new List<wf_taskhistoryEntity>();
            var listSql = new List<string>();

            var nodeModel = instance.schemejson.ToObject<WFSchemeModel>();

            do
            {
                foreach (var task in tasks)
                {
                    var nodeCurrent = nodeModel.nodes.Where(r => r.id == task.node_id).FirstOrDefault();
                    var historyCurrent = new wf_taskhistoryEntity();
                    historyCurrent = task.Adapt<wf_taskhistoryEntity>();
                    historyCurrent.result = 1;
                    historyCurrent.nodetype = nodeCurrent.type;
                    historyCurrent.opinion = $"【流程监控】{loginInfo.realname} 强制生效流程";
                    historyCurrent.authorize_user_id = loginInfo.user_id;
                    historyCurrent.authorizen_userame = loginInfo.realname;
                    historyCurrent.Create(loginInfo);
                    listHistory.Add(historyCurrent);
                    var res = ProcessNodeInject(nodeCurrent, 1, "");
                    if (res.code == (int)ResponseCode.Fail)
                    {
                        return ResponseResult.Fail(res.info, res.data);
                    }

                    task.is_done = 1;
                    var lines = nodeModel.lines.Where(r => r.from == nodeCurrent.id).ToList();
                    lines = lines.Where(r => r.wftype == 1 || r.wftype == 0).ToList();
                    if (!nodeCurrent.sqlsuccess.IsEmpty())
                    {
                        listSql.Add(nodeCurrent.sqlsuccess);
                    }
                    var turple = FindNextNode(lines, nodeModel.lines, nodeModel.nodes, instance, nodeCurrent, loginInfo);

                    listTask.AddRange(turple.Item1);
                    listHistory.AddRange(turple.Item2);
                    listSql.AddRange(turple.Item3);
                    tasks = turple.Item1;
                }
            }
            while (!tasks.IsEmpty());

            instance.is_finished = 1;

            taskService.Complete(instance, listTask, listHistory, listSql);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 调整流程, 设置运行中流程到任意节点
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="nodeId"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult Modify(string instanceId, string schemeId, string nodeId, UserModel loginInfo)
        {
            var instance = instanceService.GetEntity(r => r.instance_id == instanceId);
            if (instance == null)
            {
                return ResponseResult.Fail("该流程实例不存在");
            }
            if (instance.is_finished == 1)
            {
                return ResponseResult.Fail("该流程已结束, 不能调整");
            }
            var oldTasks = taskService.GetList(r => r.instance_id == instanceId && r.is_done == 0);
            if (oldTasks.IsEmpty())
            {
                return ResponseResult.Fail("当前任务不存在");
            }
            var isLast = false;
            if (!schemeId.IsEmpty())
            {
                var scheme = schemeService.GetEntity(r => r.scheme_id == schemeId);
                if (scheme != null)
                {
                    instance.schemejson = scheme.schemejson;
                    isLast = true;
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
            newTask.previous_id = oldTasks[0].node_id;
            newTask.nodetype = nodeNext.type;
            newTask.previousname = oldTasks[0].nodename;
            newTask.Create(loginInfo);

            var turple = GetNextAuth(nodeNext, instance, newTask.task_id, loginInfo);
            newTask.authrizes = turple.Item1;

            var history = new wf_taskhistoryEntity();
            history = oldTasks[0].Adapt<wf_taskhistoryEntity>();
            history.result = 0;
            history.opinion = isLast ? $"{loginInfo.realname} 通过流程监控调整该流程至最新版本 调整后为【{nodeNext.name}】" : $"{loginInfo.realname} 通过流程监控调整 当前待办任务为【{ oldTasks[0].nodename}】 调整后为【{nodeNext.name}】";
            history.Create(loginInfo);

            taskService.Modify((isLast ? instance : null), oldTasks, newTask, history);
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
        private ResponseResult ProcessNodeInject(WFSchemeNodeModel node, int result, string opinion)
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
                var res = methCheck.Invoke(process, new object[] { result, opinion }) as ResponseResult;//执行
                if (res == null)
                {
                    throw new Exception($"{node.name}执行程序配置错误，验证出错，请联系管理员");
                }
                if (res.code == (int)ResponseCode.Success)
                {
                    MethodInfo meth = tp.GetMethod("Approve");//加载方法
                    meth.Invoke(process, new object[] { result, opinion });//执行
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
        private Tuple<List<wf_taskEntity>, List<wf_taskhistoryEntity>, List<string>> FindNextNode(List<WFSchemeLineModel> nextlines, List<WFSchemeLineModel> alllines, List<WFSchemeNodeModel> allnodes, wf_flow_instanceEntity instance, WFSchemeNodeModel nodeCurrent, UserModel loginInfo)
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
                    if (nodeNext.type == "endround") //结束
                    {     // 之后直接结束  task 表不插值  history插入结束
                        var historyEnd = new wf_taskhistoryEntity();
                        historyEnd = instance.Adapt<wf_taskhistoryEntity>();
                        historyEnd.result = 0;
                        historyEnd.node_id = nodeNext.id;
                        historyEnd.nodename = nodeNext.name;
                        historyEnd.nodetype = nodeNext.type;
                        historyEnd.previous_id = nodeCurrent.id;
                        historyEnd.previousname = nodeCurrent.name;
                        historyEnd.opinion = "【流程结束】";
                        historyEnd.Create(loginInfo);
                        listHistory.Add(historyEnd);

                        instance.is_finished = 1;
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
                        historyCondition.Create(loginInfo);

                        var db = new DataAccess.Repository().db;
                        var sql = nodeNext.sqlcondition.Replace("@processId", $"{DataAccess.BaseConnection.ParaPre}processId");
                        var resultCondition = db.Ado.ExecuteNonQuery(sql, new { processId = instance.process_id });
                        if (resultCondition > 0)
                        {
                            historyCondition.opinion = "判断结果为【是】";
                            currentLine = alllines.Where(r => r.from == nodeNext.id && r.wftype == 1).FirstOrDefault();
                        }
                        else
                        {
                            historyCondition.opinion = "判断结果为【否】";
                            currentLine = alllines.Where(r => r.from == nodeNext.id && r.wftype == 2).FirstOrDefault();
                        }
                        listHistory.Add(historyCondition);
                    }
                    else if (nodeNext.type == "processnode") // 执行
                    {   // 之后为执行节点  task 表不插值  history插入执行  需要递归 , 可能后面都是执行 直到结束    
                        listSql.Add(nodeNext.sqlsuccess);
                        ProcessNodeInject(nodeNext, 1, "");

                        currentLine = alllines.Where(r => r.from == nodeNext.id).FirstOrDefault();

                        var historyProcess = new wf_taskhistoryEntity();
                        historyProcess = instance.Adapt<wf_taskhistoryEntity>();
                        historyProcess.result = 0;
                        historyProcess.node_id = nodeNext.id;
                        historyProcess.nodename = nodeNext.name;
                        historyProcess.nodetype = nodeNext.type;
                        historyProcess.previous_id = nodeCurrent.id;
                        historyProcess.previousname = nodeCurrent.name;
                        historyProcess.Create(loginInfo);
                        historyProcess.opinion = "系统执行操作";
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
                        listTask.Add(taskEntity);

                        bool isContainSelf = turple.Item2;  // 下一步审批人是否包含自己
                        if (isContainSelf)  // 如果下一步包含自己 则 递归
                        {
                            listSql.Add(nodeNext.sqlsuccess);
                            ProcessNodeInject(nodeNext, 1, "");

                            var historySelf = new wf_taskhistoryEntity();
                            historySelf = taskEntity.Adapt<wf_taskhistoryEntity>();
                            historySelf.result = 1;
                            historySelf.node_id = nodeNext.id;
                            historySelf.nodename = nodeNext.name;
                            historySelf.previous_id = nodeCurrent.id;
                            historySelf.previousname = nodeCurrent.name;
                            historySelf.authorize_user_id = loginInfo.user_id;
                            historySelf.authorizen_userame = loginInfo.realname;
                            historySelf.Create(loginInfo);
                            historySelf.opinion = "审批人包含本人，自动通过";
                            listHistory.Add(historySelf);

                            var nextLines = alllines.Where(r => r.from == nodeNext.id).ToList();

                            var result = FindNextNode(nextLines, alllines, allnodes, instance, nodeNext, loginInfo);

                            listTask.AddRange(result.Item1);
                            listHistory.AddRange(result.Item2);
                            listSql.AddRange(result.Item3);
                        }
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
        private Tuple<List<wf_task_authorizeEntity>, bool> GetNextAuth(WFSchemeNodeModel nodeNext, wf_flow_instanceEntity instance, string task_id, UserModel loginInfo)
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
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.manage_dept_ids.Contains(instance.department_id))
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
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.manage_dept_ids.Contains(instance.department_id))
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
                    }
                    auth.task_id = task_id;
                    auth.Create(loginInfo);
                    listAuth.Add(auth);
                }
            }

            // 添加 委托任务
            var allAuthUsers = GetUserByAuth(listAuth);
            var dateNow = DateTime.Now;
            foreach (var user in allAuthUsers)
            {
                var delegates = delegateBLL.GetList(r => r.user_id == loginInfo.user_id && r.starttime >= dateNow && r.endtime < dateNow);
                foreach (var dele in delegates)
                {
                    if (dele.flowcode == "ALL" && dele.flowcode.SplitNoEmpty(",").Contains(instance.flowcode))
                    {
                        listAuth.Add(new wf_task_authorizeEntity
                        {
                            user_id = dele.to_user_id,
                            is_add = 2,  // 委托任务
                        });
                        if (dele.to_user_id == loginInfo.user_id)
                        {
                            isContainSelf = true;
                        }
                    }
                }
            }

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
                    var userIds = allrelation.Where(r => r.relationtype == (int)UserRelationType.Post && r.object_id == auth.post_id).Select(r => r.user_id).ToList();
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
                        var manages = allrelation.Where(r => r.relationtype == (int)UserRelationType.DeptManager && r.object_id == auth.manage_dept_id).Select(r => r.user_id).ToList();
                        manages = userIds.Intersect(manages).ToList();
                        var users = alluser.Where(r => manages.Contains(r.user_id)).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                }
                if (!auth.role_id.IsEmpty())
                {
                    var userIds = allrelation.Where(r => r.relationtype == (int)UserRelationType.Role && r.object_id == auth.role_id).Select(r => r.user_id).ToList();
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
                        var manages = allrelation.Where(r => r.relationtype == (int)UserRelationType.DeptManager && r.object_id == auth.manage_dept_id).Select(r => r.user_id).ToList();
                        manages = userIds.Intersect(manages).ToList();
                        var users = alluser.Where(r => manages.Contains(r.user_id)).ToList();
                        if (!users.IsEmpty())
                        {
                            listUser.AddRange(users);
                        }
                    }
                }
                if (!auth.group_id.IsEmpty())
                {
                    var userIds = allrelation.Where(r => r.relationtype == (int)UserRelationType.Group && r.object_id == auth.group_id).Select(r => r.user_id).ToList();
                    var users = alluser.Where(r => userIds.Contains(r.user_id)).ToList();
                    if (users != null)
                    {
                        listUser.AddRange(users);
                    }
                }
            }
            return listUser;
        }

        #endregion

    }
}
