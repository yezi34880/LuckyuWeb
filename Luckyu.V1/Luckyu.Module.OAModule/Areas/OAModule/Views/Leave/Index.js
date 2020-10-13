/*
 *  请假
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
                url: luckyu.rootUrl + "/OAModule/Leave/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    {
                        name: 'state', label: "状态", width: 60, align: "center",
                        stype: "dataitem", dataitemcode: "state", formatterdataitem: "stateshow"
                    },
                    { name: 'id', label: "流水号", width: 130, key: true },
                    {
                        name: 'user_id', label: "用户", width: 80,
                        stype: "user_id",
                    },
                    {
                        name: 'department_id', label: "部门", width: 80,
                        stype: "department_id"
                    },
                    {
                        name: 'company_id', label: "公司", width: 80,
                        stype: "company_id"
                    },
                    {
                        name: 'begintime', label: "开始时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:m' },
                        stype: "daterange"
                    },
                    {
                        name: 'endtime', label: "结束时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:m' },
                        stype: "daterange"
                    },
                    {
                        name: 'spantime', label: "小时数", width: 80, align: "right",
                        stype: "numberrange"
                    },
                    {
                        name: 'leavetype', label: "类型", width: 80,
                        stype: "dataitem", dataitemcode: "leavetype",
                    },
                    { name: 'reason', label: "请假事由", width: 150, },
                    {
                        name: 'createtime', label: "创建时间", width: 100, align: "right",
                        formatter: "date", formatoptions: { newformat: 'Y-m-d H:m:s' },
                        stype: "daterange"
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "begintime",
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
                    width: 800,
                    height: 650,
                    url: luckyu.rootUrl + "/OAModule/Leave/Form",
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                page.search();
                            });
                            return false;
                        }
                    }, {
                        name: "保存并提交",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.acceptClick(index, function () {
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
                    title: "修改/查看-" + rowid,
                    width: 800,
                    height: 650,
                    url: luckyu.rootUrl + "/OAModule/Leave/Form?keyValue=" + rowid,
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                page.searchInCurrentPage();
                            });
                            return false;
                        }
                    }, {
                        name: "保存并提交",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.acceptClick(index, function () {
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/OAModule/Leave/DeleteForm", { keyValue: rowid }, function (data, info) {
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
