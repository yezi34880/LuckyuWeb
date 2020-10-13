/**
 * 审批结果
 */
var saveClick;
var bootstrap = function (layui) {

    saveClick = function (layerIndex, callBack) {
        var result = $("input[name=result]:checked").val();
        var opinion = $("#opinion").val();
        parent.layui.layer.close(layerIndex);
        return {
            result: result,
            opinion: opinion
        };
    };
};