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

            $("#help").click(function () {
                bui.confirm({
                    "title": "操作说明",
                    "height": 640,
                    "content": '<div style="text-align:left;">【协办】选择其他用户协助审批，其他用户审批后流程节点不会移动，后续审批人仅仅能够看到协办用户审批意见\r\n【代办】选择其他用户代办审批，其他用户审批后节点会移动，相当于把当前步审批让渡给代办人\r\n注：协办、代办选择用户后自己扔可以自行处理，或者等待选择人处理</div>',
                    "buttons": [{ name: "我知道了", className: "primary-reverse" }]
                });
            });

            $('#btnHelpme').click(function () {
                var dialogobj = luckyumobile.mobileUserSelect.init({
                    multiple: true,
                    callback: function (userlist) {
                        if (!!userlist && userlist.length > 0) {
                            var userIds = userlist.map(r => r.userId);
                            var usernames = userlist.map(r => r.realname).join(",");
                            bui.confirm("确定邀请以下用户协办审批？<br />" + usernames, function (e) {
                                //this 是指点击的按钮
                                var text = $(e.target).text();
                                if (text == "确定") {
                                    luckyumobile.ajax.postv2("/WorkflowModule/Task/HelpMe", { taskId: taskId, userIds: userIds }, function (data) {
                                        bui.back();
                                    });
                                }
                                this.close()
                            });
                        }
                    }
                });
                dialogobj.open();
            });

            $('#btnAddUser').click(function () {
                var dialogobj1 = luckyumobile.mobileUserSelect.init({
                    multiple: true,
                    callback: function (userlist) {
                        if (!!userlist && userlist.length > 0) {
                            var userIds = userlist.map(r => r.userId);
                            var usernames = userlist.map(r => r.realname).join(",");
                            bui.confirm("确定邀请以下用户代办审批？<br />" + usernames, function (e) {
                                //this 是指点击的按钮
                                var text = $(e.target).text();
                                if (text == "确定") {
                                    luckyumobile.ajax.postv2("/WorkflowModule/Task/AddUser", { taskId: taskId, userIds: userIds }, function (data) {
                                        bui.back();
                                    });
                                }
                                this.close()
                            });
                        }

                    }
                });
                dialogobj1.open();
            });

            $('input[name="result"]').click(function () {
                var checkValue = $('input[name="result"]:checked').val();
                if (checkValue == 2) {
                    $("#divReturn").show();
                    $("#opinion").attr("luckyu-verify", "required");
                }
                else {
                    $("#divReturn").hide();
                    $("#opinion").removeAttr("luckyu-verify", "required");
                }
            });

            $("#btnApprove").click(function () {
                if (!$("#appform").verifyForm()) {
                    return;
                }
                var result = $("input[name=result]:checked").val();
                var resultname = result == 1 ? "通过" : "驳回";
                var opinion = $("#opinion").val();
                var returnType = $("input[name=returnType]:checked").val();
                var formData = {
                    taskId: taskId,
                    result: result,
                    opinion: opinion,
                    returnType: returnType,
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

            $("#btnRead").click(function () {
                if (!$("#readform").verifyForm()) {
                    return;
                }
                var opinion = $("#readopinion").val();
                var formData = {
                    taskId: taskId,
                    result: 4,
                    opinion: opinion,
                }
                bui.confirm("确认【已阅】吗？", function (e) {
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
