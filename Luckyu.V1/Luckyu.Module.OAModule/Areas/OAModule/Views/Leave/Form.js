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
                        $('[lay-filter="FormMain"]').setFormValue(data.Leave);
                        luckyu.clientdata.getAsync('dataItem', {
                            key: data.Leave.state,
                            code: "state",
                            callback: function (_data) {
                                if (!!_data) {
                                    $("#statename").val(_data.name);
                                }
                            }
                        });
                        $('#AnnexName').initFileInput({
                            initialPreview: data.Annex.initialPreview,
                            initialPreviewConfig: data.Annex.initialPreviewConfig
                        });
                    })
            }
            else {
                $("#statename").val("起草");
                var loginInfo = luckyu.clientdata.get(['userinfo']);
                $("#username").val(loginInfo.realname);
                $("#departmentname").val(loginInfo.departmentname);
                $("#companyname").val(loginInfo.companyname);
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        var formData = $('[lay-filter="FormMain"]').getFormValue();
        var deleteAnnex = $("#AnnexName")[0].deleteAnnex;
        luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/Leave/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
            deleteAnnex: deleteAnnex,
            isSubmit: 0
        }, function (data) {
            keyValue = data.leave_id;

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
            var formData = $('[lay-filter="FormMain"]').getFormValue();
            var deleteAnnex = $("#AnnexName")[0].deleteAnnex;
            luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/Leave/SaveForm", {
                keyValue: keyValue,
                strEntity: JSON.stringify(formData),
                deleteAnnex: deleteAnnex,
                isSubmit: 1
            }, function (data) {
                keyValue = data.leave_id;

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