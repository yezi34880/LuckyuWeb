/*
 * 部门编辑/查看
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
                    debugger;
                    $("#parent_id").initDepartment({
                        companyId: xmdata.arr[0].value
                    });
                }
            });
            $("#parent_id").initLocal({});
            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OrganizationModule/Department/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="Department"]').setFormValue(data.Department);
                    })
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Department"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Department/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.department_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };

};