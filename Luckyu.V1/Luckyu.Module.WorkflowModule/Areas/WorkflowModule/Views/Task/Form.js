/**
 * 审批
 */
var approveClick, adduserClick, readClick, finishClick;
var bootstrap = function (layui) {

    var taskId = request("taskId");
    var instanceId = request("instanceId");
    var historyId = request("historyId");
    var processId = request("processId");

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {

            $('#flow').dfworkflow({
                isPreview: true,
                openNode: function (node) {

                }
            });

            $('#flow').height(window.innerHeight - 120);
            window.onresize = function () {
                $('#flow').height(window.innerHeight - 120);
            };

        },
        initLogGrid: function (data) {
            var gridLog = $("#gridLog").jqGrid({
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
                            var result = luckyu.utility.toEnum(cellvalue,
                                [
                                    { value: 1, name: '<span class="label label-success">通过</span>' },
                                    { value: 2, name: '<span class="label label-error">驳回</span>' },
                                    { value: -1, name: '<span class="label label-info">正在审批</span>' },
                                ]
                            );
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
                    for (var i = rows.length - 1; i > -1; i--) {
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
                    $("#timeline").html(html);
                }
            });

            gridLog.setGridHeight(window.innerHeight - 120);
            gridLog.setGridWidth(window.innerWidth / 12 * 9 - 40);
            window.onresize = function () {
                gridLog.setGridHeight(window.innerHeight - 120);
                gridLog.setGridWidth(window.innerWidth / 12 * 9 - 40);
            };
        },
        initData: function () {
            luckyu.ajax.getv2(luckyu.rootUrl + '/WorkflowModule/Task/GetFormData', { instanceId: instanceId, taskId: taskId, historyId: historyId }, function (data) {
                var htmlTab = '';
                var htmlIframe = '';
                for (var i = 0; i < data.ShowNode.forms.length; i++) {
                    var form = data.ShowNode.forms[i];
                    var formurl = form.formurl;
                    if (formurl.indexOf("?") < 0) {
                        formurl += '?keyValue=' + data.Instance.process_id;
                    }
                    else {
                        formurl += '&keyValue=' + data.Instance.process_id;
                    }
                    if (i === 0) {
                        htmlTab += '<li class="layui-this">' + form.formname + '</li>';
                        htmlIframe += '<div class="layui-tab-item layui-show"><iframe src="' + formurl + '"  frameborder="no" border="0"></iframe></div>';
                    }
                    else {
                        htmlTab += '<li>' + form.formname + '</li>';
                        htmlIframe += '<div class="layui-tab-item"><iframe src="' + formurl + '" frameborder="no" border="0"></iframe></div>';
                    }
                }
                $("#tabForms>.layui-tab-title").html(htmlTab);
                $("#tabForms>.layui-tab-content").html(htmlIframe);
                $("#tabForms>.layui-tab-content iframe").each(function () {
                    var iframeWindow = this.contentWindow;
                    this.onload = !!iframeWindow.load ? iframeWindow.load() : function () {
                        $("input[type!='file'],textarea", iframeWindow.document).attr("disabled", "disabled");
                        $("button[title=删除文件]").attr("disabled", "disabled");
                        setTimeout(function () {
                            $(".xm-select", iframeWindow.document).each(function () {
                                var id = $(this).attr("id");
                                var xm = iframeWindow.xmSelect.get("#" + id, true);
                                if (!!xm) {
                                    xm.update({ disabled: true });
                                }
                            });
                        }, 500);
                    };
                });

                page.initLogGrid(data.History);
                var shceme = JSON.parse(data.Scheme);

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
                $('#flow').dfworkflowSet('set', { data: shceme });
            });

        }
    };
    page.init();

    approveClick = function (layerIndex, callBack) {
        luckyu.layer.layerFormTop({
            id: "FormApp",
            title: "审核",
            width: 600,
            height: 400,
            url: luckyu.rootUrl + "/WorkflowModule/Task/ApproveForm",
            btn: [{
                name: "确定",
                callback: function (index, layero) {
                    var res = layero.find("iframe")[0].contentWindow.saveClick(index);
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/Approve', { taskId: taskId, result: res.result, opinion: res.opinion }, function (data) {
                        if (!!callBack) {
                            callBack();
                        }
                        parent.layui.layer.close(layerIndex);
                    });
                }
            }, {
                name: "取消",
                callback: function (index, layero) {

                }
            }]
        });
    };
    adduserClick = function (layerIndex, callBack) {
        luckyu.layer.userSelectForm({
            multiple: false,
            callback: function (userlist) {
                var userId = userlist[0];
                var username = luckyu.clientdata.getUserName(userId);
                luckyu.layer.layerConfirm("确定邀请 " + username + " 加签审批？", function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/AddUser', { taskId: taskId, userId: userId }, function (data) {
                        if (!!callBack) {
                            callBack();
                        }
                        parent.layui.layer.close(layerIndex);
                    });
                });
            }
        });
    };
    readClick = function (layerIndex, callBack) {
        luckyu.layer.layerConfirm("确定已阅？", function () {
            luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/Approve', { taskId: taskId, result: 0, opinion: '' }, function (data) {
                if (!!callBack) {
                    callBack();
                }
                parent.layui.layer.close(layerIndex);
            });
        });
    };
    finishClick = function (layerIndex, callBack) {
        luckyu.layer.layerConfirm("确定终止该流程？", function () {
            luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Monitor/Finish', { instanceId: instanceId }, function (data) {
                if (!!callBack) {
                    callBack();
                }
                parent.layui.layer.close(layerIndex);
            });
        });

    };
};