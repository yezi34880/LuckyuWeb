/*
 * 菜单编辑/查看
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";
    debugger;

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
                expandedKeys: []
            });

            $("#moduletype").initLocal({
                data: [
                    { "name": "系统菜单", "value": 0 },
                    { "name": "自定义表单", "value": 2 }
                ],
                initValue: "0",
                select: function (seldata) {
                    if (seldata.arr.length > 0) {
                        switch (seldata.arr[0].value) {
                            case 0:
                                $("#divmoduleurl").show();
                                $("#divformname").hide();
                                xmSelect.get("#form_id", true).setValue([]);
                                break;
                            case 2:
                                $("#divmoduleurl").hide();
                                $("#divformname").show();
                                $("#moduleurl").val('');
                                break;
                        }

                    }
                }
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
        formData.formname = formData.form_idname;
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