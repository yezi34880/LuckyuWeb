/**
 * 表单内容
 */
var saveClick;
var bootstrap = function (layui) {

    var form_id = request("form_id");
    var keyValue = request("keyValue");

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + '/FormModule/FormInput/GetFormData', { keyValue: keyValue }, function (data) {
                    $('lay-filter="FormMain"').val(data.Main);
                });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$('lay-filter="FormMain"').verifyForm()) {
            return false;
        }
        var formData = $('lay-filter="FormMain"').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/FormModule/FormInput/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData)
        }, function (data) {

            parent.layui.layer.close(layerIndex);
            if (!!callBack) {
                callBack();
            }
        });

    };

};