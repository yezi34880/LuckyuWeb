/*
 *  我的任务
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
                url: luckyu.rootUrl + "/WorkflowModule/Task/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: { tasktype: $("input[name=tasktype]:checked").val() },
                colModel: [
                    { name: 'history_id', hidden: true },
                    { name: 'task_id', hidden: true },
                    { name: 'flow_id', hidden: true },
                    { name: 'instance_id', hidden: true },
                    { name: 'process_id', hidden: true },
                    { name: 'nodetype', hidden: true },
                    { name: 'flowname', label: "流程名称", width: 80 },
                    { name: 'processname', label: "实例信息", width: 200 },
                    {
                        name: 'is_finished', label: "是否完结", width: 60, search: false, align: "center",
                        formatter: function (cellvalue, options, row) {
                            if (cellvalue === 1) {
                                return '<span class="label label-success">完成</span>';
                            }
                            else {
                                return '<span class="label label-primary">运行中</span>';
                            }
                        }
                    },
                    { name: 'submit_username', label: "提交人", width: 80, },
                    {
                        name: 'department_id', label: "部门", width: 80,
                        stype: "department_id"
                    },
                    {
                        name: 'createtime', label: "提交时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:m' },
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

            $("#aduit").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var row = grid.getRowData(rowid);
                var btns = [];
                if (!!row.task_id && !row.history_id) {
                    if (row.nodetype === "auditornode") {
                        btns.push({
                            name: "已阅",
                            callback: function (index, layero) {
                                layero.find("iframe")[0].contentWindow.readClick(index, function () {
                                    page.searchInCurrentPage();
                                });
                                return false;
                            }
                        });
                    }
                    else {
                        btns.push({
                            name: "审核",
                            callback: function (index, layero) {
                                layero.find("iframe")[0].contentWindow.approveClick(index, function () {
                                    page.searchInCurrentPage();
                                });
                                return false;
                            }
                        });
                        btns.push({
                            name: "加签",
                            callback: function (index, layero) {
                                layero.find("iframe")[0].contentWindow.adduserClick(index, function () {
                                    page.searchInCurrentPage();
                                });
                                return false;
                            }
                        });

                    }
                }
                luckyu.layer.layerFormTop({
                    id: "Form",
                    title: "审核/查看【" + row.flowname + "】" + row.processname,
                    width: 1300,
                    height: 850,
                    url: luckyu.rootUrl + "/WorkflowModule/Task/Form?taskId=" + row.task_id + "&instanceId=" + row.instance_id + "&processId=" + row.process_id + "&historyId=" + row.history_id,
                    btn: btns
                });
            });

            layui.form.on('radio(tasktype)', function (data) {
                page.search();
            });
        },
        search: function () {
            var tasktype = $("input[name=tasktype]:checked").val();
            slectRowId = '';
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: { tasktype: tasktype },
            }).trigger('reloadGrid');
        },
        searchInCurrentPage: function () {
            var tasktype = $("input[name=tasktype]:checked").val();
            var pageIndex = grid.getGridParam('page');
            pageIndex = !pageIndex ? 1 : pageIndex;
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: { tasktype: tasktype },
            }).trigger('reloadGrid', [{ page: pageIndex }]);
        },
    };
    page.init();
};
