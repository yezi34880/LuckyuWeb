(function () {

    var buildHtml = function (data, multiple) {
        var html = '';
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            html += '\
<dt class="bui-btn bui-box" style="background-color:rgb(243, 245, 248);">\
	<div class="span1">'+ item.Key.fullname + '</div>\
	<i class="icon-accordion"></i>\
</dt>';
            html += '<dd class="bui-list">'

            for (var j = 0; j < item.ValueList.length; j++) {
                var user = item.ValueList[j];
                var name = user.realname + "-" + user.loginname;
                html += '\
                <div class="bui-btn bui-box">\
                    <div class="span1">\
                        <label for="usersel_'+ user.user_id + '">' + name + '</label>\
                    </div>\
                    <input id="usersel_'+ user.user_id + '" type="' + (multiple ? "checkbox" : "radio") + '" class="bui-choose" name="users" value="' + user.user_id + '"  luckyu-name="' + user.realname + '" luckyu-loginname="' + user.loginname + '" luckyu-departmentid="' + item.Key.department_id+'" luckyu-departmentname="' + item.Key.fullname+'" />\
                </div> ';

            }
            html += '</dd>'

        }
        return html;
    };

    var mobileUserSelect = {
        init: function (op) {
            var defaultOption = {
                multiple: false,
                callback: null,
            };
            $.extend(defaultOption, op);

            var dialog = bui.dialog();
            // 先创建再打开
            var dialogobj = dialog.create({
                title: "用户选择",
                render: true,
                fullscreen: true,
                content: '\
<div class="bui-searchbar bui-box">\
    <div class="span1">\
        <div class="bui-input">\
            <i class="icon-search"></i>\
            <input type="search" id="du_keyword" value="" placeholder="请输入关键字" />\
        </div>\
    </div>\
    <div class="btn-search" id="du_search">搜索</div>\
</div>\
<dl class="bui-accordion"  id="du_userlist">\
</dl>',
                onInited: function (option) {
                    luckyumobile.ajax.getv2("/OrganizationModule/User/GetDepartmentUsers", {}, function (data) {
                        if (!!data && data.length > 0) {
                            var html = buildHtml(data, defaultOption.multiple);
                            $("#du_userlist").html(html);
                            $("#du_userlist")[0].backdata = data;

                            var uiAccordion = bui.accordion({
                                id: "#du_userlist"
                            });

                        }
                    });

                    $("#du_keyword").keypress(function (e) {
                        if (e.charCode == 13) {
                            $("#du_search").click();
                        }
                    });

                    $("#du_search").click(function () {
                        var keyword = $("#du_keyword").val();
                        var datalist = $("#du_userlist")[0].backdata;
                        var newlist = [];
                        if (!!keyword && keyword.length > 0) {
                            for (var z = 0; z < datalist.length; z++) {
                                if (datalist[z].users.length > 0) {
                                    var users = datalist[z].users.filter(function (t) {
                                        return t.F_RealName.indexOf(keyword) > -1 || t.F_Account.indexOf(keyword) > 0;
                                    });
                                    if (users.length > 0) {
                                        newlist.push({
                                            dept: datalist[z].dept,
                                            users: users
                                        });
                                    }
                                }
                            }
                        }
                        else {
                            newlist = datalist;
                        }
                        var html = '';
                        var html = buildHtml(newlist, defaultOption.multiple);
                        $("#du_userlist").html(html);

                        var uiAccordion = bui.accordion({
                            id: "#du_userlist"
                        });
                        uiAccordion.showAll();

                    });
                },
                buttons: ["取消", { name: "确定", className: "primary-reverse" }],
                callback: function (elem, option) {
                    //this 指点击的按钮
                    if (elem.target.innerText === "确定") {
                        if (!!defaultOption.callback) {
                            var selects = $('input[name=users]:checked');
                            var resultdata = [];
                            if (!!selects && selects.length > 0) {
                                for (var i = 0; i < selects.length; i++) {
                                    var that = $(selects[i]);
                                    resultdata.push({
                                        user_id: that.val(),
                                        realname: that.attr("luckyu-name"),
                                        loginname: that.attr("luckyu-loginname"),
                                        departmentid: that.attr("luckyu-departmentid"),
                                        departmentname: that.attr("luckyu-departmentname"),
                                    });
                                }
                            }
                            defaultOption.callback(resultdata);
                        }
                    }

                    dialogobj.destroy();
                }
            });

            return dialogobj;
        },
    };
    window.luckyumobile.mobileUserSelect = mobileUserSelect;

})();
