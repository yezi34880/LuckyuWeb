/**
 *  审批日志
 */
var bootstrap = function (layui) {

    var page = {
        init: function () {
            page.initData();
        },
        initLogGrid: function (instanceId, data) {
            var gridLog = $('#gridLog_' + instanceId).jqGrid({
                url: luckyu.rootUrl + '/WorkFlowModule/Task/GetWorkflowLog',
                //datatype: "json",
                //postData: { processId: processId, instanceId: instanceId },
                datatype: "local",
                data: data,
                colModel: [
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
                            var result = luckyu.utility.toEnum(cellvalue, luckyu.workflowapi.resultshow);
                            return result;
                        }
                    },
                    { label: "备注", name: "opinion", width: 350, },
                    {
                        label: "附件", name: "annex", width: 100,
                        formatter: function (cellvalue, options, rowobject) {
                            var result = '';
                            if (!!cellvalue) {
                                var list = JSON.parse(cellvalue);
                                for (var i = 0; i < list.length; i++) {
                                    result += ' <a style="color:blue;cursor:pointer;text-decoration:underline;" href="/SystemModule/Annex/ShowFile?keyValue=' + list[i].Key + '" target="_blank"> ' + list[i].Value + '</a>';
                                }
                            }
                            return result;
                        }
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                altRows: true,//隔行换色
            });

            gridLog.setGridHeight(window.innerHeight - 120);
            gridLog.setGridWidth(window.innerWidth - 150);
            $(window).resize(function () {
                gridLog.setGridHeight(window.innerHeight - 120);
                gridLog.setGridWidth(window.innerWidth  - 150);
            });
        },
        initData: function () {
            $("li[luckyu-id]").each(function (item, index) {
                var self = $(this);
                var instanceId = self.attr("luckyu-id");
                luckyu.ajax.getv2(luckyu.rootUrl + '/WorkflowModule/Task/GetFormData', { instanceId: instanceId }, function (data) {
                    page.initLogGrid(instanceId, data.History);
                    var shceme = JSON.parse(data.Instance.schemejson);

                    for (var i = 0, l = shceme.nodes.length; i < l; i++) {
                        var node = shceme.nodes[i];
                        if (node.type === "startround" || node.type === "endround") {
                            continue;
                        }
                        node.state = '3';
                        var history = data.History.filter(r => r.node_id === node.id);
                        if (!!history && history.length > 0) {
                            if (history[0].result == 1) {
                                node.state = '1';
                            }
                            else if (history[0].result == 2) {
                                node.state = '2';
                            }
                        }
                        if (data.CurrentNode.id == node.id) {
                            node.state = '0';
                        }
                    }

                    $('#flow_' + instanceId).dfworkflow({
                        isPreview: true,
                        openNode: function (node) {

                        }
                    });

                    $('#flow_' + instanceId).parent().height(window.innerHeight - 105).width(window.innerWidth - 155);
                    $(window).resize(function () {
                        $('#flow_' + instanceId).parent().height(window.innerHeight - 105).width(window.innerWidth - 155);
                    });
                    $('#flow_' + instanceId).dfworkflowSet('set', { data: shceme });
                });

            });
        },
    };
    page.init();

};