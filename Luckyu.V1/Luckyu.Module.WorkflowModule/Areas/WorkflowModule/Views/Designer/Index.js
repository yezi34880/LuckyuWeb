/*
 * 流程
 */
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var slectRowId = '';
    var formname = "流程设计 ";
    var page = {
        init: function () {
            page.initBtn();
            page.initGrid();
        },
        initGrid: function () {
            grid = $("#grid").LuckyuGrid({
                url: luckyu.rootUrl + "/WorkflowModule/Designer/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'flow_id', hidden: true, key: true },
                    { name: 'flowcode', label: "流程编码", width: 60, },
                    { name: 'flowname', label: "流程名称", width: 80, },
                    {
                        name: 'flowtype', label: "流程类型", width: 60,
                        stype: "dataitem", dataitemcode: "flowtype"
                    },
                    { name: 'remark', label: "备注", width: 80, },
                    {
                        name: 'is_enable', label: '有效', width: 40, align: "center",
                        stype: "dataitem", dataitemcode: "enable", formatterdataitem: "enableshow"
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "flowcode",
                sortorder: "ASC",
                onSelectRow: function (rowid, status) {
                    if (status) {
                        slectRowId = rowid;
                    }
                },
                gridComplete: function () {
                    if (!!slectRowId) {
                        $(this).jqGrid('setSelection', slectRowId);
                    }
                },
            });

            grid.filterToolbar();
            grid.toggleSearchBar();
            grid.resizeGrid();

            window.onresize = function () {
                grid.resizeGrid();
            };
        },
        initBtn: function () {

            $("#add").click(function () {
                luckyu.layer.layerFormTop({
                    title: formname + "新增",
                    width: 1200,
                    height: 850,
                    url: luckyu.rootUrl + "/WorkflowModule/Designer/Form",
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                page.search();
                            });
                            return false;
                        }
                    }]
                });
            });
            $("#edit").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerFormTop({
                    title: formname + "修改",
                    width: 1200,
                    height: 850,
                    url: luckyu.rootUrl + "/WorkflowModule/Designer/Form?keyValue=" + rowid,
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                page.searchInCurrentPage();
                            });
                            return false;
                        }
                    }]
                });
            });
            $("#delete").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerConfirm('确定要删除该数据吗？', function (con) {
                    luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Designer/DeleteForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });

            $("#copy").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerConfirm('确定要复制该数据吗？', function (con) {
                    luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Designer/CopyForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.search();
                    });
                });
            });
        },
        search: function () {
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: {},
            }).trigger('reloadGrid');
        },
        searchInCurrentPage: function () {
            var pageIndex = grid.getGridParam('page');
            pageIndex = !pageIndex ? 1 : pageIndex;
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: {},
            }).trigger('reloadGrid', [{ page: pageIndex }]);
        },
    };
    page.init();

};