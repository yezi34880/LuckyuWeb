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
                    {
                        label: "结果", name: "result", width: 60, align: "center",
                        formatter: function (cellvalue, options, rowObject) {
                            var result = luckyu.utility.toEnum(cellvalue, luckyu.workflowapi.resultshow);
                            return result;
                        }
                    },
                    { label: "操作用户", name: "create_username", width: 80, },
                    {
                        label: "操作时间", name: "createtime", width: 100, align: "right",
                        formatter: "date",
                        formatoptions: { newformat: 'Y-m-d H:i:s' },
                    },
                    { label: "备注", name: "opinion", width: 350, },
                ],
                rownumbers: true,
                viewrecords: true,
                altRows: true,//隔行换色
                gridComplete: function () {
                    var rows = this.p.rawData;
                    var html = '';
                    for (var i = 0; i < rows.length; i++) {
                        var row = rows[i];
                        var style = luckyu.utility.toEnum(row.result,
                            [
                                { value: 1, name: 'color:green;' },
                                { value: 2, name: 'color:red;' },
                                { value: -1, name: 'color:rgb(91, 192, 222);' },
                            ]
                        );
                        var result = luckyu.utility.toEnum(row.result,
                            [
                                { value: 1, name: '通过' },
                                { value: 2, name: '驳回' },
                                { value: -1, name: '正在审批' },
                            ]
                        );
                        html += ' \
<li class="layui-timeline-item">\
    <i class="layui-icon layui-timeline-axis" style="'+ style + '">&#xe63f;</i>\
    <div class="layui-timeline-content layui-text">\
        <h3 class="layui-timeline-title">'+ (row.nodename + ' ' + new Date(row.createtime).format("yyyy-MM-dd HH:mm:ss")) + '</h3>\
        <p>\
         '+ (result + ' ' + row.create_username) + '<br />\
        '+ row.opinion + '\
        </p>\
    </div>\
</li>';
                    }
                    $('#timeline_' + instanceId).html(html);
                }
            });

            gridLog.setGridHeight(window.innerHeight - 120);
            gridLog.setGridWidth((window.innerWidth - 70) / 12 * 9 - 60);
            $(".divline").height(window.innerHeight - 80);
            $(window).resize(function () {
                gridLog.setGridHeight(window.innerHeight - 120);
                gridLog.setGridWidth((window.innerWidth - 70) / 12 * 9 - 60);
                $(".divline").height(window.innerHeight - 80);
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

                    $('#flow_' + instanceId).parent().height(window.innerHeight - 120).width(window.innerWidth - 170);
                    $(window).resize(function () {
                        $('#flow_' + instanceId).parent().height(window.innerHeight - 120).width(window.innerWidth - 170);
                    });
                    $('#flow_' + instanceId).dfworkflowSet('set', { data: shceme });
                });

            });
        },
    };
    page.init();

};