﻿/*
 *  菜单
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
            grid = grid.jqGrid({
                url: luckyu.rootUrl + "/OrganizationModule/Module/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'module_id', hidden: true, key: true },
                    { name: 'modulecode', label: "菜单编码", width: 60, },
                    { name: 'modulename', label: "菜单名称", width: 100, },
                    { name: 'moduleurl', label: "地址", width: 200, },
                    { name: 'sort', label: "排序", width: 40, search: false },
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
                ExpandColumn: "modulename",
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
                    width: 800,
                    height: 450,
                    url: luckyu.rootUrl + "/OrganizationModule/Module/Form",
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
                    width: 800,
                    height: 450,
                    url: luckyu.rootUrl + "/OrganizationModule/Module/Form?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Module/DeleteForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });

            $("#setuser").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + "/OrganizationModule/UserRelation/GetUsers", { relationType: 3, objectId: rowid },
                    function (res) {
                        var userIds = res.data.map(function (obj) {
                            return obj.F_UserId;
                        });
                        luckyu.layer.userSelectForm({
                            initValue: userIds,
                            callback: function (userlist) {
                                if (!userlist || userlist.length < 1) {
                                    return;
                                }
                                var ids = userlist.map(r => r.userId).join(",");
                                luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/UserRelation/SetUsers", {
                                    relationType: 3,
                                    objectId: rowid,
                                    userIds: ids
                                }, function (data, info) {
                                    layui.notice.success(info);
                                    page.searchInCurrentPage();
                                });
                            }
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