/*
 * 权限分配
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var objectType = request("objectType");
    var objectId = request("objectId");

    var treeModule;
    var page = {
        bind: function () {
            treeModule = layui.eleTree.render({
                elem: '#treeModule',
                defaultExpandAll: true,
                url: luckyu.rootUrl + "/OrganizationModule/Module/GetTree",
                expandOnClickNode: false,
                showCheckbox: true,
                showLine: true,
                highlightCurrent: true,
                done: function () {
                    page.initData();
                }
            });
        },
        initData: function () {
            if (!!objectId) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/OrganizationModule/Authorize/GetFormData", { objectId: objectId }
                    , function (data) {
                        var modules = [];
                        for (var i = 0; i < data.length; i++) {
                            var x = data[i];
                            if (x.itemtype === 1) {
                                modules.push(x.item_id);
                            }
                        }
                        treeModule.setChecked(modules, true);
                    });
            }
        },
        init: function () {
            page.bind();
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        var checkModules = treeModule.getChecked(false, true);
        if (!checkModules) {
            return;
        }
        var requestData = {
            objectId: objectId,
            objectType: objectType,
            modules: checkModules.map(r => r.id),
        };
        luckyu.ajax.postv2(luckyu.rootUrl + "/OrganizationModule/Authorize/SaveForm", requestData,
            function (data) {
                parent.layui.layer.close(layerIndex);
                if (!!callback) {
                    callback();
                }
            });
    };

};