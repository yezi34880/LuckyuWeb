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
                    { name: 'id', hidden: true, key: true },
                    { name: 'catetory', label: "分类", width: 80, search: false },
                    { name: 'title', label: "标题", width: 200, search: false },
                    { name: 'source', label: "来源", width: 100, search: false  },
                    { name: 'keywords', label: "关键词", width: 100, search: false  },
                    {
                        name: 'publishtime', label: "发布时间", width: 100, align: "right", search: false ,
                        formatter: "date", formatoptions: { newformat: 'Y-m-d' },
                        stype: "daterange"
                    },
                    { name: 'remark', label: "备注", width: 100, search: false }
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

            $("#show").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerFormTop({
                    id: "Form",
                    title: "查看-" + rowid,
                    width: 800,
                    height: 650,
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
