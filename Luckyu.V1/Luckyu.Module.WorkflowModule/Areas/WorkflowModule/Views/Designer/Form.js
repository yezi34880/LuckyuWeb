/**
 * 流程
 */
var saveClick;
var bootstrap = function (layui) {

    var keyValue = request("keyValue");

    var page = {
        init: function () {
            page.bind();
            page.bindAnthorizeGrid();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();
            $("#flowtype").initDataItem({ code: "flowtype" });

            // 设计页面初始化
            $('#Designer').dfworkflow({
                openNode: function (node) {
                    if (node.type != 'endround') {
                        top.currentModifyNode = node;
                        luckyu.layer.layerFormTop({
                            id: 'NodeForm',
                            title: '节点信息【' + node.name + '】',
                            url: luckyu.rootUrl + '/WorkFlowModule/Designer/NodeForm',
                            width: 700,
                            height: 550,
                            btn: [{
                                name: "确认",
                                callback: function (index, layero) {
                                    layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                        $('#Designer').dfworkflowSet('updateNodeName', { nodeId: node.id });
                                    });
                                }
                            }]
                        });
                    }
                },
                openLine: function (line) {
                    top.currentModifyLine = line;
                    luckyu.layer.layerFormTop({
                        id: 'LineForm',
                        title: '连线信息',
                        url: luckyu.rootUrl + '/WorkFlowModule/Designer/LineForm',
                        width: 500,
                        height: 400,
                        btn: [{
                            name: "确认",
                            callback: function (index, layero) {
                                layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                    $('#Designer').dfworkflowSet('updateLineName', { lineId: line.id });
                                });
                            }
                        }]
                    });
                }
            });

            //  流程设计窗体 自适应大小
            $(".scroll-wrap").height(window.innerHeight - 102);
            window.onresize = function () {
                $(".scroll-wrap").height(window.innerHeight - 102);
            };

            layui.layer.close(loading);
        },
        bindAnthorizeGrid: function () {
            var gridAuthorize = $("#gridAuthorize").jqGrid({
                editurl: 'clientArray',
                cellsubmit: "clientArray",
                altRows: true,//隔行换色
                datatype: "local",
                colModel: [
                    {
                        name: 'objecttype', label: "类型", width: 60, editable: true,
                        edittype: 'custom',
                        editoptions: {
                            dataInit: function (ele, options) {
                                var select = $(ele).find("div.xm-select");
                                var data = [
                                    { value: "1", name: "用户" },
                                    { value: "2", name: "部门" },
                                    { value: "3", name: "公司" },
                                    { value: "4", name: "岗位" },
                                    { value: "5", name: "角色" },
                                    { value: "6", name: "小组" },
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
                    { label: 'objectids', name: 'objectids', editable: true, hidden: true },
                    {
                        name: 'objectnames', label: "明细", width: 400, editable: true,
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
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.userId).join(",");
                                                    var names = list.map(r => r.realname).join(",");
                                                    $("#objectnames_" + rowid).val(names);
                                                    $("#objectids_" + rowid).val(ids);
                                                }
                                                else {
                                                    $("#objectnames_" + rowid).val('');
                                                    $("#objectids_" + rowid).val('');
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
                    }
                ],
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: true,
                loadonce: true,
                height: 420,
            });

            $("#addRow").click(function () {
                var rowId = luckyu.utility.newGuid()
                gridAuthorize.addRowData(rowId, {}, "last");//新增一个空行
                gridAuthorize.editRow(rowId, false); //进入编辑状态
            });
            $("#deleteRow").click(function () {
                var rowId = gridAuthorize.jqGrid("getGridParam", "selrow");
                gridAuthorize.deleteRow(rowId);
            });

            gridAuthorize.setGridWidth(window.innerWidth - 20);
            window.onresize = function () {
                gridAuthorize.setGridWidth(window.innerWidth - 20);
            };

        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + '/WorkflowModule/Designer/GetFormData', { keyValue: keyValue }, function (data) {
                    $("#Workflow").setFormValue(data.Workflow);
                    $('#gridAuthorize').setGridValue(data.Authorizes);
                    var shceme = JSON.parse(data.Scheme.schemejson);
                    $('#Designer').dfworkflowSet('set', { data: shceme });
                });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formData = $("#Workflow").getFormValue();
        var gridData = $('#gridAuthorize').getGridValue();
        var schemeData = $('#Designer').dfworkflowGet();
        luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Designer/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
            strAuthorizeList: JSON.stringify(gridData),
            schemejson: JSON.stringify(schemeData)
        }, function (data) {
            keyValue = data.flow_id;
            parent.layui.layer.close(layerIndex);
            if (!!callBack) {
                callBack();
            }
        });

    };
};