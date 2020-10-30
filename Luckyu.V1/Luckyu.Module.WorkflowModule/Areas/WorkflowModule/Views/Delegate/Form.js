/*
 * 任务委托 编辑/查看
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

            $("#to_username,#touserSelect").click(function myfunction() {
                luckyu.layer.userSelectForm({
                    multiple: false,
                    callback: function (list) {
                        if (!!list && list.length > 0) {
                            $("#to_username").val(list[0].realname);
                            $("#to_userid").val(list[0].userId);
                        }
                        else {
                            $("#to_username").val("");
                            $("#to_userid").val("");
                        }
                    }
                });
            });

            $("#flowcode").initDataSource({
                url: "/WorkflowModule/Delegate/GetFlow",
                multiple: true,
                toolbar: {
                    show: true,
                },
                select: function (data) {

                }
            });

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/WorkflowModule/Delegate/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="Delegate"]').setFormValue(data.Delegate);
                    });
            }
            else {
                var loginInfo = luckyu.clientdata.get(['userinfo']);
                $("#username").val(luckyu.clientdata.getUserName(loginInfo.user_id));
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Delegate"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Delegate/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
            isSubmit: false
        }, function (data) {
            keyValue = data.delegate_id;
            if (!!callBack) {
                callBack();
            }
        });
    };

};