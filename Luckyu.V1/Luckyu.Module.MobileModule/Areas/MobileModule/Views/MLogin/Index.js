var bootstrap = function () {
    "use strict";

    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            $("#btnLogin").click(function () {
                if (!$(".login-form").verifyForm()) {
                    return;
                }
                var formData = $(".login-form").getFormValue();
                formData.password = $.md5(formData.password);
                console.log("formData", formData);
                luckyumobile.ajax.postv2("/MobileModule/MLogin/CheckLogin", formData, function (data) {
                    bui.load({ url: data, param: {} });
                    //window.location.href = data;
                });
            });
        }
    };
    page.init();

};
