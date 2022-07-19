var bootstrap = function () {
    "use strict";

    var taskId = request("taskId");
    var instanceId = request("instanceId");
    var historyId = request("historyId");
    var processId = request("processId");

    var uiUpload;
    var page = {
        init: function () {
            page.bind();
            page.bindUpload();
            page.initData();
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

            $('#btnHelpme').click(function () {
                var dialogobj = luckyumobile.mobileUserSelect.init({
                    multiple: true,
                    callback: function (userlist) {
                        if (!!userlist && userlist.length > 0) {
                            var userIds = userlist.map(r => r.userId);
                            var usernames = userlist.map(r => r.realname).join(",");
                            bui.confirm("确定将该审批转发给以下用户查看？<br />" + usernames, function (e) {
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
                            bui.confirm("确定邀请以下用户会签办理审批？<br />" + usernames, function (e) {
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

            var getAuthors = function () {
                var nextnode = $("input[name=nextnode]");
                var authors = {};
                if (nextnode.length > 0) {
                    var checknodes = $("input[name=nextnode]:checked");
                    if (checknodes.length < 1) {
                        bui.alert("请选择下一步节点");
                        return false;
                    }
                    for (var i = 0; i < checknodes.length; i++) {
                        authors[checknodes[i].value] = [];
                    }
                }
                else {
                    var checknodes = $("#ulNext div.layui-card");
                    for (var i = 0; i < checknodes.length; i++) {
                        var nodeid = $(checknodes[i]).attr("nodeid");
                        authors[nodeid] = [];
                    }
                }
                for (var nodeid in authors) {
                    var card = $('#ulNext div.layui-card[nodeid="' + nodeid + '"]');
                    var ischeckuser = card.attr("ischeckuser");
                    if (ischeckuser == 1) {
                        var nextusers = card.find("input[lay-filter=nodeuser]");
                        var seluserBtn = card.find("div.btnSelectUser");
                        if (nextusers.length > 0 || seluserBtn.length > 0) {
                            var checkusers = card.find("input[lay-filter=nodeuser]:checked");
                            if (checkusers.length < 1) {
                                bui.alert("节点未选择人员");
                                return false;
                            }
                            for (var i = 0; i < checkusers.length; i++) {
                                var $chk = $(checkusers[i]);
                                var nodeid = $chk.attr("nodeid");
                                if (!authors[nodeid]) {
                                    authors[nodeid] = [];
                                    authors[nodeid].push({
                                        userId: $chk.val(),
                                        userName: $chk.attr("uncheck")
                                    });
                                }
                                else {
                                    authors[nodeid].push({
                                        userId: $chk.val(),
                                        userName: $chk.attr("uncheck")
                                    });
                                }
                            }
                        }
                    }
                }

                return authors;
            };

            var approve = function () {
                if (!$("#appform").verifyForm()) {
                    return;
                }
                var result = $("input[name=result]:checked").val();
                var opinion = $("#opinion").val();
                var authors = getAuthors();
                var formData = {
                    taskId: taskId,
                    result: result,
                    opinion: opinion,
                    authors: authors
                }
                bui.confirm("确认提交审批吗？", function (e) {
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
            $("#btnOK").on("click", approve);
            var read = function () {
                if (!$("#appform").verifyForm()) {
                    return;
                }
                var opinion = $("#opinion").val();
                var authors = getAuthors();
                var formData = {
                    taskId: taskId,
                    opinion: opinion,
                    authors: authors,
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
            $("#read").on("click", read);

            var uiDialog;
            $("#approve").on("click", function () {
                uiDialog = bui.dialog({
                    id: "#dialogDown",
                    position: "bottom",
                    autoClose: false
                });
                uiDialog.open();
            });

            $("#btnCancel").click(function () {
                uiDialog.close();
            });
        },
        initData: function () {
            luckyumobile.ajax.getv2(luckyumobile.rootUrl + '/WorkflowModule/Task/GetFormData', { instanceId: instanceId, taskId: taskId, historyId: historyId }, function (data) {
                var instance = data.Instance;
                $("#divHeader").html(instance.flowname);

                var htmlTab = '';
                var htmlIframe = '';
                // 表单
                for (var i = 0; i < data.ShowNode.forms.length; i++) {
                    var form = data.ShowNode.forms[i];
                    var formurl = form.mobileformurl;
                    if (!!formurl) {
                        if (formurl.indexOf("?") < 0) {
                            formurl += '?keyValue=' + instance.process_id;
                        }
                        else {
                            formurl += '&keyValue=' + instance.process_id;
                        }
                        htmlTab += '<li class="bui-btn">' + form.formname + '</li>';
                        htmlIframe += '\
<li style="width:100%;padding:0 10px;'+ (i > 0 ? "display: none; " : "") + '">\
    <iframe src="'+ formurl + '" frameborder="0" border="0"></iframe>\
</li>';
                    }
                }
                $("#iframeNav").html(htmlTab);
                $("#ulForm").html(htmlIframe);
                $("#ulForm").parent().css("width", "100%");
                $("#ulForm iframe").css("height", "100%").css("width", "100%");

                if (!!taskId) {  // 审批流程才有 审批按钮
                    if (data.CurrentNode.type == "auditornode" || data.CurrentNode.type == "helpme") {  // 传阅 或者协办 就只有 确认
                        $("#read").show();
                    }
                    else {
                        $("#approve").show();
                    }
                }

                var htmlHistory = '';
                for (var i = 0; i < data.History.length; i++) {
                    var item = data.History[i];
                    htmlHistory += '\
<div class="bui-stepbar-cell '+ (item.result == 100 ? "" : "active") + '">\
    <span class="bui-stepbar-dot"></span>\
    <div class="bui-stepbar-text">\
        <h3>'+ (luckyumobile.utility.toEnum(item.result, luckyu_staticdata.m_wf_resultshow) + " " + item.nodename) + '</h3>\
';

                    if (item.result != 0) {
                        htmlHistory += '<p class="bui-stepbar-desc">审批人：' + item.create_username + '</p>';

                        if (item.result != 100) {
                            htmlHistory += '<p class="bui-stepbar-desc">提交时间：' + new Date(item.tasktime).format("yyyy-MM-dd HH:mm") + '</p>';
                            htmlHistory += '<p class="bui-stepbar-desc">办结时间：' + new Date(item.apptime).format("yyyy-MM-dd HH:mm") + '</p>';
                            htmlHistory += (!item.opinion ? '' : ('<p class="bui-stepbar-desc">意见建议：' + item.opinion + '</p>'));
                        }
                        if (!!item.annex && item.annex != '[]') {
                            var list = JSON.parse(item.annex);
                            htmlHistory += '<p class="bui-stepbar-desc">附件：';
                            for (var j = 0; j < list.length; j++) {
                                htmlHistory += ' <br/><a style="color:blue;cursor:pointer;text-decoration:underline;" href="/SystemModule/Annex/ShowFile?keyValue=' + list[j].Key + '" target="_blank"> ' + list[j].Value + '</a>';
                            }
                            htmlHistory += '</p>';
                        }
                    }

                    htmlHistory += '</div></div>';
                }
                $("#step").html(htmlHistory);

                if (!!data.NextNodes) {
                    var nextNodes = data.NextNodes;
                    var htmlNext = '';
                    for (var i = 0; i < nextNodes.length; i++) {
                        var nextnode = data.NextNodes[i];
                        var html_nodename = '<li class="bui-btn bui-box">' + nextnode.name + '</li>';
                        if (nextnode.comfirm_node == 1) {
                            html_nodename = '\
<li class="bui-btn bui-box">\
    <input id="node_' + nextnode.id + '" type="checkbox" class="bui-checkbox" name="nextnode" value="' + nextnode.id + '" />\
    <div class="span1">\
        <label for="node_' + nextnode.id + '">' + nextnode.name + '</label>\
    </div>\
</li>';
                        }
                        else if (nextnode.comfirm_node == 2) {
                            html_nodename = '\
<li class="bui-btn bui-box">\
    <input id="node_' + nextnode.id + '" type="radio" class="bui-radio" name="nextnode" value="' + nextnode.id + '" />\
    <div class="span1">\
        <label for="node_' + nextnode.id + '">' + nextnode.name + '</label>\
    </div>\
</li>';
                        }

                        var htmcheckall = '';  // 多个人且多选 ，有全选按钮
                        if (nextnode.comfirm_user == 1 && nextnode.authusers.length > 2 && nextnode.user_num != 1) {
                            htmcheckall = '\
<li class="bui-btn bui-box">\
    <input type="checkbox" class="bui-checkbox checkalluser" id="checkall_'+ nextnode.id + '" nodeid="' + nextnode.id + '"  />\
    <div class="span1">\
        <label for="checkall_'+ nextnode.id + '">全选</label>\
    </div>\
</li>';
                        }

                        htmlNext += '<div class="layui-card" nodeid="' + nextnode.id + '"><div class="layui-card-header">' + html_nodename + htmcheckall + '</div>';
                        htmlNext += '<div class="layui-card-body">';

                        // 用户
                        if (nextnode.authusers.length > 0) {
                            htmlNext += '<div class="bui-fluid-space-2">';
                            for (var j = 0; j < nextnode.authusers.length; j++) {
                                var authuser = nextnode.authusers[j];
                                var html_username = '<div class="span1" style="text-align: center;padding:5px;">' + authuser.objectnames + '</div>';
                                if (nextnode.comfirm_user == 1) {
                                    var inputtype = (nextnode.user_num == 1 ? "radio" : "checkbox");
                                    html_username = '\
<div class="span1" style="padding:5px;">\
    <input type="'+ inputtype + '" class="bui-check"  id="user_' + authuser.objectids + '" name="nodeuser_' + nextnode.id + '" nodeid="' + nextnode.id + '"  value="' + authuser.objectids + '"  lay-filter="nodeuser"   uncheck="' + authuser.objectnames + '" check="' + authuser.objectnames + '" />\
</div>';
                                }
                                htmlNext += html_username;
                            }
                            htmlNext += '</div>';
                        }
                        else if (nextnode.authusers.length == 0) { // 没有人就自己选择
                            htmlNext += '<div class="bui-btn primary small btnSelectUser" style="margin:10px;" usernumber="' + nextnode.usernumber + '"  nextnodeid="' + nextnode.id + '"  >选择用户</div><div class="bui-fluid-space-2 divUsers"></div>';
                        }

                        htmlNext += '</div></div>';
                    }
                    $("#ulNext").html(htmlNext);
                    $("#divNext").show();

                    if (!!htmlNext) {
                        // 选人
                        var btns = $(".btnSelectUser");
                        if (!!btns && btns.length > 0) {
                            btns.each(function () {
                                $(this).click(function () {
                                    var $btn = $(this);
                                    var radio = $btn.parent().parent().find("input[name=nextnode]");
                                    if (radio.length > 0) {
                                        if (!radio.is(":checked")) {
                                            radio.prop("checked", true);
                                        }
                                        if (radio.attr("type") == "radio") {
                                            var siblings = radio.parents("div.layui-card").siblings();
                                            siblings.find('input[lay-filter="nodeuser"]').prop("checked", false);
                                        }
                                    }
                                    var usernumber = $btn.attr("usernumber");
                                    var nextnodeid = $btn.attr("nextnodeid");
                                    var dialogobj = luckyumobile.mobileUserSelect.init({
                                        multiple: usernumber == 1 ? false : true,
                                        callback: function (userlist) {
                                            if (!!userlist && userlist.length > 0) {
                                                var htm = '';
                                                for (var u = 0; u < userlist.length; u++) {
                                                    htm += '\
    <div class="span1">\
        <input type="checkbox" class="bui-check"  id="user_' + userlist[u].user_id + '" name="nodeuser_' + nextnodeid + '" nodeid="' + nextnodeid + '"  value="' + userlist[u].user_id + '"  lay-filter="nodeuser" uncheck="' + userlist[u].realname + '" check="' + userlist[u].realname + '" checked />\
    </div>';
                                                }
                                                $btn.next().html(htm);
                                            }
                                        }
                                    });
                                    dialogobj.open();

                                });
                            });
                        }

                        // 全选
                        $(".checkalluser").click(function () {
                            var nodeid = $(this).attr("nodeid");
                            var ischecked = $(this).is(":checked");
                            $('div.layui-card[nodeid="' + nodeid + '"] input[type=checkbox]').prop("checked", ischecked);
                        });
                        // 取消其他节点选
                        $('input[name=nextnode]').click(function () {
                            var ischecked = $(this).is(":checked");
                            var inputtyep = $(this).attr("type");
                            var value = $(this).val();
                            // radio选中，其余radio下人员全部取消选中
                            if (ischecked && inputtyep == "radio") {
                                $('div.layui-card[nodeid]').each(function (index, ele) {
                                    var nodeid = $(ele).attr("nodeid");
                                    if (nodeid != value) {
                                        $(ele).find("input").prop('checked', false);
                                    }
                                });
                            }
                            // checkbox 取消选中自身，其下人员全部取消选中
                            if (!ischecked && inputtype == "checkbox") {
                                $('div.layui-card[nodeid="' + value + '"] input').prop('checked', false);
                            }
                        });
                        // 勾选下面人员自动勾选节点
                        $('input[lay-filter=nodeuser]').click(function (e) {
                            var ischecked = $(this).is(":checked");
                            if (ischecked) {
                                var radioorcheckbox = $(this).parents("div.layui-card-body").prev().find("input[name=nextnode]");
                                if (radioorcheckbox.length > 0) {
                                    radioorcheckbox.prop("checked", true);
                                }
                                if (radioorcheckbox.attr("type") == "radio") {
                                    var siblings = radioorcheckbox.parents("div.layui-card").siblings();
                                    siblings.find('input[lay-filter="nodeuser"]').prop("checked", false);
                                }
                            }
                        });

                    }

                }

            });
        }
    };
    page.init();

};
