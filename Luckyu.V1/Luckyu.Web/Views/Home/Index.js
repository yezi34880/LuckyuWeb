﻿layui.config({
    base: '/lib/layuiadmin/' //静态资源所在路径
}).extend({
    notice: "modules/notice",
    loading: "/loading",
    index: 'lib/index' //主入口模块
}).use(["notice", 'index', "loading"], function () {
    luckyu.notice.init();

    var page = {
        logout: function () {
            luckyu.layer.layerConfirm('确定要退出吗？', function () {
                luckyu.ajax.post(luckyu.rootUrl + "/Login/OutLogin", {}, function () { });
                window.location.href = luckyu.rootUrl + "/Login/Index";
            });
        },
        singalir: function () {
            var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();
            connection.on("ReceiveMessage", function (res) {
                //layui.notice.info(message);
                var result = JSON.parse(res);
                layui.notice.info(result.info, result.data, {
                    closeButton: true,  //显示关闭按钮
                    positionClass: "toast-bottom-right",  //弹出的位置
                    onclick: function (e) {
                        if (result.data === "审批通知") {
                            layui.index.openTabsPage("/WorkflowModule/Task/Index", "我的任务");
                        }
                        else {
                            layui.index.openTabsPage("/SystemModule/Message/ShowIndex", "通知中心");
                        }
                    }
                });
            });
            connection.start();
        },
        bind: function () {
            // 搜索数据源为 所有菜单
            var searchSource = [];
            $("#LAY-system-side-menu a[lay-href]").each(function () {
                var that = $(this);
                searchSource.push({
                    href: that.attr("lay-href"),
                    name: that.text(),
                });
            });

            // 搜索
            $("#searchKeyword").typeahead({
                source: searchSource,
                afterSelect: function (item) {
                    layui.index.openTabsPage(item.href, item.name);
                    $("#searchKeyword").val('');
                }
            });
            // 切换用户
            $("#switchUser").click(function () {
                luckyu.layer.layerForm({
                    id: "SwitchUserForm",
                    title: "切换用户",
                    maxmin: false,
                    width: 650,
                    height: 400,
                    url: luckyu.rootUrl + "/Home/SwitchUserForm"
                });
            });
            // 修改密码
            $("#modifypwd").click(function () {
                luckyu.layer.layerForm({
                    title: "修改密码",
                    width: 450,
                    height: 300,
                    url: luckyu.rootUrl + "/Home/ModifyPwdSelf",
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index);
                        }
                    }]
                });
            });
            // 清除缓存
            $("#clearCache").click(function () {
                top.luckyu.clientdata.updateAll();
                layui.notice.success("清除浏览器缓存成功");
                //var loginInfo = luckyu.clientdata.get(['userinfo']);
                //if (loginInfo.isSystem === true) {
                //    luckyu.ajax.post(luckyu.rootUrl + "/Home/ClearCache", {}, function (res) {
                //        layui.notice.success("清除服务器缓存成功");
                //    });
                //}
                luckyu.ajax.post(luckyu.rootUrl + "/Home/ClearCache", {}, function (res) {
                    layui.notice.success("清除服务器缓存成功");
                });
            });
            // 退出
            $("#logout").click(function () {
                page.logout();
            });
        },
        init: function () {
            top.luckyu.clientdata.init(function () { });
            Pace.on('done', function () {

            });
            page.bind();

            page.singalir();
        },
    };
    page.init();

});
