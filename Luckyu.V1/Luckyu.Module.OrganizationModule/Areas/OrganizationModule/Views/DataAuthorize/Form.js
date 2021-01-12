/*
 * 数据权限 编辑/查看
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            $("#objectname,#objectnameSelect").click(function () {
                var type = xmSelect.get("#objecttype", true).getValue("valueStr");
                if (type === "0") {
                    luckyu.layer.userSelectForm({
                        multiple: false,
                        callback: function (list) {
                            if (!!list && list.length > 0) {
                                $("#objectname").val(list[0].realname);
                                $("#object_id").val(list[0].userId);
                            }
                            else {
                                $("#objectname").val("");
                                $("#object_id").val("");
                            }
                        }
                    });
                }
                else if (type === "1") {
                    luckyu.layer.postSelectForm({
                        multiple: false,
                        callback: function (list) {
                            if (list != null && list.length > 0) {
                                var ids = list.map(r => r.post_id).join(",");
                                var names = list.map(r => r.postname).join(",");
                                $("#objectname").val(names);
                                $("#object_id").val(ids);
                            }
                            else {
                                $("#objectname").val("");
                                $("#object_id").val("");
                            }
                        }
                    });
                }
                else if (type === "2") {
                    luckyu.layer.roleSelectForm({
                        multiple: true,
                        callback: function (list) {
                            if (list != null && list.length > 0) {
                                var ids = list.map(r => r.role_id).join(",");
                                var names = list.map(r => r.rolename).join(",");
                                $("#objectname").val(names);
                                $("#object_id").val(ids);
                            }
                            else {
                                $("#objectname").val("");
                                $("#object_id").val("");
                            }
                        }
                    });

                }
            });

            xmSelect.get("#objectrange", true).update({
                on: function (res) {
                    if (res.arr.length > 0) {
                        if (res.arr[0].value === 0) {
                            xmSelect.get("#seeobjecttype", true).update({ disabled: false });
                            $("#seeobjectnames").removeAttr("diaabled", "diaabled");
                            $("#seeobject_ids").removeAttr("diaabled", "diaabled");
                        }
                        else {
                            xmSelect.get("#seeobjecttype", true).setValue([]).update({ disabled: true });
                            $("#seeobjectnames").val("").attr("diaabled", "diaabled");
                            $("#seeobject_ids").val("").attr("diaabled", "diaabled");
                        }
                    }
                }
            });

            $("#seeobjectnames,#seeobjectnamesSelect").click(function () {
                var type = xmSelect.get("#seeobjecttype", true).getValue("valueStr");
                if (type === "0") {
                    luckyu.layer.userSelectForm({
                        multiple: true,
                        callback: function (list) {
                            if (list.length > 0) {
                                var ids = list.map(r => r.userId).join(",");
                                var names = list.map(r => r.realname).join(",");
                                $("#objectnames_" + rowid).val(names);
                                $("#objectids_" + rowid).val(ids);
                            }
                            else {
                                $("#seeobjectnames").val("");
                                $("#seeobject_ids").val("");
                            }
                        }
                    });

                }
                else if (type === "1") {
                    luckyu.layer.departmentSelectForm({
                        multiple: true,
                        callback: function (list) {
                            if (list != null && list.length > 0) {
                                var ids = list.map(r => r.id).join(",");
                                var names = list.map(r => r.label).join(",");
                                $("#seeobjectnames").val(names);
                                $("#seeobject_ids").val(ids);
                            }
                            else {
                                $("#seeobjectnames").val("");
                                $("#seeobject_ids").val("");
                            }
                        }
                    });

                }
                else if (type === "2") {
                    luckyu.layer.companySelectForm({
                        multiple: true,
                        callback: function (list) {
                            if (list != null && list.length > 0) {
                                var ids = list.map(r => r.id).join(",");
                                var names = list.map(r => r.label).join(",");
                                $("#seeobjectnames").val(names);
                                $("#seeobject_ids").val(ids);
                            }
                            else {
                                $("#seeobjectnames").val("");
                                $("#seeobject_ids").val("");
                            }
                        }
                    });

                }
                else if (type === "3") {
                    luckyu.layer.groupSelectForm({
                        multiple: true,
                        callback: function (list) {
                            if (list != null && list.length > 0) {
                                var ids = list.map(r => r.group_id).join(",");
                                var names = list.map(r => r.groupname).join(",");
                                $("#seeobjectnames").val(names);
                                $("#seeobject_ids").val(ids);
                            }
                            else {
                                $("#seeobjectnames").val("");
                                $("#seeobject_ids").val("");
                            }
                        }
                    });

                }
            });

            layui.layer.close(loading);
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OrganizationModule/DataAuthorize/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        debugger;
                        $('[lay-filter="DataAuthorize"]').setFormValue(data.DataAuthorize);
                    })
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $('[lay-filter="DataAuthorize"]').getFormValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/DataAuthorize/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (data) {
            keyValue = data.auth_id;
            parent.layui.layer.close(layerIndex);
            if (!!callback) {
                callback();
            }
        });
    };

};