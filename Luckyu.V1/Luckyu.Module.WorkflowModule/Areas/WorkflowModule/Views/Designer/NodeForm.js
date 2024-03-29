﻿/**
 * 节点
 * */
var saveClick;
var bootstrap = function () {
    var currentNode = top.currentModifyNode;

    var page = {
        init: function () {
            page.bind();
            page.bindUserGrid();
            page.bindFormGrid();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            switch (currentNode.type) {
                case "startround":  // 开始
                    $("#tabForm").show();
                    $("#tabSQLSuccess").show();
                    $("#tabProgram").show();
                    break;
                case "stepnode":  // 一般审批
                    $("#divStep").show();
                    $("#tabUser").show();
                    $("#tabForm").show();
                    $("#tabSQLSuccess").show();
                    $("#tabSQLFail").show();
                    $("#tabProgram").show();
                    break;
                case "auditornode":  // 传阅
                    $("#divStep").show();
                    $("#tabUser").show();
                    $("#tabForm").show();
                    $("#tabSQLSuccess").show();
                    $("#tabProgram").show();
                    break;
                case "confluencenode": // 会签
                    //$("#tabUser").show();
                    //$("#tabForm").show();
                    $("#tabSQLSuccess").show();
                    $("#tabSQLFail").show();
                    $("#tabProgram").show();
                    $("#divConfluence").show();
                    break;
                case "conditionnode":  //条件
                    $("#tabSQLCondition").show();
                    break;
                case "processnode": // 执行
                    $("#tabSQLSuccess").show();
                    $("#tabProgram").show();
                    break;
            }

            layui.form.on('radio(timeout_type)', function (cbdata) {
                if (cbdata.value == 0) {
                    $("#timeout").val("");
                }
            });
            layui.form.on('radio(confluence_type)', function (cbdata) {
                if (cbdata.value != 3) {
                    $("#confluence_rate").val("");
                }
            });

            layui.layer.close(loading);

        },
        bindUserGrid: function () {
            var gridUser = $("#gridUser").jqGrid({
                editurl: 'clientArray',
                cellsubmit: "clientArray",
                altRows: true,//隔行换色
                datatype: "local",
                colModel: [
                    {
                        name: 'objecttype', label: "类型", width: 80, editable: true,
                        edittype: 'custom',
                        editoptions: {
                            dataInit: function (ele, options) {
                                var select = $(ele).find("div.xm-select");
                                var data = [
                                    { value: "1", name: "用户" },
                                    //{ value: "2", name: "部门" },
                                    //{ value: "3", name: "公司" },
                                    { value: "4", name: "岗位" },
                                    { value: "5", name: "角色" },
                                    //{ value: "6", name: "小组" },
                                    { value: "9", name: "提交人自己" },
                                ];
                                select.initLocal({
                                    data: data,
                                    initValue: select.attr("_value"),
                                    select: function (res) {
                                        var rowid = options.rowId;
                                        var xm = xmSelect.get("#objectrange_" + rowid, true);
                                        if (res.arr.length > 0) {
                                            // 只有 岗位 角色 可以选择 同公司 同部门 分管 附加条件
                                            if (res.arr[0].value !== "9") {
                                                xm.update({ disabled: true });
                                            }
                                            else if (res.arr[0].value !== "4" && res.arr[0].value !== "5") {
                                                xm.setValue([], null, true);
                                                xm.update({ disabled: true });
                                            }
                                            else {
                                                xm.update({ disabled: false });
                                            }
                                        }
                                    }
                                });
                            },
                            custom_element: function (value, options) {
                                var html = '<div class="xm-select" _value="' + value + '"></div>';
                                return html;
                            },
                            custom_value: function (ele, operation, value) {
                                var xmselect = xmSelect.get("#" + ele.attr("id"), true);
                                return xmselect.getValue("valueStr");
                            }
                        },
                    },
                    { label: 'objectids', name: 'objectids', editable: true, hidden: true },
                    {
                        name: 'objectnames', label: "明细", width: 150, editable: true,
                        edittype: 'text',
                        editoptions: {
                            readonly: true,
                            style: "background-color:white!important; cursor: pointer!important;",
                            dataInit: function (ele, options) {
                                $(ele).on("click", function () {
                                    var rowid = options.rowId;
                                    var xmselect = xmSelect.get("#objecttype_" + rowid, true);
                                    var str = xmselect.getValue("valueStr");
                                    if (str === "1") {  // 用户
                                        luckyu.layer.userSelectForm({
                                            multiple: true,
                                            callback: function (userlist) {
                                                if (!!userlist && userlist.length > 0) {
                                                    var ids = userlist.map(r => r.userId).join(',');
                                                    var names = userlist.map(r => r.realname).join(',');
                                                    $("#objectids_" + rowid).val(ids);
                                                    $("#objectnames_" + rowid).val(names);
                                                }
                                                else {
                                                    $("#objectids_" + rowid).val('');
                                                    $("#objectnames_" + rowid).val('');
                                                }
                                            }
                                        });
                                    } else if (str === "2") {  // 部门
                                        luckyu.layer.departmentSelectForm({
                                            multiple: true,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.id).join(",");
                                                    var names = list.map(r => r.label).join(",");
                                                    $("#objectnames_" + rowid).val(names);
                                                    $("#objectids_" + rowid).val(ids);
                                                }
                                                else {
                                                    $("#objectnames_" + rowid).val('');
                                                    $("#objectids_" + rowid).val('');
                                                }
                                            }
                                        });

                                    } else if (str === "3") {  // 公司
                                        luckyu.layer.companySelectForm({
                                            multiple: true,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.id).join(",");
                                                    var names = list.map(r => r.label).join(",");
                                                    $("#objectnames_" + rowid).val(names);
                                                    $("#objectids_" + rowid).val(ids);
                                                }
                                                else {
                                                    $("#objectnames_" + rowid).val('');
                                                    $("#objectids_" + rowid).val('');
                                                }
                                            }
                                        });

                                    } else if (str === "4") {  // 岗位
                                        luckyu.layer.postSelectForm({
                                            multiple: true,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.post_id).join(",");
                                                    var names = list.map(r => r.postname).join(",");
                                                    $("#objectnames_" + rowid).val(names);
                                                    $("#objectids_" + rowid).val(ids);
                                                }
                                                else {
                                                    $("#objectnames_" + rowid).val('');
                                                    $("#objectids_" + rowid).val('');
                                                }
                                            }
                                        });

                                    } else if (str === "5") {  // 角色
                                        luckyu.layer.roleSelectForm({
                                            multiple: true,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.role_id).join(",");
                                                    var names = list.map(r => r.rolename).join(",");
                                                    $("#objectnames_" + rowid).val(names);
                                                    $("#objectids_" + rowid).val(ids);
                                                }
                                                else {
                                                    $("#objectnames_" + rowid).val('');
                                                    $("#objectids_" + rowid).val('');
                                                }
                                            }
                                        });

                                    } else if (str == "6") {  //小组
                                        luckyu.layer.groupSelectForm({
                                            multiple: true,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.group_id).join(",");
                                                    var names = list.map(r => r.groupname).join(",");
                                                    $("#objectnames_" + rowid).val(names);
                                                    $("#objectids_" + rowid).val(ids);
                                                }
                                                else {
                                                    $("#objectnames_" + rowid).val('');
                                                    $("#objectids_" + rowid).val('');
                                                }
                                            }
                                        });

                                    }
                                });
                            }
                        }
                    },
                    {
                        name: 'objectrange', label: "附加条件", width: 80, editable: true,
                        edittype: 'custom',
                        editoptions: {
                            dataInit: function (ele, options) {
                                var select = $(ele).find("div.xm-select");
                                var data = [
                                    { value: "1", name: "同一公司" },
                                    { value: "2", name: "同一部门" },
                                    { value: "3", name: "分管此部门" },
                                ];
                                select.initLocal({
                                    data: data,
                                    initValue: select.attr("_value"),
                                    select: function (res) {
                                    }
                                });
                            },
                            custom_element: function (value, options) {
                                var html = '<div class="xm-select" _value="' + value + '"></div>';
                                return html;
                            },
                            custom_value: function (ele, operation, value) {
                                var xmselect = xmSelect.get("#" + ele.attr("id"), true);
                                return xmselect.getValue("valueStr");
                            }
                        },
                    },
                    {
                        name: 'is_assist', label: "辅助审批", width: 80, editable: true,
                        edittype: 'custom',
                        editoptions: {
                            dataInit: function (ele, options) {
                                var select = $(ele).find("div.xm-select");
                                var data = [
                                    { value: "0", name: "否" },
                                    { value: "1", name: "是" },
                                ];
                                select.initLocal({
                                    data: data,
                                    initValue: select.attr("_value"),
                                    select: function (res) {
                                    }
                                });
                            },
                            custom_element: function (value, options) {
                                var html = '<div class="xm-select" _value="' + value + '"></div>';
                                return html;
                            },
                            custom_value: function (ele, operation, value) {
                                var xmselect = xmSelect.get("#" + ele.attr("id"), true);
                                return xmselect.getValue("valueStr");
                            }
                        },
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: true,
                loadonce: true,
                height: 300,
            });

            $("#addRow").click(function () {
                var rowId = luckyu.utility.newGuid()
                gridUser.addRowData(rowId, {}, "last");//新增一个空行
                gridUser.editRow(rowId, false); //进入编辑状态
            });
            $("#deleteRow").click(function () {
                var rowId = gridUser.jqGrid("getGridParam", "selrow");
                gridUser.deleteRow(rowId);
            });

            gridUser.setGridWidth(window.innerWidth - 40);
            window.onresize = function () {
                gridUser.setGridWidth(window.innerWidth - 40);
            };

        },
        bindFormGrid: function () {
            var gridForm = $("#gridForm").jqGrid({
                editurl: 'clientArray',
                cellsubmit: "clientArray",
                altRows: true,//隔行换色
                datatype: "local",
                colModel: [
                    { name: 'formname', label: "名称", width: 60, editable: true, },
                    {
                        name: 'canedit', label: '可编辑', width: 80, editable: true,
                        edittype: 'custom',
                        editoptions: {
                            dataInit: function (ele, options) {
                                var select = $(ele).find("div.xm-select");
                                var data = [
                                    { value: "0", name: "不可编辑" },
                                    { value: "1", name: "可编辑" },
                                ];
                                var val = select.attr("_value");
                                val = val != "1" ? "0" : "1";
                                select.initLocal({
                                    data: data,
                                    initValue: val,
                                });
                            },
                            custom_element: function (value, options) {
                                var html = '<div class="xm-select" _value="' + value + '"></div>';
                                return html;
                            },
                            custom_value: function (ele, operation, value) {
                                var xmselect = xmSelect.get("#" + ele.attr("id"), true);
                                return xmselect.getValue("valueStr");
                            }
                        },
                    },
                    { name: 'formurl', label: '地址', width: 150, editable: true, },
                    { name: 'mobileformurl', label: '移动端地址', width: 150, editable: true, },
                ],
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: true,
                loadonce: true,
                height: 300,
            });

            $("#addRowf").click(function () {
                var rowId = luckyu.utility.newGuid()
                gridForm.addRowData(rowId, {}, "last");//新增一个空行
                gridForm.editRow(rowId, false); //进入编辑状态
            });
            $("#deleteRowf").click(function () {
                var rowId = gridForm.jqGrid("getGridParam", "selrow");
                gridForm.deleteRow(rowId);
            });

            gridForm.setGridWidth(window.innerWidth - 40);
            window.onresize = function () {
                gridForm.setGridWidth(window.innerWidth - 40);
            };
        },
        initData: function () {
            $("#NodeForm").setFormValue(currentNode);
            if (!!currentNode.authusers && currentNode.authusers.length > 0) {
                $("#gridUser").setGridValue(currentNode.authusers);
            }
            if (!!currentNode.forms && currentNode.forms.length > 0) {
                $("#gridForm").setGridValue(currentNode.forms);
            }
        }
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        var formData = $("#NodeForm").getFormValue();
        var gridUserData = $("#gridUser").getGridValue();
        var gridFormData = $("#gridForm").getGridValue();

        for (var key in formData) {
            currentNode[key] = formData[key];
        }
        currentNode.authusers = gridUserData;
        currentNode.forms = gridFormData;

        if (!!callBack) {
            callBack();
        }
        top.layui.layer.close(layerIndex);
    };
};