/*
 * 请假编辑/查看
 */
var saveClick, acceptClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var deleteAnnex = [];

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            layui.layer.close(loading);
        },
        // 附件
        bindAnnex: function (previews) {
            var has = $(".file-input");
            if (!!has && has.length > 0) {
                $("#AnnexName").fileinput("destroy");
            }
            $("#AnnexName").fileinput({
                language: "zh",
                uploadUrl: "/SystemModule/Annex/UploadAnnex",
                deleteUrl: "/SystemModule/Annex/VirtualDeleteAnnex",
                showUpload: false, //是否显示上传按钮
                showRemove: true,
                uploadAsync: false, //是否异步
                initialPreviewAsData: true,
                overwriteInitial: false,  // 新选择图片是否替换原有预览图，false为不替换
                initialPreview: !previews ? [] : previews.initialPreview,
                initialPreviewConfig: !previews ? [] : previews.initialPreviewConfig,
                layoutTemplates: {
                    actionUpload: "",
                },
            }).on('filedeleted', function (event, key, jqXHR, data) { // 删除已上传文件,只记录key,保存一并删除
                deleteAnnex.push(key);
            });
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

                        page.bindAnnex(data.Annex);
                    })
            }
            else {
                $("#statename").val("起草");
                var loginInfo = luckyu.clientdata.get(['userinfo']);
                $("#username").val(luckyu.clientdata.getUserName(loginInfo.user_id));
                $("#deptname").val(luckyu.clientdata.getDepartmentName(loginInfo.department_id));
                $("#companyname").val(luckyu.clientdata.getCompanyName(loginInfo.company_id));

                page.bindAnnex(null);
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        var formData = $('[lay-filter="Leave"]').getFormValue();
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
        });
    };

    acceptClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        luckyu.layer.layerConfirm('确定要提交吗？', function () {
            var formData = $('[lay-filter="Leave"]').getFormValue();
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
                parent.layui.layer.close(layerIndex);
            });
        });
    };

};