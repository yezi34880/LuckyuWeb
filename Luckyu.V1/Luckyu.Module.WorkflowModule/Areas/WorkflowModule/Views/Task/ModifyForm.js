/**
 * 调整流程
 */
var bootstrap = function (layui) {

    var instanceId = request("instanceId");
    var page = {
        init: function () {
            page.bind();
            page.initData();
            page.initGridHelpMe();
            page.initGridAddUser();
            page.initGridHistory();
        },
        bind: function () {

            $('#flow').dfworkflow({
                isPreview: true,
                openNode: function (node) {
                    switch (node.type) {
                        case "startround":
                            layui.notice.error("不能调整到开始节点");
                            return;
                        case "endround":
                            layui.notice.error("不能调整到结束节点");
                            return;
                        case "conditionnode":
                            layui.notice.error("不能调整到条件节点");
                            return;
                        case "processnode":
                            layui.notice.error("不能调整到执行节点");
                            return;
                    }
                    if (node.state === '0') {
                        layui.notice.error("当前节点正在审批，无需调整");
                        return;
                    }

                    luckyu.layer.layerConfirm("确定调整该流程到节点 " + node.name + " ？", function () {
                        luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Monitor/Modify', { instanceId: instanceId, schemeId: '', nodeId: node.id }, function (data) {
                            layui.notice.success("操作成功");
                            var layerIndex = parent.layer.getFrameIndex(window.name);
                            parent.layer.close(layerIndex);
                        });
                    });
                }
            });
            $('#flowLast').dfworkflow({
                isPreview: true,
                openNode: function (node) {
                    switch (node.type) {
                        case "startround":
                            layui.notice.error("不能调整到开始节点");
                            return;
                        case "endround":
                            layui.notice.error("不能调整到结束节点");
                            return;
                        case "conditionnode":
                            layui.notice.error("不能调整到条件节点");
                            return;
                        case "processnode":
                            layui.notice.error("不能调整到执行节点");
                            return;
                    }
                    if (node.state === '0') {
                        layui.notice.error("当前节点正在审批，无需调整");
                        return;
                    }

                    var scheme_id = $('#flowLast').attr("luckyu-schemeid");
                    luckyu.layer.layerConfirm("确定调整该流程到最新版本，并调整节点到 " + node.name + " ？", function () {
                        luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Monitor/Modify', { instanceId: instanceId, schemeId: scheme_id, nodeId: node.id }, function (data) {
                            layui.notice.success("操作成功");
                            var layerIndex = parent.layer.getFrameIndex(window.name);
                            parent.layer.close(layerIndex);
                        });
                    });
                }
            });

            $('#flow').height(window.innerHeight - 185);
            $('#flowLast').height(window.innerHeight - 145);
            $(window).resize(function () {
                $('#flow').height(window.innerHeight - 185);
                $('#flowLast').height(window.innerHeight - 145);
            });

        },
        initGridHelpMe: function () {
            var gridHelpMe = $("#gridHelpMe").jqGrid({
                url: luckyu.rootUrl + '/WorkFlowModule/Task/GetHelpMePage',
                datatype: "json",
                postData: { instanceId: instanceId },
                colModel: [
                    { name: "task_id", hidden: true, key: true },
                    { label: "节点名称", name: "nodename", width: 80, },
                    { label: "操作用户", name: "create_username", width: 80, },
                    {
                        label: "操作", name: "_action", width: 100, formatter: function (cellvalue, option, rowObject) {
                            var html = '<a style="cursor:pointer;color:red;" onclick="deleteHelpMe(\'' + rowObject.task_id + '\')">删除</a>';
                            return html;
                        }
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                altRows: true,//隔行换色
            });

            window.deleteHelpMe = function (taskId) {
                luckyu.layer.layerConfirm("确定删除该协办任务？", function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/DeleteTask', { taskId: taskId }, function (data) {
                        layui.notice.success("操作成功");
                        gridHelpMe.trigger("reloadGrid");
                        //var layerIndex = parent.layer.getFrameIndex(window.name);
                        //parent.layer.close(layerIndex);
                    });
                });
            };

            gridHelpMe.setGridWidth(window.innerWidth - 40);
            $(window).resize(function () {
                gridHelpMe.setGridWidth(window.innerWidth - 40);
            });
        },
        initGridAddUser: function () {
            var gridAddUser = $("#gridAddUser").jqGrid({
                url: luckyu.rootUrl + '/WorkFlowModule/Task/GetAddUserPage',
                datatype: "json",
                postData: { instanceId: instanceId },
                colModel: [
                    { name: "auth_id", hidden: true, key: true },
                    { name: "task_id", hidden: true, },
                    { label: "节点名称", name: "nodename", width: 80, },
                    { label: "操作用户", name: "username", width: 80, },
                    {
                        label: "操作", name: "_action", width: 100, formatter: function (cellvalue, option, rowObject) {
                            var html = '<a style="cursor:pointer;color:red;" onclick="deleteAddUser(\'' + rowObject.auth_id + '\')">删除</a>';
                            return html;
                        }
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                altRows: true,//隔行换色
            });

            window.deleteAddUser = function (authId) {
                luckyu.layer.layerConfirm("确定删除该代办用户 ？", function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/DeleteTaskAuth', { authId: authId }, function (data) {
                        layui.notice.success("操作成功");
                        gridAddUser.trigger("reloadGrid");
                        //var layerIndex = parent.layer.getFrameIndex(window.name);
                        //parent.layer.close(layerIndex);
                    });
                });
            };

            gridAddUser.setGridWidth(window.innerWidth - 40);
            $(window).resize(function () {
                gridAddUser.setGridWidth(window.innerWidth - 40);
            });
        },
        initGridHistory: function () {
            var gridHistory = $("#gridHistory").jqGrid({
                url: luckyu.rootUrl + '/WorkFlowModule/Task/GetTaskLog',
                datatype: "json",
                postData: { instanceId: instanceId },
                colModel: [
                    { name: "history_id", hidden: true },
                    { label: "节点名称", name: "nodename", width: 80, },
                    { label: "操作用户", name: "create_username", width: 80, },
                    {
                        label: "操作时间", name: "createtime", width: 100, align: "right",
                        formatter: "date",
                        formatoptions: { newformat: 'Y-m-d H:i:s' },
                    },
                    {
                        label: "结果", name: "result", width: 60, align: "center",
                        formatter: function (cellvalue, options, rowObject) {
                            var result = luckyu.utility.toEnum(cellvalue, luckyu_staticdata.wf_resultshow);
                            return result;
                        }
                    },
                    { label: "备注", name: "opinion", width: 350, },
                    {
                        label: "操作", name: "_action", width: 100, formatter: function (cellvalue, option, rowObject) {
                            var html = '';
                            if (!!rowObject.history_id) {
                                html = '<a style="cursor:pointer;color:red;" onclick="deleteHistory(\'' + rowObject.history_id + '\')">删除</a>';
                            }
                            return html;
                        }
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                altRows: true,//隔行换色
            });

            window.deleteHistory = function (history_id) {
                luckyu.layer.layerConfirm("确定删除该审批记录吗 ？", function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/DeleteHistory', { historyId: history_id }, function (data) {
                        layui.notice.success("操作成功");
                        gridHistory.trigger("reloadGrid");
                        //var layerIndex = parent.layer.getFrameIndex(window.name);
                        //parent.layer.close(layerIndex);
                    });
                });
            };
            gridHistory.setGridHeight(window.innerHeight - 200);
            gridHistory.setGridWidth(window.innerWidth - 40);
            $(window).resize(function () {
                gridHistory.setGridWidth(window.innerWidth - 40);
                gridHistory.setGridHeight(window.innerHeight - 200);
        });

        },
        initData: function () {
            luckyu.ajax.getv2(luckyu.rootUrl + '/WorkflowModule/Task/GetTaskScheme', { instanceId: instanceId }, function (data) {
                var shceme = JSON.parse(data.Instance.schemejson);
                for (var i = 0, l = shceme.nodes.length; i < l; i++) {
                    var node = shceme.nodes[i];
                    if (node.type === "startround" || node.type === "endround") {
                        continue;
                    }
                    node.state = '3';
                    if (data.CurrentNode.id == node.id) {
                        node.state = '0';
                    }
                }
                $('#flow').dfworkflowSet('set', { data: shceme });
                var shcemeLast = JSON.parse(data.LastScheme.schemejson);
                $('#flowLast').dfworkflowSet('set', { data: shcemeLast });
                $('#flowLast').attr("luckyu-schemeid", data.LastScheme.scheme_id);
            });

        }
    };
    page.init();
};