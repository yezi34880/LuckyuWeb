/*
 * 描 述：获取客户端数据
 */
(function ($, luckyu) {
    "use strict";

    var loadSate = {
        no: -1,  // 还未加载
        yes: 1,  // 已经加载成功
        ing: 0,  // 正在加载中
        fail: 2  // 加载失败
    };

    var clientDataFn = {};
    var clientAsyncData = {};

    var clientData = {};

    function initLoad(callback) {
        var res = loadSate.yes;
        for (var id in clientDataFn) {
            var _fn = clientDataFn[id];
            if (_fn.state == loadSate.fail) {
                res = loadSate.fail;
                break;
            }
            else if (_fn.state == loadSate.no) {
                res = loadSate.ing;
                _fn.init();
            }
            else if (_fn.state == loadSate.ing) {
                res = loadSate.ing;
            }
        }
        if (res == loadSate.yes) {
            callback(true);
        } else if (res == loadSate.fail) {
            callback(false);
        }
        else {
            setTimeout(function () {
                initLoad(callback);
            }, 100);
        }
    }
    function get(key, data) {
        var res = "";
        var len = data.length;
        if (len == undefined) {
            res = data[key];
        }
        else {
            for (var i = 0; i < len; i++) {
                if (key(data[i])) {
                    res = data[i];
                    break;
                }
            }
        }
        return res;
    }

    /*******************登录后数据***********************/
    // 注册数据的加载方法
    // 登录用户信息
    clientDataFn.userinfo = {
        state: loadSate.no,
        init: function () {
            //初始化加载数据
            clientDataFn.userinfo.state = loadSate.ing;
            luckyu.ajax.getNoloading(luckyu.rootUrl + '/Home/GetUserInfo', {}, function (res) {
                if (res.code == 200) {
                    clientData.userinfo = res.data;
                    clientDataFn.userinfo.state = loadSate.yes;
                }
                else {
                    clientDataFn.userinfo.state = loadSate.fail;
                }
            });
        }
    };

    /*******************使用时异步获取*******************/
    var storage = {
        get: function (name) {
            if (localStorage) {
                return JSON.parse(localStorage.getItem(name)) || {};
            }
            else {
                return clientData[name] || {};
            }
        },
        set: function (name, data) {
            if (localStorage) {
                localStorage.setItem(name, JSON.stringify(data));
            }
            else {
                clientData[name] = data;
            }
        }
    };
    // 公司信息
    clientAsyncData.company = {
        states: loadSate.no,
        init: function () {
            if (clientAsyncData.company.states == loadSate.no) {
                clientAsyncData.company.states = loadSate.ing;
                var ver = storage.get("companyData").ver || "";
                luckyu.ajax.getNoloading(top.luckyu.rootUrl + '/OrganizationModule/Company/GetMap', { ver: ver }, function (res) {
                    var data = res.data;
                    if (!data) {
                        clientAsyncData.company.states = loadSate.fail;
                    } else {
                        if (data.ver) {
                            storage.set("companyData", data);
                        }
                        clientAsyncData.company.states = loadSate.yes;
                        clientAsyncData.company.init();
                    }
                });
            }
        },
        get: function (op) {
            clientAsyncData.company.init();
            if (clientAsyncData.company.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.company.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("companyData").data || {};
                op.callback(data[op.key] || {}, op);
            }
        },
        getAll: function (op) {
            clientAsyncData.company.init();
            if (clientAsyncData.company.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.company.getAll(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("companyData").data || {};
                op.callback(data, op);
            }
        },
        update: function () {
            clientAsyncData.company.states = loadSate.no;
            clientAsyncData.company.init();
        }

    };
    // 部门信息
    clientAsyncData.department = {
        states: loadSate.no,
        init: function () {
            if (clientAsyncData.department.states == loadSate.no) {
                clientAsyncData.department.states = loadSate.ing;
                var ver = storage.get("departmentData").ver || "";
                luckyu.ajax.getNoloading(top.luckyu.rootUrl + '/OrganizationModule/Department/GetMap', { ver: ver }, function (res) {
                    var data = res.data;
                    if (!data) {
                        clientAsyncData.department.states = loadSate.fail;
                    } else {
                        if (data.ver) {
                            storage.set("departmentData", data);
                        }
                        clientAsyncData.department.states = loadSate.yes;
                        clientAsyncData.department.init();
                    }
                });
            }
        },
        get: function (op) {
            clientAsyncData.department.init();
            if (clientAsyncData.department.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.department.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("departmentData").data || {};
                op.callback(data[op.key] || {}, op);
            }
        },
        gets: function (op) {
            clientAsyncData.department.init();
            if (clientAsyncData.department.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.department.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("departmentData").data || {};
                var keyList = op.key.split(',');
                var list = []
                $.each(keyList, function (_index, _item) {
                    var _item1 = data[_item] || {};
                    list.push(_item1);
                });
                op.callback(list || [], op);
            }
        },
        getAll: function (op) {
            clientAsyncData.department.init();
            if (clientAsyncData.department.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.department.getAll(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("departmentData").data || {};
                op.callback(data, op);
            }
        },
        update: function () {
            clientAsyncData.department.states = loadSate.no;
            clientAsyncData.department.init();
        }

    };
    // 人员信息
    clientAsyncData.user = {
        states: loadSate.no,
        init: function () {
            if (clientAsyncData.user.states == loadSate.no) {
                clientAsyncData.user.states = loadSate.ing;
                var ver = storage.get("userData").ver || "";
                luckyu.ajax.getNoloading(top.luckyu.rootUrl + '/OrganizationModule/User/GetMap', { ver: ver }, function (res) {
                    var data = res.data;
                    if (!data) {
                        clientAsyncData.user.states = loadSate.fail;
                    } else {
                        if (data.ver) {
                            storage.set("userData", data);
                        }
                        clientAsyncData.user.states = loadSate.yes;
                        clientAsyncData.user.init();
                    }
                });
            }
        },
        get: function (op) {
            clientAsyncData.user.init();
            if (clientAsyncData.user.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.user.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("userData").data || {};
                op.callback(data[op.key] || {}, op);
            }
        },
        gets: function (op) {
            clientAsyncData.user.init();
            if (clientAsyncData.user.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.user.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("userData").data || {};

                var keyList = op.key.split(',');
                var list = []
                $.each(keyList, function (_index, _item) {
                    var _item1 = data[_item] || {};
                    list.push(_item1);
                });
                op.callback(list || [], op);
            }
        },
        getAll: function (op) {
            clientAsyncData.user.init();
            if (clientAsyncData.user.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.user.getAll(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("userData").data || {};
                op.callback(data, op);
            }
        },
        update: function () {
            clientAsyncData.user.states = loadSate.no;
            clientAsyncData.user.init();
        }

    };
    // 用户组
    clientAsyncData.group = {
        states: loadSate.no,
        init: function () {
            if (clientAsyncData.group.states == loadSate.no) {
                clientAsyncData.group.states = loadSate.ing;
                var ver = storage.get("groupData").ver || "";
                luckyu.ajax.getNoloading(top.luckyu.rootUrl + '/OrganizationModule/Group/GetMap', { ver: ver }, function (res) {
                    var data = res.data;
                    if (!data) {
                        clientAsyncData.group.states = loadSate.fail;
                    } else {
                        if (data.ver) {
                            storage.set("groupData", data);
                        }
                        clientAsyncData.group.states = loadSate.yes;
                        clientAsyncData.group.init();
                    }
                });
            }
        },
        get: function (op) {
            clientAsyncData.group.init();
            if (clientAsyncData.group.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.group.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("groupData").data || {};
                op.callback(data[op.key] || {}, op);
            }
        },
        getAll: function (op) {
            clientAsyncData.group.init();
            if (clientAsyncData.group.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.group.getAll(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("groupData").data || {};
                op.callback(data, op);
            }
        },
        update: function () {
            clientAsyncData.group.states = loadSate.no;
            clientAsyncData.group.init();
        }

    };
    // 数据字典
    clientAsyncData.dataItem = {
        states: loadSate.no,
        init: function () {
            if (clientAsyncData.dataItem.states == loadSate.no) {
                clientAsyncData.dataItem.states = loadSate.ing;
                var ver = storage.get("dataItemData").ver || "";
                luckyu.ajax.getNoloading(top.luckyu.rootUrl + '/SystemModule/DataItem/GetMap', { ver: ver }, function (res) {
                    var data = res.data;
                    if (!data) {
                        clientAsyncData.dataItem.states = loadSate.fail;
                    } else {
                        if (data.ver) {
                            storage.set("dataItemData", data);
                        }
                        clientAsyncData.dataItem.states = loadSate.yes;
                        clientAsyncData.dataItem.init();
                    }
                });
            }
        },
        get: function (op) {
            clientAsyncData.dataItem.init();
            if (clientAsyncData.dataItem.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.dataItem.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("dataItemData").data || {};

                // 数据字典翻译
                var _item = clientAsyncData.dataItem.find(op.key, data[op.code] || {});
                op.callback(_item, op);
            }
        },
        getAll: function (op) {
            clientAsyncData.dataItem.init();
            if (clientAsyncData.dataItem.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.dataItem.getAll(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("dataItemData").data || {};
                var res = {};
                $.each(data[op.code] || {}, function (_index, _item) {
                    res[_index] = _item;
                });
                op.callback(res, op);
            }
        },
        gets: function (op) {
            clientAsyncData.dataItem.init();
            if (clientAsyncData.dataItem.states == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.dataItem.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = storage.get("dataItemData").data || {};

                var keyList = op.key.split(',');
                var _text = []
                $.each(keyList, function (_index, _item) {
                    var _item = clientAsyncData.dataItem.find(_item, data[op.code] || {});
                    _text.push(_item.name);
                });
                op.callback(String(_text), op);
            }
        },
        find: function (key, data) {
            var res = {};
            for (var id in data) {
                if (data[id].value == key) {
                    res = data[id];
                    break;
                }
            }
            return res;
        },
        update: function () {
            clientAsyncData.dataItem.states = loadSate.no;
            clientAsyncData.dataItem.init();
        }
    };
    // 获取自定义数据 url key keyId
    //调用方式：
    /* luckyu.clientdata.getAsync('commonData', {
         url: url,
         key: value,
         keyId: valueId,
         callback: function (item) {
             callback(item.F_FullName);
         }
     });
     调用示例：
     luckyu.clientdata.getAsync('commonData', {
                                 url: "/SystemModule/ExchangeRate/GetExchangeEntity",
                                 key: cellvalue,
                                 keyId: "ErCode",
                                 callback: function (item, op) {
                                     result = item.CurrencyName;
                                 }
                             });
     */
    clientAsyncData.commonData = {
        states: {},
        get: function (op) {
            if (clientAsyncData.commonData.states[op.url] == undefined || clientAsyncData.commonData.states[op.url] == loadSate.no) {
                clientAsyncData.commonData.states[op.url] = loadSate.ing;
                clientAsyncData.commonData.load(op.url);
            }
            if (clientAsyncData.commonData.states[op.url] == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.commonData.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = clientData[op.url] || [];
                if (!!data) {
                    op.callback(clientAsyncData.commonData.find(op.key, op.keyId, data) || {}, op);
                } else {
                    op.callback({}, op);
                }
            }
        },
        gets: function (op) {
            if (clientAsyncData.commonData.states[op.url] == undefined || clientAsyncData.commonData.states[op.url] == loadSate.no) {
                clientAsyncData.commonData.states[op.url] = loadSate.ing;
                clientAsyncData.commonData.load(op.url);
            }
            if (clientAsyncData.commonData.states[op.url] == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.commonData.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = clientData[op.url] || [];
                if (!!data) {
                    var keyList = op.key.split(',');
                    var list = []
                    $.each(keyList, function (_index, key) {
                        if (!key) {
                            return;
                        }
                        var item = clientAsyncData.commonData.find(key, op.keyId, data) || {};
                        if (!!item) {
                            list.push(item);
                        }
                    });
                    op.callback(list, op);
                } else {
                    op.callback([], op);
                }
            }
        },
        getAll: function (op) {
            if (clientAsyncData.commonData.states[op.url] == undefined || clientAsyncData.commonData.states[op.url] == loadSate.no) {
                clientAsyncData.commonData.states[op.url] = loadSate.ing;
                clientAsyncData.commonData.load(op.url);
            }
            if (clientAsyncData.commonData.states[op.url] == loadSate.ing) {
                setTimeout(function () {
                    clientAsyncData.commonData.get(op);
                }, 100);// 如果还在加载100ms后再检测
            }
            else {
                var data = clientData[op.url] || [];
                if (!!data) {
                    op.callback(data, op);
                } else {
                    op.callback([], op);
                }
            }
        },
        load: function (url) {
            luckyu.ajax.getNoloading(top.luckyu.rootUrl + url, {}, function (res) {
                var data = res.data;
                if (!!data) {
                    clientData[url] = data;
                }
                clientAsyncData.commonData.states[url] = loadSate.yes;
            });
        },
        find: function (key, keyId, data) {
            var res = {};
            for (var i = 0, l = data.length; i < l; i++) {
                if (data[i][keyId] == key) {
                    res = data[i];
                    break;
                }
            }
            return res;
        }
    };

    luckyu.clientdata = {
        init: function (callback) {
            initLoad(function (res) {
                callback(res);
                if (res) {// 开始异步加载数据
                    clientAsyncData.company.init();
                    clientAsyncData.department.init();
                    clientAsyncData.group.init();
                    clientAsyncData.user.init();
                    clientAsyncData.dataItem.init();
                }
            });
        },
        get: function (nameArray) {//[key,function (v) { return v.key == value }]
            var res = "";
            if (!nameArray) {
                return res;
            }
            var len = nameArray.length;
            var data = clientData;
            for (var i = 0; i < len; i++) {
                res = get(nameArray[i], data);
                if (res != "" && res != undefined) {
                    data = res;
                }
                else {
                    break;
                }
            }
            res = res || "";
            return res;
        },
        getAsync: function (name, op) {//
            return clientAsyncData[name].get(op);
        },
        getAllAsync: function (name, op) {//
            return clientAsyncData[name].getAll(op);
        },
        getsAsync: function (name, op) {//
            return clientAsyncData[name].gets(op);
        },
        update: function (name) {
            clientAsyncData[name].update();
        },
        updateAll: function () {
            clientAsyncData.company.update();
            clientAsyncData.department.update();
            clientAsyncData.group.update();
            clientAsyncData.user.update();
            clientAsyncData.dataItem.update();
        },

        getDataitemName: function (value, code) {
            var result = '';
            luckyu.clientdata.getAsync('dataItem', {
                key: value,
                code: code,
                callback: function (_data) {
                    if (!!_data.name) {
                        result = _data.name;
                    }
                }
            });
            return result;

        },
        getUserName: function (userId) {
            var result = "";
            luckyu.clientdata.getAsync('user', {
                key: userId,
                callback: function (_data) {
                    if (!!_data.name) {
                        result = _data.name;
                    }
                }
            });
            return result;
        },
        getUserInfo: function (userId) {
            var result = {};
            luckyu.clientdata.getAsync('user', {
                key: userId,
                callback: function (_data) {
                    if (!!_data.name) {
                        result = _data;
                    }
                }
            });
            return result;
        },
        getDepartmentName: function (departmentId) {
            var result = "";
            luckyu.clientdata.getAsync('department', {
                key: departmentId,
                callback: function (_data) {
                    if (!!_data.name) {
                        result = _data.name;
                    }
                }
            });
            return result;
        },
        getCompanyName: function (companyId) {
            var result = "";
            luckyu.clientdata.getAsync('company', {
                key: companyId,
                callback: function (_data) {
                    if (!!_data.name) {
                        result = _data.name;
                    }
                }
            });
            return result;

        }

    };

})(window.jQuery, top.luckyu);
