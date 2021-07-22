/**
 * 审批
 */
var approveClick, adduserClick, readClick, helpmeClick;
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

            layui.form.on('radio(result)', function (data) {
                if (data.value == 2) {
                    $("#divReturn").show();
                    $("#opinion").attr("lay-verify", "required");
                }
                else {
                    $("#divReturn").hide();
                    $("#opinion").removeAttr("lay-verify");
                }
            });

            $("#AnnexName").initFileInput({
                folderPre: "WFApprove"
            });
            $("#AnnexName1").initFileInput({
                folderPre: "WFApprove"
            });

        },
        initLogGrid: function (data) {
            var gridLog = $("#gridLog").jqGrid({
                //url: luckyu.rootUrl + '/WorkFlowModule/Task/GetWorkflowLog',
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
                            var result = luckyu.utility.toEnum(cellvalue, luckyu_staticdata.wf_resultshow);
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
            gridLog.setGridWidth(window.innerWidth - 40);
            $(window).resize(function () {
                gridLog.setGridHeight(window.innerHeight - 120);
                gridLog.setGridWidth(window.innerWidth - 40);
            });
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
                    this.onload = !!iframeWindow.loaded ? iframeWindow.loaded() : function () {
                        $("input[type!='file'],textarea", iframeWindow.document).attr("disabled", "disabled");
                        setTimeout(function () {
                            $("button[title=删除文件]", iframeWindow.document).hide();
                            $("div.input-group.file-caption-main", iframeWindow.document).hide();
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
                $('#flow').dfworkflowSet('set', { data: shceme });
            });

        }
    };
    page.init();

    approveClick = function (layerIndex, callBack) {

        // 这里有个难点 ，可能有多个iframe页，但每个页面save方法也可能是异步的，必须每个都验证通过最后材质执行审批动作，
        // 例如一个页面必填项未填写，则不走审批
        // 暂时没想好
        var flag = true;
        $("#tabForms>.layui-tab-content iframe").each(function () {
            var iframeWindow = this.contentWindow;
            if (!!iframeWindow.approveSave) {
                flag = iframeWindow.approveSave();
            }
        });

        // 验证通过 才执行审批，必填项等
        if (flag) {
            layui.layer.open({
                type: 1,
                title: '审核',
                content: $('#divApprove'),
                area: ['800px', '500px'], //宽高
                btn: ['确定'],
                yes: function (index, layero) {
                    if (!$("#divApprove").verifyForm()) {
                        return false;
                    }
                    var result = $("input[name=result]:checked").val();
                    var opinion = $("#opinion").val();
                    var returnType = $("input[name=returnType]:checked").val();
                    luckyu.ajax.postv2('/WorkflowModule/Task/Approve', {
                        taskId: taskId,
                        result: result,
                        opinion: opinion,
                        returnType: returnType,
                    }, function (data) {
                        if (!!callBack) {
                            callBack();
                        }
                        var history_id = data.Item3[0].history_id;
                        $("#AnnexName").uploadFile({
                            exId: history_id,
                            callback: function () {
                                parent.layui.layer.close(layerIndex);
                            }
                        });
                    });
                },
                success: function (layero, index) {
                    $("div.layui-layer-btn", parent.document).hide();
                },
                cancel: function (index, layero) {
                    $("div.layui-layer-btn", parent.document).show();
                }
            });
        }
    };
    readClick = function (layerIndex, callBack) {
        layui.layer.open({
            type: 1,
            title: '审核',
            content: $('#divRead'),
            area: ['800px', '500px'], //宽高
            btn: ['确定'],
            yes: function (index, layero) {
                if (!$("#divRead").verifyForm()) {
                    return false;
                }
                var opinion = $("#opinion1").val();
                luckyu.ajax.postv2('/WorkflowModule/Task/Approve', {
                    taskId: taskId,
                    result: 4,
                    opinion: opinion,
                }, function (data) {
                    if (!!callBack) {
                        callBack();
                    }
                    var history_id = data.Item3[0].history_id;
                    $("#AnnexName1").uploadFile({
                        exId: history_id,
                        callback: function () {
                            parent.layui.layer.close(layerIndex);
                        }
                    });
                });

            },
            success: function (layero, index) {
                $("div.layui-layer-btn", parent.document).hide();
            },
            cancel: function (index, layero) {
                $("div.layui-layer-btn", parent.document).show();
            }
        });

    };
    adduserClick = function (layerIndex, callBack) {
        luckyu.layer.userSelectForm({
            multiple: true,
            callback: function (userlist) {
                var userIds = userlist.map(r => r.userId);
                var usernames = userlist.map(r => r.realname).join(",");
                luckyu.layer.layerConfirm("确定邀请以下用户加签审批？<br />" + usernames, function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/AddUser', { taskId: taskId, userIds: userIds, remark: '' }, function (data) {
                        if (!!callBack) {
                            callBack();
                        }
                        parent.layui.layer.close(layerIndex);
                    });
                });
            }
        });
    };
    helpmeClick = function (layerIndex, callBack) {
        luckyu.layer.userSelectForm({
            multiple: true,
            callback: function (userlist) {
                var userIds = userlist.map(r => r.userId);
                var usernames = userlist.map(r => r.realname).join(",");
                luckyu.layer.layerConfirm("确定邀请以下用户协办审批？<br />" + usernames, function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/HelpMe', { taskId: taskId, userIds: userIds, remark: '' }, function (data) {
                        if (!!callBack) {
                            callBack();
                        }
                        parent.layui.layer.close(layerIndex);
                    });
                });
            }
        });
    }
};