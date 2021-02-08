/*
 * 下拉框插件
 */
(function (layui) {
    "use strict";
    luckyu.select = {
        init: function () {
            $.fn.extend({
                /**
                 * 数据字典下拉
                 */
                initDataItem: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        code: "", // dataitem code 
                        filterable: true, //开启搜索
                        multiple: false, // 是否多选
                        toolbar: {}, // 工具条
                        direction: "auto", //方向 auto / up / down
                        tips: "请选择",
                        initValue: null, //默认选中
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);

                    var initxmSelect = function (xmdata, xmOption) {
                        if (!xmdata) {
                            xmdata = [];
                        }
                        var op = {
                            el: id,
                            theme: {
                                color: 'var(--tn-nav-color)',
                            },
                            filterable: xmOption.filterable,
                            tips: xmOption.tips,
                            data: xmdata,
                            direction: "auto",
                            initValue: (!!xmOption.initValue ? xmOption.initValue.split(',') : null),
                            size: "small",
                            filterMethod: function (val, item, index, prop) {  // 搜索忽略大小写
                                var value = val.toLowerCase();
                                if (value == item.value) {//把value相同的搜索出来
                                    return true;
                                }
                                if (item.name.indexOf(value) != -1) {//名称中包含的搜索出来
                                    return true;
                                }
                                if (item.name.toLowerCase().indexOf(value) != -1) {// 不区分大小写
                                    return true;
                                }
                                return false;//不知道的就不管了
                            },
                            on: xmOption.select
                        };
                        var verify = self.attr("lay-verify");
                        if (!!verify) {
                            op.layVerify = verify;
                            self.removeAttr("lay-verify");
                        }
                        if (xmOption.multiple === false) {
                            op.radio = true;
                            op.clickClose = true;
                        }
                        else {
                            op.toolbar = xmOption.toolbar;
                        }
                        if (xmdata.length > 50) {
                            op.paging = true;
                            op.pageSize = 50;
                        }
                        var xmSelf = xmSelect.get(id, true);
                        if (!!xmSelf) {
                            xmSelf.update(op);
                        }
                        else {
                            xmSelf = xmSelect.render(op);
                        }
                        return xmSelf;
                    };

                    var xmData = [];
                    var xmselect;
                    luckyu.clientdata.getAllAsync("dataItem", {
                        code: defaultOption.code,
                        callback: function (datas) {
                            if (!!datas) {
                                for (var key in datas) {
                                    var obj = {
                                        name: datas[key].name,
                                        value: datas[key].value,
                                    };
                                    xmData.push(obj);
                                }
                            }
                        }
                    });   // 先取缓存 ,缓存没有再查
                    if (!!xmData && xmData.length > 0) {
                        xmselect = initxmSelect(xmData, defaultOption, self);
                    }
                    else {
                        luckyu.ajax.getSync(luckyu.rootUrl + "/SystemModule/Dataitem/GetDataItemDatailsByCode", { code: defaultOption.code }, function (res) {
                            var data = res.data;
                            for (var i = 0; i < data.length; i++) {
                                var obj = {
                                    name: data[i].showname,
                                    value: data[i].itemvalue,
                                };
                                xmData.push(obj);
                            }
                            xmselect = initxmSelect(xmData, defaultOption, self);
                        });
                    }
                    return xmselect;
                },
                /**
                 * 任意数据源下拉框
                 */
                initDataSource: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        url: "", // 数据接口
                        requestParam: {}, // 请求参数
                        initValue: null,// 默认选中
                        direction: "auto", //方向 auto / up / down
                        nameField: "name", //返回数据显示字段名
                        toolbar: {}, // 工具条
                        valueField: "value",//返回数据值字段名
                        filterable: true, //开启搜索
                        multiple: false, // 是否多选
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);

                    var initxmSelect = function (xmdata, xmOption) {
                        if (!xmdata) {
                            xmdata = [];
                        }
                        if (typeof (xmOption.nameField) !== "string" && xmOption.nameField.length > 0) {
                            for (var i = 0; i < xmdata.length; i++) {
                                var val = '';
                                for (var j = 0; j < xmOption.nameField.length; j++) {
                                    val += xmdata[i][xmOption.nameField[j]] + ' ';
                                }
                                xmdata[i][xmOption.nameField[0]] = $.trim(val);
                            }
                            xmOption.nameField = xmOption.nameField[0];
                        }

                        var op = {
                            el: id,
                            theme: {
                                color: 'var(--tn-nav-color)',
                            },
                            filterable: xmOption.filterable,
                            data: xmdata,
                            prop: {
                                name: defaultOption.nameField,
                                value: defaultOption.valueField
                            },
                            direction: "auto",
                            initValue: (!!xmOption.initValue ? xmOption.initValue.split(',') : null),
                            size: "small",
                            filterMethod: function (val, item, index, prop) {  // 搜索忽略大小写
                                var value = val.toLowerCase();
                                if (value == item[defaultOption.valueField]) {//把value相同的搜索出来
                                    return true;
                                }
                                if (item[defaultOption.nameField].indexOf(value) != -1) {//名称中包含的搜索出来
                                    return true;
                                }
                                if (item[defaultOption.nameField].toLowerCase().indexOf(value) != -1) {// 不区分大小写
                                    return true;
                                }
                                return false;//不知道的就不管了
                            },
                            on: xmOption.select
                        };
                        var verify = self.attr("lay-verify");
                        if (!!verify) {
                            op.layVerify = verify;
                            self.removeAttr("lay-verify");
                        }
                        if (xmOption.multiple === false) {
                            op.radio = true;
                            op.clickClose = true;
                        }
                        else {
                            op.toolbar = xmOption.toolbar;
                        }
                        if (xmdata.length > 50) {
                            op.paging = true;
                            op.pageSize = 50;
                        }
                        var xmSelf = xmSelect.get(id, true);
                        if (!!xmSelf) {
                            xmSelf.update(op);
                        }
                        else {
                            xmSelf = xmSelect.render(op);
                        }
                        return xmSelf;
                    };

                    var xmData = [];
                    var xmselect;
                    luckyu.clientdata.getAllAsync("commonData", {
                        url: defaultOption.url,
                        callback: function (datas) {
                            if (!!datas) {
                                xmData = datas;
                            }
                        }
                    });   // 先取缓存 ,缓存没有再查
                    if (!!xmData && xmData.length > 0) {
                        xmselect = initxmSelect(xmData, defaultOption);
                    }
                    else {
                        luckyu.ajax.getSync(defaultOption.url, defaultOption.requestParam, function (res) {
                            if (!!res.data && !!res.code) {  // 保险处理 ，方式后台直接返回数据还是包了一层
                                xmData = res.data;
                            }
                            else {
                                xmData = res;
                            }
                            xmselect = initxmSelect(xmData, defaultOption);
                        });
                    }
                    return xmselect;
                },
                /**
                 * 树形下拉
                 * */
                initSelectTree: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        url: '',
                        requestParam: {}, // 请求参数
                        filterable: true, //开启搜索
                        multiple: false, // 是否多选
                        direction: "auto", //方向 auto / up / down
                        initValue: null, //默认选中
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);

                    var initxmSelect = function (xmdata, xmOption) {
                        if (!xmdata) {
                            xmdata = [];
                        }
                        var op = {
                            el: id,
                            theme: {
                                color: 'var(--tn-nav-color)',
                            },
                            filterable: defaultOption.filterable,
                            initValue: (!!defaultOption.initValue ? defaultOption.initValue.split(',') : null),
                            size: "small",
                            tree: {
                                show: true, //是否显示树状结构
                                showFolderIcon: true, //是否展示三角图标
                                showLine: true,	//是否显示虚线
                                indent: 20,//间距
                                expandedKeys: true,//默认展开节点的数组, 为 true 时, 展开所有节点
                                strict: false,  //是否严格遵守父子模式
                            },
                            data: xmdata,
                            on: defaultOption.select
                        };
                        var verify = self.attr("lay-verify");
                        if (!!verify) {
                            op.layVerify = verify;
                            self.removeAttr("lay-verify");
                        }
                        if (xmOption.multiple === false) {
                            op.radio = true;
                            op.clickClose = true;
                        }
                        //if (xmdata.length > 50) {
                        //    op.paging = true;
                        //    op.pageSize = 50;
                        //}
                        var xmSelf = xmSelect.get(id, true);
                        if (!!xmSelf) {
                            xmSelf.update(op);
                        }
                        else {
                            xmSelf = xmSelect.render(op);
                        }
                        return xmSelf;
                    };
                    var xmselect;
                    luckyu.ajax.getSync(defaultOption.url, defaultOption.requestParam,
                        function (res) {
                            var xmData;
                            if (!!res.data && !!res.code) {  // 保险处理 ，方式后台直接返回数据还是包了一层
                                xmData = res.data;
                            }
                            else {
                                xmData = res;
                            }
                            xmselect = initxmSelect(xmData, defaultOption, self);
                        });
                    return xmselect;
                },
                /**
                 * 本地数据下拉
                 */
                initLocal: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        filterable: false, //开启搜索
                        multiple: false, // 是否多选
                        initValue: null, //默认选中
                        direction: "auto", //方向 auto / up / down
                        nameField: "name", //返回数据显示字段名
                        valueField: "value",//返回数据值字段名
                        data: [],// 数据
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);
                    var op = {
                        el: id,
                        theme: {
                            color: 'var(--tn-nav-color)',
                        },
                        prop: {
                            name: defaultOption.nameField,
                            value: defaultOption.valueField
                        },
                        filterable: defaultOption.filterable,
                        data: defaultOption.data,
                        initValue: (!!defaultOption.initValue ? defaultOption.initValue.split(',') : null),
                        size: "small",
                        on: defaultOption.select
                    };
                    if (defaultOption.multiple === false) {
                        op.radio = true;
                        op.clickClose = true;
                    }
                    var verify = self.attr("lay-verify");
                    if (!!verify) {
                        op.layVerify = verify;
                        self.removeAttr("lay-verify");
                    }
                    var xmSelf = xmSelect.get(id, true);
                    if (!!xmSelf) {
                        xmSelf.update(op);
                    }
                    else {
                        xmSelf = xmSelect.render(op);
                    }
                    return xmSelf;
                },
                /**
                 * 树形公司下拉
                 */
                initCompany: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        filterable: true, //开启搜索
                        multiple: false, // 是否多选
                        direction: "auto", //方向 auto / up / down
                        initValue: null, //默认选中
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);
                    luckyu.ajax.getSync(luckyu.rootUrl + "/OrganizationModule/Company/GetSelectTree", {},
                        function (res) {
                            var xmData = !!res ? res : [];
                            var op = {
                                el: id,
                                theme: {
                                    color: 'var(--tn-nav-color)',
                                },
                                filterable: defaultOption.filterable,
                                initValue: (!!defaultOption.initValue ? defaultOption.initValue.split(',') : null),
                                size: "small",
                                tree: {
                                    show: true, //是否显示树状结构
                                    showFolderIcon: true, //是否展示三角图标
                                    showLine: true,	//是否显示虚线
                                    indent: 20,//间距
                                    expandedKeys: true,//默认展开节点的数组, 为 true 时, 展开所有节点
                                    strict: false,  //是否严格遵守父子模式
                                },
                                data: xmData,
                                on: defaultOption.select
                            };
                            if (defaultOption.multiple === false) {
                                op.radio = true;
                                op.clickClose = true;
                            }
                            var verify = self.attr("lay-verify");
                            if (!!verify) {
                                op.layVerify = verify;
                                self.removeAttr("lay-verify");
                            }
                            var xmSelf = xmSelect.get(id, true);
                            if (!!xmSelf) {
                                xmSelf.update(op);
                            }
                            else {
                                xmSelect.render(op);
                            }
                        });
                },
                /**
                 * 树形部门下拉
                 */
                initDepartment: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        companyId: "",// 公司编码
                        filterable: true, //开启搜索
                        initValue: null,
                        direction: "auto", //方向 auto / up / down
                        multiple: false, // 是否多选
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);
                    luckyu.ajax.getSync(luckyu.rootUrl + "/OrganizationModule/Department/GetSelectTree",
                        { companyId: defaultOption.companyId },
                        function (res) {
                            var xmData = res;
                            var op = {
                                el: id,
                                theme: {
                                    color: 'var(--tn-nav-color)',
                                },
                                filterable: defaultOption.filterable,
                                initValue: (!!defaultOption.initValue ? defaultOption.initValue.split(',') : null),
                                size: "small",
                                tree: {
                                    show: true, //是否显示树状结构
                                    showFolderIcon: true, //是否展示三角图标
                                    showLine: true,	//是否显示虚线
                                    indent: 20,//间距
                                    expandedKeys: true,//默认展开节点的数组, 为 true 时, 展开所有节点
                                    strict: false,  //是否严格遵守父子模式
                                },
                                data: xmData,
                                on: defaultOption.select
                            };
                            if (defaultOption.multiple === false) {
                                op.radio = true;
                                op.clickClose = true;
                            }
                            var verify = self.attr("lay-verify");
                            if (!!verify) {
                                op.layVerify = verify;
                                self.removeAttr("lay-verify");
                            }
                            var xmSelf = xmSelect.get(id, true);
                            if (!!xmSelf) {
                                xmSelf.update(op);
                            }
                            else {
                                xmSelect.render(op);
                            }
                        });
                },
                /**
                 * 树形岗位下拉
                 */
                initPost: function (option) {
                    var self = $(this);
                    var id = "#" + self.attr("id");
                    self.addClass("xm-select");
                    var defaultOption = {
                        companyId: "",// 公司编码
                        filterable: true, //开启搜索
                        initValue: null,
                        direction: "auto", //方向 auto / up / down
                        multiple: false, // 是否多选
                        select: null, // 选中事件 function (data){}
                    };
                    $.extend(defaultOption, option);
                    luckyu.ajax.getSync(luckyu.rootUrl + "/OrganizationModule/Post/GetSelectTree",
                        { companyId: defaultOption.companyId },
                        function (res) {
                            var xmData = res;
                            var op = {
                                el: id,
                                theme: {
                                    color: 'var(--tn-nav-color)',
                                },
                                filterable: defaultOption.filterable,
                                initValue: (!!defaultOption.initValue ? defaultOption.initValue.split(',') : null),
                                size: "small",
                                tree: {
                                    show: true, //是否显示树状结构
                                    showFolderIcon: true, //是否展示三角图标
                                    showLine: true,	//是否显示虚线
                                    indent: 20,//间距
                                    expandedKeys: true,//默认展开节点的数组, 为 true 时, 展开所有节点
                                    strict: false,  //是否严格遵守父子模式
                                },
                                data: xmData,
                                on: defaultOption.select
                            };
                            if (defaultOption.multiple === false) {
                                op.radio = true;
                                op.clickClose = true;
                            }
                            var verify = self.attr("lay-verify");
                            if (!!verify) {
                                op.layVerify = verify;
                                self.removeAttr("lay-verify");
                            }
                            var xmSelf = xmSelect.get(id, true);
                            if (!!xmSelf) {
                                xmSelf.update(op);
                            }
                            else {
                                xmSelect.render(op);
                            }
                        });
                },
                /**
                 * 数据字典Checkbox、Radio
                 */
                initDataItemCheckbox: function (option) {
                    var self = $(this);
                    var defaultOption = {
                        type: "checkbox",  // checkbox radio
                        code: "", // dataitem code 
                        initValue: null,
                    };
                    $.extend(defaultOption, option);

                    var initCtrl = function (ctrlOption, ctrlDatas, ctrlElem) {
                        if (!ctrlDatas) {
                            ctrlDatas = [];
                        }
                        var html = '';
                        var id = ctrlElem.attr("id");
                        for (var i = 0; i < ctrlDatas.length; i++) {
                            var ischecked = false;
                            if (!!ctrlOption.initValue) {
                                if (ctrlOption.type === 'radio') {
                                    if (ctrlOption.initValue === ctrlDatas[i].value) {
                                        ischecked = true;
                                    }
                                }
                                else {
                                    if (ctrlOption.initValue.indexOf(ctrlDatas[i].value) >= 0) {
                                        ischecked = true;
                                    }
                                }
                            }
                            html += '<input type="' + ctrlOption.type + '" ' + (ctrlOption.type == "checkbox" ? 'lay-skin="primary"' : '') + ' name="' + id + '" value="' + ctrlDatas[i].value + '" title="' + ctrlDatas[i].name + '" ' + (ischecked ? 'checked' : '') + ' />';
                        }
                        ctrlElem.append(html).removeAttr("id");
                        layui.form.render();
                    };

                    var ctrlData = [];
                    luckyu.clientdata.getAllAsync("dataItem", {
                        code: defaultOption.code,
                        callback: function (datas) {
                            if (!!datas) {
                                for (var key in datas) {
                                    var obj = {
                                        name: datas[key].text,
                                        value: datas[key].value,
                                    };
                                    ctrlData.push(obj);
                                }
                            }
                        }
                    });   // 先取缓存 ,缓存没有再查
                    if (!!ctrlData && ctrlData.length > 0) {
                        initCtrl(defaultOption, ctrlData, self);
                    }
                    else {
                        luckyu.ajax.getSync(luckyu.rootUrl + "/SystemModule/Dataitem/GetDataItemDatailsByCode", { code: defaultOption.code }, function (res) {
                            initCtrl(defaultOption, res, self);
                        });
                    }

                },
            });
        },
        /**
         * 省市区联动下拉框
         * @param {object} 省市区选择器，{provinceEle:"",cityEle:"",countryEle:""}
         */
        initArea: function (option) {
            var createArea = function (ele, select) {
                var op = {
                    el: ele,
                    theme: {
                        color: 'var(--tn-nav-color)',
                    },
                    filterable: true,
                    size: "small",
                    data: [],
                    direction: "auto", //方向 auto / up / down
                    radio: true,
                    clickClose: true,
                    on: function (data) {
                        if (!!select) {
                            select(data);
                        }
                    }
                };
                return xmSelect.render(op);
            }
            var updateArea = function (ele, parentId) {
                luckyu.ajax.getSync(
                    luckyu.rootUrl + "/BaseModule/Area/GetAreaByParentId",
                    { parentId: parentId },
                    function (res) {
                        ele.update({
                            data: res
                        })
                    });
            };

            var province, city, country;
            if (!!option.provinceEle) {
                province = createArea(option.provinceEle, function (data) {
                    if (!!option.cityEle && !!data.arr.length > 0) {
                        updateArea(city, data.arr[0].value);
                    }
                    if (option.countryEle) {
                        country.update({
                            data: []
                        })
                    }
                });
            }
            if (!!option.cityEle) {
                city = createArea(option.cityEle, function (data) {
                    if (option.countryEle && !!data.arr.length > 0) {
                        updateArea(country, data.arr[0].value);
                    }
                });
            }
            if (!!option.countryEle) {
                country = createArea(option.countryEle, null);
            }
            updateArea(province, "0");
        }
    };
    luckyu.select.init();
})(window.layui);