var bootstrap = function () {
    "use strict";

    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            bui.btn({ id: ".bui-page", handle: ".bui-btn" }).load();

            $("#verifycode_img").click(function () {
                $("#verifycode").val('');
                $("#verifycode_img").attr("src", "/MobileModule/MLogin/VerifyCode?v=" + Math.random());
            });


            $("#btnLogin").click(function () {
                if (!$(".login-form").verifyForm()) {
                    return;
                }
                var formData = $(".login-form").getFormValue();
                formData.password = $.md5(formData.password);
                //console.log("formData", formData);
                luckyumobile.ajax.postv2("/MobileModule/MLogin/CheckLogin", formData, function (data) {
                    bui.load({ url: data, param: {} });
                    //window.location.href = data;
                });
            });
        }
    };
    page.init();

};
