/*
 *  数据字典 分类
 */
var bootstrap = function (layui) {
    "use strict";
    var grid = $("#grid");
    var slectRowId = '';
    var page = {
        init: function () {
            page.initBtn();
            page.initGrid();
        },
        initGrid: function () {
            grid = grid.jqGrid({
                url: luckyu.rootUrl + "/SystemModule/Dataitem/ClassifyPage",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'dataitem_id', hidden: true, key: true },
                    { name: 'itemcode', label: "字典编码", width: 60, },
                    { name: 'itemname', label: "字典名称", width: 120, },
                    { name: 'sort', label: "排序", width: 40, search: false, align: "right" },
                    { name: 'remark', label: "备注", width: 80, },
                    {
                        name: 'is_system', label: '仅管理员编辑', width: 40, align: "center",
                        stype: "dataitem", dataitemcode: "enable", formatterdataitem: "enableshow"
                    },
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
                sortname: "sort",
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

                // treegrid
                treeGrid: true,
                treeGridModel: "adjacency",
                ExpandColumn: "itemname",
                treeReader: {
                    parent_id_field: "parent_id",
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
            $("#searchfilter").click(function () {
                grid.toggleSearchBar();
            });
            $("#reset").click(function () {
                grid.clearSearchBar();
            });

            $("#add").click(function () {
                luckyu.layer.layerFormTop({
                    id: "ClassifyForm",
                    title: "新增",
                    width: 600,
                    height: 450,
                    url: luckyu.rootUrl + "/SystemModule/Dataitem/ClassifyForm",
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
                    id: "ClassifyForm",
                    title: "修改",
                    width: 600,
                    height: 450,
                    url: luckyu.rootUrl + "/SystemModule/Dataitem/ClassifyForm?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/SystemModule/Dataitem/DeleteClassifyForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });

        },
        search: function () {
            slectRowId = '';
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