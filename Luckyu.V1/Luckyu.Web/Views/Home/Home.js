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
                                name: "转发查看",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.helpmeClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "会签办理",
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
                                //var html = '<i class="fa fa-question-circle questionInfo" id="questionBtn"></i>';
                                //$("div.layui-layer-btn", layero).append(html);
                                //$("#questionBtn", layero).click(function () {
                                //    top.layui.layer.alert(luckyu_staticdata.wf_description);
                                //});
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
                                name: "转发查看",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.helpmeClick(index, function () {
                                        page.refrash();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "会签办理",
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
                        var title = self.find("span.stitle").html();
                        luckyu.layer.layerFormTop({
                            title: title,
                            width: 1300,
                            height: 850,
                            url: luckyu.rootUrl + "/OAModule/News/ShowForm?keyValue=" + id,
                        });
                    });
                }
                else {
                    $("#news").html('');
                }
            });
            luckyu.ajax.getNoloading('/SystemModule/Message/GetNotReadCount', {}, function (res) {
                if (res.code = 200) {
                    var h = '<span class="layui-badge">' + (res.data > 99 ? "99+" : res.data) + '</span>';
                    $("#spanMessage", parent.document).html(h);
                }
            });
        }
    };
    page.init();
});
