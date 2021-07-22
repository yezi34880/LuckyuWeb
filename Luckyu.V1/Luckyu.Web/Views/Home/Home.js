layui.config({
    base: '/lib/layuiadmin/' //静态资源所在路径
}).extend({
    notice: "modules/notice",
}).use(["notice"], function () {
    luckyu.notice.init();

    var page = {
        init: function () {
            page.bind();
            page.refrash();
        },
        bind: function () {
            if (!!top.layui.element) {
                top.layui.element.on('tab(layadmin-layout-tabs)', function (data) {
                    var id = $(this).attr("lay-id");
                    if (!!id && id === "home") {
                        page.refrash();
                    }
                });
            }

            $("#moretask").click(function () {
                top.layui.index.openTabsPage('/WorkflowModule/Task/Index', '我的任务');
            });
            $("#newsMore").click(function () {
                top.layui.index.openTabsPage('/OAModule/News/ShowIndex', '公告通知');
            });
            $("#moretaskDelegate").click(function () {
                top.layui.index.openTabsPage('/WorkflowModule/Task/Index?tasktype=4', '我的任务');
            });
        },
        refrash: function () {
            luckyu.ajax.getNoloading('/WorkflowModule/Task/Page', { page: 1, rows: 5, tasktype: 1 }, function (res) {
                if (res.records > 0) {
                    var html = template("templeteTask", res);
                    $("#task").html(html);
                    $("#task div.ahoit-msg-line a").click(function () {
                        var self = $(this);
                        var btns = [];
                        var nodetype = self.attr("luckyu-nodetype");
                        var task_id = self.attr("luckyu-taskId");
                        var instance_id = self.attr("luckyu-instanceId");
                        var process_id = self.attr("luckyu-processId");
                        if (nodetype === "auditornode" || nodetype === "helpme") {
                            btns.push({
                                name: "确认",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.readClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                        }
                        else {
                            btns.push({
                                name: "审核",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.approveClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "协办",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.helpmeClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "加签",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.adduserClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                        }

                        luckyu.layer.layerFormTop({
                            title: "审核",
                            width: 1300,
                            height: 850,
                            url: luckyu.rootUrl + "/WorkflowModule/Task/Form?taskId=" + task_id + "&instanceId=" + instance_id + "&processId=" + process_id,
                            btn: btns,
                            success: function (layero, index) {
                                var html = '<i class="fa fa-question-circle questionInfo" id="questionBtn"></i>';
                                $("div.layui-layer-btn", layero).append(html);
                                $("#questionBtn", layero).click(function () {
                                    top.layui.layer.alert(luckyu_staticdata.wf_description);
                                });
                            }

                        });
                    });
                }
                else {
                    $("#task").html('');
                }
            });
            luckyu.ajax.getNoloading('/WorkflowModule/Task/Page', { page: 1, rows: 5, tasktype: 4 }, function (res) {
                if (res.records > 0) {
                    var html = template("templeteTask", res);
                    $("#taskDelegate").html(html);
                    $("#taskDelegate div.ahoit-msg-line a").click(function () {
                        var self = $(this);
                        var btns = [];
                        var nodetype = self.attr("luckyu-nodetype");
                        var task_id = self.attr("luckyu-taskId");
                        var instance_id = self.attr("luckyu-instanceId");
                        var process_id = self.attr("luckyu-processId");
                        if (nodetype === "auditornode" || nodetype === "helpme") {
                            btns.push({
                                name: "确认",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.readClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                        }
                        else {
                            btns.push({
                                name: "审核",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.approveClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "协办",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.helpmeClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "加签",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.adduserClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                        }

                        luckyu.layer.layerFormTop({
                            title: "审核",
                            width: 1300,
                            height: 850,
                            url: luckyu.rootUrl + "/WorkflowModule/Task/Form?taskId=" + task_id + "&instanceId=" + instance_id + "&processId=" + process_id,
                            btn: btns,
                            success: function (layero, index) {
                                var html = '<i class="fa fa-question-circle questionInfo" id="questionBtn"></i>';
                                $("div.layui-layer-btn", layero).append(html);
                                $("#questionBtn", layero).click(function () {
                                    top.layui.layer.alert('【协办】选择其他用户协助审批，其他用户审批后流程节点不会移动，后续审批人仅仅能够看到协办用户审批意见<br />【加签】选择其他用户加签审批，其他用户审批后节点会移动，相当于把当前步审批让渡给加签人<br />注：协办、加签选择用户后自己扔可以自行处理，或者等待选择人处理');
                                });

                            }

                        });
                    });
                }
                else {
                    $("#taskDelegate").html('');
                }
            });
            luckyu.ajax.getNoloading('/OAModule/News/ShowPage', { page: 1, rows: 5 }, function (res) {
                if (res.records > 0) {
                    var html = template("templeteNews", res);
                    $("#news").html(html);
                    $("#news div.ahoit-msg-line a").click(function () {
                        var self = $(this);
                        var id = self.attr("luckyu-id");
                        var title = self.attr("title");
                        luckyu.layer.layerFormTop({
                            title: title,
                            width: 1300,
                            height: 850,
                            url: luckyu.rootUrl + "/OAModule/News/Show?keyValue=" + id,
                        });
                    });
                }
                else {
                    $("#news").html('');
                }
            });
        }
    };
    page.init();
});
