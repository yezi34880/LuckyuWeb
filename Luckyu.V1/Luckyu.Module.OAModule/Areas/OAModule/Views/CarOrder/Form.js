/*
 * 约车 编辑/查看
 */
var saveClick, acceptClick;
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
        var formData = $('[lay-filter="CarOrder"]').getFormValue();
        var deleteAnnex = $("#AnnexName")[0].deleteAnnex;
        luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/CarOrder/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
            deleteAnnex: deleteAnnex,
            isSubmit: 0
        }, function (data) {
            keyValue = data.order_id;

            if (!!callBack) {
                callBack();
            }
            $("#AnnexName").uploadFile({
                exId: keyValue
            });

        });
    };

    acceptClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        luckyu.layer.layerConfirm('确定要提交吗？', function () {
            var formData = $('[lay-filter="CarOrder"]').getFormValue();
            var deleteAnnex = $("#AnnexName")[0].deleteAnnex;
            luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/CarOrder/SaveForm", {
                keyValue: keyValue,
                strEntity: JSON.stringify(formData),
                deleteAnnex: deleteAnnex,
                isSubmit: 1
            }, function (data) {
                keyValue = data.order_id;

                if (!!callBack) {
                    callBack();
                }
                $("#AnnexName").uploadFile({
                    exId: keyValue,
                    callback: function () {
                        parent.layui.layer.close(layerIndex);
                    }
                });

            });
        });
    };

};