var bootstrap = function () {
    "use strict";

    var taskId = request("taskId");

    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            // 绑定跳转
            //bui.btn({ id: ".bui-page", handle: ".bui-btn" }).load();

            //按钮在tab外层,需要传id
            var tab = bui.tab({
                id: "#tabFoot",
                menu: "#tabFootNav"
            });
            var tabForm = bui.tab({
                id: "#tabForm",
            });

            $("#ulForm iframe").each(function () {
                var that = $(this);
                var parent = that.parent();
                that.width(parent.width()).height(parent.height());
            });

            $("#btnApprove").click(function () {
                var result = $("input[name=result]:checked").val();
                var resultname = result == 1 ? "通过" : "驳回";
                var opinion = $("#opinion").val();
                var formData = {
                    taskId: taskId,
                    result: result,
                    opinion: opinion,
                }
                bui.confirm("确认【" + resultname + "】吗？", function (e) {
                    //this 是指点击的按钮
                    var text = $(e.target).text();
                    if (text == "确定") {
                        luckyumobile.ajax.postv2("/WorkflowModule/Task/Approve", formData, function (data) {
                            bui.back();
                        });
                    }
                    this.close()
                });
            });

        }
    };
    page.init();

};
