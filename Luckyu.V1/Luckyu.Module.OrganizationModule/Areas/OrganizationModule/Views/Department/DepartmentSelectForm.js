/*
 * 部门选择
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var multiple = request("multiple");
    multiple = multiple === "true" || multiple === "1" ? true : false;
    var alreadys = top.alreadyselect;
    top.alreadyselect = null;

    var treeDepartment;
    var page = {
        bind: function () {
            treeDepartment = layui.eleTree.render({
                elem: '#treeDepartment',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/OrganizationModule/Department/GetAllDepartmentTree",
                expandOnClickNode: false,
                showCheckbox: true,
                showLine: true,
                highlightCurrent: true,
                checkStrictly: true,
                done: function () {
                    if (!!alreadys && alreadys.length > 0) {
                        treeDepartment.setChecked(alreadys, true)
                    }
                }
            });
            layui.eleTree.on("nodeChecked(treeDepartment)", function (d) {
                if (multiple === false) {
                    treeDepartment.unCheckNodes()
                    treeDepartment.setChecked([d.data.currentData.id], true)
                }
            })
            $("#treeDepartment").resizeEleTree();
            window.onresize = function () {
                $("#treeDepartment").resizeEleTree();
            };
        },
        init: function () {
            page.bind();
        },
    };
    page.init();

    saveClick = function (layerIndex) {
        var nodes = treeDepartment.getChecked(false, false);
        parent.layui.layer.close(layerIndex);
        return nodes;
    };

};