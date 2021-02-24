/*
 * 约车 编辑/查看
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

            $("#AnnexName").initFileInput({
                folderPre: "CarOrder"
            });

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OAModule/CarOrder/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="CarOrder"]').setFormValue(data.CarOrder);
                        $("#statename").val(luckyu.clientdata.getDataitemName(data.CarOrder.state, "state"));
                        //$("#username").val(luckyu.clientdata.getUserName(data.CarOrder.user_id));
                        $("#deptname").val(luckyu.clientdata.getDepartmentName(data.CarOrder.department_id));
                        $("#companyname").val(luckyu.clientdata.getCompanyName(data.CarOrder.company_id));

                        $('#AnnexName').initFileInput({
                            initialPreview: data.Annex.initialPreview,
                            initialPreviewConfig: data.Annex.initialPreviewConfig
                        });
                    })
            }
            else {
                $("#statename").val("起草");
                var loginInfo = luckyu.clientdata.get(['userinfo']);
                $("#username").val(luckyu.clientdata.getUserName(loginInfo.user_id));
                $("#deptname").val(luckyu.clientdata.getDepartmentName(loginInfo.department_id));
                $("#companyname").val(luckyu.clientdata.getCompanyName(loginInfo.company_id));
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var flag = true;
        var formData = $('[lay-filter="CarOrder"]').getFormValue();
        luckyu.ajax.postSyncv2(luckyu.rootUrl + "/OAModule/CarOrder/ApproveSave", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.order_id;
            flag = data.code === 200;
            if (!!callBack) {
                callBack();
            }
        });
        return flag;
    };

};