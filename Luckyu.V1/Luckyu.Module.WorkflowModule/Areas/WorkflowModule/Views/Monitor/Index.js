/*
 *  流程监控
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
                url: luckyu.rootUrl + "/WorkflowModule/Monitor/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: { is_finished: $("input[name=is_finished]:checked").val() },
                colModel: [
                    { name: 'instance_id', hidden: true, key: true },
                    { name: 'flow_id', hidden: true },
                    { name: 'process_id', hidden: true },
                    { name: 'flowname', label: "流程名称", width: 120 },
                    { name: 'processname', label: "实例信息", width: 150 },
                    {
                        name: 'submit_username', label: "提交人", width: 80,
                    },
                    {
                        name: 'createtime', label: "提交时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:i' },
                        stype: "daterange"
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "createtime",
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

            layui.form.on('radio(is_finished)', function (data) {
                if (data.value == 0) {
                    $("#divAction").show();
                }
                else {
                    $("#divAction").hide();
                }
                page.search();
            });

            $("#see").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var row = grid.getRowData(rowid);
                luckyu.layer.layerFormTop({
                    title: "查看【" + row.flowname + "】" + row.processname,
                    width: 1300,
                    height: 850,
                    url: luckyu.rootUrl + "/WorkflowModule/Task/Form?instanceId=" + row.instance_id + "&processId=" + row.process_id
                });
            });

            $("#finish").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var row = grid.getRowData(rowid);
                luckyu.layer.layerConfirm("确定退回该流程？", function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Monitor/Finish', { instanceId: row.instance_id }, function (data) {
                        layui.notice.success("操作成功");
                        page.searchInCurrentPage();
                    });
                });
            });
            $("#sleep").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var row = grid.getRowData(rowid);
                luckyu.layer.layerConfirm("确定挂起该流程？", function () {
                    luckyu.ajax.postv2(luckyu.rootUrl + '/WorkflowModule/Monitor/Sleep', { instanceId: row.instance_id }, function (data) {
                        layui.notice.success("操作成功");
                        page.searchInCurrentPage();
                    });
                });
            });

            // 调整流程
            $("#modify").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var row = grid.getRowData(rowid);
                luckyu.layer.layerFormTop({
                    title: "调整流程",
                    width: 1300,
                    height: 850,
                    url: luckyu.rootUrl + "/WorkflowModule/Monitor/ModifyForm?instanceId=" + row.instance_id + "&processId=" + row.process_id,
                });
            });

        },
        search: function () {
            var is_finished = $("input[name=is_finished]:checked").val();
            slectRowId = '';
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: { is_finished: is_finished },
            }).trigger('reloadGrid');
        },
        searchInCurrentPage: function () {
            var is_finished = $("input[name=is_finished]:checked").val();
            var pageIndex = grid.getGridParam('page');
            pageIndex = !pageIndex ? 1 : pageIndex;
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: { is_finished: is_finished },
            }).trigger('reloadGrid', [{ page: pageIndex }]);
        },
    };
    page.init();
};
