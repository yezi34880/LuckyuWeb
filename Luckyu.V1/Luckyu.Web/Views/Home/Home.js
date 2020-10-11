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
            $("#moretask").click(function () {
                top.layui.index.openTabsPage('/WorkflowModule/Task/Index', '我的任务');
            });
        },
        refrash: function () {
            luckyu.ajax.getNoloading('/Home/Refrash', {}, function (res) {
                if (res.code === 200) {
                    var html = '';
                    for (var i = 0; i < res.data.Task.length; i++) {
                        var row = res.data.Task[i];
                        html += '\
<div class="ahoit-msg-line">\
    <a style="text-decoration: none;" luckyu-nodetype="'+ row.nodetype + '" luckyu-taskId="' + row.task_id + '" luckyu-instanceId="' + row.instance_id + '" luckyu-processId="' + row.process_id + '" title="' + row.processname + '">●&nbsp;&nbsp;【' + row.flowname + '】' + row.processname + ' </a>\
    <label>'+ row.createtime + '</label>\
</div>';
                    }
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
                                        page.searchInCurrentPage();
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
                                        page.searchInCurrentPage();
                                    });
                                    return false;
                                }
                            });
                            btns.push({
                                name: "加签",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.adduserClick(index, function () {
                                        page.searchInCurrentPage();
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
        }
    };
    page.init();
});
