/*
 *  角色选择
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var multiple = request("multiple");
    multiple = multiple === "true" || multiple === "1" ? true : false;
    var alreadys = top.alreadyselect;
    top.alreadyselect = null;

    var grid = $("#grid");
    var page = {
        init: function () {
            page.initGrid();
        },
        initGrid: function () {
            grid = grid.jqGrid({
                url: luckyu.rootUrl + "/OrganizationModule/Role/GetRoleList",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'role_id', hidden: true, key: true },
                    { name: 'rolecode', label: "角色编码", width: 60, },
                    { name: 'rolename', label: "角色名称", width: 120, },
                    //{ name: 'sort', label: "排序", width: 40, search: false },
                    { name: 'remark', label: "备注", width: 80, },
                ],
                rownumbers: true,
                viewrecords: true,
                sortname: "sort",
                sortorder: "ASC",
                multiselect: true,
                multiboxonly: true,
                beforeSelectRow: function (rowId, evnet) {
                    if (!multiple) {
                        var jqGridRowid = grid.jqGrid("getGridParam", "selarrrow");
                        for (var i = jqGridRowid.length - 1; i > -1; i--) {
                            if (rowId !== jqGridRowid[i]) {
                                grid.setSelection(jqGridRowid[i], false);
                            }
                        }
                    }
                },
                loadComplete: function () {
                    $("#jqgh_grid_cb").hide();
                    if (!!alreadys && alreadys.length > 0) {
                        for (var i = 0; i < alreadys.length; i++) {
                            grid.setSelection(alreadys[i]);
                        }
                    }
                }
            });
            //grid.filterToolbar();
            //grid.toggleSearchBar();
            grid.resizeGrid();
            window.onresize = function () {
                grid.resizeGrid();
            };
        },
        search: function () {
            slectRowId = '';
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: {},
            }).trigger('reloadGrid');
        },
    };
    page.init();

    saveClick = function (layerIndex) {
        var ids = grid.jqGrid('getGridParam', 'selarrrow');
        var rows = [];
        for (var i = 0; i < ids.length; i++) {
            var rowData = grid.jqGrid('getRawRowData', ids[i]);
            rows.push(rowData);
        }
        parent.layui.layer.close(layerIndex);
        return rows;
    }
};