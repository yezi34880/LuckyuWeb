/*
 *  小组
 */
var bootstrap = function (layui) {
    "use strict";
    var grid = $("#grid");
    var slectRowId = '';
    var formname = "小组 ";
    var page = {
        init: function () {
            page.initBtn();
            page.initGrid();
        },
        initGrid: function () {
            grid = grid.LuckyuGrid({
                url: luckyu.rootUrl + "/OrganizationModule/Group/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'group_id', hidden: true, key: true },
                    { name: 'groupcode', label: "小组编码", width: 60, },
                    { name: 'groupname', label: "小组名称", width: 120, },
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
                    width: 600,
                    height: 450,
                    url: luckyu.rootUrl + "/OrganizationModule/Group/Form",
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
                    width: 600,
                    height: 450,
                    url: luckyu.rootUrl + "/OrganizationModule/Group/Form?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Group/DeleteForm", { keyValue: rowid }, function (data, info) {
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