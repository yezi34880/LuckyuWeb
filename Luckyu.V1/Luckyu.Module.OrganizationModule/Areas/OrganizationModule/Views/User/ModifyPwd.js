/*
 * 修改密码
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");

    saveClick = function (layerIndex, callback) {
        var requestData = {
            keyValue: keyValue,
            password: $("#Password").val()
        };
        var password1 = $("#Password1").val();
        if (requestData.password !== password1) {
            layui.notice.error("两次密码输入不一致");
            return;
        }
        requestData.password = $.md5(requestData.password);
        ahoit.ajax.postv2(ahoit.rootUrl + "/OrganizationModule/User/ModifyPassword", requestData,
            function (data) {
                parent.layui.layer.close(layerIndex);
                if (!!callback) {
                    callback();
                }
            });
    };

};