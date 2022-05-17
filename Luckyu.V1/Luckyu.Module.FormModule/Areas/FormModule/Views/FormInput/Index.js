/*
 *  自定义表单
 */
var bootstrap = function (layui) {
    "use strict";

    var form_id = request("form_id");
    var formdata;

    luckyu.ajax.getSync(luckyu.rootUrl + "/FormModule/FormDesigner/GetTableInfo", { form_id: form_id }, function (res) {
        if (res.code != 200 || !res.data.FormTable || !res.data.FormColumns) {
            layui.layer.alert("自定义表单出错，请联系管理员", { icon: 2 });
            return;
        }
        formdata = res.data;
    });

    if (!formdata) {
        return;
    }

    var formname = formdata.FormTable.formname;
    var gridColModel = [
        { name: 'l_id', hidden: true, key: true, hidedlg: true, },
        {
            name: 'state', label: "状态", width: 60, align: "center",
            stype: "dataitem", dataitemcode: "state", formatterdataitem: "stateshow"
        },
    ];
    for (var i = 0; i < formdata.FormColumns.length; i++) {
        var col = formdata.FormColumns[i];
        var colModel = {
            name: col.columncode,
            label: col.columnname,
            width: col.formlength * 20,
        };
        switch (col.columntype) {
            case 'date':
                colModel.stype = "daterange";
                colModel.formatter = "date";
                colModel.formatoptions = { newformat: 'Y-m-d' };
                colModel.align = "right";
                break;
            case 'datetime':
                colModel.stype = "daterange";
                colModel.formatter = "date";
                colModel.formatoptions = { newformat: 'Y-m-d H:i' };
                colModel.align = "right";
                break;
            case 'number':
                colModel.stype = "numberrange";
                colModel.align = "right";
                break;
        }
        gridColModel.push(colModel);
    }

    var grid = $("#grid");
    var slectRowId = '';
    var page = {
        init: function () {
            page.initBtn();
            page.initGrid();
        },
        initGrid: function () {
            grid = grid.LuckyuGrid({
                url: luckyu.rootUrl + "/FormModule/FormInput/Page?form_id=" + form_id,
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colMenu: true,
                colModel: gridColModel,
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
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
                    title: formname + " 新增",
                    width: 900,
                    height: 650,
                    url: luckyu.rootUrl + "/FormModule/FormInput/Form?form_id=" + form_id,
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                page.search();
                            });
                            return false;
                        }
                    }
                        //, {
                        //name: "保存并提交",
                        //callback: function (index, layero) {
                        //    layero.find("iframe")[0].contentWindow.acceptClick(index, function () {
                        //        page.search();
                        //    });
                        //    return false;
                        //}
                        //}
                    ]
                });
            });
            $("#edit").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerFormTop({
                    title: formname + " 修改/查看-" + rowid,
                    width: 900,
                    height: 650,
                    url: luckyu.rootUrl + "/FormModule/FormInput/Form?form_id=" + form_id + "&keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/FormModule/FormInput/DeleteForm?form_id=" + form_id, { keyValue: rowid }, function (data, info) {
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
