/***
 * 尽量避免js全局变量的污染，基础全局方法方法全部写在window.luckyu下面
 * ***/
(function (layui) {
    "use strict";

    layui.use(['layer'], function () { });
    var luckyu = {};

    luckyu.rootUrl = "";  // 跟路径

    luckyu.httpCode = {
        exception: 500,
        fail: 400,
        success: 200
    };

    // 工具方法
    luckyu.utility = {
        /**
         * 字典转换
         * @param {any} value
         * @param {any} enumlist
         */
        toEnum: function (value, enumlist) {
            var result = '';
            var one = enumlist.find(r => r.value == value);
            if (!!one) {
                result = one.name;
            }
            return result;
        },
        /**
         * 转换数字类型
         * */
        toFloat: function (obj, decimal) {
            if (!!obj) {
                var num = parseFloat(obj);
                if (isNaN(num)) {
                    num = 0;
                }
                if (decimal && decimal > 0) {
                    num = parseFloat(num.toFixed(decimal));
                }
                return num;
            }
            else {
                return 0;
            }
        },

        // 数字转字符串显示
        floatToString: function (num, decimal) {
            return isNaN(num) ? "" : num.toFixed(decimal);
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
        // 生成GUID
        newGuid: function () {
            function S4() {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            }
            //return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
            return (S4() + S4() + S4() + S4() + S4() + S4() + S4() + S4());
        },
        // 打开新页面，下载
        download: function (options) {
            var defaults = {
                method: "GET",
                url: "",
                param: []
            };
            var options = $.extend(defaults, options);
            if (options.url && options.param) {
                var $form = $('<form action="' + options.url + '" method="' + (options.method || 'post') + '"></form>');
                for (var key in options.param) {
                    var $input = $('<input type="hidden" />').attr('name', key).val(options.param[key]);
                    $form.append($input);
                }
                $form.appendTo('body').submit().remove();
            };
        },

        /**  
         * 数字格式转换成千分位  
         *@param{Object}num  
         */
        commafy: function (num) {
            //1.先去除空格,判断是否空值和非数   
            num = num + "";
            num = num.replace(/[ ]/g, ""); //去除空格  
            if (num == "") {
                return;
            }
            if (isNaN(num)) {
                return;
            }
            //2.针对是否有小数点，分情况处理   
            var index = num.indexOf(".");
            if (index == -1) {//无小数点   
                var reg = /(-?\d+)(\d{3})/;
                while (reg.test(num)) {
                    num = num.replace(reg, "$1,$2");
                }
            } else {
                var intPart = num.substring(0, index);
                var pointPart = num.substring(index + 1, num.length);
                var reg = /(-?\d+)(\d{3})/;
                while (reg.test(intPart)) {
                    intPart = intPart.replace(reg, "$1,$2");
                }
                num = intPart + "." + pointPart;
            }
            return num;
        },

        /**  
         * 去除千分位  
         *@param{Object}num  
         */
        delcommafy: function (num) {
            num = num.replace(/[ ]/g, "");//去除空格  
            num = num.replace(/,/gi, '');
            return num;
        },

        // 转化成十进制
        toDecimal: function (num) {
            if (num == null) {
                num = "0";
            }
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num))
                num = "0";
            var sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            var cents = num % 100;
            num = Math.floor(num / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + '' +
                    num.substring(num.length - (4 * i + 3));
            return (((sign) ? '' : '-') + num + '.' + cents);
        },
        /*
         * 文件大小转换
         */
        countFileSize: function (size) {
            if (size < 1024.00)
                return luckyu.utility.toDecimal(size) + " 字节";
            else if (size >= 1024.00 && size < 1048576)
                return luckyu.utility.toDecimal(size / 1024.00) + " KB";
            else if (size >= 1048576 && size < 1073741824)
                return luckyu.utility.toDecimal(size / 1024.00 / 1024.00) + " MB";
            else if (size >= 1073741824)
                return luckyu.utility.toDecimal(size / 1024.00 / 1024.00 / 1024.00) + " GB";
        },

        // #region 日期相关
        parseDate: function (strDate) {
            var myDate;
            if (strDate.indexOf("/Date(") > -1)
                myDate = new Date(parseInt(strDate.replace("/Date(", "").replace(")/", ""), 10));
            else
                myDate = new Date(Date.parse(strDate.replace(/-/g, "/").replace("T", " ").split(".")[0]));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
            return myDate;
        },
        // 日期格式化v日期,format:格式
        formatDate: function (v, format) {
            if (!v) return "";
            var d = v;
            if (typeof v === 'string') {
                if (v.indexOf("/Date(") > -1)
                    d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
                else
                    d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0]));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
            }
            var o = {
                "M+": d.getMonth() + 1,  //month
                "d+": d.getDate(),       //day
                "h+": d.getHours(),      //hour
                "m+": d.getMinutes(),    //minute
                "s+": d.getSeconds(),    //second
                "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
                "S": d.getMilliseconds() //millisecond
            };
            if (/(y+)/.test(format)) {
                format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
            }
            for (var k in o) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
            return format;
        },
        // 获取当前时间;format为格式
        getDate: function (format, strInterval, Number) {
            var myDate = new Date();
            if (!!strInterval) {
                myDate = myDate.DateAdd(strInterval, Number);
            }
            var res = luckyu.utility.formatDate(myDate, format);
            return res;
        },
        // 月
        getMonth: function () {
            var res = {
                begin: '',
                end: ''
            };
            var currentDate = luckyu.utility.parseDate(luckyu.utility.formatDate(new Date(), "yyyy-MM-01"));
            var endDate = currentDate.DateAdd('m', 1).DateAdd('d', -1);

            res.begin = luckyu.utility.formatDate(currentDate, 'yyyy-MM-dd 00:00:00');
            res.end = luckyu.utility.formatDate(endDate, 'yyyy-MM-dd 23:59:59');

            return res;
        },
        getPreMonth: function () {
            var res = {
                begin: '',
                end: ''
            };
            var currentDate = luckyu.utility.parseDate(luckyu.utility.formatDate(new Date(), "yyyy-MM-01"));
            var preMonth = currentDate.DateAdd('d', -1);

            res.begin = luckyu.utility.formatDate(preMonth, 'yyyy-MM-01 00:00:00');
            res.end = luckyu.utility.formatDate(preMonth, 'yyyy-MM-dd 23:59:59');

            return res;
        },
        // 季度
        getCurrentQuarter: function () {
            var currentDate = new Date();
            return luckyu.utility.getQuarter(currentDate.getFullYear(), currentDate.getMonth());
        },
        getPreQuarter: function () {
            var currentDate = new Date().DateAdd('q', -1);
            return luckyu.utility.getQuarter(currentDate.getFullYear(), currentDate.getMonth());
        },
        getQuarter: function (Year, month) {
            var res = {
                begin: '',
                end: ''
            };
            switch (month) {
                case 0:
                case 1:
                case 2:
                    res.begin = Year + "-01-01 00:00:00";
                    res.end = Year + "-03-31 23:59:59";
                    break;
                case 3:
                case 4:
                case 5:
                    res.begin = Year + "-04-01 00:00:00";
                    res.end = Year + "-06-30 23:59:59";
                    break;
                case 6:
                case 7:
                case 8:
                    res.begin = Year + "-07-01 00:00:00";
                    res.end = Year + "-09-30 23:59:59";
                    break;
                case 9:
                case 10:
                case 11:
                    res.begin = Year + "-10-01 00:00:00";
                    res.end = Year + "-12-31 23:59:59";
                    break;
            }
            return res;
        },
        // 年
        getYear: function () {
            var currentDate = new Date();
            var res = {
                begin: '',
                end: ''
            };
            var year = currentDate.getFullYear();
            res.begin = year + '-01-01 00:00:00';
            res.end = year + '-12-31 23:59:59';
            return res;
        },
        getPreYear: function () {
            var currentDate = new Date();
            var res = {
                begin: '',
                end: ''
            };
            var year = currentDate.getFullYear() - 1;
            res.begin = year + '-01-01 00:00:00';
            res.end = year + '-12-31 23:59:59';
            return res;
        },
        getFirstHalfYear: function () {
            var currentDate = new Date();
            var res = {
                begin: '',
                end: ''
            };
            var year = currentDate.getFullYear();
            res.begin = year + '-01-01 00:00:00';
            res.end = year + '-06-30 23:59:59';
            return res;
        },
        getSecondHalfYear: function () {
            var currentDate = new Date();
            var res = {
                begin: '',
                end: ''
            };
            var year = currentDate.getFullYear();
            res.begin = year + '-07-01 00:00:00';
            res.end = year + '-12-31 23:59:59';
            return res;
        },
        // #endregion
    };

    // Ajax
    luckyu.ajax = {
        post: function (url, postData, successCallback) {
            var loadid = luckyu.layer.loading();
            if (!!$.validateToken) {
                postData["__RequestVerificationToken"] = $.validateToken;
            }
            $.ajax({
                url: url,
                data: postData,
                type: "post",
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    successCallback(res);
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    luckyu.layer.closeLoading(loadid);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        postv2: function (url, postData, successCallback) {
            var loadid = luckyu.layer.loading();
            postData["__RequestVerificationToken"] = $.validateToken;
            $.ajax({
                url: url,
                data: postData,
                type: "post",
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    if (res.code == 200) {
                        successCallback(res.data, res.info);
                    }
                    else {
                        layui.layer.alert(res.info, { icon: 2 });
                        //layui.notice.error(res.info);
                    }
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    layui.layer.closeAll();
                    layui.layer.alert(res.info, { icon: 2 });
                    //layui.notice.error(res.info);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        get: function (url, postData, successCallback) {
            var loadid = luckyu.layer.loading();
            $.ajax({
                url: url,
                data: postData,
                type: "get",
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    successCallback(res);
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    luckyu.layer.closeLoading(loadid);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        getv2: function (url, postData, successCallback) {
            var loadid = luckyu.layer.loading();
            $.ajax({
                url: url,
                data: postData,
                type: "get",
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    if (res.code == 200) {
                        successCallback(res.data, res.info);
                    }
                    else {
                        layui.layer.alert(res.info, { icon: 2 });
                        //layui.notice.error(res.info);
                    }
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    luckyu.layer.closeLoading(loadid);
                    layui.layer.alert(errorMsg, { icon: 2 });
                    //layui.notice.error(res.info);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        postSync: function (url, postData, successCallbacck) {
            var loadid = luckyu.layer.loading();
            if (!!$.validateToken) {
                postData["__RequestVerificationToken"] = $.validateToken;
            }
            $.ajax({
                url: url,
                data: postData,
                type: "post",
                async: false,
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    successCallbacck(res);
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    luckyu.layer.closeLoading(loadid);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });

        },
        postSyncv2: function (url, postData, successCallbacck) {
            var loadid = layui.layer.load();
            if (!!$.validateToken) {
                postData["__RequestVerificationToken"] = $.validateToken;
            }
            $.ajax({
                url: url,
                data: postData,
                type: "post",
                async: false,
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    if (res.code == 200) {
                        successCallback(res.data, res.info);
                    }
                    else {
                        layui.layer.alert(res.info, { icon: 2 });
                        //layui.notice.error(res.info);
                    }
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    layui.layer.close(loadid);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        getSync: function (url, postData, successCallback) {
            var loadid = luckyu.layer.loading();
            $.ajax({
                url: url,
                data: postData,
                type: "get",
                async: false,
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    successCallback(res);
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    luckyu.layer.closeLoading(loadid);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });

        },
        getSyncv2: function (url, postData, successCallback) {
            var loadid = layui.layer.load();
            $.ajax({
                url: url,
                data: postData,
                type: "get",
                async: false,
                dataType: "json",
                success: function (res) {
                    luckyu.layer.closeLoading(loadid);
                    if (res.code == 200) {
                        successCallback(res.data, res.info);
                    }
                    else {
                        layui.layer.alert(res.info, { icon: 2 });
                        //layui.notice.error(res.info);
                    }
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    layui.layer.close(loadid);
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        postNoloading: function (url, postData, successCallback) {
            $.ajax({
                url: url,
                data: postData,
                type: "post",
                dataType: "json",
                success: function (res) {
                    successCallback(res);
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
        getNoloading: function (url, postData, successCallback) {
            $.ajax({
                url: url,
                data: postData,
                type: "get",
                dataType: "json",
                async: false,//同步加载
                success: function (res) {
                    successCallback(res);
                },
                error: function (XMLHttpRequest, errorMsg, exception) {
                    console.log("========= ajax error start ==========");
                    console.log(url);
                    console.log(postData);
                    console.log(errorMsg);
                    console.log(exception);
                    console.log("========= ajax error end ==========");
                },
            });
        },
    };

    // 需要提前执行的方法放在这里，如js扩展方法
    luckyu.init = function () {
        String.prototype.trimLeft = function (char) {
            return this.replace(new RegExp('^\\' + char + '+', 'g'), '');
        };

        String.prototype.trimRight = function (char) {
            return this.replace(new RegExp('\\' + char + '+$', 'g'), '');
        };

        String.prototype.trim = function (char) {
            return this.replace(new RegExp('^\\' + char + '+|\\' + char + '+$', 'g'), '');
        };

        String.prototype.splieNoEmpty = function (s) {
            return this.split(s).filter(t => t != '');
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

    };

    luckyu.init();
    window.luckyu = luckyu;

    if (!!top.luckyu) {
        window.luckyu.clientdata = top.luckyu.clientdata;
    }

})(window.layui);