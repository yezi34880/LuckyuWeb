﻿/*
 * 部门
 */
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var treeCompany;
    var slectRowId = '';
    var page = {
        init: function () {
            page.initTree();
            page.initBtn();
            page.initGrid();
        },
        initTree: function () {
            treeCompany = layui.eleTree.render({
                elem: '#treeCompany',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/OrganizationModule/Company/GetTree",
                expandOnClickNode: false,
                showLine: true,
                highlightCurrent: true,
            });
            layui.eleTree.on("nodeClick(treeCompany)", function (d) {
                $("#company").html(' - ' + d.data.currentData.label);
                page.search({ companyId: d.data.currentData.id });
            })
        },
        initGrid: function () {
            grid = $("#grid").jqGrid({
                url: luckyu.rootUrl + "/OrganizationModule/Department/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'department_id', hidden: true, key: true },
                    { name: 'departmentcode', label: "部门编号", width: 60, },
                    { name: 'fullname', label: "部门名称", width: 80, },
                    { name: 'shortname', label: "部门简称", width: 60, },
                    { name: 'email', label: "Email", width: 60, },
                    { name: 'phone', label: "联系电话", width: 60, },
                    { name: 'sort', label: "排序", width: 40, search: false },
                    //{ name: 'Manager', label: "分管", width: 150, },
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
                ExpandColumn: "fullname",
                treeReader: {
                    parent_id_field: "parent_id",
                },

            });

            grid.filterToolbar();
            grid.toggleSearchBar();
            grid.resizeGrid();
            $("#treeCompany").resizeEleTree();
            window.onresize = function () {
                grid.resizeGrid();
                $("#treeCompany").resizeEleTree();
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
                    height: 550,
                    url: luckyu.rootUrl + "/OrganizationModule/Department/Form",
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
                    height: 550,
                    url: luckyu.rootUrl + "/OrganizationModule/Department/Form?keyValue=" + rowid,
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
            $("#delete").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerConfirm('确定要删除该数据吗？', function (con) {
                    luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Department/DeleteForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });
            // 设置分管
            $("#manager").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + "/OrganizationModule/UserRelation/GetUsers", { relationType: 4, objectId: rowid },
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
                                    relationType: 4,
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
        search: function (postData) {
            if (!postData) {
                var node = treeCompany.getHighlightNode();
                if (!!node) {
                    postData = { companyId: node.id };
                }
            }
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: postData,
            }).trigger('reloadGrid');
        },
        searchInCurrentPage: function () {
            var postData = {};
            var node = treeCompany.getHighlightNode();
            if (!!node) {
                postData = { companyId: node.id };
            }
            var pageIndex = grid.getGridParam('page');
            pageIndex = !pageIndex ? 1 : pageIndex;
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: postData,
            }).trigger('reloadGrid', [{ page: pageIndex }]);
        },
    };
    page.init();

};