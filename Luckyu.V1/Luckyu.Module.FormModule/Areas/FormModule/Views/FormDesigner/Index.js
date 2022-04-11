/*
 *  表单设计
 */
var bootstrap = function (layui) {
    "use strict";
    var grid = $("#grid");
    var slectRowId = '';
    var formname = "表单设计 ";
    var page = {
        init: function () {
            page.initBtn();
            page.initGrid();
        },
        initGrid: function () {
            grid = grid.LuckyuGrid({
                url: luckyu.rootUrl + "/FormModule/FormDesigner/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colMenu: true,
                colModel: [
                    { name: 'form_id', hidden: true, key: true, hidedlg: true, },
                    { name: 'formcode', label: "编码", width: 80, },
                    { name: 'formname', label: "名称", width: 80, },
                    { name: 'remark', label: "备注", width: 100, },
                    { name: 'create_username', label: "创建人", width: 100, },
                    {
                        name: 'createtime', label: "创建时间", width: 100, align: "right",
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
            $("#add").click(function () {
                luckyu.layer.layerFormTop({
                    title: formname + "新增",
                    width: 1200,
                    height: 950,
                    url: luckyu.rootUrl + "/FormModule/FormDesigner/Form",
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
                    title: formname + "修改/查看-" + rowid,
                    width: 1200,
                    height: 950,
                    url: luckyu.rootUrl + "/FormModule/FormDesigner/Form?keyValue=" + rowid,
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
            //$("#delete").click(function () {
            //    var rowid = grid.getGridParam("selrow");
            //    if (!rowid) {
            //        layui.notice.error("没有选中任何行数据");
            //        return;
            //    }
            //    luckyu.layer.layerConfirm('确定要删除该数据吗？', function (con) {
            //        luckyu.ajax.postv2(luckyu.rootUrl + "/FormModule//FormDesigner/DeleteForm", { keyValue: rowid }, function (data, info) {
            //            layui.notice.success(info);
            //            page.searchInCurrentPage();
            //        });
            //    });
            //});

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
