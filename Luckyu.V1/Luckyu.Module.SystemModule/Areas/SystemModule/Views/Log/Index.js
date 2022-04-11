/*
 *  单据编码
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
                url: luckyu.rootUrl + "/SystemModule/Log/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: { logtype: $("input[name=log_type]:checked").val() },
                colModel: [
                    { name: 'log_id', hidden: true, key: true },
                    { name: 'op_type', label: "操作", width: 50, },
                    { name: 'user_name', label: "用户", width: 80, },
                    {
                        name: 'log_time', label: "时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:i' },
                        searchoptions: {
                            // 只能选择月份
                            dataInit: function (elem) {
                                $(elem).focus(function () {
                                    WdatePicker({
                                        el: this,
                                        dateFmt: 'yyyy-MM',
                                        onpicked: function (dp) {
                                            grid[0].triggerToolbar();
                                            $(elem).blur();
                                        },
                                        oncleared: function (dp) {
                                            grid[0].triggerToolbar();
                                            $(elem).blur();
                                        }
                                    });
                                });
                            }
                        }
                    },
                    { name: 'module', label: "模块", width: 150, },
                    { name: 'log_content', label: "内容", width: 500, search: false },
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "log_time",
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
            $("#show").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var rowData = grid.getRowData(rowid);
                luckyu.layer.layerFormTop({
                    title: "日志查看",
                    width: 900,
                    height: 600,
                    url: luckyu.rootUrl + "/SystemModule/Log/Show?keyValue=" + rowid + "&date=" + rowData.log_time,
                });
            });

            layui.form.on('radio(log_type)', function (data) {
                page.search();
            });

        },
        search: function () {
            var logtype = $("input[name=log_type]:checked").val();
            slectRowId = '';
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: { logtype: logtype },
            }).trigger('reloadGrid');
        },
    };
    page.init();
};
