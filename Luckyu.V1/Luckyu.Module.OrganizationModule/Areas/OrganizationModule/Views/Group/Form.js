/*
 * 小组编辑/查看
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
                luckyu.ajax.getv2(luckyu.rootUrl + "/OrganizationModule/Group/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="Group"]').setFormValue(data.Group);
                    })
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Group"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Group/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.group_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };
};