/*
 *  新闻通知
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
                url: luckyu.rootUrl + "/OAModule/News/ShowPage",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'news_id', hidden: true, key: true },
                    { name: 'category', label: "分类", width: 80 },
                    {
                        name: 'title', label: "标题", width: 400,
                        formatter: function (cellvalue, option, row) {
                            var result = cellvalue;
                            if (row.sort > 0) {
                                result = '<i class="fa fa-star"></i> ' + result;
                            }
                            return result;
                        }
                    },
                    { name: 'source', label: "来源", width: 100 },
                    { name: 'keywords', label: "关键词", width: 100 },
                    {
                        name: 'publishtime', label: "发布时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d' },
                        stype: "daterange"
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                //sortname: "publishtime",
                //sortorder: "DESC",
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

            $("#show").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var row = grid.getRowData(rowid);
                luckyu.layer.layerFormTop({
                    title: row.title,
                    width: 1300,
                    height: 850,
                    url: luckyu.rootUrl + "/OAModule/News/Show?keyValue=" + rowid,
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
