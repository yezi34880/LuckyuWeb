/*
 * 单据号生成规则
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var keyValue = request("keyValue");
    var grid;
    var page = {
        initGrid: function () {
            grid = $("#grid").jqGrid({
                editurl: 'clientArray',
                cellsubmit: "clientArray",
                altRows: true,//隔行换色
                datatype: "local",
                colModel: [
                    { name: 'ItemId', hidden: true, key: true },
                    { name: 'Type', label: "类型", hidden: true, },
                    { name: 'TypeName', label: "类型", width: 100 },
                    { name: 'Format', label: "格式", width: 100 },
                    {
                        name: 'BeNumber', label: "参与流水号", width: 50,
                        formatter: function (cellvalue, options, rowObject) {
                            switch (cellvalue) {
                                case 1: return '<i class="fa fa-toggle-on"></i>';
                                case 0: return '<i class="fa fa-toggle-off"></i>';
                                default: return '';
                            }
                        },
                        unformat: function (cellvalue, options, ele) {
                            switch (ele.innerHTML) {
                                case '<i class="fa fa-toggle-on"></i>': return 1;
                                default: return 0;
                            }
                        },
                    },
                ],
                rownumbers: true,
                viewrecords: true,
                loadonce: true,
            });

            grid.setGridWidth(window.innerWidth - 10);
            grid.setGridHeight(window.innerHeight - 180);
            window.onresize = function () {
                grid.setGridWidth(window.innerWidth - 10);
                grid.setGridHeight(window.innerHeight - 180);
            };

            $("#addRow").click(function () {
                luckyu.layer.layerFormTop({
                    id: "FormDetail",
                    title: "新增规则",
                    width: 400,
                    height: 500,
                    url: luckyu.rootUrl + "/BaseModule/CodeRule/RuleJsonForm",
                    btn: [{
                        name: "确定",
                        filter: "layer-iframesubmit",
                        callback: function (index, layero) {
                            if (layui.form.layerverify(layero) === false) {
                                return;
                            }
                            var data = layero.find("iframe")[0].contentWindow.saveClick(index);
                            if (!!data) {
                                var rowId = luckyu.utility.newGuid();
                                data.ItemId = rowId;
                                grid.addRowData(rowId, data, "last");
                            }
                        }
                    }]
                });
            });
            $("#editRow").click(function () {
                var rowid = grid.getGridParam("selrow");
                if (!rowid) {
                    layui.notice.error("没有选中任何行数据");
                    return;
                }
                var rowData = grid.getRowData(rowid);
                top._selectruleitem = rowData;
                luckyu.layer.layerFormTop({
                    id: "FormDetail",
                    title: "修改规则",
                    width: 400,
                    height: 500,
                    url: luckyu.rootUrl + "/BaseModule/CodeRule/RuleJsonForm",
                    btn: [{
                        name: "确定",
                        filter: "layer-iframesubmit",
                        callback: function (index, layero) {
                            if (layui.form.layerverify(layero) === false) {
                                return;
                            }
                            var data = layero.find("iframe")[0].contentWindow.saveClick(index);
                            if (!!data) {
                                grid.setRowData(rowid, data);
                            }
                        }
                    }]
                });

            });
            //删除
            $("#deleteRow").click(function () {
                var rowId = grid.getGridParam("selrow");
                grid.deleteRow(rowId);
            });
            // 上移
            $("#moveUp").click(function () {
                var rowId = grid.getGridParam("selrow");
                luckyu.grid.moveRowUp(rowId);
            });
            // 下移
            $("#moveDown").click(function () {
                var rowId = grid.getGridParam("selrow");
                luckyu.grid.moveRowDown(rowId);
            });
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + "/BaseModule/CodeRule/GetFormData", { keyValue: keyValue }
                    , function (data) {
                        $('[lay-filter="CodeRule"]').setFormValue(data);
                        var json = JSON.parse(data.contentjson);
                        if (!!json) {
                            $("#grid").setGridParam({ data: json }).trigger('reloadGrid');
                        }
                    });
            }
        },
        init: function () {
            page.initGrid();
            page.initData();
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        var formData = $('[lay-filter="CodeRule"]').getFormValue();
        var ruledatail = grid.getRowData();
        if (!!ruledatail && ruledatail.length > 0) {
            var numberRules = ruledatail.filter(r => r.Type === 'number')
            if (!!numberRules && numberRules.length > 1) {
                layui.notice.error("只能有一个流水号规则");
                return;
            }
        }
        formData.contentjson = JSON.stringify(ruledatail);
        var loading = layui.layer.load();
        luckyu.ajax.post(luckyu.rootUrl + "/BaseModule/CodeRule/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData),
        }, function (res) {
            layui.layer.close(loading);
            if (res.code === 200) {
                parent.layui.layer.close(layerIndex);
                if (!!callback) {
                    callback();
                }
            }
            else {
                layui.notice.error(res.info);
            }
        });
    };

};