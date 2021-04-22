var bootstrap = function () {
    "use strict";

    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            // 绑定跳转
            //bui.btn({ id: ".bui-page", handle: ".bui-btn" }).load();

            //按钮在tab外层,需要传id
            var tab = bui.tab({
                id: "#tabFoot",
                menu: "#tabFootNav"
            });

        }
    };
    page.init();

};
