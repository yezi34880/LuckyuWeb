var bootstrap = function () {
    "use strict";

    var keyValue = request("keyValue");

    var page = {
        init: function () {
            page.initData();
        },
        initData: function () {
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
    };
    page.init();

};
