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
            $('#divtimeline').height(window.innerHeight - 90);
            window.onresize = function () {
                $('#flow').height(window.innerHeight - 120);
                $('#divtimeline').height(window.innerHeight - 90);
            };

            $("#AnnexName").initFileInput({
                folderPre: "WFApprove"
            });
            $("#AnnexName1").initFileInput({
                folderPre: "WFApprove"
            });

            // 选择用户
            $("#usernames,#spanuserselect").click(function () {
                luckyu.layer.userSelectForm({
                    multiple: true,
                    callback: function (userlist) {
                        var userids = userlist.map(r => r.userId).join(",");
                        var usernames = userlist.map(r => r.realname).join(",");
                        $("#userids").val(userids);
                        $("#usernames").val(usernames);
                    }
                });
            });
        },
        saveFrame: function () {
            // 这里有个难点 ，可能有多个iframe页，但每个页面save方法也可能是异步的，必须每个都验证通过最后才执行审批动作，
            // 例如后台验证一个页面必填项未填写，则不走审批
            // 暂时没想好
            var flag = true;
            $("#tabForms>.layui-tab-content iframe").each(function () {
                var iframeWindow = this.contentWindow;
                if (!!iframeWindow.approveSave) {
                    flag = iframeWindow.approveSave();
                }
            });
            return flag;
        },
        getAuthors: function () {
            var nextnode = $("input[name=nextnode]");
            var authors = {};
            if (nextnode.length > 0) {
                var checknodes = $("input[name=nextnode]:checked");
                if (checknodes.length < 1) {
                    $("#tabNext").click();
                    layui.layer.alert("请选择下一步节点", { icon: 2 });
                    return false;
                }
                authors[checknodes[0].value] = [];
            }
            var nextusers = $("input[lay-filter=nodeuser]");
            if (nextusers.length > 0) {
                var checkusers = $("input[lay-filter=nodeuser]:checked");
                if (checkusers.length < 1) {
                    $("#tabNext").click();
                    layui.layer.alert("请选择下一步用户", { icon: 2 });
                    return false;
                }
                for (var i = 0; i < checkusers.length; i++) {
                    var $chk = $(checkusers);
                    var nodeid = $chk.attr("nodeid");
                    if (!authors[nodeid]) {
                        authors[nodeid] = [];
                        authors[nodeid].push({
                            Key: $chk.val(),
                            Value: $chk.attr("title")
                        });
                    }
                    else {
                        authors[nodeid].push({
                            Key: $chk.val(),
                            Value: $chk.attr("title")
                        });
                    }
                }
            }
            return authors;
        },
        initData: function () {
            luckyu.ajax.getv2(luckyu.rootUrl + '/WorkflowModule/Task/GetFormData', { instanceId: instanceId, taskId: taskId, historyId: historyId }, function (data) {
                var htmlTab = '';
                var htmlIframe = '';
                // 表单
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

                // 历史记录
                var htmlHistory = '';
                for (var i = 0; i < data.History.length; i++) {
                    var his = data.History[i];
                    htmlHistory += '\
<li class="layui-timeline-item">\
    <i class="layui-icon layui-timeline-axis">&#xe63f;</i>\
    <div class="layui-timeline-content layui-text">\
        <h3 class="layui-timeline-title">'+ luckyu.utility.toEnum(his.result, luckyu_staticdata.wf_resultshow) + ' ' + his.nodename + '</h3>\
        <p>';
                    if (his.result != 0) {
                        htmlHistory += "审批人：" + his.create_username;

                        if (his.result != 100) {
                            htmlHistory += "<br/>提交时间：" + new Date(his.tasktime).format("yyyy-MM-dd HH:mm")
                            htmlHistory += "<br/>办结时间：" + new Date(his.apptime).format("yyyy-MM-dd HH:mm")
                            htmlHistory += (!his.opinion ? '' : ('<br/>意见建议：' + his.opinion));
                        }
                        if (!!his.annex && his.annex != '[]') {
                            var list = JSON.parse(his.annex);
                            htmlHistory += "<br/>附件：";
                            for (var j = 0; j < list.length; j++) {
                                htmlHistory += ' <br/><a style="color:blue;cursor:pointer;text-decoration:underline;" href="/SystemModule/Annex/ShowFile?keyValue=' + list[j].Key + '" target="_blank"> ' + list[j].Value + '</a>';
                            }
                        }
                    }

                    htmlHistory += '</p></div></li>';
                }
                $("#ultimeline").html(htmlHistory);

                // 流程图
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

                // 下一步
                if (!!data.NextNodes) {
                    var html = '';
                    for (var i = 0; i < data.NextNodes.length; i++) {
                        var nextnode = data.NextNodes[i];
                        var html_nodename = nextnode.name;
                        if (data.CurrentNode.comfirm_node == 1) {
                            html_nodename = '<input type="checkbox" id="node_' + nextnode.id + '" name="nextnode" lay-filter="nextnode" value="' + nextnode.id + '" title="' + nextnode.name + '"  />';
                        }
                        else if (data.CurrentNode.comfirm_node == 2) {
                            html_nodename = '<input type="radio" id="node_' + nextnode.id + '" name="nextnode" lay-filter="nextnode" value="' + nextnode.id + '" title="' + nextnode.name + '"  />';
                        }
                        html += '<div class="layui-card"><div class="layui-card-header">' + html_nodename + '</div>';
                        html += '<div class="layui-card-body">';
                        for (var j = 0; j < nextnode.authusers.length; j++) {
                            var authuser = nextnode.authusers[j];
                            var html_username = authuser.objectnames;
                            if (nextnode.comfirm_user == 1) {
                                html_username = '<input type="' + (nextnode.user_num == 1 ? "radio" : "checkbox") + '" id="user_' + authuser.objectids + '" name="nodeuser_' + nextnode.id + '" nodeid="' + nextnode.id + '" lay-filter="nodeuser" value="' + authuser.objectids + '" title="' + authuser.objectnames + '" lay-skin="primary" disabled  />';
                            }
                            html += '<div class="layui-col-xs4">' + html_username + '</div>';
                        }
                        html += '</div></div>';
                    }
                    $("#divNext").html(html);
                    layui.form.render();

                    layui.form.on('radio(nextnode)', function (data) {
                        $('input[lay-filter=nodeuser]').prop('checked', false).attr("disabled", "disabled");
                        $('input[name=' + "nodeuser_" + data.value + ']').removeAttr("disabled");
                        layui.form.render();
                    });
                }
            });
        },
    };
    page.init();

    approveClick = function (layerIndex, callBack) {

        var flag = page.saveFrame();
        // 验证通过 才执行审批，必填项等
        if (flag) {
            luckyu.layer.layerHtml({
                title: '审核',
                content: $('#divApprove'),
                height: 500,
                width: 800,
                btn: [{
                    name: "确认",
                    callback: function (index, layero) {
                        if (!$("#divApprove").verifyForm()) {
                            return false;
                        }

                        var result = $("input[name=result]:checked").val();
                        var opinion = $("#opinion").val();
                        var authors = page.getAuthors();
                        luckyu.ajax.postv2('/WorkflowModule/Task/Approve', {
                            taskId: taskId,
                            result: result,
                            opinion: opinion,
                            authors: authors,
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
                    }
                }],
                success: function (layero, index) {
                    $("div.layui-layer-btn", parent.document).hide();
                },
                cancel: function (index, layero) {
                    $("div.layui-layer-btn", parent.document).show();
                },
                end: function () {
                    $("div.layui-layer-btn", parent.document).show();
                }
            });
        }
    };
    readClick = function (layerIndex, callBack) {
        $("#divResult").hide();

        luckyu.layer.layerHtml({
            title: '确认',
            content: $('#divApprove'),
            height: 500,
            width: 800,
            btn: [{
                name: "确定",
                callback: function (index, layero) {
                    if (!$("#divApprove").verifyForm()) {
                        return false;
                    }
                    var opinion = $("#opinion").val();
                    var authors = page.getAuthors();
                    luckyu.ajax.postv2('/WorkflowModule/Task/Approve', {
                        taskId: taskId,
                        opinion: opinion,
                        authors: authors
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
                }
            }],
            success: function (layero, index) {
                $("div.layui-layer-btn", parent.document).hide();
            },
            cancel: function (index, layero) {
                $("div.layui-layer-btn", parent.document).show();
            }
        });
    };
    adduserClick = function (layerIndex, callBack) {
        luckyu.layer.layerHtml({
            title: '邀请会签办理',
            content: $('#divAddUser'),
            height: 500,
            width: 800,
            btn: [{
                name: "确定",
                callback: function (index, layero) {
                    if (!$("#divAddUser").verifyForm()) {
                        return false;
                    }

                    var remark = $("#opinion1").val();
                    var userIds = $("#userids").val();
                    var usernames = $("#usernames").val();

                    luckyu.layer.layerConfirm("确定邀请以下用户会签办理审批？<br />" + usernames, function () {
                        luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/AddUser', { taskId: taskId, userIds: userIds.split(','), remark: remark }, function (data) {
                            if (!!callBack) {
                                callBack();
                            }
                            var history_id = data;
                            $("#AnnexName1").uploadFile({
                                exId: history_id,
                                callback: function () {
                                    parent.layui.layer.close(layerIndex);
                                }
                            });
                        });
                    });
                }
            }],
            success: function (layero, index) {
                $("div.layui-layer-btn", parent.document).hide();
            },
            cancel: function (index, layero) {
                $("div.layui-layer-btn", parent.document).show();
            }
        });
    };
    helpmeClick = function (layerIndex, callBack) {
        luckyu.layer.layerHtml({
            title: '转发查看',
            content: $('#divAddUser'),
            height: 500,
            width: 800,
            btn: [{
                name: "确定",
                callback: function (index, layero) {
                    if (!$("#divAddUser").verifyForm()) {
                        return false;
                    }
                    var remark = $("#opinion1").val();
                    var userIds = $("#userids").val();
                    var usernames = $("#usernames").val();

                    luckyu.layer.layerConfirm("确定转发给以下用户查看？<br />" + usernames, function () {
                        luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Task/HelpMe', { taskId: taskId, userIds: userIds.split(','), remark: remark }, function (data) {
                            if (!!callBack) {
                                callBack();
                            }
                            var history_id = data;
                            $("#AnnexName1").uploadFile({
                                exId: history_id,
                                callback: function () {
                                    parent.layui.layer.close(layerIndex);
                                }
                            });
                        });
                    });
                }
            }],
            success: function (layero, index) {
                $("div.layui-layer-btn", parent.document).hide();
            },
            cancel: function (index, layero) {
                $("div.layui-layer-btn", parent.document).show();
            }
        });
    }
};