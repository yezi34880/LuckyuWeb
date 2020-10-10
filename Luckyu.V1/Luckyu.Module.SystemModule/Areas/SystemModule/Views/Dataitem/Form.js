/*
 * 字典  编辑
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var dataitem_id = request("dataitem_id");
    var itemcode = request("itemcode");
    var page = {
        init: function () {
            page.initData();
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/SystemModule/Dataitem/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="Dataitem"]').setFormValue(data.DataitemDetail);
                    });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Dataitem"]').getFormValue();
        formData.dataitem_id = dataitem_id;
        formData.itemcode = itemcode;
        luckyu.ajax.postv2(luckyu.rootUrl + "/SystemModule/Dataitem/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.detail_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };

};