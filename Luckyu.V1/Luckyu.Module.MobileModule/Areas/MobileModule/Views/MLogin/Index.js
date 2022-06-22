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
                $("#verifycode_img").attr("src", "/Login/VerifyCode?v=" + Math.random());
            });


            $("#btnLogin").click(function () {
                if (!$(".login-form").verifyForm()) {
                    return;
                }
                var formData = $(".login-form").getFormValue();
                formData.password = $.md5(formData.password);
                //console.log("formData", formData);
                luckyumobile.ajax.post("/MobileModule/MLogin/CheckLogin", formData, function (res) {
                    if (res.code === 200) {
                        bui.load({ url: res.data, param: {} });
                    }
                    else {
                        bui.alert(res.info);
                        if (res.data.wrongnum > 2) {
                            $("#divVCode").show();
                            $("#verifycode_img").click();
                        }
                    }
                    //window.location.href = data;
                });
            });
        }
    };
    page.init();

};
