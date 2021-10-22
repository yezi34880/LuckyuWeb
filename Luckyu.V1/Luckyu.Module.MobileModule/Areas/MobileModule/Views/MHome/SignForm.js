var bootstrap = function () {
    "use strict";

    var position;
    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            if (!navigator.geolocation) {
                bui.alert("获取定位失败");
                return;
            }
            navigator.geolocation.getCurrentPosition(function (position) {
                position = position;
            });

            $("#btnSignIn").click(function () {
                page.getLocation();
                console.log(position);

            });
            $("#btnSignOut").click(function () {
                page.getLocation();
                console.log(position);

            });

        },
        getLocation: function () {
            if (!position) {
                bui.alert("获取定位失败，请重试");
                navigator.geolocation.getCurrentPosition(function (position) {
                    position = position;
                });
                return;
            }
            return position;
        }
    };
    page.init();
};
