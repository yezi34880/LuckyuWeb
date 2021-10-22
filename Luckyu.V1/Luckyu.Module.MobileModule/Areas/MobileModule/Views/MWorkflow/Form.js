var bootstrap = function () {
    "use strict";

    var taskId = request("taskId");

    var uiUpload;
    var page = {
        init: function () {
            page.bind();
            page.bindUpload();
        },
        bindUpload: function () {
            // 拍照上传
            var $facePhoto = $("#buiPhoto");
            var $uploadBtn = $("#btnUpload").parent();

            uiUpload = bui.upload({
                showProgress: true,
                // needNative: false
            });

            // 上拉菜单 js 初始化:
            var uiActionsheet = bui.actionsheet({
                trigger: "#btnUpload",
                buttons: [{ name: "拍照上传", value: "camera" }, { name: "从相册选取", value: "photo" }],
                callback: function (e) {
                    var ui = this;
                    var val = $(e.target).attr("value");

                    switch (val) {
                        case "camera":
                            ui.hide();
                            uiUpload.add({
                                "from": "camera",
                                // "needCompress": true, // 1.5.3新增压缩
                                // "width": 300,
                                "destinationType": "file", //  file | blob | data 
                                "onSuccess": function (val, data) {
                                    // bui.alert(val)
                                    // 展示base64本地图片 建议直接调用start方法上传以后再展示远程地址,避免应用崩溃
                                    this.toBase64({
                                        onSuccess: function (url) {
                                            $uploadBtn.before(templatePhoto(url))
                                        },
                                        onFail: function (url) {
                                            bui.alert(url)
                                            // $uploadBtn.before(templatePhoto(url))
                                        }
                                    });
                                    // 直接调用start上传图片

                                    // this.start({
                                    //     url: "https://imgurl.org/upload/ftp",
                                    //     data: {
                                    //         test: 111,
                                    //         file: this.data().file
                                    //     },
                                    //     onSuccess: function(data) {
                                    //         bui.alert(data);
                                    //         // 成功
                                    //     },
                                    //     onFail: function(data) {
                                    //         console.log(data, "fail");
                                    //         // 失败
                                    //     },
                                    // })

                                }
                            })

                            break;
                        case "photo":
                            ui.hide();
                            uiUpload.add({
                                "from": "",
                                "onSuccess": function (val, data) {

                                    // 展示base64本地图片 建议直接调用start方法上传以后再展示远程地址,避免应用崩溃
                                    this.toBase64({
                                        onSuccess: function (url) {
                                            $uploadBtn.before(templatePhoto(url))

                                        }
                                    });
                                    // bui.alert(val)
                                    // this.start({
                                    //     header: {},
                                    //     url: "https://imgurl.org/upload/ftp",
                                    //     onSuccess: function(data) {
                                    //         bui.alert(data);
                                    //         // 成功
                                    //     },
                                    //     onFail: function(data) {
                                    //         console.log(data, "fail");
                                    //         // 失败
                                    //     },
                                    // })

                                }
                            })

                            break;
                        case "cancel":
                            ui.hide();
                            break;
                    }
                }
            })

            function templatePhoto(url) {
                return `<div class="span1" data-index="${uiUpload.currentIndex()}" data-name="${uiUpload.currentName()}">
                <div class="bui-upload-thumbnail"><img src="${url}" alt="" /><i class="icon-removefill"></i></div>
            </div>`
            }

            // 选择图片文件
            $("#getSelect").on("click", function (argument) {

                bui.alert(uiUpload.data());

            })

            // 选择图片文件
            $facePhoto.on("click", ".icon-removefill", function (e) {
                var $item = $(this).parents(".span1");
                var index = $item.attr("data-index");
                var name = $item.attr("data-name");

                // 删除对应的上传数据
                uiUpload.remove(name);
                // 删除dom显示
                $item.remove();
                e.stopPropagation();

            });
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
                that.ready(function () {
                    debugger;
                    that.find("header").hide();
                    that.find("input").each(function () {
                        var ele = $(this);
                        ele.attr("disabled", "disabled");
                    });
                });
            });

            $("#help").click(function () {
                bui.confirm({
                    "title": "操作说明",
                    "height": 640,
                    "content": '<div style="text-align:left;">' + luckyu_staticdata.wf_description + '</div>',
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
                            bui.confirm("确定邀请以下用户加签审批？<br />" + usernames, function (e) {
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

            var approve = function () {
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
                        luckyumobile.ajax.postv2("/WorkflowModule/Task/Approve", formData, function (data, info) {
                            var history_id = data.Item3[0].history_id;
                            var uploaddata = uiUpload.data();
                            if (!!uploaddata && uploaddata.length > 0) {
                                uiUpload.startAll({
                                    url: "/SystemModule/Annex/UploadAnnex",
                                    data: {
                                        folderPre: "WFApprove",
                                        exId: history_id,
                                        __RequestVerificationToken: $.validateToken
                                    },
                                    needFileinfo: true,
                                    onSuccess: function (data) {
                                        bui.alert(info, function () {
                                            bui.back();
                                        });
                                    },
                                    onFail: function (data) {
                                        bui.alert(data)
                                    },
                                    onEnd: function (res) {
                                        console.log(res)
                                        console.log(res.length)
                                    }
                                })
                            }
                            else {
                                bui.alert(info, function () {
                                    bui.back();
                                });
                            }
                        });
                    }
                    this.close()
                });
            };
            var read = function () {
                if (!$("#appform").verifyForm()) {
                    return;
                }
                var opinion = $("#opinion").val();
                var formData = {
                    taskId: taskId,
                    result: 4,
                    opinion: opinion,
                }
                bui.confirm("确认【已阅】吗？", function (e) {
                    //this 是指点击的按钮
                    var text = $(e.target).text();
                    if (text == "确定") {
                        luckyumobile.ajax.postv2("/WorkflowModule/Task/Approve", formData, function (data, info) {
                            var history_id = data.Item3[0].history_id;
                            var uploaddata = uiUpload.data();
                            if (!!uploaddata && uploaddata.length > 0) {
                                uiUpload.startAll({
                                    url: "/SystemModule/Annex/UploadAnnex",
                                    data: {
                                        folderPre: "WFApprove",
                                        exId: history_id,
                                        __RequestVerificationToken: $.validateToken
                                    },
                                    needFileinfo: true,
                                    onSuccess: function (data) {
                                        bui.alert(info, function () {
                                            bui.back();
                                        });
                                    },
                                    onFail: function (data) {
                                        bui.alert(data)
                                    },
                                    onEnd: function (res) {
                                        console.log(res)
                                        console.log(res.length)
                                    }
                                })
                            }
                            else {
                                bui.alert(info, function () {
                                    bui.back();
                                });
                            }
                        });
                    }
                    this.close()
                });
            };

            var btnApprove = $("#btnApprove");
            if (btnApprove.html() == "审批") {
                $("#btnApprove").on("click", approve);
            }
            else {
                $("#btnApprove").on("click", read);
            }


        }
    };
    page.init();

};
