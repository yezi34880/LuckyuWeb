/*
 * 数据字典 选择
 */
var okClick;
var bootstrap = function (layui) {
    "use strict";
    var grid;
    var treeDataitem;
    var page = {
        init: function () {
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
                showRadio: true,
                isOnlyShowLeafRadio: true,
                radioType: 'all',
                radioOnClickNode: true,
            });
            treeDataitem.on("radio", function (d, data) {
                $("#show").html(' - ' + d.data.label + '【' + d.data.ext.code + '】');
                page.search({ classifyId: d.data.id });
            });

            $("#keyword").on("input", function (e) {
                var val = e.currentTarget.value;
                treeDataitem.search(val, function (value, data) {
                    if (!value) return true;
                    return data.label.indexOf(value) > -1 || data.ext.code.indexOf(value) > -1;
                });
            });

        },
        initGrid: function () {
            grid = $("#grid").LuckyuGrid({
                url: luckyu.rootUrl + "/SystemModule/Dataitem/SelectPage",
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
                ],
                rownumbers: true,
                viewrecords: true,
                rowNum: 30,
                rowList: [30, 50, 100],
                pager: "#gridPager",
                sortname: "sort",
                sortorder: "ASC",
            });

            grid.filterToolbar();
            grid.toggleSearchBar();
            grid.resizeGrid();
            $("#treeDataitem").resizeEleTree();
            $(window).resize(function () {
                grid.resizeGrid();
                $("#treeDataitem").resizeEleTree();
            });
        },
        initBtn: function () {

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
    };
    page.init();

    okClick = function () {
        var node = treeDataitem.getRadioChecked();
        var resdata = {};
        if (!!node && node.length > 0) {
            resdata = {
                code: node[0].ext.code,
                name: node[0].label
            };
        }
        return resdata;
    };

};