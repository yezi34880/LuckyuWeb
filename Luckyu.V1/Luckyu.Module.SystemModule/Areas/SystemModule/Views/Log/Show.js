/*
 * 日志  编辑
 */
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var date = request("date");
    var page = {
        init: function () {
            page.initData();
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/SystemModule/Log/GetFormData", { keyValue: keyValue, date: date }
                    , function (data) {
                        $('[lay-filter="Log"]').setFormValue(data.Log);
                        //$('#ip').val(data.Log.host + " " + data.Log.ip_address + " " + data.Log.ip_location);
                    });
            }
        },
    };
    page.init();

};