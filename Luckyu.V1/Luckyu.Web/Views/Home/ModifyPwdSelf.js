/*
 * 修改密码
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    saveClick = function (layerIndex, callback) {
        var requestData = {
            oldpassword: $("#OldPassword").val(),
            newPassword: $("#Password").val()
        };
        var password1 = $("#Password1").val();
        if (requestData.newPassword !== password1) {
            layui.notice.error("两次密码输入不一致");
            return;
        }
        requestData.oldpassword = $.md5(requestData.oldpassword);
        requestData.newPassword = $.md5(requestData.newPassword);
        luckyu.ajax.postv2(luckyu.rootUrl + "/Home/ModifyPasswordSelf", requestData, function (data) {
            parent.layui.layer.close(layerIndex);
        });
    };

};