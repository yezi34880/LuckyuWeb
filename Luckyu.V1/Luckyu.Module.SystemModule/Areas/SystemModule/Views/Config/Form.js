/*
 * 配置  编辑
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var page = {
        init: function () {
            page.initData();
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/SystemModule/Config/GetFormData", { keyValue: keyValue }, function (data) {
                    $('[lay-filter="Config"]').setFormValue(data.Config);
                });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Config"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/SystemModule/Config/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.config_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };

};