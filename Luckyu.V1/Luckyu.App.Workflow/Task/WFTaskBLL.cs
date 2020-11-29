﻿using Luckyu.App.Organization;
using Luckyu.Utility;
using Mapster;
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
        private UserBLL userBLL = new UserBLL();
        private UserRelationBLL userrelationBLL = new UserRelationBLL();
        private WFDelegateBLL delegateBLL = new WFDelegateBLL();
        #endregion

        #region Get
        public JqgridPageResponse<WFTaskModel> Page(JqgridPageRequest jqPage, int tasktype, UserModel loginInfo)
        {
            var page = new JqgridPageResponse<WFTaskModel>();
            if (tasktype == 1)  // 待办
            {
                var page1 = taskService.Page(jqPage, loginInfo);
                page = page1.Adapt<JqgridPageResponse<WFTaskModel>>();
            }
            else if (tasktype == 2) // 已办
            {
                var page1 = taskhistoryService.Page(jqPage, loginInfo);
                page = page1.Adapt<JqgridPageResponse<WFTaskModel>>();
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

        public JqgridPageResponse<WFTaskModel> MonitorPage(JqgridPageRequest jqPage)
        {
            var page = taskService.MonitorPage(jqPage);
            var page1 = page.Adapt<JqgridPageResponse<WFTaskModel>>();
            return page1;
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
                {"Scheme",  instance.schemejson },
                { "History",historys}
            };
            return ResponseResult.Success(dic);

        }
        #endregion

        #region Set
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

            var instanceEntity = new wf_flow_instanceEntity();
            instanceEntity = flow.Adapt<wf_flow_instanceEntity>();

            instanceEntity.process_id = processId;
            instanceEntity.processname = processName;
            instanceEntity.processcontent = processContent;

            // 单据提交人 后期 同公司 同部门 根据这个值计算 后期考虑作为入参 代为提交别人单据的情况 先不管
            instanceEntity.submit_user_id = loginInfo.user_id;
            instanceEntity.submit_username = loginInfo.realname;

            instanceEntity.Create(loginInfo);
            instanceEntity.schemejson = scheme.schemejson;

            var nodeModel = instanceEntity.schemejson.ToObject<WFSchemeModel>();
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
            historyStart = instanceEntity.Adapt<wf_taskhistoryEntity>();
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
            ProcessNodeInject(nodeStart, 1, "");

            var lines = nodeModel.lines.Where(r => r.from == nodeStart.id).ToList();
            if (lines.IsEmpty())
            {
                return ResponseResult.Fail("该流程的流程图开始节点没有连线，请联系管理员");
            }
            var turple = FindNextNode(lines, nodeModel.lines, nodeModel.nodes, instanceEntity, nodeStart, loginInfo);
            if (!turple.Item1.IsEmpty())
            {
                listTask.AddRange(turple.Item1);
            }
            if (!turple.Item2.IsEmpty())
            {
                listHistory.AddRange(turple.Item2);
            }
            if (!turple.Item3.IsEmpty())
            {
                listSql.AddRange(turple.Item3);
            }
            taskService.Create(instanceEntity, listTask, listHistory, listSql);
            return ResponseResult.Success();
        }

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
            if (task == null)
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
            ProcessNodeInject(nodeCurrent, result, opinion);

            bool IsContiune = false;
            // 如果是会签, 必须满足条件才进入下一节点
            if (result == 2)  // 拒绝 不管会签与否都到下一拒绝节点
            {
                IsContiune = true;
            }
            else  // 同意 判断会签条件
            {
                if (nodeCurrent.type == "confluencenode")  // 会签
                {
                    if (nodeCurrent.confluence_type == 1) // 全部
                    {
                        var countAgree = taskhistoryService.GetCount(r => r.task_id == task.task_id && r.node_id == task.node_id && r.result == 1);
                        var countAll = users.Count;
                        if (countAll == countAgree + 1)
                        {
                            IsContiune = true;
                        }
                    }
                    else if (nodeCurrent.confluence_type == 2) //任意一人
                    {
                        IsContiune = true;
                    }
                    if (nodeCurrent.confluence_type == 3) // 按比例
                    {
                        var countAgree = taskhistoryService.GetCount(r => r.task_id == task.task_id && r.node_id == task.node_id && r.result == 1);
                        var countAll = users.Count;
                        if ((countAgree + 1) / countAll * 100 > nodeCurrent.confluence_rate)
                        {
                            IsContiune = true;
                        }
                    }
                }
                else
                {
                    IsContiune = true;
                }
            }

            if (IsContiune)
            {
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
                    listTask.AddRange(turple.Item1);
                }
                if (!turple.Item2.IsEmpty())
                {
                    listHistory.AddRange(turple.Item2);
                }
                if (!turple.Item3.IsEmpty())
                {
                    listSql.AddRange(turple.Item3);
                }
                if (instance.is_finished == 1)
                {
                    task.is_finished = 1;
                }
            }
            taskService.Approve(instance, task, listTask, listHistory, listSql);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 加签
        /// </summary>
        /// <param name="taskId">当前流程</param>
        /// <param name="userId">加签人员</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ResponseResult AddUser(string taskId, string userId, UserModel loginInfo)
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
            var auth = new wf_task_authorizeEntity();
            auth.task_id = task.task_id;
            auth.user_id = userId;
            auth.is_add = 1;  // 加签审批
            auth.Create(loginInfo);

            var user = userBLL.GetEntityByCache(r => r.user_id == userId);

            var history = new wf_taskhistoryEntity();
            history = task.Adapt<wf_taskhistoryEntity>();
            history.opinion = $"{loginInfo.realname} 申请 {user.realname} 【加签】审批";
            history.Create(loginInfo);

            taskService.AddUser(auth, history);
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
            ProcessNodeInject(firstnode, 2, "");

            var history = new wf_taskhistoryEntity();
            history = firsttask.Adapt<wf_taskhistoryEntity>();
            history.result = 2;
            history.opinion = $"{loginInfo.realname} 强制结束流程";
            history.Create(loginInfo);

            taskService.Finish(instance, tasks, history, firstnode.sqlfail);
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
        private void ProcessNodeInject(WFSchemeNodeModel node, int result, string opinion)
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
                MethodInfo meth = tp.GetMethod("Approve");//加载方法
                meth.Invoke(process, new object[] { result, opinion });//执行
            }
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
        /// <param name="instanceEntity">流程实例</param>
        /// <param name="nodeCurrent">当前节点</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private Tuple<List<wf_taskEntity>, List<wf_taskhistoryEntity>, List<string>> FindNextNode(List<WFSchemeLineModel> nextlines, List<WFSchemeLineModel> alllines, List<WFSchemeNodeModel> allnodes, wf_flow_instanceEntity instanceEntity, WFSchemeNodeModel nodeCurrent, UserModel loginInfo)
        {
            var listTask = new List<wf_taskEntity>();
            var listHistory = new List<wf_taskhistoryEntity>();
            var listSql = new List<string>();
            foreach (var line in nextlines)
            {
                var currentLine = line;
                while (true)
                {
                    var nodeNext = allnodes.Where(r => r.id == currentLine.to).FirstOrDefault();
                    if (nodeNext.type == "endround") //结束
                    {     // 开始之后 直接结束  task 表不插值  history插入结束

                        var historyEnd = new wf_taskhistoryEntity();
                        historyEnd = instanceEntity.Adapt<wf_taskhistoryEntity>();
                        historyEnd.result = 0;
                        historyEnd.node_id = nodeNext.id;
                        historyEnd.nodename = nodeNext.name;
                        historyEnd.nodetype = nodeNext.type;
                        historyEnd.previous_id = nodeCurrent.id;
                        historyEnd.previousname = nodeCurrent.name;
                        historyEnd.opinion = "流程结束";
                        historyEnd.Create(loginInfo);
                        listHistory.Add(historyEnd);

                        instanceEntity.is_finished = 1;
                        break;
                    }
                    else if (nodeNext.type == "conditionnode") // 条件判断
                    {  // 开始之后 为条件判断节点  task 表不插值  history插入条件判断  需要递归 
                        var historyCondition = new wf_taskhistoryEntity();
                        historyCondition = instanceEntity.Adapt<wf_taskhistoryEntity>();
                        historyCondition.result = 0;
                        historyCondition.node_id = nodeNext.id;
                        historyCondition.nodename = nodeNext.name;
                        historyCondition.nodetype = nodeNext.type;
                        historyCondition.previous_id = nodeCurrent.id;
                        historyCondition.previousname = nodeCurrent.name;
                        historyCondition.Create(loginInfo);

                        var db = new DataAccess.Repository().db;
                        var sql = nodeNext.sqlcondition.Replace("@processId", $"{DataAccess.BaseConnection.ParaPre}processId");
                        var resultCondition = db.Ado.ExecuteNonQuery(sql, new { processId = instanceEntity.process_id });
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
                    {   // 开始之后 为执行节点  task 表不插值  history插入执行  需要递归 , 可能后面都是执行 知道结束    
                        listSql.Add(nodeNext.sqlsuccess);
                        currentLine = alllines.Where(r => r.from == nodeNext.id).FirstOrDefault();

                        ProcessNodeInject(nodeNext, 1, "");

                        var historyProcess = new wf_taskhistoryEntity();
                        historyProcess = instanceEntity.Adapt<wf_taskhistoryEntity>();
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
                    else  // 传阅 会签  一般审批 如果包含自己  自动通过
                    {
                        var taskEntity = new wf_taskEntity();
                        taskEntity = instanceEntity.Adapt<wf_taskEntity>();
                        taskEntity.node_id = nodeNext.id;
                        taskEntity.nodename = nodeNext.name;
                        taskEntity.previous_id = nodeCurrent.id;
                        taskEntity.nodetype = nodeNext.type;
                        taskEntity.previousname = nodeCurrent.name;
                        taskEntity.Create(loginInfo);

                        var turple = GetNextAuth(nodeNext, instanceEntity, taskEntity.task_id, loginInfo);
                        var listAuth = turple.Item1;
                        bool isContainSelf = turple.Item2;  // 下一步审批人是否包含自己

                        taskEntity.authrizes = listAuth;
                        if (isContainSelf)
                        {
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

                            currentLine = alllines.Where(r => r.from == nodeNext.id).FirstOrDefault();
                        }
                        else
                        {
                            listTask.Add(taskEntity);
                            break;
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
        /// <param name="instanceEntity"></param>
        /// <param name="task_id"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private Tuple<List<wf_task_authorizeEntity>, bool> GetNextAuth(WFSchemeNodeModel nodeNext, wf_flow_instanceEntity instanceEntity, string task_id, UserModel loginInfo)
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
                                auth.company_id = instanceEntity.company_id;
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.company_id == instanceEntity.company_id)
                                {
                                    isContainSelf = true;
                                }
                            }
                            else if (nodeAuth.objectrange == 2)
                            {
                                auth.department_id = instanceEntity.department_id;
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.department_id == instanceEntity.department_id)
                                {
                                    isContainSelf = true;
                                }
                            }
                            else if (nodeAuth.objectrange == 3)
                            {
                                auth.manage_dept_id = instanceEntity.department_id;
                                if (loginInfo.post_ids.Contains(objectId) && loginInfo.manage_dept_ids.Contains(instanceEntity.department_id))
                                {
                                    isContainSelf = true;
                                }
                            }
                            break;
                        case 5:
                            auth.role_id = objectId;
                            if (nodeAuth.objectrange == 1)
                            {
                                auth.company_id = instanceEntity.company_id;
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.company_id == instanceEntity.company_id)
                                {
                                    isContainSelf = true;
                                }

                            }
                            else if (nodeAuth.objectrange == 2)
                            {
                                auth.department_id = instanceEntity.department_id;
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.department_id == instanceEntity.department_id)
                                {
                                    isContainSelf = true;
                                }
                            }
                            else if (nodeAuth.objectrange == 3)
                            {
                                auth.manage_dept_id = instanceEntity.department_id;
                                if (loginInfo.role_ids.Contains(objectId) && loginInfo.manage_dept_ids.Contains(instanceEntity.department_id))
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
                    if (dele.flowcode == "ALL" && dele.flowcode.SplitNoEmpty(",").Contains(instanceEntity.flowcode))
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
