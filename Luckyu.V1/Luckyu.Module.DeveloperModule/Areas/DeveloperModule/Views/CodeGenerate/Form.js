/*
 * 代码生成
 */
var bootstrap = function (layui) {
    "use strict";

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            $("#entity").click(function () {
                var dbtablename = xmSelect.get("#dbtablename", true).getValue("valueStr");
                if (!dbtablename) {
                    layui.layer.alert("请选择数据库表", { icon: 2 });
                    return;
                }
                luckyu.ajax.postv2("/DeveloperModule/CodeGenerate/GenerateEntity", { dbtablename: dbtablename }, function (resdata) {
                    $("#contents").val(resdata);
                });
            });
            $("#service").click(function () {
                var dbtablename = xmSelect.get("#dbtablename", true).getValue("valueStr");
                if (!dbtablename) {
                    layui.layer.alert("请选择数据库表", { icon: 2 });
                    return;
                }
                luckyu.ajax.postv2("/DeveloperModule/CodeGenerate/GenerateService", { dbtablename: dbtablename }, function (resdata) {
                    $("#contents").val(resdata);
                });

            });
            $("#bll").click(function () {
                var dbtablename = xmSelect.get("#dbtablename", true).getValue("valueStr");
                if (!dbtablename) {
                    layui.layer.alert("请选择数据库表", { icon: 2 });
                    return;
                }
                luckyu.ajax.postv2("/DeveloperModule/CodeGenerate/GenerateBLL", { dbtablename: dbtablename }, function (resdata) {
                    $("#contents").val(resdata);
                });

            });

            $("#downloadtemplete").click(function () {
                var dbtablename = xmSelect.get("#dbtablename", true).getValue("valueStr");
                if (!dbtablename) {
                    layui.layer.alert("请选择数据库表", { icon: 2 });
                    return;
                }
                luckyu.utility.download({
                    url: "/DeveloperModule/CodeGenerate/DownloadTemplete",
                    param: { dbtablename: dbtablename}
                });
            });
            layui.layer.close(loading);
        },
    };
    page.init();

};