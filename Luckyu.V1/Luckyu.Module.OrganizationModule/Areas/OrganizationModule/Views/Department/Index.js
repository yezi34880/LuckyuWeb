/*
 * 部门
 */
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var treeCompany;
    var slectRowId = '';
    var formname = "部门 ";
    var page = {
        init: function () {
            $('div.split-pane').splitPane();

            page.initTree();
            page.initBtn();
            page.initGrid();
        },
        initTree: function () {
            treeCompany = layui.eleTree({
                el: '#treeCompany',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/OrganizationModule/Company/GetTree",
                expandOnClickNode: false,
                showLine: true,
                highlightCurrent: true,
            });
            treeCompany.on("click", function (d) {
                $("#company").html(' - ' + d.data.label);
                page.search({ companyId: d.data.id });
            })
        },
        initGrid: function () {
            grid = $("#grid").LuckyuGrid({
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
            $("#treeCompany").resizeEleTree();
            $(window).resize(function () {
                grid.resizeGrid();
                $("#treeCompany").resizeEleTree();
            });
            $('div.split-pane').on("dividerdragend", function () {
                grid.resizeGrid();
            });

        },
        initBtn: function () {

            $("#add").click(function () {
                luckyu.layer.layerFormTop({
                    title: formname + "新增",
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
                    title: formname + "修改",
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