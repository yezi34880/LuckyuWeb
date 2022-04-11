/*
 * 用户
 */
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var treeCompany;
    var slectRowId = '';
    var formname = "用户 ";
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
                url: luckyu.rootUrl + "/OrganizationModule/Department/GetTree",
                expandOnClickNode: false,
                showLine: true,
                highlightCurrent: true,
            });
            treeCompany.on("click", function (d) {
                $("#company").html(' - ' + d.data.label);
                var id = d.data.id;
                var tag = d.data.ext.tag;
                var postData = {
                    organizationId: id,
                    organizationTag: tag
                };
                page.search(postData);
            })
        },
        initGrid: function () {
            grid = $("#grid").LuckyuGrid({
                url: luckyu.rootUrl + "/OrganizationModule/User/Page",
                datatype: "json",
                altRows: true,//隔行换色
                postData: {},
                colModel: [
                    { name: 'user_id', hidden: true, key: true },
                    { name: 'loginname', label: "用户名", width: 80, },
                    { name: 'realname', label: "姓名", width: 100, },
                    { name: 'nickname', label: "昵称", width: 80, },
                    { name: 'usercode', label: "工号", width: 80, },
                    {
                        name: 'sex', label: "性别", width: 60,
                        stype: "dataitem", dataitemcode: "sex",

                        //stype: "select",
                        //searchoptions: {
                        //    value: { "-1": "全部", "1": "男", "2": "女" },
                        //    defaultValue: "-1"
                        //},
                        //formatter: function (cellvalue, options, rowObject) {
                        //    switch (cellvalue) {
                        //        case 1: return '男';
                        //        case 2: return '女';
                        //        default: return '';
                        //    }
                        //}
                    },
                    { name: 'email', label: "Email", width: 100, },
                    { name: 'mobile', label: "联系电话", width: 100, },
                    { name: 'sort', label: "排序", width: 50, search: false },
                    { name: 'remark', label: "备注", width: 80, },
                    {
                        name: 'is_enable', label: '有效', width: 60, align: "center",
                        stype: "dataitem", dataitemcode: "enable", formatterdataitem: "enableshow"
                    },
                    { name: 'np_roles', label: "角色", width: 200, search: false, sortable: false },
                    { name: 'np_posts', label: "岗位", width: 200, search: false, sortable: false },
                    { name: 'np_groups', label: "小组", width: 200, search: false, sortable: false },
                    { name: 'np_depts', label: "分管部门", width: 200, search: false, sortable: false },

                ],
                shrinkToFit: false,
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "--sort",
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
                    url: luckyu.rootUrl + "/OrganizationModule/User/Form",
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
                    url: luckyu.rootUrl + "/OrganizationModule/User/Form?keyValue=" + rowid,
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
                    luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/User/DeleteForm", { keyValue: rowid }, function (data, info) {
                        layui.notice.success(info);
                        page.searchInCurrentPage();
                    });
                });
            });
            $("#setonoff").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/User/SetOnOff", { keyValue: rowid }, function (data, info) {
                    layui.notice.success(info);
                    page.searchInCurrentPage();
                });
            });
            $("#modifypwd").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerFormTop({
                    title: "修改",
                    width: 800,
                    height: 550,
                    url: luckyu.rootUrl + "/OrganizationModule/User/modifypwd?keyValue=" + rowid,
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () { });
                            return false;
                        }
                    }]
                });
            });

            $("#setmodule").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.layer.layerFormTop({
                    title: "功能授权",
                    width: 400,
                    height: 550,
                    url: luckyu.rootUrl + "/OrganizationModule/Authorize/Form?objectType=2&objectId=" + rowid,
                    btn: [{
                        name: "保存",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                            });
                        }
                    }]
                });

            });

            $("#setrole").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + '/OrganizationModule/UserRelation/GetRelations', { relationType: 1, userId: rowid }, function (res) {
                    var alreadyselect = [];
                    if (!!res.data && res.data.length > 0) {
                        alreadyselect = res.data.map(r => r.object_id);
                    }
                    top.alreadyselect = alreadyselect;
                    luckyu.layer.layerFormTop({
                        title: "设置角色",
                        width: 800,
                        height: 550,
                        url: luckyu.rootUrl + "/OrganizationModule/Role/RoleSelectForm?multiple=true",
                        btn: [{
                            name: "确定",
                            callback: function (index, layero) {
                                var roles = layero.find("iframe")[0].contentWindow.saveClick(index);
                                var objectIds = roles.map(r => r.role_id);
                                var requestData = {
                                    relationType: 1,
                                    userId: rowid,
                                    objectIds: objectIds
                                };
                                luckyu.ajax.postv2(luckyu.rootUrl + '/OrganizationModule/UserRelation/SetRelations', requestData, function (data, info) {
                                    layui.notice.success(info);
                                    page.searchInCurrentPage();
                                });
                            }
                        }]
                    });
                });
            });
            $("#setpost").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + '/OrganizationModule/UserRelation/GetRelations', { relationType: 2, userId: rowid }, function (res) {
                    var alreadyselect = [];
                    if (!!res.data && res.data.length > 0) {
                        alreadyselect = res.data.map(r => r.object_id);
                    }
                    top.alreadyselect = alreadyselect;
                    luckyu.layer.layerFormTop({
                        title: "设置岗位",
                        width: 800,
                        height: 550,
                        url: luckyu.rootUrl + "/OrganizationModule/Post/PostSelectForm?multiple=true",
                        btn: [{
                            name: "确定",
                            callback: function (index, layero) {
                                var posts = layero.find("iframe")[0].contentWindow.saveClick(index);
                                var objectIds = posts.map(r => r.post_id);
                                var requestData = {
                                    relationType: 2,
                                    userId: rowid,
                                    objectIds: objectIds
                                };
                                luckyu.ajax.postv2(luckyu.rootUrl + '/OrganizationModule/UserRelation/SetRelations', requestData, function (data, info) {
                                    layui.notice.success(info);
                                    page.searchInCurrentPage();
                                });
                            }
                        }]
                    });
                });
            });
            $("#setgroup").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + '/OrganizationModule/UserRelation/GetRelations', { relationType: 3, userId: rowid }, function (res) {
                    var alreadyselect = [];
                    if (!!res.data && res.data.length > 0) {
                        alreadyselect = res.data.map(r => r.object_id);
                    }
                    top.alreadyselect = alreadyselect;
                    luckyu.layer.layerFormTop({
                        title: "设置小组",
                        width: 800,
                        height: 550,
                        url: luckyu.rootUrl + "/OrganizationModule/Group/GroupSelectForm?multiple=true",
                        btn: [{
                            name: "确定",
                            callback: function (index, layero) {
                                var posts = layero.find("iframe")[0].contentWindow.saveClick(index);
                                var objectIds = posts.map(r => r.group_id);
                                var requestData = {
                                    relationType: 3,
                                    userId: rowid,
                                    objectIds: objectIds
                                };
                                luckyu.ajax.postv2(luckyu.rootUrl + '/OrganizationModule/UserRelation/SetRelations', requestData, function (data, info) {
                                    layui.notice.success(info);
                                    page.searchInCurrentPage();
                                });
                            }
                        }]
                    });
                });
            });

            $("#setmanager").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }

                luckyu.layer.layerFormTop({
                    title: "设置分管",
                    width: 800,
                    height: 550,
                    url: luckyu.rootUrl + "/OrganizationModule/DepartmentManage/ManageForm?userId=" + rowid,
                    btn: [{
                        name: "确定",
                        callback: function (index, layero) {
                            layero.find("iframe")[0].contentWindow.saveClick(index, function () {
                                page.searchInCurrentPage();
                            });
                            return false;
                        }
                    }]
                });

            });

            $("#setdepartment").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + '/OrganizationModule/UserRelation/GetRelations', { relationType: 6, userId: rowid }, function (res) {
                    var alreadyselect = [];
                    if (!!res.data && res.data.length > 0) {
                        alreadyselect = res.data.map(r => r.object_id);
                    }
                    top.alreadyselect = alreadyselect;
                    luckyu.layer.layerFormTop({
                        title: "设置部门",
                        width: 500,
                        height: 600,
                        url: luckyu.rootUrl + "/OrganizationModule/Department/DepartmentSelectForm?multiple=true",
                        btn: [{
                            name: "确定",
                            callback: function (index, layero) {
                                var depts = layero.find("iframe")[0].contentWindow.saveClick(index);
                                var objectIds = depts.map(r => r.id);
                                var requestData = {
                                    relationType: 6,
                                    userId: rowid,
                                    objectIds: objectIds
                                };
                                luckyu.ajax.postv2(luckyu.rootUrl + '/OrganizationModule/UserRelation/SetRelations', requestData, function (data, info) {
                                    layui.notice.success(info);
                                    page.searchInCurrentPage();
                                });
                            }
                        }]
                    });
                });

            });
            $("#setcompany").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                luckyu.ajax.get(luckyu.rootUrl + '/OrganizationModule/UserRelation/GetRelations', { relationType: 5, userId: rowid }, function (res) {
                    var alreadyselect = [];
                    if (!!res.data && res.data.length > 0) {
                        alreadyselect = res.data.map(r => r.object_id);
                    }
                    top.alreadyselect = alreadyselect;
                    luckyu.layer.layerFormTop({
                        title: "设置公司",
                        width: 500,
                        height: 600,
                        url: luckyu.rootUrl + "/OrganizationModule/Company/CompanySelectForm?multiple=true",
                        btn: [{
                            name: "确定",
                            callback: function (index, layero) {
                                var companys = layero.find("iframe")[0].contentWindow.saveClick(index);
                                var objectIds = companys.map(r => r.id);
                                var requestData = {
                                    relationType: 5,
                                    userId: rowid,
                                    objectIds: objectIds
                                };
                                luckyu.ajax.postv2(luckyu.rootUrl + '/OrganizationModule/UserRelation/SetRelations', requestData, function (data, info) {
                                    layui.notice.success(info);
                                    page.searchInCurrentPage();
                                });
                            }
                        }]
                    });
                });

            });
        },
        search: function (postData) {
            if (!postData) {
                var node = treeCompany.getHighlightNode();
                if (!!node.id) {
                    postData = {
                        organizationId: node.id,
                        organizationTag: node.ext.tag
                    };
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
            if (!!node.id) {
                postData = {
                    organizationId: node.id,
                    organizationTag: node.ext.tag
                };
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