/**
 * 设置分管
 * */
var saveClick;
var bootstrap = function (layui) {
    var userId = request("userId");
    var page = {
        init: function () {
            page.initGrid();
            page.initData();
        },
        initGrid: function () {
            var gridManage = $("#gridManage").jqGrid({
                editurl: 'clientArray',
                cellsubmit: "clientArray",
                altRows: true,//隔行换色
                datatype: "local",
                colModel: [
                    { label: 'id', name: 'id', key: true, editable: true, hidden: true },
                    {
                        name: 'relationtype', label: "类型", width: 100, editable: true,
                        edittype: 'custom',
                        editoptions: {
                            dataInit: function (ele, options) {
                                var select = $(ele).find("div.xm-select");
                                var data = [
                                    { value: "1", name: "角色" },
                                    { value: "2", name: "岗位" },
                                ];
                                select.initLocal({
                                    data: data,
                                    initValue: select.attr("_value"),
                                    select: function (res) {
                                        var rowid = options.rowId;
                                        $("#objectnames_" + rowid).val('');
                                        $("#objectids_" + rowid).val('');
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
                    { label: 'object_id', name: 'object_id', editable: true, hidden: true },
                    {
                        name: 'objectname', label: "类型名称", width: 100, editable: true,
                        edittype: 'text',
                        editoptions: {
                            readonly: true,
                            style: "background-color:white!important; cursor: pointer!important;",
                            dataInit: function (ele, options) {
                                $(ele).on("click", function () {
                                    var rowid = options.rowId;
                                    var xmselect = xmSelect.get("#relationtype_" + rowid, true);
                                    var str = xmselect.getValue("valueStr");
                                    if (str === "1") {  // 角色
                                        luckyu.layer.roleSelectForm({
                                            multiple: false,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    $("#objectname_" + rowid).val(list[0].rolename);
                                                    $("#object_id_" + rowid).val(list[0].role_id);
                                                }
                                                else {
                                                    $("#objectname_" + rowid).val('');
                                                    $("#object_id_" + rowid).val('');
                                                }
                                            }
                                        });
                                    } else if (str === "2") {  // 岗位
                                        luckyu.layer.postSelectForm({
                                            multiple: false,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    $("#objectname_" + rowid).val(list[0].postname);
                                                    $("#object_id_" + rowid).val(list[0].post_id);
                                                }
                                                else {
                                                    $("#objectname_" + rowid).val('');
                                                    $("#object_id_" + rowid).val('');
                                                }
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    },
                    { label: 'department_id', name: 'department_id', editable: true, hidden: true },
                    {
                        name: 'departmentname', label: "部门名称", width: 350, editable: true,
                        edittype: 'text',
                        editoptions: {
                            readonly: true,
                            style: "background-color:white!important; cursor: pointer!important;",
                            dataInit: function (ele, options) {
                                $(ele).on("click", function () {
                                    var rowid = options.rowId;
                                    luckyu.layer.departmentSelectForm({
                                        multiple: true,
                                        callback: function (list) {
                                            if (list != null && list.length > 0) {
                                                var ids = list.map(r => r.id).join(",");
                                                var names = list.map(r => r.label).join(",");
                                                $("#departmentname_" + rowid).val(names);
                                                $("#department_id_" + rowid).val(ids);
                                            }
                                            else {
                                                $("#departmentname_" + rowid).val("");
                                                $("#department_id_" + rowid).val("");
                                            }
                                        }
                                    });
                                });
                            }
                        }
                    }
                ],
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: true,
                loadonce: true,
                height: 330,
            });

            $("#addRow").click(function () {
                var rowId = luckyu.utility.newGuid()
                gridManage.addRowData(rowId, {}, "last");//新增一个空行
                gridManage.editRow(rowId, false); //进入编辑状态
            });
            $("#deleteRow").click(function () {
                var rowId = gridManage.jqGrid("getGridParam", "selrow");
                gridManage.deleteRow(rowId);
            });

            gridManage.setGridWidth(window.innerWidth - 20);
            $(window).resize(function () {
                gridManage.setGridWidth(window.innerWidth - 20);
            });
        },
        initData: function () {
            if (!!userId) {
                luckyu.ajax.getv2("/OrganizationModule/DepartmentManage/GetFormData", { userId: userId }, function (data) {
                    $("#gridManage").setGridValue(data.DepartmentManage);
                });
            }
        }
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var gridData = $("#gridManage").getGridValue();
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/DepartmentManage/SaveForm", {
            userId: userId,
            strList: JSON.stringify(gridData),
        }, function (data) {
            parent.layui.layer.close(layerIndex);
            if (!!callBack) {
                callBack();
            }
        });
    };
};