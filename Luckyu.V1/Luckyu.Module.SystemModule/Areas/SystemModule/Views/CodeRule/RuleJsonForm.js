/*
 * 编码规则规则 明细行
 */
var saveClick;
var bootstrap = function (layui) {
    "use strict";

    var page = {
        bind: function () {
            $("#Type").initLocal({
                data: [
                    { name: "动态传入字符", value: "var" },
                    { name: "固定字符", value: "custom" },
                    { name: "公司", value: "company" },
                    { name: "部门", value: "dept" },
                    { name: "人员", value: "user" },
                    { name: "日期时间", value: "date" },
                    { name: "流水号", value: "number" },
                ],
                select: function (data) {
                    if (!!data.arr && data.arr.length > 0) {
                        var value = data.arr[0].value;
                        var name = data.arr[0].name;
                        $("#TypeName").val(name);
                        switch (value) {
                            case "var":
                                $("#Format").val("").attr("readonly", "readonly");
                                break;
                            case 'number':
                                $("#BeNumber").prop("checked", false).attr("disabled", "disabled");
                                $("#Format").removeAttr("readonly");
                                layui.form.render("checkbox");
                                break;
                            default:
                                $("#BeNumber").removeAttr("disabled");
                                $("#Format").removeAttr("readonly");
                                layui.form.render("checkbox");
                                break;
                        }
                    }
                    else {
                        $("#TypeName").val('');
                    }
                }
            });
        },
        initData: function () {
            if (!!top._selectruleitem) {
                $('div[lay-filter="RuleItemModel"]').setFormValue(top._selectruleitem);
                top._selectruleitem = null;
            }
        },
        init: function () {
            page.bind();
            page.initData();
        },
    };
    page.init();

    saveClick = function (layerIndex, callback) {
        var data = $('div[lay-filter="RuleItemModel"]').getFormValue();
        parent.layui.layer.close(layerIndex);
        return data;
    };
};