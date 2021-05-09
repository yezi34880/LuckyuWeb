/**
 * 数字范围输入,用于Jqgrid列头查询 
 * 余洋 2020-3-27
 */
(function ($) {

    $.fn.numberrange = function (op) {
        var defaultOp = {
            done: null
        };
        $.extend(defaultOp, op);
        var $self = $(this);
        $self.addClass("luckyu_numberrange_input");
        $self.attr("readonly", "readonly");

        $self.on("click", function () {
            var id = "luckyu_numberrange_" + $self.attr('id');
            var exsit = $("#" + id);
            if (!!exsit && exsit.length > 0) {
                return;
            }
            var $div = $('<div id="' + id + '" class="luckyu_numberrange"></div>');
            var top = $self.offset().top;
            var left = $self.offset().left;
            var height = $self.height();
            $div.css("top", top + height + 15);
            $div.css("left", left);

            var $inputMin = $('<input type="text" id="luckyu_min" class="numinput" /> ');
            var $inputMax = $('<input type="text" id="luckyu_max" class="numinput" /> ');
            $inputMin.numeric();
            $inputMax.numeric();
            var currentValue = $self.val();
            if (!!currentValue && currentValue.indexOf('~') > -1) {
                var currentValues = currentValue.split(' ~ ');
                $inputMin.val($.trim(currentValues[0]));
                $inputMax.val($.trim(currentValues[1]));
            }
            var $ok = $('<button type="button" class="layui-btn layui-btn-sm">确定</button>');

            $ok.on("click", function () {
                var min = $("#luckyu_min").val();
                var max = $("#luckyu_max").val();
                var text = min + ' - ' + max;
                $self.val(text);
                $div.remove();
                if (!!defaultOp.done) {
                    defaultOp.done(text, min, max);
                }
            });

            $div.append($inputMin).append($inputMax).append($ok);
            $("body").append($div);
        });

        $(document).click(function (e) {
            var et = e.target || e.srcElement;
            var $et = $(et);
            if (!$et.hasClass('luckyu_numberrange') && !$et.hasClass('luckyu_numberrange_input') && $et.parents('.luckyu_numberrange').length < 1) {
                $('.luckyu_numberrange').remove();
            }
        });

    };
})(window.jQuery);