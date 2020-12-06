/**
 * 阅
 */
var saveClick;
var bootstrap = function (layui) {

    saveClick = function (layerIndex, callBack) {
        var opinion = $("#opinion").val();
        parent.layui.layer.close(layerIndex);
        return {
            opinion: opinion
        };
    };
};