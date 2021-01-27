/*
 *  公司
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
                url: luckyu.rootUrl + "/OrganizationModule/Company/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'company_id', hidden: true, key: true },
                    { name: 'companycode', label: "公司编码", width: 60, },
                    { name: 'fullname', label: "公司名称", width: 120, },
                    { name: 'shortname', label: "简称", width: 80, },
                    { name: 'legalperson', label: "法人", width: 60, },
                    { name: 'email', label: "Email", width: 60, },
                    { name: 'phone', label: "电话", width: 60, },
                    {
                        name: 'foundeddate', label: "成立时间", width: 60,
                        formatter: "date", formatoptions: { newformat: 'Y-m-d' },
                        searchoptions: {
                            dataInit: function (elem) {
                                $(elem).luckyurangedate({
                                    dfvalue: false,
                                    select: function (begin, end) {
                                        grid[0].triggerToolbar();
                                    }
                                });
                            }
                        }
                    },
                    { name: 'sort', label: "排序", width: 40, search: false },
                    { name: 'remark', label: "备注", width: 80, },
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
                ExpandColumn: "fullname",
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
                    id: "Form",
                    title: "新增",
                    width: 1000,
                    height: 650,
                    url: luckyu.rootUrl + "/OrganizationModule/Company/Form",
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
                    width: 1000,
                    height: 650,
                    url: luckyu.rootUrl + "/OrganizationModule/Company/Form?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Company/DeleteForm", { keyValue: rowid }, function (data, info) {
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