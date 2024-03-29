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
        initCustomControl: function (divForm) {
            $(divForm).initControl();
        },
        bind: function () {
            var loading = luckyu.layer.loading();

            $(".component-group").each(function () {
                var compnent = this;
                new Sortable(compnent, {
                    group: {
                        name: 'shared',
                        pull: 'clone',
                        put: false // 不允许拖拽进这个列表
                    },
                    animation: 150,
                    sort: false // 设为false，禁止sort
                });
            });

            var formBuilder = document.getElementById("formBuilder");
            new Sortable(formBuilder, {
                group: 'shared',
                animation: 150,
                ghostClass: "sortableghost",
                chosenClass: "sortablechosen",
                swapClass: 'highlight',
                onAdd: function (evt) {
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
                    content: '<form class="layui-form" autocomplete="off"><div>' + htm + '</div></form>',
                    area: ['750px', '600px'], //宽高
                    success: function (layero, layerindex) {
                        debugger;
                        var divForm = $(layero).find("form.layui-form");
                        page.initCustomControl(divForm);
                    }
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
            // 数据库字段类型
            xmSelect.render({
                el: '#dbtype',
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
                    { name: 'varchar', value: "varchar" },
                    { name: 'text', value: "text" },
                    { name: 'int', value: "int" },
                    { name: 'decimal', value: "decimal" },
                    { name: 'datetime', value: "datetime" },
                ],
                select: function (seldata) {
                    if (xmdata.arr.length > 0) {
                        var divActive = $("#formBuilder div.form-item.active");
                        if (!!divActive && divActive.length > 0) {
                            var divID = divActive.attr("id");
                            var ctrlID = divID.substr(3, divID.length - 3);
                            var colObjects = formJson.filter(t => t.columncode == ctrlID);
                            if (!!colObjects && colObjects.length > 0) {
                                var colObject = colObjects[0];
                                colObject.dbtype = xmdata.arr[0].value;
                            }
                        }


                    }
                }
            })

            // 记录改变之前的值
            // id 改变 需要记录下原始id
            $("#columncode").focus(function (e) {
                $(this).data('old', e.target.value);
            });
            $("#columncode").change(function (e) {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var oldValue = $(this).data("old");
                    var columncode = $(this).val();
                    var columntype = divActive.attr("columntype");
                    switch (columntype) {
                        case "date":
                        case "datetime":
                        case "input":
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
            $("#placeholder").change(function (e) {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var divID = divActive.attr("id");
                    var ctrlID = divID.substr(3, divID.length - 3);
                    var placeholder = $(this).val();
                    var columntype = divActive.attr("columntype");
                    switch (columntype) {
                        case "": {

                            break;
                        }
                        default: {
                            var ctrl = divActive.find("#" + ctrlID);
                            $(ctrl).attr("placeholder", placeholder);
                            break;
                        }
                    }

                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.placeholder = placeholder;
                    }
                }
            });
            $("#defaultvalue").change(function (e) {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var divID = divActive.attr("id");
                    var ctrlID = divID.substr(3, divID.length - 3);
                    var defaultvalue = $(this).val();
                    var columntype = divActive.attr("columntype");
                    switch (columntype) {
                        case "": {

                            break;
                        }
                        default: {
                            var ctrl = divActive.find("#" + ctrlID);
                            $(ctrl).val(defaultvalue);
                            break;
                        }
                    }

                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.defaultvalue = defaultvalue;
                    }
                }
            });

            // 时期时间 格式
            $("#dateformat").change(function (e) {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var divID = divActive.attr("id");
                    var ctrlID = divID.substr(3, divID.length - 3);
                    var defaultvalue = $(this).val();
                    var columntype = divActive.attr("columntype");
                    switch (columntype) {
                        case "datetime": {
                            $("#" + ctrlID).off("focus").on("focus", function () {
                                WdatePicker({
                                    dateFmt: dateformat
                                });
                            });
                            break;
                        }
                    }

                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.defaultvalue = defaultvalue;
                    }
                }
            });

            // #region 下拉框
            // 下拉框类型
            xmSelect.render({
                el: '#selecttype',
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
                    { name: '数据字典', value: "dataitem" },
                    { name: '数据库数据源', value: "datasource" },
                    { name: '本地数据源', value: "datalocal" },
                ],
                on: function (xmdata) {
                    if (xmdata.arr.length > 0) {
                        var val = xmdata.arr[0].value;
                        $("#type_" + val).show().siblings().hide();

                        var divActive = $("#formBuilder div.form-item.active");
                        if (!!divActive && divActive.length > 0) {
                            var divID = divActive.attr("id");
                            var ctrlID = divID.substr(3, divID.length - 3);

                            $("#" + ctrlID).attr("luckyu-type", val);
                            switch (val) {
                                case "datasource":
                                    var url = "/FormModule/FormInput/GetDataSource?formcode=" + $("#formcode") + "&columncode=" + ctrlID;
                                    $("#" + ctrlID).attr("luckyu-url", url);
                                    break;
                                default:
                                    $("#" + ctrlID).removeAttr("luckyu-url");
                                    break;
                            }

                        }


                    }
                }
            })
            // 数据字典数据源设置
            $("#dataitem_name,#dataitem_nameSelect").click(function () {
                luckyu.layer.layerFormTop({
                    title: "数据字典选择",
                    width: 850,
                    height: 650,
                    url: luckyu.rootUrl + "/SystemModule/Dataitem/DataitemSelectForm",
                    btn: [{
                        name: "确定",
                        callback: function (index, layero) {
                            var dataitem = layero.find("iframe")[0].contentWindow.okClick();
                            if (!!dataitem) {
                                $("#dataitem_name").val(dataitem.name);

                                var divActive = $("#formBuilder div.form-item.active");
                                if (!!divActive && divActive.length > 0) {
                                    var divID = divActive.attr("id");
                                    var ctrlID = divID.substr(3, divID.length - 3);
                                    $("#" + ctrlID).attr("luckyu-code", dataitem.code);
                                    $("#" + ctrlID).initDataItem({ code: dataitem.code });

                                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                                    if (!!colObjects && colObjects.length > 0) {
                                        var colObject = colObjects[0];
                                        colObject.dataitemcode = dataitem.code;
                                    }
                                }

                            }
                            top.layui.layer.close(index);
                        }
                    }]
                });

            });
            // 本地数据库
            $("#datasource").change(function () {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var divID = divActive.attr("id");
                    var ctrlID = divID.substr(3, divID.length - 3);
                    var datasource = $(this).val();
                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.datasource = datasource;
                    }
                }

            });
            // 本地数据库
            $("#datalocal").change(function () {
                var divActive = $("#formBuilder div.form-item.active");
                if (!!divActive && divActive.length > 0) {
                    var divID = divActive.attr("id");
                    var ctrlID = divID.substr(3, divID.length - 3);
                    var datalocal = $(this).val();
                    $("#" + ctrlID).attr("luckyu-local", datalocal);
                    var colObjects = formJson.filter(t => t.columncode == ctrlID);
                    if (!!colObjects && colObjects.length > 0) {
                        var colObject = colObjects[0];
                        colObject.datalocal = datalocal;
                    }
                }

            });

            // #endregion 

            layui.layer.close(loading);
        },
        renderForm: function (formJson) {
            debugger;
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
                    item.placeholder = !!item.placeholder ? item.placeholder : "";
                    item.defaultvalue = !!item.defaultvalue ? item.defaultvalue : "";
                    switch (item.columntype) {
                        case "input": {
                            item.dblength = 255;
                            item.dbtype = !!item.dbtype ? item.dbtype : "varchar";
                            item.columnname = !!item.columnname ? item.columnname : "单行文本";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <input id="'+ item.columncode + '" type="text" class="layui-input" placeholder="' + item.placeholder + '" value="' + item.defaultvalue + '" />\
                    </div>\
                </div>';
                            break;
                        }
                        case "textarea": {
                            item.dblength = !!item.dblength ? item.dblength : 500;
                            item.dbtype = !!item.dbtype ? item.dbtype : "varchar";
                            item.columnname = !!item.columnname ? item.columnname : "多行文本";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <textarea id="'+ item.columncode + '" class="layui-textarea"  placeholder="' + item.placeholder + '">' + item.defaultvalue + '</textarea>\
                    </div>\
                </div>';
                            break;
                        }
                        case "datetime": {
                            item.dbtype = !!item.dbtype ? item.dbtype : "datetime";
                            item.columnname = !!item.columnname ? item.columnname : "日期时间";
                            item.dateformat = !!item.dateformat ? item.dateformat : "yyyy-MM-dd";

                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <input id="'+ item.columncode + '" type="text" class="layui-input Wdate" onfocus="WdatePicker({ dateFmt: \'' + item.dateformat + '\' })"  placeholder="' + item.placeholder + '" value="' + item.defaultvalue + '" />\
                    </div>\
                </div>';
                            break;
                        }
                        case "number": {
                            item.dbtype = !!item.dbtype ? item.dbtype : "decimal";
                            item.dblength = !!item.dblength ? item.dblength : 18;
                            item.dbdigits = !!item.dbdigits ? item.dbdigits : 2;
                            item.columnname = !!item.columnname ? item.columnname : "数字";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <input id="'+ item.columncode + '" type="text" class="layui-input" placeholder="' + item.placeholder + '" value="' + item.defaultvalue + '" lay-verify="number" />\
                    </div>\
                </div>';
                            break;
                        }
                        case "select": {
                            item.dbtype = !!item.dbtype ? item.dbtype : "varchar";
                            item.columnname = !!item.columnname ? item.columnname : "下拉框";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                            <div id="'+ item.columncode + '" class="xm-select" luckyu-type="datalocal" placeholder="' + item.placeholder + '" luckyu-initvalue="' + item.defaultvalue + '"></div>\
                    </div>\
                </div>';
                            break;
                        }
                        case "uediter": {
                            item.dbtype = !!item.dbtype ? item.dbtype : "text";
                            item.columnname = !!item.columnname ? item.columnname : "富文本";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <script id="'+ item.columncode + '" type="text/plain" style="height:300px;"  class="luckyu-editor">\
                        <\/script>\
                    </div>\
                </div>';
                            break;
                        }
                        case "upload": {
                            var formcode = $("#formcode").val();
                            item.columnname = !!item.columnname ? item.columnname : "文件上传";
                            formHtml += '\
                <div class="layui-col-xs'+ item.formlength + ' form-item" id="div' + item.columncode + '" columntype="' + item.columntype + '">\
                    <label class="layui-form-label">'+ item.columnname + '</label>\
                    <div class="layui-input-block">\
                        <input id="AnnexName" name="AnnexName" class="fileinput" multiple type="file" luckyu-folderPre="'+ formcode + '" />\
                    </div>\
                </div>';
                            break;
                        }
                    }
                }

                var $formBuilder = $("#formBuilder");
                $formBuilder.html(formHtml);
                layui.form.render();
                $formBuilder.find('div.form-item').each(function (k, n) {
                    var that = $(n);
                    if (that.hasClass("active")) {
                        that.removeClass('active');
                    }
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

                page.initCustomControl("#formBuilder");
            }
        },
        showCtrlPropety: function (divID) {
            debugger;
            var ctrlID = divID.substr(3, divID.length - 3);
            var colObjects = formJson.filter(t => t.columncode == ctrlID);
            if (!!colObjects && colObjects.length > 0) {
                var colObject = colObjects[0];
                $("#columncode").val(colObject.columncode);
                $("#columnname").val(colObject.columnname);
                $("#placeholder").val(colObject.placeholder);
                $("#defaultvalue").val(colObject.defaultvalue);
                $("#dblength").val(colObject.dblength);
                $("#dateformat").val(colObject.dateformat);
                xmSelect.get("#dbtype", true).setValue([colObject.dbtype]);
                xmSelect.get("#formlength", true).setValue([colObject.formlength]);

                $("#dbdigits").val(colObject.dbdigits);

            }

            var columntype = $("#" + divID).attr("columntype");
            $("div.ctrlpropety").each(function () {
                $(this).hide();
            });
            $("#" + columntype + "Propety").show();
        },
        initData: function () {
            if (!!keyValue) {
                luckyu.ajax.getv2(luckyu.rootUrl + '/FormModule/FormDesigner/GetFormData', { keyValue: keyValue }, function (data) {
                    $("#formcode").val(data.Form.formcode).attr("readonly", "readonly");
                    $("#formname").val(data.Form.formname);
                    $("#remark").val(data.Form.remark);

                    //$("#columncode").attr("readonly", "readonly");

                    $("#formBuilder").html(data.Form.formhtml);
                    formHtml = data.Form.formHtml;
                    formJson = data.Form.formJson;
                });
            }
        },
    };
    page.init();

    saveClick = function (layerIndex, callBack) {
        if (!$("#divFormInfo").verifyForm()) {
            return false;
        }
        var formData = $('#divFormInfo').getFormValue();
        formData.formhtml = formHtml;
        formData.formjson = JSON.stringify(formJson);
        luckyu.ajax.postv2(luckyu.rootUrl + "/FormModule/FormDesigner/SaveForm", {
            keyValue: keyValue,
            strEntity: JSON.stringify(formData)
        }, function (data) {

            parent.layui.layer.close(layerIndex);
            if (!!callBack) {
                callBack();
            }
        });

    };

};