/*
 * 表单
 */
(function (layui) {
    "use strict";
    luckyu.form = {
        //eleTree自适应与grid同样的高度
        resizeEleTree: function (tree) {
            var $tree = $(tree);
            var top = $tree.offset().top;
            var height = window.innerHeight - top - 7;
            $tree.css('overflow', 'auto');
            $tree.height(height);
        },
        init: function () {
            $.fn.extend({
                initControl: function () {
                    $(this).find('[lay-verify]').each(function () {
                        var self = $(this);
                        var verify = self.attr("lay-verify");
                        if (verify.indexOf("required") > -1) {
                            var label = self.parent().prev();
                            if (label.find("i.redstar").length < 1) {
                                label.prepend('<i class="redstar">*</i>');
                            }
                        }
                        if (verify.indexOf("number") > -1 && !self.hasClass("numeric")) {
                            self.numeric();
                        }
                    });
                    $(this).find('[luckyu-type]').each(function () {
                        var self = $(this);
                        var type = self.attr("luckyu-type");
                        if (type === "dataitem") {
                            var code = self.attr("luckyu-code");
                            var multiple = self.attr("luckyu-multiple");
                            multiple = multiple === "true" ? true : false;
                            self.initDataItem({ code: code, multiple: multiple });
                        }
                        else if (type === "datasource") {
                            var url = self.attr("luckyu-url");
                            var name = self.attr("luckyu-name");
                            var value = self.attr("luckyu-value");
                            var multiple = self.attr("luckyu-multiple");
                            multiple = multiple === "true" ? true : false;
                            self.initDataSource({ url: url, nameField: name, valueField: value, multiple: multiple });
                        }
                        else if (type === "datalocal") {
                            var data = self.attr("luckyu-data");
                            var jsonData = JSON.parse(data);
                            var multiple = self.attr("luckyu-multiple");
                            multiple = multiple === "true" ? true : false;
                            self.initLocal({ data: jsonData, multiple: multiple });
                        }
                    });
                    $(this).find(".luckyu-editor").each(function () {
                        var self = $(this);
                        var id = self.attr("id");
                        var height = self.height();
                        height = !height ? 500 : height;
                        UE.getEditor(id, {
                            initialFrameHeight: height,
                            autoHeight: true
                        });
                    });
                },
                /**
                 * 验证表单
                 * */
                verifyForm: function () {
                    return layui.form.verify(this);
                },
                getFormValue: function () {
                    var objData = {};
                    $(this).find("input,select,textarea,.xm-select,.luckyu-editor").each(function () {
                        var self = $(this);
                        if (self.parents(".ui-jqgrid").length > 0) {
                            return;
                        }
                        var id = self.attr('id');
                        var name = self.attr('name');
                        name = !!name ? name : id;
                        if (!name) {
                            return;
                        }
                        if (self.is("input") || self.is("textarea") || self.is("select")) {
                            var type = $(this).attr('type');
                            if (self.hasClass("xm-select-default")) {
                                return;
                            }
                            switch (type) {
                                case "radio":
                                    if (self.is(":checked")) {
                                        objData[name] = self.val();
                                    }
                                    break;
                                case "checkbox":
                                    if (self.is(":checked")) {
                                        var val = self.val();
                                        objData[name] = !val || val === "on" ? 1 : self.val();
                                    } else {
                                        objData[name] = 0;
                                    }
                                    break;
                                case "datetime":
                                    objData[name] = $.trim(self.val());
                                    break;
                                default: objData[name] = self.val(); break;
                            }
                        }
                        else if (self.hasClass("xm-select")) {
                            var val = xmSelect.get("#" + id, true).getValue("valueStr");
                            objData[name] = val;
                        }
                        else if (self.hasClass("luckyu-editor")) {
                            var ue = UE.getEditor(id);
                            if (ue) {
                                objData[name] = ue.getContent(null, null, true);
                            }
                        }
                    });
                    return objData;
                },
                getGridValue: function () {
                    var rowdatas = [];
                    var grid = $(this);
                    var rowIds = grid.getDataIDs();
                    if (!!rowIds && rowIds.length > 0) {
                        for (var i = 0; i < rowIds.length; i++) {
                            var data = luckyu.grid.getEditingRowData(grid, rowIds[i]);
                            rowdatas.push(data);
                        }
                    }
                    return rowdatas;
                },
                setFormValue: function (data) {
                    var $this = $(this);
                    for (var id in data) {
                        var value = data[id];
                        if (value === null || value === undefined || value === NaN) {
                            continue;
                        }
                        var $obj = $this.find('#' + id);
                        if ($obj.length < 1) {
                            $obj = $this.find('[name=' + id + ']');
                            if ($obj.length < 1) {
                                continue;
                            }
                        }
                        if ($obj.is("input") || $obj.is("textarea") || $obj.is("select")) {
                            var type = $obj.attr('type');
                            switch (type) {
                                case "file":
                                    break;
                                case "radio":
                                    $obj.siblings('[value="' + value + '"]').prop("checked", true);
                                    layui.form.render("radio");
                                    break;
                                case "checkbox":
                                    $obj.prop("checked", !(value === 0));
                                    layui.form.render("checkbox");
                                    break;
                                case "datetime":
                                    if (!!value) {
                                        var format = 'yyyy-MM-dd';;
                                        if (!!$obj[0].initcfg && !!$obj[0].initcfg.dateFmt) {
                                            format = $obj[0].initcfg.dateFmt;
                                        }
                                        var val = (new Date(value)).format(format);
                                        $obj.val(val);
                                    }
                                    break;
                                default:
                                    $obj.val(value);
                                    break;
                            }
                        }
                        else if ($obj.hasClass("xm-select")) {
                            var xmselect = xmSelect.get($obj[0], true);
                            if (!xmselect) {
                                continue;
                            }
                            var xmValue = [];
                            if (xmselect.options.radio === true) {
                                xmValue.push(value);
                            }
                            else {
                                if (typeof (value) === 'string') {
                                    xmValue = value.split(',');
                                }
                                else {
                                    for (var z = 0; z < value.length; z++) {
                                        xmValue.push(value[z]);
                                    }
                                }
                            }
                            var select = $("#" + id);
                            var ignore = select.attr("ahoit-loadignore");
                            if (!ignore || ignore === "0" || ignore === "false") {
                                xmselect.setValue(xmValue, null, true);
                            }
                            else {
                                xmselect.setValue(xmValue, null, false);
                            }
                        }
                        else if ($obj.hasClass("edui-default") || $obj.hasClass("luckyu-editor")) {
                            var ue = UE.getEditor(id);
                            if (ue) {
                                ue.ready(function () {
                                    this.setContent(data[this.key]);
                                });
                            }
                        }
                    }
                },
                /**
                 * 赋值 但可以有选择的忽略字段
                 * @param {any} data 数据
                 * @param {any} ignoreFields 忽略字段
                 */
                setFormValueIgnoreField: function (data, ignoreFields) {
                    var $this = $(this);
                    for (var id in data) {
                        if (ignoreFields.indexOf(id) > -1) {
                            continue;
                        }
                        var value = data[id];
                        if (value === null || value === undefined || value === NaN) {
                            continue;
                        }
                        var $obj = $this.find('#' + id);
                        if ($obj.length < 1) {
                            $obj = $this.find('[name=' + id + ']');
                            if ($obj.length < 1) {
                                continue;
                            }
                        }
                        if ($obj.is("input") || $obj.is("textarea") || $obj.is("select")) {
                            var type = $obj.attr('type');
                            switch (type) {
                                case "radio":
                                    $obj.siblings('[value="' + value + '"]').prop("checked", true);
                                    layui.form.render("radio");
                                    break;
                                case "checkbox":
                                    $obj.prop("checked", value === 1);
                                    layui.form.render("checkbox");
                                    break;
                                case "datetime":
                                    if (!!value) {
                                        var format = 'yyyy-MM-dd';;
                                        if (!!$obj[0].initcfg && !!$obj[0].initcfg.dateFmt) {
                                            format = $obj[0].initcfg.dateFmt;
                                        }
                                        var val = (new Date(value)).format(format);
                                        $obj.val(val);
                                    }
                                    break;
                                default:
                                    $obj.val(value);
                                    break;
                            }
                        }
                        else if ($obj.hasClass("xm-select")) {
                            var xmselect = xmSelect.get("#" + id, true);
                            if (!xmselect) {
                                continue;
                            }
                            var xmValue = [];
                            if (xmselect.options.radio === true) {
                                xmValue.push(value);
                            }
                            else {
                                if (typeof (value) === 'string') {
                                    xmValue = value.split(',');
                                }
                                else {
                                    for (var z = 0; z < value.length; z++) {
                                        xmValue.push(value[z]);
                                    }
                                }
                            }
                            var select = $("#" + id);
                            var ignore = select.attr("ahoit-loadignore");
                            if (!ignore || ignore === "0" || ignore === "false") {
                                xmselect.setValue(xmValue, null, true);
                            }
                            else {
                                xmselect.setValue(xmValue, null, false);
                            }
                        }
                        else if ($obj.hasClass("edui-default") || $obj.hasClass("luckyu-editor")) {
                            var ue = UE.getEditor(id);
                            var ueValue = value;
                            if (ue) {
                                ue.ready(function () {
                                    ue.setContent(ueValue);
                                });
                            }
                        }
                    }
                },
                /*
                * 填充表格数据，并进入编辑状态
                */
                setGridValue: function (data) {
                    var $grid = $(this);
                    $grid.setGridParam({ data: data }).trigger('reloadGrid');
                    if (!!data && data.length > 0) {
                        var colModel = $grid.getGridParam("colModel");
                        var rowIds = $grid.getDataIDs();
                        for (var i = 0; i < rowIds.length; i++) {
                            var rowId = rowIds[i];
                            $grid.editRow(rowId, false); //进入编辑状态
                            if (!!colModel && colModel.length > 0) {
                                for (var x = 0; x < colModel.length; x++) {
                                    var model = colModel[x];
                                    var value = data[i][model.name];
                                    if (!value) {
                                        continue;
                                    }
                                    if (model.edittype === "custom") {
                                        var xmselect = xmSelect.get("#" + model.name + "_" + rowId, true);
                                        if (!xmselect) {
                                            continue;
                                        }
                                        var xmValue = [];
                                        if (xmselect.options.radio === true) {
                                            xmValue.push(value);
                                        }
                                        else {
                                            if (typeof (value) === 'string') {
                                                xmValue = value.split(',');
                                            }
                                            else {
                                                for (var z = 0; z < value.length; z++) {
                                                    xmValue.push(value[z]);
                                                }
                                            }
                                        }
                                        xmselect.setValue(xmValue, null, true);
                                    }
                                    else if (!!model.editoptions && !!model.editoptions.class && model.editoptions.class === "Wdate") {
                                        var $obj = $("#" + model.name + "_" + rowId);
                                        if (!!$obj && $obj.length > 0) {
                                            var format = 'yyyy-MM-dd';;
                                            if (!!$obj[0].initcfg && !!$obj[0].initcfg.dateFmt) {
                                                format = $obj[0].initcfg.dateFmt;
                                            }
                                            var val = (new Date(value)).format(format);
                                            $obj.val(val);
                                        }
                                    }
                                }

                            }
                        }
                    }
                },
                resizeEleTree: function () {
                    var $grid = $(this);
                    luckyu.form.resizeEleTree($grid);
                }
            });
        }
    };
    luckyu.form.init();
})(window.layui);