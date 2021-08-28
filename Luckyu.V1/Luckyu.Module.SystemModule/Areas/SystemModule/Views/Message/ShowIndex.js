/*
 *  消息
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
                url: luckyu.rootUrl + "/SystemModule/Message/ShowPage",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'message_id', hidden: true, key: true },
                    { name: 'catetory', label: "分类", width: 80, },
                    { name: 'contents', label: "内容", width: 500, },
                    { name: 'send_username', label: "发送人", width: 100, },
                    {
                        name: 'sendtime', label: "发送时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:m' },
                        stype: "daterange"
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                shrinkToFit: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "sendtime",
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
