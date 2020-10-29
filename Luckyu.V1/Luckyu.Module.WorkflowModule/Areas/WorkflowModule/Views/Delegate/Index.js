/*
 *  任务委托
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
            grid = grid.LuckyuGrid({
                url: luckyu.rootUrl + "/WorkflowModule/Delegate/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'id', hidden: true, key: true },
                    { name: 'catetory', label: "分类", width: 80, },
                    { name: 'title', label: "标题", width: 200, },
                    { name: 'source', label: "来源", width: 100, },
                    { name: 'keywords', label: "关键词", width: 100, },
                    {
                        name: 'publishtime', label: "发布时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:m' },
                        stype: "daterange"
                    },
                    {
                        name: 'is_publish', label: '是否发布', width: 40, search: false,
                        formatter: function (cellvalue, options, rowObject) {
                            switch (cellvalue) {
                                case 1: return '<i class="fa fa-toggle-on"></i>';
                                case 0: return '<i class="fa fa-toggle-off"></i>';
                                default: return '';
                            }
                        }
                    },
                    {
                        name: 'sort', label: '是否置顶', width: 40, search: false,
                        formatter: function (cellvalue, options, rowObject) {
                            switch (cellvalue) {
                                case 99: return '<i class="fa fa-toggle-on"></i>';
                                case 0: return '<i class="fa fa-toggle-off"></i>';
                                default: return '';
                            }
                        }
                    },
                    { name: 'remark', label: "备注", width: 100, }
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "publishtime",
                sortorder: "DESC",
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
            $("#searchfilter").click(function () {
                grid.toggleSearchBar();
            });
            $("#reset").click(function () {
                grid.clearSearchBar();
            });

            $("#add").click(function () {
                luckyu.layer.layerFormTop({
                    id: "Form",
                    title: "新增",
                    width: 1100,
                    height: 750,
                    url: luckyu.rootUrl + "/WorkflowModule/Delegate/Form",
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
                    id: "Form",
                    title: "修改/查看",
                    width: 1100,
                    height: 750,
                    url: luckyu.rootUrl + "/WorkflowModule/Delegate/Form?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Delegate/DeleteForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });

            $("#publish").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Delegate/Publish", { keyValue: rowid }, function (data, info) {
                    layui.notice.success(info);
                    page.searchInCurrentPage();
                });
            });
            $("#settop").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.postv2(luckyu.rootUrl + "/WorkflowModule/Delegate/SetTop", { keyValue: rowid }, function (data, info) {
                    layui.notice.success(info);
                    page.searchInCurrentPage();
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
