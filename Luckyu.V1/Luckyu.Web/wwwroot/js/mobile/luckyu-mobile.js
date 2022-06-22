(function () {

    var luckyumobile = !window.luckyumobile ? {} : window.luckyumobile;

    luckyumobile = {
        rootUrl: '',
        form: {
            inputWarm: function (ele) {
                var $ele = $(ele);
                var warmCount = 0;
                var warmTimer = setInterval(function () {
                    if ($ele.hasClass("input-warming")) {
                        $ele.removeClass("input-warming");
                    }
                    else {
                        $ele.addClass("input-warming");
                    }
                    if (warmCount >= 4) {
                        clearInterval(warmTimer);
                    }
                    warmCount++;
                }, 300);
                $ele.addClass("input-warming");
            },
        },
        utility: {
            /**
             * 枚举字段 显示
             * */
            toEnum: function (value, enumlist) {
                var result = '';
                var one = enumlist.find(r => r.value == value);
                if (!!one) {
                    result = one.name;
                }
                return result;
            },
            /**
             * 获取Url参数
             * @param {any} name
             */
            getUrlParam: function (name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
                var r = window.location.search.substr(1).match(reg);
                return (r != null) ? unescape(r[2]) : "";
            },
        },
        ajax: {
            get: function (url, postData, successCallback) {
                var uiLoading = bui.loading({
                    width: 40,
                    height: 40,
                });
                uiLoading.show();
                $.ajax({
                    type: 'GET',
                    url: url,
                    async: true,
                    data: postData,
                    dataType: 'json',
                    success: function (res) {
                        uiLoading.hide();
                        successCallback(res);
                    },
                    error: function (xhr, type) {
                        uiLoading.hide();
                        console.log("========= ajax error start ==========");
                        console.log(url);
                        console.log(postData);
                        console.log(xhr);
                        console.log(type);
                        console.log("========= ajax error end ==========");
                    }
                });
            },
            getv2: function (url, postData, successCallback) {
                var uiLoading = bui.loading({
                    width: 40,
                    height: 40,
                });
                uiLoading.show();
                $.ajax({
                    type: 'GET',
                    url: url,
                    async: true,
                    data: postData,
                    dataType: 'json',
                    success: function (res) {
                        uiLoading.hide();
                        if (res.code == 200) {
                            successCallback(res.data, res.info);
                        }
                        else {
                            bui.alert(res.info);
                            //bui.hint({ content: res.info, position: "top", skin: 'danger', showClose: true, autoClose: true, timeout: 3000 });
                        }
                    },
                    error: function (xhr, type) {
                        uiLoading.hide();
                        console.log("========= ajax error start ==========");
                        console.log(url);
                        console.log(postData);
                        console.log(xhr);
                        console.log(type);
                        console.log("========= ajax error end ==========");
                    }
                });
            },
            post: function (url, postData, successCallback) {
                var uiLoading = bui.loading({
                    width: 40,
                    height: 40,
                });
                uiLoading.show();
                $.ajax({
                    type: 'POST',
                    url: url,
                    async: true,
                    data: postData,
                    dataType: 'json',
                    success: function (res) {
                        uiLoading.hide();
                        successCallback(res);
                    },
                    error: function (xhr, type) {
                        uiLoading.hide();
                        console.log("========= ajax error start ==========");
                        console.log(url);
                        console.log(postData);
                        console.log(xhr);
                        console.log(type);
                        console.log("========= ajax error end ==========");
                    }
                });
            },
            postv2: function (url, postData, successCallback) {
                var uiLoading = bui.loading({
                    width: 40,
                    height: 40,
                });
                uiLoading.show();
                $.ajax({
                    type: 'POST',
                    url: url,
                    async: true,
                    data: postData,
                    dataType: 'json',
                    success: function (res) {
                        uiLoading.hide();
                        if (res.code == 200) {
                            successCallback(res.data, res.info);
                        }
                        else {
                            bui.alert(res.info);
                            //bui.hint({ content: res.info, position: "top", skin: 'danger', showClose: true, autoClose: true, timeout: 3000 });
                        }
                    },
                    error: function (xhr, type) {
                        uiLoading.hide();
                        console.log("========= ajax error start ==========");
                        console.log(url);
                        console.log(postData);
                        console.log(xhr);
                        console.log(type);
                        console.log("========= ajax error end ==========");
                    }
                });
            },
            getNoloading: function (url, postData, successCallback) {
                $.ajax({
                    type: 'GET',
                    url: url,
                    async: true,
                    data: postData,
                    dataType: 'json',
                    success: function (res) {
                        successCallback(res);
                    },
                    error: function (xhr, type) {
                        console.log("========= ajax error start ==========");
                        console.log(url);
                        console.log(postData);
                        console.log(xhr);
                        console.log(type);
                        console.log("========= ajax error end ==========");
                    }
                });
            },
            postNoloading: function (url, postData, successCallback) {
                $.ajax({
                    type: 'POST',
                    url: url,
                    async: true,
                    data: postData,
                    dataType: 'json',
                    success: function (res) {
                        successCallback(res);
                    },
                    error: function (xhr, type) {
                        console.log("========= ajax error start ==========");
                        console.log(url);
                        console.log(postData);
                        console.log(xhr);
                        console.log(type);
                        console.log("========= ajax error end ==========");
                    }
                });
            },

        },
        init: function () {
            String.prototype.trimLeft = function (char) {
                return this.replace(new RegExp('^\\' + char + '+', 'g'), '');
            };

            String.prototype.trimRight = function (char) {
                return this.replace(new RegExp('\\' + char + '+$', 'g'), '');
            };

            String.prototype.trim = function (char) {
                return this.replace(new RegExp('^\\' + char + '+|\\' + char + '+$', 'g'), '');
            };

            Date.prototype.DateAdd = function (strInterval, Number) {
                var dtTmp = this;
                switch (strInterval) {
                    case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));// 秒
                    case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));// 分
                    case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));// 小时
                    case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));// 天
                    case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));// 星期
                    case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());// 季度
                    case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());// 月
                    case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());// 年
                }
            }
            // 对Date的扩展，将 Date 转化为指定格式的String
            // 月(M)、日(d)、小时(H)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
            // 年(y)可以用 1-4 个占位符，毫秒(f)只能用 1 个占位符(是 1-3 位的数字) 
            // 例子： 
            // (new Date()).format("yyyy-MM-dd HH:mm:ss.f") ==> 2006-07-02 08:09:04.423 
            // (new Date()).format("yyyy-M-d H:m:s.f")      ==> 2006-7-2 8:9:4.18 
            Date.prototype.format = function (fmt) {
                if (this.getFullYear() <= 1970) {
                    return '';
                }
                var o = {
                    "M+": this.getMonth() + 1,  // 月份 
                    "d+": this.getDate(),       // 日 
                    "H+": this.getHours(),      // 小时 
                    "m+": this.getMinutes(),    // 分 
                    "s+": this.getSeconds(),    // 秒 
                    "q+": Math.floor((this.getMonth() + 3) / 3), // 季度 
                    "f": this.getMilliseconds() // 毫秒 
                };

                if (/(y+)/.test(fmt))
                    fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));

                for (var k in o) {
                    if (new RegExp("(" + k + ")").test(fmt))
                        fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
                }

                return fmt;
            };

            Date.prototype.addDays = function (days) {
                var dat = new Date(this.valueOf());
                dat.setDate(dat.getDate() + days);
                return dat;
            };

            /**
             *  jquery 在 两个class 之间 切换
             * @param {any} class1
             * @param {any} class2
             */
            $.fn.toggle2classes = function (class1, class2) {
                if (!class1 || !class2)
                    return this;

                return this.each(function () {
                    var $elm = $(this);

                    if ($elm.hasClass(class1) || $elm.hasClass(class2))
                        $elm.toggleClass(class1 + ' ' + class2);

                    else
                        $elm.addClass(class1);
                });
            };

            /**
             * 表单验证
             * */
            $.fn.verifyForm = function () {
                var $that = $(this);
                var flag = true;
                $that.find("input").each(function () {
                    var $ele = $(this);
                    if (!!($ele.width() || $ele.height()) && $ele.css("display") !== "none") {  // is:visible
                        var verify = $ele.attr("luckyu-verify");
                        if (!!verify) {
                            if (verify.indexOf("required") > -1) {
                                var val = $ele.val();
                                if (!val) {
                                    var $elename = $ele.attr("placeholder");
                                    if (!$elename) {

                                    }
                                    var msg = (!!$elename ? ($elename + " ") : "") + "必填项不能为空";
                                    bui.hint({ content: msg, position: "top", skin: 'danger', showClose: true, autoClose: true, timeout: 3000 });
                                    flag = false;
                                    luckyumobile.form.inputWarm($ele);
                                    $ele.focus();
                                    return false;
                                }
                            }
                        }

                    }
                });
                return flag;

            };
            /**
             * 获取表单数据
             * */
            $.fn.getFormValue = function () {
                var $that = $(this);
                var data = {};
                $that.find("input,textarea").each(function () {
                    var $ele = $(this);
                    var id = $ele.attr("id");
                    var name = $ele.attr('name');
                    name = !!name ? name : id;

                    var type = $ele.attr("type");
                    switch (type) {
                        case "checkbox":
                            if ($ele.is(":checked")) {
                                var val = $ele.val();
                                data[name] = !val || val === "on" ? 1 : val;
                            } else {
                                data[name] = 0;
                            }
                            break;
                        case "radio":
                            if ($ele.is(":checked")) {
                                data[name] = $ele.val();
                            }
                            break;
                        default:
                            data[name] = $ele.val();
                            break;
                    }
                });
                return data;
            }
            /**
             * 填充表单数据
             */
            $.fn.setFormValue = function (data) {
                var $this = $(this);
                for (var key in data) {
                    var ele = $this.find("#" + key);
                    if (!ele || ele.length < 1) {
                        continue;
                    }
                    var value = data[key];
                    ele.val(value);
                }
            }
        }
    };
    luckyumobile.init();

    window.luckyumobile = luckyumobile;
    if (!!top.luckyumobile.clientdata) {
        window.luckyumobile.clientdata = top.luckyumobile.clientdata;
    }
})();