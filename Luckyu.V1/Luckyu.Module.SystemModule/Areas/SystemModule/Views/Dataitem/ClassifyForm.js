/*
 * 字典分类 编辑
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
            $("#parent_id").initSelectTree({
                url: luckyu.rootUrl + "/SystemModule/Dataitem/GetSelectTree"
            });

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/SystemModule/Dataitem/GetClassifyFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="DataitemClassify"]').setFormValue(data.DataitemClassify);
                    });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="DataitemClassify"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/SystemModule/Dataitem/SaveClassifyForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.dataitem_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };

};