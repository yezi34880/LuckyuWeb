/*
 * 公司选择
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var multiple = request("multiple");
    multiple = multiple === "true" || multiple === "1" ? true : false;
    var alreadys = top.alreadyselect;
    top.alreadyselect = null;

    var tree;
    var page = {
        bind: function () {
            tree = layui.eleTree.render({
                elem: '#treeTree',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/OrganizationModule/Company/GetAllCompanyTree?multiple=" + multiple,
                expandOnClickNode: false,
                showCheckbox: true,
                showLine: true,
                highlightCurrent: true,
                checkStrictly: true,
                done: function () {
                    if (!!alreadys && alreadys.length > 0) {
                        tree.setChecked(alreadys, true)
                    }
                }
            });
            layui.eleTree.on("nodeChecked(tree)", function (d) {
                if (multiple === false) {
                    tree.unCheckNodes()
                    tree.setChecked([d.data.currentData.id], true)
                }
                if (multiple === true && d.data.currentData.ext.tag === "all") {
                    if (d.isChecked) {
                        var nodes = tree.getAllNodeData();
                        var arr = [];
                        for (var i = 0; i < nodes.length; i++) {
                            arr.push(nodes[i].id);
                        }
                        tree.setChecked(arr, false);
                    }
                    else {
                        tree.unCheckNodes();
                    }
                }
            })
            $("#treeTree").resizeEleTree();
            window.onresize = function () {
                $("#treeTree").resizeEleTree();
            };
        },
        init: function () {
            page.bind();
        },
    };
    page.init();

    saveClick = function (layerIndex) {
        var nodes = tree.getChecked(false, false);
        parent.layui.layer.close(layerIndex);
        return nodes;
    };

};