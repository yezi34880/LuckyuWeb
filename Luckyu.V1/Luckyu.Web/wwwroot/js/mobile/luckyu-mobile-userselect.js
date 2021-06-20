(function () {

    var buildHtml = function (data, multiple) {
        var html = '';
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            var name = item.realname + "-" + item.loginname;
            html += '\
<li class="bui-btn bui-box">\
    <div class="span1">\
        <label for="user_'+ item.user_id + '">' + name + '</label>\
    </div>\
    <input id="user_'+ item.user_id + '" type="' + (multiple ? "checkbox" : "radio") + '" class="bui-choose" name="users" value="' + item.user_id + '"  luckyu-name="' + item.realname + '" luckyu-loginname="' + item.loginname + '" />\
</li> ';
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
<ul class="bui-list" id="du_userlist">\
</ul>',
                onInited: function (option) {
                    luckyumobile.ajax.getv2("/OrganizationModule/User/GetUserByCompanyDept", {}, function (data) {
                        if (!!data && data.length > 0) {
                            var html = buildHtml(data, defaultOption.multiple);
                            $("#du_userlist").html(html);
                            $("#du_userlist")[0].backdata = data;
                        }
                    });

                    $("#du_search").click(function () {
                        var keyword = $("#du_keyword").val();
                        var userlist = $("#du_userlist")[0].backdata;
                        var newlist = userlist;
                        if (!!keyword && keyword.length > 0) {
                            newlist = userlist.filter(function (t) {
                                return t.realname.indexOf(keyword) > -1 || t.loginname.indexOf(keyword) > 0;
                            });
                        }

                        var html = '';
                        var html = buildHtml(newlist, defaultOption.multiple);
                        $("#du_userlist").html(html);
                    });
                },
                buttons: [{ name: "确定", className: "primary-reverse" }, "取消"],
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
                                        loginname: that.attr("luckyu-loginname")
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
