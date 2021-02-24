﻿/*
 * 表格插件
 */
(function (layui) {
    "use strict";

    luckyu.grid = {
        /**
         * 自适应高度、宽度
         * */
        resizeGrid: function (grid) {
            var $grid = $(grid);
            if (!$grid || $grid.length < 1) {
                return;
            }
            var gridid = $grid.attr("id");
            var top = $grid.offset().top;
            var isPage = $grid.getGridParam("pager");
            var pagerHeight = 10;
            var isFooter = $grid.getGridParam("footerrow");
            if (isPage) {
                pagerHeight = pagerHeight + $(isPage).height();
            }
            if (isFooter) {
                pagerHeight = pagerHeight + $('#gbox_' + gridid + ' div.ui-jqgrid-sdiv').height();
            }
            var height = window.innerHeight - top - pagerHeight;
            var width = $grid.parents(".ui-jqgrid").parent().width() - 10;
            $grid.setGridHeight(height).setGridWidth(width);
        },
        /**
         * 清空所有搜索
         */
        clearSearchBar: function (grid) {
            var $grid = $(grid);
            if (!$grid || $grid.length < 1) {
                return;
            }
            var colModel = $grid[0].p.colModel;
            for (var i = 0; i < colModel.length; i++) {
                var name = colModel[i].name;
                var $that = $("#gs_" + name);
                if (!!$that && $that.length > 0) {
                    $that.attr("_realValue", "");
                }
            }
            $grid[0].clearToolbar();
        },
        /**
         * 显示/隐藏搜索行，并保持表格高度不变
         */
        toggleSearchBar: function (grid) {
            var $grid = $(grid);
            if (!$grid || $grid.length < 1) {
                return;
            }
            var gridid = $grid.attr("id");
            var gridSelector = "#gbox_" + gridid;
            var toolbar = $(gridSelector + " .ui-search-toolbar");
            var height = $grid.getGridParam("height");
            var toolHeight = 42;
            if (toolbar.is(":visible")) {
                $grid.setGridHeight(height + toolHeight);
            }
            else {
                $grid.setGridHeight(height - toolHeight);
            }
            $grid[0].toggleToolbar();
        },
        /**
         * 统一搜索设置, 回车执行搜索,搜索之前参数调整 参数清除
         */
        filterToolbar: function (grid) {
            var $grid = $(grid);
            if (!$grid || $grid.length < 1) {
                return;
            }
            $grid.jqGrid('filterToolbar', {
                autosearch: true,  // true输入后回车键搜索，false文本改变搜索
                stringResult: true,
                beforeSearch: function () {  // 重新设置参数 手动触发查询
                    if (!!this.p.postData.filters) {
                        var filters = JSON.parse(this.p.postData.filters)
                        var rules = filters.rules;
                        if (!!rules && rules.length > 0) {
                            var colModel = $grid.jqGrid("getGridParam", "colModel");
                            for (var i = 0; i < rules.length; i++) {
                                var rule = rules[i];
                                var $ele = $("#gs_" + rule.field);
                                var val1 = $ele.val();
                                var val2 = $ele.attr("_realValue");
                                rule.data = val2 || val1;

                                rule.stype = "";
                                var model = colModel.filter(r => r.name === rule.field);
                                if (!!model && model.length > 0) {
                                    rule.stype = model[0].stype;
                                    if (!!model[0].dataitemcode) {
                                        rule.stype = "dataitem";
                                    }
                                }
                            }
                        }
                        this.p.postData.filters = JSON.stringify(filters);
                    }
                },
                beforeClear: function (a, b, c, d) {
                    this.p.postData._search = false;
                    if (!!this.p.postData.filters) {
                        this.p.postData.filters = [];
                    }
                },
                onClearSearchValue: function (elem, colIndex, s, d) {
                    $(elem).attr("_realValue", "").val("");

                },
            });
        },
        /**
         * 获取正在编辑行数据
        */
        getEditingRowData: function (grida, rowid) {
            grida = $(grida);
            var nm, tmp = {}, editable, ind;

            ind = grida.jqGrid("getInd", rowid, true);
            if (ind === false) {
                return "";
            }
            var rowdata = grida.getRowData(rowid);
            editable = $(ind).attr("editable");
            if (editable === "1") {
                var cm;
                var colModel = grida.jqGrid("getGridParam", "colModel");
                $("td", ind).each(function (i) {
                    // cm = $('#mygrid').p.colModel[i];
                    cm = colModel[i];
                    nm = cm.name;
                    if (nm != 'cb' && nm != 'subgrid' && cm.editable === true && nm != 'rn' && !$(this).hasClass('not-editable-cell')) {
                        switch (cm.edittype) {
                            case "checkbox":
                                var cbv = [1, 0];
                                if (cm.editoptions) {
                                    cbv = cm.editoptions.value.split(":");
                                }
                                tmp[nm] = $("input", this).is(":checked") ? cbv[0] : cbv[1];
                                break;
                            case 'text':
                            case 'password':
                            case 'textarea':
                            case "button":
                                tmp[nm] = $("input, textarea", this).val();
                                break;
                            case 'select':
                                if (!cm.editoptions.multiple) {
                                    tmp[nm] = $("select option:selected", this).val();
                                } else {
                                    var sel = $("select", this);
                                    tmp[nm] = $(sel).val();
                                    if (tmp[nm]) { tmp[nm] = tmp[nm].join(","); } else { tmp[nm] = ""; }
                                }
                                break;
                            case "custom":
                                var xmselect = $(this.innerHTML).find(".xm-select");
                                if (!!xmselect) {
                                    var xmsel = xmSelect.get("#" + xmselect.attr("id"), true);
                                    var val = xmsel.getValue("valueStr");
                                    if (xmsel.options.radio === false) {
                                        val = val + ",";
                                    }
                                    tmp[nm] = val;
                                }
                                break;
                        }
                    }
                    else {
                        tmp[nm] = rowdata[nm];
                    }
                });
            }
            return tmp;
        },
        /**
         * 行上移
         */
        moveRowUp: function (rowId) {
            var $tr = $("#" + rowId);
            if ($tr.index() > 1) {
                var tr1 = $tr.prev();
                var num = $tr.find("td.jqgrid-rownum");
                var num1 = tr1.find("td.jqgrid-rownum");
                var temp = num.html();
                num.html(num1.html());
                num1.html(temp);
                $tr.fadeOut("fast").fadeIn("fast");
                tr1.before($tr);
            }
        },
        /**
         * 行下移
         */
        moveRowDown: function (rowId) {
            var $tr = $("#" + rowId);
            var grid = $tr.parents("table");
            var rowcount = grid.getGridParam("reccount");

            if ($tr.index() < rowcount) {
                var tr1 = $tr.next();
                var num = $tr.find("td.jqgrid-rownum");
                var num1 = tr1.find("td.jqgrid-rownum");
                var temp = num.html();
                num.html(num1.html());
                num1.html(temp);
                $tr.fadeOut("fast").fadeIn("fast");
                tr1.after($tr);
            }
        },
        // 下拉框 编辑统一配置 
        EditSelectOption: function (initDate, layVerify) {
            var op = {
                dataInit: function (ele, options) {
                    var select = $(ele).find("div.xm-select");
                    initDate(select);
                },
                custom_element: function (value, options) {
                    var html = '<div class="xm-select" ' + (!!layVerify ? 'lay-verify="' + layVerify + '"' : '') + '></div>';
                    return html;
                },
                custom_value: function (ele, operation, value) {
                    var xmselect = xmSelect.get("#" + ele.attr("id"), true)
                    return xmselect.getValue("valueStr");
                }
            };
            if (!!layVerify) {
                op["lay-verify"] = layVerify;
            }
            return op;
        },

        init: function () {
            $.fn.extend({
                /**
                * 获取正在编辑行数据
                */
                getEditingRowData: function (rowId) {
                    return luckyu.plugin.grid.getEditingRowData(this, rowId);
                },
                /**
                 * 新增行并进入编辑状态
                 * */
                addEditRow: function (rowId, data, position, mainId) {
                    var $grid = $(this);
                    var colModel = $grid.getGridParam("colModel");
                    var customModel = colModel.filter(function (t) {
                        return t.edittype === 'custom';
                    });
                    $grid.addRowData(rowId, data, position, mainId);
                    $grid.editRow(rowId, false); //进入编辑状态
                    if (!!customModel && customModel.length > 0) {
                        for (var x = 0; x < customModel.length; x++) {
                            var model = customModel[x];
                            var xmselect = xmSelect.get("#" + model.name + "_" + rowId, true);
                            if (!xmselect) {
                                continue;
                            }
                            var xmValue = [];
                            if (xmselect.radio === true || typeof (value) !== 'string') {
                                xmValue.push(data[model.name]);
                            }
                            else {
                                xmValue = data[model.name].split(',');
                            }
                            xmselect.setValue(xmValue);
                            if (!!xmselect.options.on) {
                                var xmdata = {
                                    arr: []
                                };
                                for (var y = 0; y < xmValue.length; y++) {
                                    xmdata.arr.push({
                                        value: xmValue[y]
                                    });
                                }
                                xmselect.options.on(xmdata);
                            }
                        }
                    }
                },
                /**
                 * 获取当前行展示数据
                 * @param {any} rowid
                 */
                getDisplayRowData: function (rowid, option) {
                    var defaultOption = {
                        ignoreCols: [],
                        appendCols: [],
                    };
                    $.extend(defaultOption, option);
                    var grida = $(this);
                    var nm, tmp = {}, editable, ind;
                    ind = grida.jqGrid("getInd", rowid, true);
                    if (ind === false) { return success; }
                    editable = $(ind).attr("editable");
                    if (editable === "1") {
                        var cm;
                        var colModel = grida.jqGrid("getGridParam", "colModel");
                        $("td", ind).each(function (i) {
                            cm = colModel[i];
                            if ((!cm.label || defaultOption.ignoreCols.indexOf(cm.name) > -1) && defaultOption.appendCols.indexOf(cm.name) < 0) {
                                return;
                            }
                            nm = cm.label || cm.name;
                            if (nm != 'cb' && nm != 'subgrid' && cm.editable === true && nm != 'rn' && !$(this).hasClass('not-editable-cell')) {
                                switch (cm.edittype) {
                                    case "checkbox":
                                        var cbv = [1, 0];
                                        if (cm.editoptions) {
                                            cbv = cm.editoptions.value.split(":");
                                        }
                                        tmp[nm] = $("input", this).is(":checked") ? cbv[0] : cbv[1];
                                        break;
                                    case 'text':
                                    case 'password':
                                    case 'textarea':
                                    case "button":
                                        tmp[nm] = $("input, textarea", this).val();
                                        break;
                                    case 'select':
                                        if (!cm.editoptions.multiple) {
                                            tmp[nm] = $("select option:selected", this).text();
                                        } else {
                                            var selectedText = [];
                                            $("select option:selected", this).each(
                                                function (i, selected) {
                                                    selectedText[i] = $(selected).text();
                                                }
                                            );
                                            tmp[nm] = selectedText.join(",");
                                        }
                                        if (cm.formatter && cm.formatter == 'select') { tmp2 = {}; }
                                        break;
                                    case "custom":
                                        var xmselect = $(this.innerHTML).find(".xm-select");
                                        if (!!xmselect) {
                                            xmSelect.get("#" + xmselect.attr("id"), true)
                                            tmp[nm] = xmSelect.get("#" + xmselect.attr("id"), true).getValue("nameStr");
                                        }
                                        break;
                                }
                            }

                        });
                    }
                    return tmp;
                },
                /**
                 * 获取显示数据
                 * */
                getDisplayData: function (option) {
                    var rowdatas = [];
                    var grid = $(this);
                    var rowIds = grid.getDataIDs();
                    if (!!rowIds && rowIds.length > 0) {
                        for (var i = 0; i < rowIds.length; i++) {
                            var data = grid.getDisplayRowData(rowIds[i], option);
                            rowdatas.push(data);
                        }
                    }
                    return rowdatas;
                },
                /**
                 *  必填项设置红星
                 * */
                setRedStar: function () {
                    var grida = $(this);
                    var id = grida.attr("id");
                    var colModel = grida.getGridParam("colModel");
                    for (var i = 0; i < colModel.length; i++) {
                        var col = colModel[i];
                        if (!!col.editoptions) {
                            if (col.editoptions["lay-verify"] === "required") {
                                $("#jqgh_" + id + "_" + col.name).prepend('<i class="redstar">*</i>');
                            }

                        }
                    }
                },

                resizeGrid: function () {
                    var grida = $(this);
                    luckyu.grid.resizeGrid(grida);
                },
                clearSearchBar: function () {
                    var grida = $(this);
                    luckyu.grid.clearSearchBar(grida);
                },
                toggleSearchBar: function () {
                    var grida = $(this);
                    luckyu.grid.toggleSearchBar(grida);
                },
                filterToolbar: function () {
                    var grida = $(this);
                    luckyu.grid.filterToolbar(grida);
                },
                deleteRow: function (rowId) {
                    var grida = $(this);
                    grida.delGridRow(rowId, {
                        afterShowForm: function ($form) {
                            $("#dData", $form.parent()).click();
                        }
                    });
                },

                /**
                 * 自定义初始化 jqgrid
                 * @param {any} op
                 */
                LuckyuGrid: function (op) {
                    var self = $(this);
                    for (var i = 0; i < op.colModel.length; i++) {
                        var col = op.colModel[i];
                        if (col.stype === "dataitem") {  // 数据字典 单选
                            col.ltype = col.stype;
                            col.stype = "select";
                            if (!col.formatter) {
                                if (!!col.formatterdataitem) {
                                    col.formatter = function (cellvalue, options, rowObject) {
                                        var result = '';
                                        luckyu.clientdata.getAsync('dataItem', {
                                            key: cellvalue,
                                            code: options.colModel.formatterdataitem,
                                            callback: function (_data) {
                                                if (!!_data.name) {
                                                    result = _data.name;
                                                }
                                            }
                                        });
                                        return result;
                                    };
                                }
                                else {
                                    col.formatter = function (cellvalue, options, rowObject) {
                                        var result = '';
                                        luckyu.clientdata.getAsync('dataItem', {
                                            key: cellvalue,
                                            code: options.colModel.dataitemcode,
                                            callback: function (_data) {
                                                if (!!_data.name) {
                                                    result = _data.name;
                                                }
                                            }
                                        });
                                        return result;
                                    };
                                }
                            }
                            col.searchoptions = {
                                //dataInit: function (ele, op, a) {
                                //    debugger;
                                //},
                                value: function () {
                                    var selectoption = { "-1": "全部" };
                                    var thiscolname = this.name;
                                    var thiscol = op.colModel.filter(z => z.name === thiscolname);
                                    luckyu.clientdata.getAllAsync('dataItem', {
                                        code: thiscol[0].dataitemcode,
                                        callback: function (_datas) {
                                            for (var key in _datas) {
                                                selectoption[_datas[key].value] = _datas[key].name;
                                            }
                                        }
                                    });
                                    return selectoption;
                                },
                                defaultValue: "-1"
                            };
                        }
                        else if (col.stype === "dataitems") {  // 数据字典 多选
                            col.ltype = col.stype;
                            col.stype = "select";
                            if (!col.formatter) {
                                if (!!col.formatterdataitem) {
                                    col.formatter = function (cellvalue, options, rowObject) {
                                        var result = '';
                                        cellvalue = cellvalue.trimRight(',');
                                        luckyu.clientdata.getsAsync('dataItem', {
                                            key: cellvalue,
                                            code: options.colModel.formatterdataitem,
                                            callback: function (_data) {
                                                if (!!_data) {
                                                    result = _data;
                                                }
                                            }
                                        });
                                        return result;
                                    };
                                }
                                else {
                                    col.formatter = function (cellvalue, options, rowObject) {
                                        var result = '';
                                        cellvalue = cellvalue.trimRight(',');
                                        luckyu.clientdata.getsAsync('dataItem', {
                                            key: cellvalue,
                                            code: options.colModel.dataitemcode,
                                            callback: function (_data) {
                                                if (!!_data) {
                                                    result = _data;
                                                }
                                            }
                                        });
                                        return result;
                                    };
                                }
                            }
                            col.searchoptions = {
                                value: function () {
                                    var selectoption = { "-1": "全部" };
                                    var thiscolname = this.name;
                                    var thiscol = op.colModel.filter(z => z.name === thiscolname);
                                    luckyu.clientdata.getAllAsync('dataItem', {
                                        code: thiscol[0].dataitemcode,
                                        callback: function (_datas) {
                                            for (var key in _datas) {
                                                selectoption[_datas[key].value] = _datas[key].name;
                                            }
                                        }
                                    });
                                    return selectoption;
                                },
                                defaultValue: "-1"
                            };
                        }
                        else if (col.stype === "user_id") {
                            col.formatter = function (cellvalue, options, rowObject) {
                                var result = "";
                                luckyu.clientdata.getAsync('user', {
                                    key: cellvalue,
                                    callback: function (_data) {
                                        if (!!_data.name) {
                                            result = _data.name;
                                        }
                                    }
                                });
                                return result;
                            };
                            col.searchoptions = {
                                dataInit: function (elem) {
                                    $(elem).attr("readonly", "readonly").click(function () {
                                        var alreadyValues = $(elem).attr("_realValue");
                                        luckyu.layer.userSelectForm({
                                            initValue: (!!alreadyValues ? alreadyValues.split(',') : []),
                                            multiple: true,
                                            callback: function (list) {
                                                if (list != null && list.length > 0) {
                                                    var ids = list.map(r => r.userId).join(",");
                                                    var names = list.map(r => r.realname).join(",");
                                                    $(elem).attr("_realValue", ids);
                                                    $(elem).val(names);
                                                    self[0].triggerToolbar();
                                                }
                                                else {
                                                    $(elem).attr("_realValue", "").val("");
                                                    self[0].triggerToolbar();
                                                }
                                            }
                                        });
                                    });
                                }
                            };
                        }
                        else if (col.stype === "department_id") {
                            col.formatter = function (cellvalue, options, rowObject) {
                                var result = "";
                                luckyu.clientdata.getAsync('department', {
                                    key: cellvalue,
                                    callback: function (_data) {
                                        if (!!_data.name) {
                                            result = _data.name;
                                        }
                                    }
                                });
                                return result;
                            };
                            col.searchoptions = {
                                dataInit: function (elem) {
                                    $(elem).attr("readonly", "readonly").click(function () {
                                        var alreadyValues = $(elem).attr("_realValue");
                                        luckyu.layer.departmentSelectForm({
                                            initValue: (!!alreadyValues ? alreadyValues.split(',') : []),
                                            multiple: true,
                                            callback: function (nodelist) {
                                                if (!!nodelist && nodelist.length > 0) {
                                                    var ids = nodelist.map(r => r.id).join(",");
                                                    var names = nodelist.map(r => r.label).join(",");
                                                    $(elem).attr("_realValue", ids).val(names);
                                                    self[0].triggerToolbar();
                                                }
                                                else {
                                                    $(elem).attr("_realValue", "").val("");
                                                    self[0].triggerToolbar();
                                                }
                                            }
                                        });
                                    });
                                }
                            };
                        }
                        else if (col.stype === "company_id") {
                            col.formatter = function (cellvalue, options, rowObject) {
                                var result = "";
                                luckyu.clientdata.getAsync('company', {
                                    key: cellvalue,
                                    callback: function (_data) {
                                        if (!!_data.name) {
                                            result = _data.name;
                                        }
                                    }
                                });
                                return result;
                            };
                            col.searchoptions = {
                                dataInit: function (elem) {
                                    $(elem).attr("readonly", "readonly").click(function () {
                                        var alreadyValues = $(elem).attr("_realValue");
                                        luckyu.layer.companySelectForm({
                                            initValue: (!!alreadyValues ? alreadyValues.split(',') : []),
                                            multiple: true,
                                            callback: function (nodelist) {
                                                if (!!nodelist && nodelist.length > 0) {
                                                    var ids = nodelist.map(r => r.id).join(",");
                                                    var names = nodelist.map(r => r.label).join(",");
                                                    $(elem).attr("_realValue", ids).val(names);
                                                    self[0].triggerToolbar();
                                                }
                                                else {
                                                    $(elem).attr("_realValue", "").val("");
                                                    self[0].triggerToolbar();
                                                }
                                            }
                                        });
                                    });
                                }
                            };
                        }
                        else if (col.stype === "daterange") {
                            col.formatter = function (cellvalue, options, rowObject) {
                                var result = "";
                                if (cellvalue > '1900-1-1') {
                                    result = cellvalue;
                                }
                                return result;
                            };
                            col.searchoptions = {
                                dataInit: function (elem) {
                                    $(elem).luckyurangedate({
                                        dfvalue: false,
                                        select: function (begin, end) {
                                            self[0].triggerToolbar();
                                        }
                                    });
                                }
                            };
                        }
                        else if (col.stype === "numberrange") {
                            col.searchoptions = {
                                dataInit: function (elem) {
                                    $(elem).numberrange({
                                        done: function (value, min, max) {
                                            self[0].triggerToolbar();
                                        }
                                    });
                                }
                            };
                        }
                    }
                    if (op.footerrow === true) {
                        var gridComplete = function () {
                            var colModel = self.jqGrid("getGridParam", "colModel");
                            var models = colModel.filter(r => r.footerSummary === true);
                            var rows = self.jqGrid("getRowData");
                            var footerData = {};
                            for (var i = 0; i < models.length; i++) {
                                var m = models[i];
                                footerData[m.name] = 0;
                            }
                            for (var i = 0, l = rows.length; i < l; i++) {
                                var row = rows[i];
                                for (var j = 0; j < models.length; j++) {
                                    var m = models[j];
                                    var value = parseFloat(row[m.name]);
                                    footerData[m.name] += value;
                                }
                            }
                            self.jqGrid("footerData", "set", footerData);
                        };
                        if (!!op.gridComplete) {
                            op.gridComplete = function () {
                                op.gridComplete();
                                gridComplete();
                            }
                        }
                        else {
                            op.gridComplete = gridComplete;
                        }
                    }
                    var grida = $(this).jqGrid(op);
                    return grida;
                },

            });
        }
    };
    luckyu.grid.init();
})(window.layui);