/*
 * 流程
 */
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var slectRowId = '';
    var page = {
        init: function () {
            page.initBtn();
            page.initGrid();
        },
        initGrid: function () {
            grid = $("#grid").jqGrid({
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
                        stype: "select",
                        searchoptions: {
                            value: function () {
                                var selectoption = { "-1": "全部" };
                                luckyu.clientdata.getAllAsync('dataItem', {
                                    code: 'flowtype',
                                    callback: function (_datas) {
                                        for (var key in _datas) {
                                            selectoption[_datas[key].value] = _datas[key].name;
                                        }
                                    }
                                });
                                return selectoption;
                            },
                            defaultValue: "-1"
                        }
                    },
                    { name: 'remark', label: "备注", width: 80, },
                    {
                        name: 'is_enable', label: '有效', width: 40,
                        stype: "select",
                        searchoptions: {
                            value: { "-1": "全部", "1": "有效", "0": "无效" },
                            defaultValue: "-1"
                        },
                        formatter: function (cellvalue, options, rowObject) {
                            switch (cellvalue) {
                                case 1: return '<i class="fa fa-toggle-on"></i>';
                                case 0: return '<i class="fa fa-toggle-off"></i>';
                                default: return '';
                            }
                        }
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
                    id: "Form",
                    title: "修改",
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