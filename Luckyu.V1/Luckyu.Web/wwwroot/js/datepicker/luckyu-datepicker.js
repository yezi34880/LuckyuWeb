/*
 *  时间范围选择器 
 *  @YY@-当年 
 *  @MM@-当月 
 *  @DD@-当天 
 *  @JJ@-当季度
 */
(function ($, luckyu) {
    "use strict";
    $.luckyudate = {
        init: function ($self) {
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            $self.addClass('luckyu-search-date Wdate layui-input');
            var $container = $('<div class="luckyu-search-date-container" id="search_date_container_' + DefaultOption.id + '"><div class="luckyu-search-date-arrow"><div class="luckyu-search-date-inside"></div></div></div>');

            var $btnlist = $('<div class="luckyu-search-date-content-btns" id="search_date_content_btns_' + DefaultOption.id + '"></div>');
            var $customDate = $('<div class="luckyu-search-date-btn-block"><a href="javascript:;" data-value="customDate">自定义</a></div>');

            var $clearDate = $('<div class="luckyu-search-date-btn-block"><a href="javascript:;" data-value="clearDate">清空</a></div>');
            $btnlist.append($customDate);
            $btnlist.append($clearDate);
            $container.append($btnlist);

            var $datepickerContent = $('<div class="luckyu-search-date-datepicker-content"></div>');

            var $datepicker1 = $('<div class="luckyu-search-date-datepicker-container first" id="search_date_datepicker1_' + DefaultOption.id + '"  ></div>');
            var $datepicker2 = $('<div class="luckyu-search-date-datepicker-container" id="search_date_datepicker2_' + DefaultOption.id + '"  ></div>');
            var $datepickerBtn = $('<div class="luckyu-search-date-datepicker-btn"><a class="layui-btn layui-btn-sm">确定</a></div>');
            $datepickerContent.append($datepicker1);
            $datepickerContent.append($datepicker2);
            $datepickerContent.append($datepickerBtn);

            $container.append($datepickerContent);
            $(document.body).append($container);

            WdatePicker({ eCont: 'search_date_datepicker1_' + DefaultOption.id, onpicked: function (dp) { DefaultOption._begindate = dp.cal.getDateStr() + " 00:00:00"; }, minDate: DefaultOption.minDate, maxDate: DefaultOption.maxDate });// 开始时间
            WdatePicker({ eCont: 'search_date_datepicker2_' + DefaultOption.id, onpicked: function (dp) { DefaultOption._enddate = dp.cal.getDateStr() + " 23:59:59"; }, minDate: DefaultOption.minDate, maxDate: DefaultOption.maxDate });// 结束时间


            /*事件的绑定*/
            $self.on('click', function (e) {
                var $self = $(this);;
                var DefaultOption = $self[0]._luckyudate.DefaultOption;
                var $container = $('#search_date_container_' + DefaultOption.id);
                if ($container.is(':hidden')) {
                    var top = $self.offset().top;
                    var left = $self.offset().left;
                    var height = $self.height();
                    $container.css("top", top + height + 5);
                    $container.css("left", left);
                    if (left + $container.width() > document.body.clientWidth) {
                        $container.css("left", document.body.clientWidth - $container.width() - 50);
                    }

                    $container.show();
                }
                else {
                    $container.hide();
                }
            });
            $(document).click(function (e) {
                var et = e.target || e.srcElement;
                var $et = $(et);
                if (!$et.hasClass('luckyu-search-date') && $et.parents('.luckyu-search-date').length <= 0 && $et.parents('.luckyu-search-date-container').length <= 0) {
                    $('.luckyu-search-date-container').hide();
                }
            });

            $customDate.find('a').on('click', function (e) {
                var $this = $(this);
                var $container = $this.parents('.luckyu-search-date-container');
                $container.find('.luckyu-search-date-content-btns a.active').removeClass('active');
                $container.addClass('width');
                $this.addClass('active');

                if ($container.offset().left + $container.width() > document.body.clientWidth) {
                    $container.css("left", document.body.clientWidth - $container.width() - 50);
                }
                $container.find('.luckyu-search-date-datepicker-content').show();
            });
            $clearDate.find('a').on('click', function (e) {
                var $this = $(this);
                var $container = $this.parents('.luckyu-search-date-container');
                var $self = $("#" + $container[0].id.replace("search_date_container_", ""));
                var DefaultOption = $self[0]._luckyudate.DefaultOption;
                $container.hide();
                $container.find('.luckyu-search-date-content-btns a.active').removeClass('active');
                $container.removeClass('width');
                $container.find('.luckyu-search-date-datepicker-content').hide();
                $self.val("");

                DefaultOption._begindate = "1900-01-01";
                DefaultOption._enddate = "3000-01-01";
                if (!!DefaultOption.select) {
                    DefaultOption.select(DefaultOption._begindate, DefaultOption._enddate);
                }

            });

            // 时间确定按钮
            $datepickerBtn.find('a').on('click', function () {
                var $this = $(this);
                var $container = $this.parents('.luckyu-search-date-container');
                var $self = $("#" + $container[0].id.replace("search_date_container_", ""));
                var DefaultOption = $self[0]._luckyudate.DefaultOption;
                $container.hide();
                var label = luckyu.utility.formatDate(DefaultOption._begindate, 'yyyy-MM-dd') + ' - ' + luckyu.utility.formatDate(DefaultOption._enddate, 'yyyy-MM-dd');
                $self.val(label);

                if (!!DefaultOption.select) {
                    DefaultOption.select(DefaultOption._begindate, DefaultOption._enddate);
                }
            });
        },
        monthinit: function ($self) {// 月：上月，本月
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            var $btnlist = $('#search_date_content_btns_' + DefaultOption.id);
            var $block = $('<div class="luckyu-search-date-btn-block"></div>');
            if (DefaultOption.premShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="preM">上月</a>');
            }
            if (DefaultOption.mShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="currentM">本月</a>');
            }
            $btnlist.prepend($block);
            DefaultOption = null;
        },
        jinit: function ($self) {// 季度
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            var $btnlist = $('#search_date_content_btns_' + DefaultOption.id);
            var $block = $('<div class="luckyu-search-date-btn-block"></div>');
            if (DefaultOption.prejShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="preJ">上季度</a>');
            }
            if (DefaultOption.jShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="currentJ">本季度</a>');
            }
            $btnlist.prepend($block);
            DefaultOption = null;
        },
        yinit: function ($self) {
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            var $btnlist = $('#search_date_content_btns_' + DefaultOption.id);
            var $block = $('<div class="luckyu-search-date-btn-block"></div>');
            if (DefaultOption.ysShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="yS">上半年</a>');
            }
            if (DefaultOption.yxShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="yX">下半年</a>');
            }
            if (DefaultOption.preyShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="preY">去年</a>');
            }
            if (DefaultOption.yShow) {
                $block.append('<a href="javascript:;" class="datebtn" data-value="currentY">今年</a>');
            }
            $btnlist.prepend($block);
            DefaultOption = null;
        },
        custmerinit: function ($self) {
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            var $btnlist = $('#search_date_content_btns_' + DefaultOption.id);
            var $block = $('<div class="luckyu-search-date-btn-block"></div>');

            for (var i = 0, l = DefaultOption.dfdata.length; i < l; i++) {
                var item = DefaultOption.dfdata[i];
                $block.append('<a href="javascript:;" class="datebtn" data-value="' + i + '">' + item.name + '</a>');
            }

            $btnlist.prepend($block);
            DefaultOption = null;
        },
        bindEvent: function ($self) {
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            var $container = $('#search_date_container_' + DefaultOption.id);
            $container.find('.datebtn').on('click', function () {
                var $this = $(this);
                var $container = $this.parents('.luckyu-search-date-container');
                var $self = $("#" + $container[0].id.replace("search_date_container_", ""));
                var value = $this.attr('data-value');
                $.luckyudate.select($self, value);
            });
        },
        select: function ($self, value) {
            var DefaultOption = $self[0]._luckyudate.DefaultOption;
            var $container = $('#search_date_container_' + DefaultOption.id);
            var $btnlist = $('#search_date_content_btns_' + DefaultOption.id);
            $btnlist.find('.active').removeClass('active');
            $btnlist.find('.datebtn[data-value="' + value + '"]').addClass('active');
            switch (value) {
                case 'preM':
                    var d = luckyu.utility.getPreMonth();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'currentM':
                    var d = luckyu.utility.getMonth();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'preJ':
                    var d = luckyu.utility.getPreQuarter();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'currentJ':
                    var d = luckyu.utility.getCurrentQuarter();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'yS':
                    var d = luckyu.utility.getFirstHalfYear();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'yX':
                    var d = luckyu.utility.getSecondHalfYear();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'preY':
                    var d = luckyu.utility.getPreYear();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                case 'currentY':
                    var d = luckyu.utility.getYear();
                    DefaultOption._begindate = d.begin;
                    DefaultOption._enddate = d.end;
                    break;
                default:
                    var rowid = parseInt(value);
                    var data = DefaultOption.dfdata[rowid];
                    if (!!data) {
                        DefaultOption._begindate = data.begin();
                        DefaultOption._enddate = data.end();
                    }
                    else {
                        DefaultOption._begindate = '';
                        DefaultOption._enddate = '';
                    }
                    break;
            }
            $container.hide();
            var label = luckyu.utility.formatDate(DefaultOption._begindate, 'yyyy-MM-dd') + ' - ' + luckyu.utility.formatDate(DefaultOption._enddate, 'yyyy-MM-dd');
            $self.val(label);
            $container.removeClass('width');
            $container.find('.luckyu-search-date-datepicker-content').hide();
            if (!!DefaultOption.select) {
                DefaultOption.select(DefaultOption._begindate, DefaultOption._enddate);
            }
        }
    };

    $.fn.luckyudate = function (op) {
        var DefaultOption = {
            // 自定义数据
            dfdata: [],
            // 月
            mShow: true,
            premShow: true,
            // 季度
            jShow: true,
            prejShow: true,
            // 年
            ysShow: true,
            yxShow: true,
            preyShow: true,
            yShow: true,

            dfvalue: false,//preM,currentM,preJ,currentJ,yS,yX,preY,currentY,
            select: false,

            minDate: '',
            maxDate: '',
        };
        $.extend(DefaultOption, op || {});
        var $self = $(this);
        DefaultOption.id = $self.attr('id');
        if (!DefaultOption.id) {
            return false;
        }
        $self[0]._luckyudate = { "DefaultOption": DefaultOption };
        $.luckyudate.init($self);
        $.luckyudate.yinit($self);
        $.luckyudate.jinit($self);
        $.luckyudate.monthinit($self);
        $.luckyudate.custmerinit($self);
        $.luckyudate.bindEvent($self);
        if (DefaultOption.dfvalue != false) {
            $.luckyudate.select($self, DefaultOption.dfvalue);
        }
        return $self;
    };
    $.fn.luckyurangedate = function (op) {
        var DefaultOption = {
            dfdata: [
                { name: '今天', begin: function () { return luckyu.utility.getDate('yyyy-MM-dd 00:00:00') }, end: function () { return luckyu.utility.getDate('yyyy-MM-dd 23:59:59') } },
                { name: '本月', begin: function () { return new Date().format("yyyy-MM") + "-01 00:00:00"  }, end: function () { return luckyu.utility.getDate('yyyy-MM-dd 23:59:59') } },
                { name: '本季度', begin: function () { return luckyu.utility.getCurrentQuarter().begin }, end: function () { return luckyu.utility.getDate('yyyy-MM-dd 23:59:59') } },
                { name: '本年度', begin: function () { return new Date().format("yyyy") + "-01-01 00:00:00" }, end: function () { return ahoit.utility.getDate('yyyy-MM-dd 23:59:59') } },
            ],
            // 月
            mShow: false,
            premShow: false,
            // 季度
            jShow: false,
            prejShow: false,
            // 年
            ysShow: false,
            yxShow: false,
            preyShow: false,
            yShow: false,
            // 默认
            dfvalue: '3',
        };
        $.extend(DefaultOption, op || {});
        var $self = $(this);
        DefaultOption.id = $self.attr('id');
        if (!DefaultOption.id) {
            return false;
        }
        $self[0]._luckyudate = { "DefaultOption": DefaultOption };
        $.luckyudate.init($self);
        $.luckyudate.yinit($self);
        $.luckyudate.jinit($self);
        $.luckyudate.monthinit($self);
        $.luckyudate.custmerinit($self);
        $.luckyudate.bindEvent($self);
        if (DefaultOption.dfvalue != false) {
            $.luckyudate.select($self, DefaultOption.dfvalue);
        }
        return $self;
    };

    $.fn.ahoitdatevalue = function () {
        var $self = $(this);
        var DefaultOption = $self[0]._ahoitdate.DefaultOption;
        if (!DefaultOption) {
            return {};
        }
        var result = {
            begindate: DefaultOption._begindate,
            enddate: DefaultOption._enddate
        };
        return result;
    };
})(jQuery, top.luckyu);
