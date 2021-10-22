var bootstrap = function () {
    "use strict";

    var keyValue = request("keyValue");

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var begintime = bui.pickerdate({
                handle: "#begintime",
                bindValue: true,
                formatValue: "yyyy-MM-dd HH:mm",
            });
            var endtime = bui.pickerdate({
                handle: "#endtime",
                bindValue: true,
                formatValue: "yyyy-MM-dd HH:mm",
            });

        },
        initData: function () {
            if (!!keyValue) {
                luckyumobile.ajax.getv2("/OAModule/Leave/GetFormData", { keyValue: keyValue }, function (data) {
                    $("#formLeave").setFormValue(data.Leave);

                    luckyumobile.clientdata.getAsync('dataItem', {
                        key: data.Leave.leavetype,
                        code: "leavetype",
                        callback: function (_data) {
                            if (!!_data.name) {
                                $("#leavetype").val(_data.name);
                            }
                        }
                    });
                    luckyumobile.clientdata.getAsync('dataItem', {
                        key: data.Leave.state,
                        code: "state",
                        callback: function (_data) {
                            if (!!_data.name) {
                                $("#statename").val(_data.name);
                            }
                        }
                    });

                });
            }
            else {
                $("#statename").val("起草");
                var loginInfo = luckyu.clientdata.get(['userinfo']);
                $("#username").val(loginInfo.realname);
                $("#departmentname").val(loginInfo.departmentname);
                $("#companyname").val(loginInfo.companyname);
            }
        }
    };
    page.init();

};
