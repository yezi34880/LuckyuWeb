var bootstrap = function () {
    "use strict";

    var page = {
        init: function () {
            top.luckyumobile.clientdata.init(function () { });
            page.bind();
        },
        bind: function () {
            // 绑定跳转
            bui.btn({ id: ".bui-page", handle: ".bui-btn" }).load();

            //按钮在tab外层,需要传id
            var tab = bui.tab({
                id: "#tabFoot",
                menu: "#tabFootNav"
            });

            //折叠菜单示例
            var uiAccordion = bui.accordion({
                id: "#accordion"
            })
            uiAccordion.showAll();

            // 小红点
            luckyumobile.ajax.getv2('/WorkflowModule/Task/GetActiveCount', {}, function (data) {
                if (data > 0) {
                    if (data > 99) {
                        data = 99;
                    }
                    $("#iActive").html('<span class="bui-badges">' + data + '</span>');
                }
            });
            luckyumobile.ajax.getv2('/WorkflowModule/Task/GetDelegateCount', {}, function (data) {
                if (data > 0) {
                    if (data > 99) {
                        data = 99;
                    }
                    $("#iDelegate").html('<span class="bui-badges">' + data + '</span>');
                }
            });

            $("#signout").click(function () {
                bui.confirm("确认要退出吗？", function (e) {
                    //this 是指点击的按钮
                    var text = $(e.target).text();
                    if (text === "确定") {
                        luckyumobile.ajax.postv2("/Login/OutLogin", {}, function (data) {
                            bui.load({ url: "/MobileModule/MLogin/Index", param: {} });
                        });
                    }
                    this.close()
                });
            });

        }
    };
    page.init();

};
