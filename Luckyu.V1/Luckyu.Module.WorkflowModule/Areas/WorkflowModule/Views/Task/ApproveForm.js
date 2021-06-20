/**
 * 审批结果
 */
var saveClick;
var bootstrap = function (layui) {
    var page = {
        init: function () {
            layui.form.on('radio(result)', function (data) {
                if (data.value == 2) {
                    $("#divReturn").show();
                    $("#opinion").attr("luckyu-verify", "required");
                }
                else {
                    $("#divReturn").hide();
                    $("#opinion").removeAttr("luckyu-verify", "required");
                }
            });
        }
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var result = $("input[name=result]:checked").val();
        var opinion = $("#opinion").val();
        var returnType = $("input[name=returnType]:checked").val();
        parent.layui.layer.close(layerIndex);
        return {
            result: result,
            opinion: opinion,
            returnType: returnType,
        };
    };
};