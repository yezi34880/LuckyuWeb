﻿/**
 * 表单设计
 */
var saveClick;
var bootstrap = function (layui) {

    var keyValue = request("keyValue");

    var formJson = [];
    var formHtml = '';

    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            var compnent = document.getElementById("compnent1");
            new Sortable(compnent, {
                group: {
                    name: 'shared',
                    pull: 'clone',
                    put: false // 不允许拖拽进这个列表
                },
                animation: 150,
                sort: false // 设为false，禁止sort
            });

            var formBuilder = document.getElementById("formBuilder");
            new Sortable(formBuilder, {
                group: 'shared',
                animation: 150,
                ghostClass: "sortableghost",
                chosenClass: "sortablechosen",
                swapClass: 'highlight',
                onAdd: function (evt) {
                    debugger;
                    var col = {
                        columntype: evt.item.dataset.tag,
                        isselected: true,
                    };
                    formJson.push(col);
                    page.renderForm(formJson);
                },
                onEnd: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    evt.to;    // target list
                    evt.from;  // previous list
                    evt.oldIndex;  // element's old index within old parent
                    evt.newIndex;  // element's new index within new parent
                    evt.clone // the clone element
                    evt.pullMode;  // when item is in another sortable: `"clone"` if cloning, `true` if moving
                },
            });

            // 预览
            $("#preview").click(function () {
                var htm = $("#formBuilder").html();
                layui.layer.open({
                    type: 1,
                    title: '表单预览 ' + $("#formname").val(),
                    content: '<form id="formBase" class="layui-form" lay-filter="formBase" autocomplete="off"><div>' + htm + '</div></form>',
                    area: ['700px', '500px'], //宽高
                });
            });
            // 删除单个控件
            $("#remove").click(function () {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var name = divActive.attr("id");
                    name = name.substr(3, name.length - 3);
                    divActive.remove();
                    formHtml = $("#formBuilder").html();
                    var itemRemove = formJson.filter(t => t.columncode == name);
                    formJson.splice(itemRemove[0], 1);
                }
                else {
                    layui.layer.alert("没有选中要删除的控件", { icon: 2 });
                }
            });
            // 清空
            $("#removeall").click(function () {
                formJson = [];
                formHtml = '';
                $("#formBuilder").html("");
            });

            // 控件长度
            xmSelect.render({
                el: '#formlength',
                radio: true,
                clickClose: true,
                model: {
                    label: {
                        type: 'text',
                        text: {
                            //左边拼接的字符
                            left: '',
                            //右边拼接的字符
                            right: '',
                            //中间的分隔符
                            separator: ', ',
                        },
                    }
                },
                data: [
                    { name: '1', value: "12" },
                    { name: '1/2', value: "6" },
                    { name: '1/3', value: "4" },
                    { name: '1/4', value: "3" },
                    { name: '1/6', value: "2" },
                ],
                on: function (xmdata) {
                    if (xmdata.arr.length > 0) {
                        var divActive = $("#formBuilder div.form-item.active");
                        if (!!divActive && divActive.length > 0) {
                            var lengthClass = "layui-col-xs" + xmdata.arr[0].value;
                            divActive.removeClass("layui-col-xs12")
                                .removeClass("layui-col-xs6")
                                .removeClass("layui-col-xs4")
                                .removeClass("layui-col-xs3")
                                .removeClass("layui-col-xs2")
                                .addClass(lengthClass);

                            var divID = divActive.attr("id");
                            var ctrlID = divID.substr(3, divID.length - 3);
                            var colObjects = formJson.filter(t => t.columncode == ctrlID);
                            if (!!colObjects && colObjects.length > 0) {
                                var colObject = colObjects[0];
                                colObject.formlength = xmdata.arr[0].value;
                            }
                        }
                    }
                },
            })

            // 记录改变之前的值
            $("#columncode").focus(function (e) {
                $(this).data('old', e.target.value);
            });
            $("#columncode").change(function (e) {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var oldValue = event.target.dataset.old;
                    var columncode = $(this).val();
                    var columntype = divActive.attr("columntype");
                    switch (columntype) {
                        case "input": {
                            var ctrl = divActive.find("#" + oldValue);
                            $(ctrl).attr("id", columncode);
                            break;
                        }
                        case "textarea": {
                            var ctrl = divActive.find("#" + oldValue);
                            $(ctrl).attr("id", columncode);
                            break;
                        }
                    }
                    divActive.attr("id", "div" + columncode);

                    var colObjects = formJson.filter(t => t.columncode == oldValue);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.columncode = columncode;
                    }
                }
            });
            $("#columnname").change(function (e) {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var columnname = $(this).val();
                    var label = divActive.find("label.layui-form-label");
                    if (!!label && label.length > 0) {
                        $(label).html(columnname);
                    }
                    var divID = divActive.attr("id");
                    var ctrlID = divID.substr(3, divID.length - 3);
                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.columnname = columnname;
                    }
                }

            });

            layui.layer.close(loading);
        },
        renderForm: function (formJson) {
            if (!!formJson && formJson.length > 0) {
                formHtml = '';
                var selectedname = "";
                for (var i = 0; i < formJson.length; i++) {
                    var item = formJson[i];
                    var inputID = !!item.columncode ? item.columncode : item.columntype + i.toString();
                    if (item.isselected == true) {
                        selectedname = inputID;
                    }
                    item.isselected = false; // 恢复原状
                    item.columncode = inputID;
                    item.formlength = !!item.formlength ? item.formlength : "12";
                    switch (item.columntype) {
                        case "input": {
                            item.columnname = !!item.columnname ? item.columnname : "单行文本";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <input id="'+ item.columncode + '" type="text" class="layui-input" />\
                    </div>\
                </div>';
                            break;
                        }
                        case "textarea": {
                            item.columnname = !!item.columnname ? item.columnname : "多行文本";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <textarea id="'+ item.columncode + '" class="layui-textarea"></textarea>\
                    </div>\
                </div>';
                            break;
                        }
                        case "date": {

                            break;
                        }
                        case "datetime": {

                            break;
                        }
                        default:
                    }
                }

                var $formBuilder = $("#formBuilder");
                $formBuilder.html(formHtml);
                $formBuilder.find('div.form-item').each(function (k, n) {
                    var that = $(n);
                    if (that.hasClass("active")) {
                        that.removeClass('active');
                    }
                    console.log(selectedname);
                    if (!!selectedname && "div" + selectedname == that.attr("id")) {
                        that.addClass('active');
                        var divID = "div" + selectedname;
                        page.showCtrlPropety(divID);
                    }
                    // 绑定点击事件 并且显示编辑属性
                    that.on("click", function () {
                        var thisItem = $(this);
                        thisItem.addClass('active').siblings("div.form-item").removeClass("active");
                        // 属性
                        var divID = thisItem.attr("id");
                        page.showCtrlPropety(divID);
                    });
                });

                layui.form.render();
            }
        },
        showCtrlPropety: function (divID) {
            var ctrlID = divID.substr(3, divID.length - 3);
            var colObjects = formJson.filter(t => t.columncode == ctrlID);
            if (!!colObjects && colObjects.length > 0) {
                var colObject = colObjects[0];
                $("#columncode").val(colObject.columncode);
                $("#columnname").val(colObject.columnname);
                xmSelect.get("#formlength", true).setValue([colObject.formlength]);
            }
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + '/FormModule/FormDesigner/GetFormData', { keyValue: keyValue }, function (data) {

                });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$(".layui-form").verifyForm()) {
            return false;
        }
        var formhtml = formHtml;
        var formjson = JSON.stringify(formJson);

        //luckyu.ajax.postv2(luckyu.rootUrl + "/FormModule/FormDesigner/SaveForm", {
        //    keyValue: keyValue,
        //}, function (data) {

        //    parent.layui.layer.close(layerIndex);
        //    if (!!callBack) {
        //        callBack();
        //    }
        //});

    };

};