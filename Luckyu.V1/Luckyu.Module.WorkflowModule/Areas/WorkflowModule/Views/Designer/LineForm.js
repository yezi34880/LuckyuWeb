/**
 * 连线
 * */
var saveClick;
var bootstrap = function () {
    var currentLine = top.currentModifyLine;
    console.log(currentLine);

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            $("#linecondition").initLocal({
                initValue: "normal",
                data: [
                    { name: '正常流转', value: '0' },
                    { name: '是', value: '1' },
                    { name: '否', value: '2' },
                ]
            });
        },
        initData: function () {
            $("#LineForm").setFormValue(currentLine);
        }
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        var formData = $("#LineForm").getFormValue();
        for (var key in formData) {
            currentLine[key] = formData[key];
        }
        if (!!callBack) {
            callBack();
        }
        top.layui.layer.close(layerIndex);
    }
};