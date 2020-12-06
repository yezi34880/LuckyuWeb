/**
 * 调整流程
 */
var bootstrap = function (layui) {

    var instanceId = request("instanceId");
    var page = {
        init: function () {
            page.bind();
            page.initData();
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

            $('#flow').height(window.innerHeight - 120);
            $('#flowLast').height(window.innerHeight - 80);
            $(window).resize(function () {
                $('#flow').height(window.innerHeight - 120);
                $('#flowLast').height(window.innerHeight - 80);
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