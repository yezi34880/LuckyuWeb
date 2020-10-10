/*
 * 用户编辑/查看
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
            $("#company_id").initCompany({
                select: function (xmdata) {
                    $("#department_id").initDepartment({
                        companyId: xmdata.arr[0].value
                    });
                }
            });
            $("#department_id").initLocal({});
            $("#sex").initLocal({
                data: [
                    { name: "男", value: 1 },
                    { name: "女", value: 2 },
                ]
            });
            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OrganizationModule/User/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="User"]').setFormValue(data.User);
                    })
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="User"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/User/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.user_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };

};