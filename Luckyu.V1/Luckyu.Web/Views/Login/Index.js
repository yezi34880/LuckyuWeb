/**
 * 登录
 */

layui.config({
    version: 20200725, //一般用于更新组件缓存，默认不开启。设为true即让浏览器不缓存。也可以设为一个固定的值，如：201610
    base: '/lib/layuiadmin/modules/'
}).use(['layer', 'form', 'notice'], function () {
    luckyu.notice.init();

    var page = {
        login: function () {
            layui.form.on('submit(*)', function (data) {
                var postData = {
                    username: $.trim($("#username").val()),
                    password: $.trim($("#password").val()),
                    verifycode: $.trim($("#verifycode").val()),
                };
                postData.password = $.md5(postData.password);
                luckyu.ajax.post(luckyu.rootUrl + "/Login/CheckLogin", postData, function (res) {
                    if (res.code === 200) {
                        window.location.href = res.data;
                    }
                    else {
                        layui.notice.error(res.info);
                        $("#verifycode_img").click();
                    }
                });

            });
        },
        init: function () {
            if (window.location.href != top.window.location.href) {
                top.window.location.href = window.location.href;
            }

            // 回车键
            document.onkeydown = function (e) {
                e = e || window.event;
                if ((e.keyCode || e.which) == 13) {
                    $('#btnLogin').trigger('click');
                }
            }

            $("#verifycode_img").click(function () {
                $("#verifycode").val('');
                $("#verifycode_img").attr("src", luckyu.rootUrl + "/Login/VerifyCode?v=" + Math.random());
            });

            $("#btnLogin").click(function () {
                page.login();
            });

        }
    };

    page.init();
});
