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
                top.layui.index.openTabsPage('/OAModule/News/ShowIndex', '消息中心');
            });
        },
        refrash: function () {
            luckyu.ajax.getNoloading('/WorkflowModule/Task/HomeShow', {}, function (res) {
                if (res.code === 200) {
                    var html = template("templeteNews", res.data);
                    $("#task").html(html);
                    $("#task div.ahoit-msg-line a").click(function () {
                        var self = $(this);
                        var btns = [];
                        var nodetype = self.attr("luckyu-nodetype");
                        var task_id = self.attr("luckyu-taskId");
                        var instance_id = self.attr("luckyu-instanceId");
                        var process_id = self.attr("luckyu-processId");
                        if (nodetype === "auditornode") {
                            btns.push({
                                name: "已阅",
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
                            id: "Form",
                            title: "审核",
                            width: 1300,
                            height: 850,
                            url: luckyu.rootUrl + "/WorkflowModule/Task/Form?taskId=" + task_id + "&instanceId=" + instance_id + "&processId=" + process_id,
                            btn: btns
                        });
                    });
                }
            });
            luckyu.ajax.getNoloading('/OAModule/News/HomeShow', {}, function (res) {
                if (res.code === 200) {
                    var html = template("templeteNews", res.data);
                    $("#news").html(html);
                    $("#news div.ahoit-msg-line a").click(function () {
                        var self = $(this);
                        var id = self.attr("luckyu-id");
                        var title = self.attr("title");
                        luckyu.layer.layerFormTop({
                            id: "Form",
                            title: title,
                            width: 1300,
                            height: 850,
                            url: luckyu.rootUrl + "/OAModule/News/Show?keyValue=" + id,
                        });
                    });
                }
            });
        }
    };
    page.init();
});
