/*
 * 请假编辑/查看
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
                folderPre: "Leave"
            });

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OAModule/Leave/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="Leave"]').setFormValue(data.Leave);
                        $("#statename").val(luckyu.clientdata.getDataitemName(data.Leave.state, "state"));
                        $("#username").val(luckyu.clientdata.getUserName(data.Leave.user_id));
                        $("#deptname").val(luckyu.clientdata.getDepartmentName(data.Leave.department_id));
                        $("#companyname").val(luckyu.clientdata.getCompanyName(data.Leave.company_id));

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
        var formData = $('[lay-filter="Leave"]').getFormValue();
        var deleteAnnex = $("#AnnexName")[0].deleteAnnex;
        luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/Leave/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
            deleteAnnex: deleteAnnex,
            isSubmit: false
        }, function (data) {
            keyValue = data.id;

            if (!!callBack) {
                callBack();
            }
            $("#AnnexName").uploadFile(keyValue);

        });
    };

    acceptClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        luckyu.layer.layerConfirm('确定要提交吗？', function () {
            var formData = $('[lay-filter="Leave"]').getFormValue();
            var deleteAnnex = $("#AnnexName")[0].deleteAnnex;
            luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/Leave/SaveForm", {
                keyValue: keyValue,
                strEntity: JSON.stringify(formData),
                deleteAnnex: deleteAnnex,
                isSubmit: true
            }, function (data) {
                keyValue = data.id;

                if (!!callBack) {
                    callBack();
                }
                $("#AnnexName").upload(keyValue, function () {
                    parent.layui.layer.close(layerIndex);
                });
            });
        });
    };

};