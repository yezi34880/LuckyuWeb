﻿/*
 * 用户选择
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var companyId = '';  // 显示哪个公司的人员
    var userlist = {};
    var userlistselected = [];
    var multiple = true;

    if (!!top._UserSelectFormOption.initValue) {
        if (typeof (top._UserSelectFormOption.initValue) === 'string') {
            userlistselected.push(top._UserSelectFormOption.initValue);
        }
        else {
            userlistselected = top._UserSelectFormOption.initValue;
        }
    }
    if (!!top._UserSelectFormOption.companyId) {
        companyId = top._UserSelectFormOption.companyId;
    }
    if (top._UserSelectFormOption.multiple === false) {
        multiple = top._UserSelectFormOption.multiple;
    }

    // 渲染用户列表
    function renderUserlist(list) {
        var $warp = $('<div></div>');
        for (var i = 0, l = list.length; i < l; i++) {
            var item = list[i];
            var active = "";
            var imgName = "UserCard02.png";
            if (item.F_Gender == 1) {
                imgName = "UserCard01.png";
            }
            if (userlistselected.indexOf(item.user_id) != -1) {
                active = "active";
            }
            var _cardbox = "";
            _cardbox += '<div class="card-box ' + active + '" data-value="' + item.user_id + '" >';
            _cardbox += '    <div class="card-box-img">';
            _cardbox += '        <img src="/image/' + imgName + '" />';
            _cardbox += '    </div>';
            _cardbox += '    <div class="card-box-content">';
            _cardbox += '        <p>账户：' + item.loginname + '</p>';
            _cardbox += '        <p>姓名：' + item.realname + '</p>';
            _cardbox += '        <p>部门：<span data-id="' + item.department_id + '"></span></p>';
            _cardbox += '    </div>';
            _cardbox += '</div>';
            var $cardbox = $(_cardbox);
            $cardbox[0].userinfo = item;
            $warp.append($cardbox);
            top.luckyu.clientdata.getAsync('department', {
                key: item.department_id,
                callback: function (_data, op) {
                    $warp.find('[data-id="' + op.key + '"]').text(_data.name);
                }
            });
        }
        $warp.find('.card-box').on('click', function () {
            var $this = $(this);
            var userid = $this.attr('data-value');
            if ($this.hasClass('active')) {
                $this.removeClass('active');
                removeUser(userid);
                userlistselected.splice(userlistselected.indexOf(userid), 1);
            }
            else {
                if (!multiple) { // 单选时，选择一个自动取消上一个
                    $this.siblings().removeClass('active');
                    for (var z = 0; z < userlistselected.length; z++) {
                        var thisuserid = userlistselected[z];
                        removeUser(thisuserid);
                    }
                    userlistselected = [];
                }
                $this.addClass('active');
                userlistselected.push(userid);
                addUser($this[0].userinfo);
            }
        });

        $('#user_list').html($warp);
    };
    function addUser(useritem) {
        var $warp = $('#selected_user_list');
        var _html = '<div class="user-selected-box" data-value="' + useritem.user_id + '" >';
        _html += '<p><span data-id="' + useritem.company_id + '"></span></p>';
        _html += '<p><span data-id="' + useritem.department_id + '"></span>【' + useritem.realname + '-' + useritem.loginname + '】</p>';
        _html += '<span class="user-reomve" title="移除选中人员"></span>';
        _html += '</div>';
        $warp.append(_html);
        top.luckyu.clientdata.getAsync('department', {
            key: useritem.department_id,
            callback: function (_data, op) {
                $warp.find('[data-id="' + op.key + '"]').text(_data.name);
            }
        });
        top.luckyu.clientdata.getAsync('company', {
            key: useritem.company_id,
            callback: function (_data, op) {
                $warp.find('[data-id="' + op.key + '"]').text(_data.name);
            }
        });

        $warp.find("div.user-selected-box[data-value='" + useritem.user_id + "'] span.user-reomve").one("click", function () {
            var $divUser = $(this).parent();
            var userid = $divUser.attr('data-value');
            removeUser(userid);
            userlistselected.splice(userlistselected.indexOf(userid), 1);
            $divUser.remove();
            $('#user_list').find('[data-value="' + userid + '"]').removeClass('active');
        });
    };
    function removeUser(userid) {
        var $warp = $('#selected_user_list');
        $warp.find('[data-value="' + userid + '"]').remove();
    };

    var page = {
        initTree: function () {
            var treeDepartment = layui.eleTree.render({
                elem: '#treeDepartment',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/OrganizationModule/Department/GetTree?companyId=" + companyId,
                expandOnClickNode: false,
                highlightCurrent: true,
            });
            layui.eleTree.on("nodeClick(treeDepartment)", function (d) {
                var id = d.data.currentData.id;
                var tag = d.data.currentData.ext.tag;
                var requestParam = {
                    organizationId: id,
                    organizationTag: tag
                };
                if (!!userlist[id]) {
                    renderUserlist(userlist[id]);
                }
                else {
                    luckyu.ajax.get(luckyu.rootUrl + "/OrganizationModule/User/GetUserByCompanyDept", requestParam, function (res) {
                        if (res.code === 200) {
                            userlist[id] = res.data || [];
                            renderUserlist(userlist[id]);
                        }
                    });
                }
            });

            $("#treeDepartment").resizeEleTree();
            window.onresize = function () {
                $("#treeDepartment").resizeEleTree();
            };
        },
        bind: function () {
            // 已选人员按钮
            $('#user_selected_btn').on('click', function () {
                $('#form_warp_right').animate({ right: '0px' }, 300);
            });
            $('#user_selected_btn_close').on('click', function () {
                $('#form_warp_right').animate({ right: '-180px' }, 300);
            });

            $("#search").click(function () {
                page.search()
            });

            $("#keyword").keydown(function (event) {
                if (event.keyCode == 13) {
                    page.search();
                    return false;
                }
            });
        },
        search: function () {
            var keyword = $.trim($("#keyword").val());
            if (!!keyword) {
                luckyu.ajax.get(luckyu.rootUrl + "/OrganizationModule/User/SearchUser",
                    { keyword: keyword }, function (res) {
                        if (res.code === 200) {
                            renderUserlist(res.data);
                        }
                    });
            }
        },
        init: function () {
            page.initTree();
            page.bind();

            // 初始化选中
            var $warp = $('#selected_user_list');
            for (var i = 0; i < userlistselected.length; i++) {
                var userId = userlistselected[i];
                if (!!userId) {
                    top.luckyu.clientdata.getAsync('user', {
                        key: userId,
                        callback: function (_data, op) {
                            var _html = '<div class="user-selected-box" data-value="' + op.key + '" >';
                            _html += '<p><span data-id="' + _data.companyId + '"></span></p>';
                            _html += '<p><span data-id="' + _data.departmentId + '"></span>【' + _data.name + '-' + _data.account + '】</p>';
                            _html += '<span class="user-reomve" title="移除选中人员"></span>';
                            _html += '</div>';
                            $warp.append(_html);
                            top.luckyu.clientdata.getAsync('department', {
                                key: _data.departmentId,
                                callback: function (_data1, op1) {
                                    $warp.find('[data-id="' + op1.key + '"]').text(_data1.name);
                                }
                            });
                            top.luckyu.clientdata.getAsync('company', {
                                key: _data.companyId,
                                callback: function (_data2, op2) {
                                    $warp.find('[data-id="' + op2.key + '"]').text(_data2.name);
                                }
                            });
                        }
                    });
                }
            }
            $warp.find("span.user-reomve").one("click", function () {
                var $divUser = $(this).parent();
                var userid = $divUser.attr('data-value');
                removeUser(userid);
                userlistselected.splice(userlistselected.indexOf(userid), 1);
                $divUser.remove();
                $('#user_list').find('[data-value="' + userid + '"]').removeClass('active');
            });

        },
    };
    page.init();

    saveClick = function () {
        var userInfolist = [];
        if (!!userlistselected && userlistselected.length > 0) {
            for (var i = 0; i < userlistselected.length; i++) {
                top.luckyu.clientdata.getAsync('user', {
                    key: userlistselected[i],
                    callback: function (_data, op) {
                        var userinfo = {
                            userId: op.key,
                            account: _data.account,
                            realname: _data.name,
                            companyId: _data.companyId,
                            departmentId: _data.departmentId,
                        };
                        top.luckyu.clientdata.getAsync('department', {
                            key: _data.departmentId,
                            callback: function (_data1, op1) {
                                userinfo.companyName = _data1.name;
                            }
                        });
                        top.luckyu.clientdata.getAsync('company', {
                            key: _data.companyId,
                            callback: function (_data2, op2) {
                                userinfo.departmentName = _data2.name;
                            }
                        });
                        userInfolist.push(userinfo);
                    }
                });

            }
        }

        return userInfolist ? userInfolist : [];
    };

};