/*
 * 菜单编辑/查看
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
                url: luckyu.rootUrl + "/OrganizationModule/Module/GetSelectTree",
            });

            layui.layer.close(loading);

        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OrganizationModule/Module/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="Module"]').setFormValue(data.Module);
                    })
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Module"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Module/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.module_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };
};