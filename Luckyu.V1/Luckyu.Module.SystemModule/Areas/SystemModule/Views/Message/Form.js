/*
 * 系统消息 发送
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            var loading = luckyu.layer.loading();
            $("#to_username,#to_usernameSelect").click(function () {
                luckyu.layer.userSelectForm({
                    multiple: true,
                    callback: function (list) {
                        if (!!list && list.length > 0) {
                            var userIds = list.map(r => r.userId).join(',');
                            var usernames = list.map(r => r.realname).join(',');
                            $("#to_username").val(usernames);
                            $("#to_userid").val(userIds);
                        }
                        else {
                            $("#to_username").val("");
                            $("#to_userid").val("");
                        }
                    }
                });
            });

            layui.layer.close(loading);
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="Message"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/SystemModule/Message/SaveForm", {
            keyValue: '',
            strEntity: JSON.stringify(formData),
        }, function (data) {
            if (!!callBack) {
                callBack();
            }
            parent.layui.layer.close(layerIndex);
        });
    };

};