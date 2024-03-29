﻿/*
 * 数据字典
 */
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var treeDataitem;
    var slectRowId = '';
    var formname = "数据字典 ";
    var page = {
        init: function () {
            $('div.split-pane').splitPane();
            page.initTree();
            page.initBtn();
            page.initGrid();
        },
        initTree: function () {
            treeDataitem = layui.eleTree({
                el: '#treeDataitem',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/SystemModule/Dataitem/GetTree",
                expandOnClickNode: false,
                showLine: true,
                highlightCurrent: true,
            });
            $("#keyword").on("input", function (e) {
                var val = e.currentTarget.value;
                treeDataitem.search(val, function (value, data) {
                    if (!value) return true;
                    return data.label.indexOf(value) > -1 || data.ext.code.indexOf(value) > -1;
                });
            });

            treeDataitem.on("click", function (d) {
                $("#show").html(' - ' + d.data.label + '【' + d.data.ext.code + '】');
                page.search({ classifyId: d.data.id });
            })
        },
        initGrid: function () {
            grid = $("#grid").LuckyuGrid({
                url: luckyu.rootUrl + "/SystemModule/Dataitem/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'detail_id', hidden: true, key: true },
                    { name: 'itemcode', label: "编码", width: 60, },
                    { name: 'showname', label: "显示值", width: 80, },
                    { name: 'itemvalue', label: "实际值", width: 60, },
                    { name: 'itemvalue2', label: "实际值2", width: 60, },
                    { name: 'remark', label: "备注", width: 80, },
                    { name: 'sort', label: "排序", width: 40, search: false },
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
            });

            grid.filterToolbar();
            grid.toggleSearchBar();
            grid.resizeGrid();
            $("#treeDataitem").resizeEleTree();
            $(window).resize(function () {
                grid.resizeGrid();
                $("#treeDataitem").resizeEleTree();
            });
            $('div.split-pane').on("dividerdragend", function () {
                grid.resizeGrid();
            });
        },
        initBtn: function () {

            $("#add").click(function () {
                var node = treeDataitem.getHighlightNode();
                if (!node.id) {
                    layui.notice.error("请选择字典分类");
                    return;
                }
                var dataitem_id = node.id;
                var itemcode = node.ext.code;
                luckyu.layer.layerFormTop({
                    title: formname + s"新增",
                    width: 800,
                    height: 550,
                    url: luckyu.rootUrl + "/SystemModule/Dataitem/Form?dataitem_id=" + dataitem_id + "&itemcode=" + itemcode,
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
                    url: luckyu.rootUrl + "/SystemModule/Dataitem/Form?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/SystemModule/Dataitem/DeleteForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });
        },
        search: function (postData) {
            if (!postData) {
                var node = treeDataitem.getHighlightNode();
                if (!!node) {
                    postData = { classifyId: node.id };
                }
            }
            grid.jqGrid('resetSelection');
            grid.jqGrid('setGridParam', {
                postData: postData,
            }).trigger('reloadGrid');
        },
        searchInCurrentPage: function () {
            var postData = {};
            var node = treeDataitem.getHighlightNode();
            if (!!node) {
                postData = { classifyId: node.id };
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