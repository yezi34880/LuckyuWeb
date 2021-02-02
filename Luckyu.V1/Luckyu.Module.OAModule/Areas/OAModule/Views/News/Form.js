/*
 * 新闻通知 编辑/查看
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OAModule/News/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="News"]').setFormValue(data.News);
                    })
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="News"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/News/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.news_id;
            if (!!callBack) {
                callBack();
            }
            parent.layui.layer.close(layerIndex);
        });
    };

};