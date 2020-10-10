/*
 * 弹出框插件
 */
(function (layui) {
    "use strict";
    layui.use(['layer'], function () { });

    luckyu.layer = {
        loading: function () {
            var index = layui.layer.load(0, {
                shade: [0.3, '#000']
            });
            return index;
        },
        closeLoading: function (loadIndex) {
            layui.layer.close(loadIndex);
        },
        layerConfirm: function (msg, success, cancel) {
            top.layui.layer.confirm(msg, {
                btn: ['确认', '取消'],
                title: "系统提示",
                icon: 0,
            }, function (indexo) {
                if (!!success) {
                    success(indexo);
                }
                top.layui.layer.close(indexo);
            }, function (indexo) {
                if (!!cancel) {
                    success(indexo);
                }
                top.layui.layer.close(indexo);
            });
        },
        layerForm: function (option) {
            option.width = option.width > $(window).width() ? $(window).width() - 10 : option.width;
            option.height = option.height > $(window).height() ? $(window).height() - 10 : option.height;

            var defaultOp = {
                id: option.id,
                type: 2,
                title: option.title,
                area: [option.width + 'px', option.height + 'px'],
                maxmin: option.maxmin === false ? false : true, //开启最大化最小化按钮
                content: option.url,
                closeComfirm: option.closeComfirm || false,  // 点击关闭 是否出现确认提示
                success: option.success || null,  // 加载完成触发事件
                cancel: option.cancel || null,  // 点击右上角关闭按钮时回调 
                end: option.end || null,  // 弹出层销毁时回调
                btn: null,
            };
            if (defaultOp.closeComfirm === true) {
                defaultOp.cancel = function (index, layero) {
                    top.layui.layer.confirm('确定要关闭吗！', function (indexo) {
                        layui.layer.close(index);
                        layui.layer.close(indexo);
                        if (!!option.cancel) {
                            option.cancel(index, layero);
                        }
                    });
                    return false;
                };
            }
            if (!!option.btn && option.btn.length > 0) {
                defaultOp.btn = [];
                for (var i = 0; i < option.btn.length; i++) {
                    defaultOp.btn.push(option.btn[i].name);

                    if (i < 1) {
                        defaultOp.yes = option.btn[i].callback;
                    }
                    else {
                        defaultOp["btn" + (i + 1).toString()] = option.btn[i].callback;
                    }
                }
            }

            layui.layer.open(defaultOp);
        },
        /**
        * 顶级弹出菜单
        **/
        layerFormTop: function (option) {
            option.width = option.width > $(top).width() ? $(top).width() - 10 : option.width;
            option.height = option.height > $(top).height() ? $(top).height() - 10 : option.height;

            var defaultOp = {
                id: option.id,
                type: 2,
                title: option.title,
                area: [option.width + 'px', option.height + 'px'],
                maxmin: option.maxmin === false ? false : true, //开启最大化最小化按钮
                content: option.url,
                closeComfirm: option.closeComfirm || false,  // 点击关闭 是否出现确认提示
                success: option.success || null,  // 加载完成触发事件
                cancel: option.cancel || null,  // 点击右上角关闭按钮时回调 
                end: option.end || null,  // 弹出层销毁时回调
                btn: null,
                btnDisabledAtFirst: option.btnDisabledAtFirst || false,   // 打开页面时 按钮是否不可点击  注意 一旦为true 需要手动结束loading 否则按钮无法点击 $("div.layui-layer-btn.layui-layer-btn-", parent.document).loading("hide");
            };
            if (defaultOp.closeComfirm === true) {
                defaultOp.cancel = function (index, layero) {
                    top.layui.layer.confirm('确定要关闭吗！', function (indexo) {
                        top.layui.layer.close(index);
                        top.layui.layer.close(indexo);
                        if (!!option.cancel) {
                            option.cancel(index, layero);
                        }
                    });
                    return false;
                };
            }
            if (!!option.btn && option.btn.length > 0) {
                defaultOp.btn = [];
                for (var i = 0; i < option.btn.length; i++) {
                    defaultOp.btn.push(option.btn[i].name);
                    if (i < 1) {
                        defaultOp.yes = option.btn[i].callback;
                    }
                    else {
                        defaultOp["btn" + (i + 1).toString()] = option.btn[i].callback;
                    }
                }
            }

            top.layui.layer.open(defaultOp);
            if (defaultOp.btnDisabledAtFirst === true) {
                $("div.layui-layer-btn.layui-layer-btn-", top.document).loading("show", { opacity: 1 });
            }
        },
        openTab: function (href, title, fromTabId) {
            //执行跳转
            var topLayui = parent === self ? layui : top.layui;

            var parentTabId = fromTabId;
            if (!!fromTabId || fromTabId == undefined) {
                parentTabId = luckyu.tabs.getCurrentTabId();
            }

            topLayui.index.openTabsPage(href, title, parentTabId);
        },
        /**
         * 人员选择弹框
         */
        userSelectForm: function (option) {
            var defaultOption = {
                companyId: null, // 公司范围，为空则在所有公司中选择
                multiple: true, //多选，为false，选择一个替换上一个
                initValue: [], // 初始选中数据
                callback: null // 返回值
            };
            $.extend(defaultOption, option);
            top._UserSelectFormOption = {
                companyId: defaultOption.companyId,
                multiple: defaultOption.multiple,
                initValue: defaultOption.initValue
            };
            luckyu.layer.layerFormTop({
                id: "UserSelectForm",
                title: "成员管理",
                width: 850,
                height: 550,
                url: luckyu.rootUrl + "/OrganizationModule/User/UserSelectForm",
                btn: [{
                    name: "确定",
                    callback: function (index, layero) {
                        var userlist = layero.find("iframe")[0].contentWindow.saveClick();
                        top.layui.layer.close(index);
                        defaultOption.callback(userlist);
                    }
                }]
            });

        },
        /**
         * 用户组选择弹框
         * */
        groupSelectForm: function (option) {
            var defaultOption = {
                multiple: true, //多选 
                initValue: [], // 初始选中数据
                callback: null // 返回值
            };
            $.extend(defaultOption, option);
            top.alreadyselect = defaultOption.initValue;
            luckyu.layer.layerFormTop({
                id: "GroupSelectForm",
                title: "小组选择",
                width: 650,
                height: 460,
                url: luckyu.rootUrl + "/OrganizationModule/Group/GroupSelectForm?multiple=" + defaultOption.multiple,
                btn: [{
                    name: "确定",
                    callback: function (index, layero) {
                        var groups = layero.find("iframe")[0].contentWindow.saveClick();
                        defaultOption.callback(groups);
                        top.layui.layer.close(index);
                    }
                }]
            });
        },
        /**
         * 角色选择弹框
         * */
        roleSelectForm: function (option) {
            var defaultOption = {
                multiple: true, //多选 
                initValue: [], // 初始选中数据
                callback: null // 返回值
            };
            $.extend(defaultOption, option);
            top.alreadyselect = defaultOption.initValue;
            luckyu.layer.layerFormTop({
                id: "RoleSelectForm",
                title: "角色选择",
                width: 650,
                height: 460,
                url: luckyu.rootUrl + "/OrganizationModule/Role/RoleSelectForm?multiple=" + defaultOption.multiple,
                btn: [{
                    name: "确定",
                    callback: function (index, layero) {
                        var roles = layero.find("iframe")[0].contentWindow.saveClick();
                        defaultOption.callback(roles);
                        top.layui.layer.close(index);
                    }
                }]
            });
        },
        /**
         * 岗位选择弹框
         * */
        postSelectForm: function (option) {
            var defaultOption = {
                multiple: true, //多选 
                initValue: [], // 初始选中数据
                callback: null // 返回值
            };
            $.extend(defaultOption, option);
            top.alreadyselect = defaultOption.initValue;
            luckyu.layer.layerFormTop({
                id: "PostSelectForm",
                title: "岗位选择",
                width: 650,
                height: 460,
                url: luckyu.rootUrl + "/OrganizationModule/Post/PostSelectForm?multiple=" + defaultOption.multiple,
                btn: [{
                    name: "确定",
                    callback: function (index, layero) {
                        var roles = layero.find("iframe")[0].contentWindow.saveClick();
                        defaultOption.callback(roles);
                        top.layui.layer.close(index);
                    }
                }]
            });
        },
        /**
         * 部门选择弹框
         * */
        departmentSelectForm: function (option) {
            var defaultOption = {
                multiple: true, //多选 
                initValue: [], // 初始选中数据
                callback: null // 返回值
            };
            $.extend(defaultOption, option);
            top.alreadyselect = defaultOption.initValue;
            luckyu.layer.layerFormTop({
                id: "DepartmentSelectForm",
                title: "部门选择",
                width: 450,
                height: 660,
                url: luckyu.rootUrl + "/OrganizationModule/Department/DepartmentSelectForm?multiple=" + defaultOption.multiple,
                btn: [{
                    name: "确定",
                    callback: function (index, layero) {
                        var depts = layero.find("iframe")[0].contentWindow.saveClick();
                        defaultOption.callback(depts);
                        top.layui.layer.close(index);
                    }
                }]
            });
        },
        /**
         * 公司选择弹框
         * */
        companySelectForm: function (option) {
            var defaultOption = {
                multiple: true, //多选 
                initValue: [], // 初始选中数据
                callback: null // 返回值
            };
            $.extend(defaultOption, option);
            top.alreadyselect = defaultOption.initValue;
            luckyu.layer.layerFormTop({
                id: "CompanySelectForm",
                title: "公司选择",
                width: 450,
                height: 660,
                url: luckyu.rootUrl + "/OrganizationModule/Company/CompanyForm?multiple=" + defaultOption.multiple,
                btn: [{
                    name: "确定",
                    callback: function (index, layero) {
                        var companys = layero.find("iframe")[0].contentWindow.saveClick();
                        defaultOption.callback(companys);
                        top.layui.layer.close(index);
                    }
                }]
            });
        },



    };
})(window.layui);