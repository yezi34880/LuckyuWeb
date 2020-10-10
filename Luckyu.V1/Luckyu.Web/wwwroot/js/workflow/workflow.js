/*
 * 描 述：工作流绘制
 */
(function ($, luckyu) {
    "use strict";

    $.dfworkflow = {
        render: function ($self) {
            var DefaultOption = $self[0].DefaultOption;
            $self.addClass('workflow');
            // 添加工具栏
            if (!DefaultOption.isPreview) {
                var $tool = $('<div class="workflow-tool" ></div>');
                $tool[0].DefaultOption = DefaultOption;
                $tool.append('<a  type="cursor" class="workflow-btndown" id="' + DefaultOption.id + '_btn_cursor" title="' + DefaultOption.nodeRemarks.cursor + '" ><b class="ico_cursor" /></a>');
                $tool.append('<a  type="direct" class="workflow-btn" id="' + DefaultOption.id + '_btn_direct" title="' + DefaultOption.nodeRemarks.direct + '" ><b class="ico_direct"/></a>');
                var toolbtnlen = DefaultOption.toolBtns.length;
                if (toolbtnlen > 0) {
                    $tool.append('<span></span>');
                    for (var i = 0; i < toolbtnlen; ++i) {
                        var btn = DefaultOption.toolBtns[i];
                        $tool.append('<a  type="' + btn + '" id="' + DefaultOption.id + '_btn_' + btn + '" class="workflow-btn" title="' + DefaultOption.nodeRemarks[btn] + '" ><b class="ico_' + btn + '"/></a>');//加入自定义按钮
                    }
                }
                DefaultOption.currentBtn = "cursor";
                $tool.on("click", function (e) {
                    var $this = $(this);
                    var DefaultOption = $this[0].DefaultOption;
                    e = e || window.event;
                    var tar;
                    switch (e.target.tagName) {
                        case "SPAN": return false;
                        case "DIV": return false;
                        case "B": tar = e.target.parentNode; break;
                        case "A": tar = e.target;
                    };
                    var type = $(tar).attr("type");
                    $.dfworkflow.switchToolBtn(DefaultOption, type);
                    return false;
                });
                $self.append($tool);
            }
            else {
                $self.addClass('workflow-preview');
            }


            // 工作流画板(工作区域)
            $self.append('<div class="workflow-work"></div>');
            var $workArea = $("<div class='workflow-workinner' style='width:5000px;height:5000px'></div>")
                .attr({ "unselectable": "on", "onselectstart": 'return false', "onselect": 'document.selection.empty()' });

            $self.children(".workflow-work").dfscroll();
            $self.children(".workflow-work").find('.scroll-box').append($workArea);
            $self.children(".workflow-work").find('.scroll-box').css({ 'width': 5000, 'height': 5000 });

            $workArea[0].DefaultOption = DefaultOption;
            $.dfworkflow.initDraw($workArea, DefaultOption);
            $workArea.on("click", $.dfworkflow.clickWorkArea);

            $.dfworkflow.initNodeEvent($workArea);
            $.dfworkflow.initLineEvent($workArea);


            //对结点进行移动或者RESIZE时用来显示的遮罩层
            var $ghost = $("<div class='workflow-rsghost'></div>").attr({ "unselectable": "on", "onselectstart": 'return false', "onselect": 'document.selection.empty()' });
            $self.append($ghost);

            var $lineMove = $('<div class="workflow-linemover" style="display:none" ></div>');//操作折线时的移动框
            $workArea.append($lineMove);

            $lineMove.on("mousedown", { $workArea: $workArea }, function (e) {
                if (e.button == 2) return false;
                var lm = $(this);
                lm.css({ "background-color": "#333" });
                var $workArea = e.data.$workArea;

                var ev = $.dfworkflow.mousePosition(e), t = $.dfworkflow.getElCoordinate($workArea[0]);
                var X, Y;
                X = ev.x - t.left;
                Y = ev.y - t.top;
                var p = lm.position();
                var vX = X - p.left, vY = Y - p.top;
                var isMove = false;
                document.onmousemove = function (e) {
                    if (!e) e = window.event;
                    var ev = $.dfworkflow.mousePosition(e);
                    var ps = lm.position();
                    X = ev.x - t.left;
                    Y = ev.y - t.top;
                    if (lm.data("type") == "lr") {
                        X = X - vX;
                        if (X < 0) X = 0;
                        else if (X > 5000)
                            X = 5000;
                        lm.css({ left: X + "px" });
                    }
                    else if (lm.data("type") == "tb") {
                        Y = Y - vY;
                        if (Y < 0) Y = 0;
                        else if (Y > 5000)
                            Y = 5000;
                        lm.css({ top: Y + "px" });
                    }
                    isMove = true;
                }
                document.onmouseup = function (e) {
                    var lineId = lm.data("tid");
                    var DefaultOption = $('#' + lineId)[0].DefaultOption;
                    if (isMove) {
                        var p = lm.position();
                        if (lm.data("type") == "lr")
                            $.dfworkflow.setLineM(lineId, p.left + 3);
                        else if (lm.data("type") == "tb")
                            $.dfworkflow.setLineM(lineId, p.top + 3);
                    }
                    lm.css({ "background-color": "transparent" });
                    if (DefaultOption.focusId == lm.data("tid")) {
                        $.dfworkflow.focusItem(lm.data("tid"));
                    }
                    document.onmousemove = null;
                    document.onmouseup = null;
                }
            });

            // 设置线条工具条选定线时显示的操作框
            var $lineOper = $("<div class='workflow-lineoper' style='display:none'><b class='lr'></b><b class='tb'></b><b class='sl'></b><b class='x'></b></div>");
            $workArea.append($lineOper);
            $lineOper.on("click", function (e) {
                if (!e) e = window.event;
                if (e.target.tagName != "A" && e.target.tagName != "B") return;
                var id = $(this).data("tid");
                var type = $(e.target).attr("class");
                if (type == 'x') {
                    $.dfworkflow.delLine(id);
                    this.style.display = "none";
                }
                else {
                    $.dfworkflow.setLineType(id, type);
                }
            });
        },
        switchToolBtn: function (DefaultOption, type) {
            var $oldBtn = $('#' + DefaultOption.id + "_btn_" + DefaultOption.currentBtn);
            var $newBtn = $('#' + DefaultOption.id + "_btn_" + type);
            $oldBtn.removeClass('workflow-btndown');
            $oldBtn.addClass('workflow-btn');

            $newBtn.removeClass('workflow-btn');
            $newBtn.addClass('workflow-btndown');

            DefaultOption.currentBtn = type;
        },
        initDraw: function ($workArea, DefaultOption) {
            var $draw;
            var elem;
            $draw = document.createElementNS("http://www.w3.org/2000/svg", "svg");//可创建带有指定命名空间的元素节点
            $workArea.prepend($draw);
            var defs = document.createElementNS("http://www.w3.org/2000/svg", "defs");
            $draw.appendChild(defs);
            defs.appendChild($.dfworkflow.getSvgMarker("arrow1", "gray"));
            defs.appendChild($.dfworkflow.getSvgMarker("arrow2", "#ff3300"));
            defs.appendChild($.dfworkflow.getSvgMarker("arrow3", "#225ee1"));

            $draw.id = 'draw_' + DefaultOption.id;
            $draw.style.width = "5000px";
            $draw.style.height = "5000px";
        },
        getSvgMarker: function (id, color) {
            var m = document.createElementNS("http://www.w3.org/2000/svg", "marker");
            m.setAttribute("id", id);
            m.setAttribute("viewBox", "0 0 6 6");
            m.setAttribute("refX", 5);
            m.setAttribute("refY", 3);
            m.setAttribute("markerUnits", "strokeWidth");
            m.setAttribute("markerWidth", 6);
            m.setAttribute("markerHeight", 6);
            m.setAttribute("orient", "auto");
            var path = document.createElementNS("http://www.w3.org/2000/svg", "path");
            path.setAttribute("d", "M 0 0 L 6 3 L 0 6 z");
            path.setAttribute("fill", color);
            path.setAttribute("stroke-width", 0);
            m.appendChild(path);
            return m;
        },
        clickWorkArea: function (e) {
            var $this = $(this);
            var DefaultOption = $this[0].DefaultOption;
            if (!DefaultOption.isPreview) {


                e = e || window.event;

                var type = DefaultOption.currentBtn;
                if (type == "cursor") {
                    var t = $(e.target);
                    var n = t.prop("tagName");
                    if (n == "svg" || (n == "DIV" && t.prop("class").indexOf("workflow-workinner") > -1)) {
                        $.dfworkflow.blurItem(DefaultOption);
                    }
                    return;
                }
                else if (type == "direct") {
                    return;
                }
                var X, Y;
                var ev = $.dfworkflow.mousePosition(e), t = $.dfworkflow.getElCoordinate(this);

                X = ev.x - t.left;
                Y = ev.y - t.top;

                var name = DefaultOption.nodeRemarks[type];
                var executeadd = true;
                if (type == 'startround') {
                    name = "开始";
                    if (DefaultOption.hasStartround) {
                        layui.notice.error('只能有一个开始节点');
                        return false;
                    }
                }
                if (type == 'endround') {
                    name = "结束";
                    if (DefaultOption.hasEndround) {
                        layui.notice.error('只能有一个结束节点');
                        return false;
                    }
                }
                $.dfworkflow.addNode($this, DefaultOption, { id: luckyu.utility.newGuid(), name: name, left: X, top: Y, type: type });
            }
        },
        // 取消所有结点/连线被选定的状态
        blurItem: function (DefaultOption) {
            if (DefaultOption.focusId != "") {
                var $item = $("#" + DefaultOption.focusId);
                if ($item.prop("tagName") == "DIV") {
                    $item.removeClass("workflow-nodefocus");
                    $item.find('.workflow-nodeclose').hide();
                    $item.removeClass("workflow-nodemark");
                }
                else {
                    var lineData = $.dfworkflow.getLine(DefaultOption, DefaultOption.focusId);


                    if (!lineData.marked) {
                        if (lineData.linecondition == '2') {
                            $item[0].childNodes[1].setAttribute("stroke", "#ff3300");
                            $item[0].childNodes[1].setAttribute("marker-end", "url(#arrow2)");
                        }
                        else {
                            $item[0].childNodes[1].setAttribute("stroke", "gray");
                            $item[0].childNodes[1].setAttribute("marker-end", "url(#arrow1)");
                        }
                    }
                    var $lineOper = $('.workflow-lineoper');
                    var $lineMove = $('.workflow-linemover');
                    $lineMove.hide().removeData("type").removeData("tid");
                    $lineOper.hide().removeData("tid");
                }
            }
            DefaultOption.focusId = "";
            return true;
        },
        // 选定某个结点/转换线
        focusItem: function (id) {
            var $item = $("#" + id);
            if ($item.length == 0) {
                return;
            }
            $item.removeClass("workflow-nodemark");
            var DefaultOption = $item[0].DefaultOption;
            if (!this.blurItem(DefaultOption)) {//先执行"取消选中",如果返回FLASE,则也会阻止选定事件继续进行.
                return;
            }
            if ($item.prop("tagName") == "DIV") {

                $item.addClass("workflow-nodefocus");
                $item.find('.workflow-nodeclose').show();
            }
            else {//如果是连接线
                $item[0].childNodes[1].setAttribute("stroke", "#225ee1");
                $item[0].childNodes[1].setAttribute("marker-end", "url(#arrow3)");
                var x, y, from, to;
                from = $item.attr("from").split(",");
                to = $item.attr("to").split(",");
                from[0] = parseInt(from[0], 10);
                from[1] = parseInt(from[1], 10);
                to[0] = parseInt(to[0], 10);
                to[1] = parseInt(to[1], 10);

                var lineData = $.dfworkflow.getLine(DefaultOption, id);

                if (lineData.type == "lr") {
                    from[0] = lineData.M;
                    to[0] = from[0];
                    var $lineMove = $('.workflow-linemover');
                    $lineMove.css({
                        width: "5px", height: (to[1] - from[1]) * (to[1] > from[1] ? 1 : -1) + "px",
                        left: from[0] - 3 + "px",
                        top: (to[1] > from[1] ? from[1] : to[1]) + 1 + "px",
                        cursor: "e-resize", display: "block"
                    }).data({ "type": "lr", "tid": id });
                }
                else if (lineData.type == "tb") {
                    from[1] = lineData.M;
                    to[1] = from[1];
                    var $lineMove = $('.workflow-linemover');
                    $lineMove.css({
                        width: (to[0] - from[0]) * (to[0] > from[0] ? 1 : -1) + "px", height: "5px",
                        left: (to[0] > from[0] ? from[0] : to[0]) + 1 + "px",
                        top: from[1] - 3 + "px",
                        cursor: "s-resize", display: "block"
                    }).data({ "type": "tb", "tid": id });
                }
                x = (from[0] + to[0]) / 2 - 35;
                y = (from[1] + to[1]) / 2 + 6;

                var $lineOper = $('.workflow-lineoper');
                $lineOper.css({ display: "block", left: x + "px", top: y + "px" }).data("tid", id);
            }
            DefaultOption.focusId = id;
            $.dfworkflow.switchToolBtn(DefaultOption, "cursor");
        },
        //获取一个DIV的绝对坐标的功能函数,即使是非绝对定位,一样能获取到
        getElCoordinate: function (dom) {
            var t = dom.offsetTop;
            var l = dom.offsetLeft;
            dom = dom.offsetParent;
            while (dom) {
                t += dom.offsetTop;
                l += dom.offsetLeft;
                dom = dom.offsetParent;
            }; return {
                top: t,
                left: l
            };
        },
        // 获取鼠标定位点坐标
        mousePosition: function (ev) {
            if (!ev) ev = window.event;
            if (ev.pageX || ev.pageY) {
                return { x: ev.pageX, y: ev.pageY };
            }
            return {
                x: ev.clientX + document.documentElement.scrollLeft - document.body.clientLeft,
                y: ev.clientY + document.documentElement.scrollTop - document.body.clientTop
            };
        },

        // 节点操作
        //增加一个流程结点,传参为一个JSON,有id,name,top,left,width,height,type(结点类型)等属性
        addNode: function ($workArea, DefaultOption, node, isold) {
            var mark = node.type;
            var $node;

            if (!node.width || node.width < 150) node.width = 150;
            if (!node.height || node.height < 65) node.height = 65;
            if (!node.top || node.top < 0) node.top = 0;
            if (!node.left || node.left < 0) node.left = 0;
            if (mark == "conditionnode") {
                node.width = 160;
                node.height = 90;
                $node = $('<div class="workflow-node item-conditionnode" id="' + node.id + '" ><div class="workflow-nodeico"></div><b class="ico_' + node.type + 'div"></b><div class="workflow-nodetext">' + node.name + '</div><div class="workflow-nodeassemble" ></div></div>');

            }
            else if (mark != "startround" && mark != "endround") {
                $node = $('<div class="workflow-node" id="' + node.id + '" ><div class="workflow-nodeico"><b class="ico_' + node.type + '"></b></div><div class="workflow-nodetext">' + node.name + '</div><div class="workflow-nodeassemble" ></div></div>');
            }
            else {
                node.width = 52;
                node.height = 52;
                if (mark == 'startround') {
                    node.name = "开始";
                    DefaultOption.hasStartround = true;
                }
                else if (mark == 'endround') {
                    node.name = "结束";
                    DefaultOption.hasEndround = true;
                }
                $node = $('<div class="workflow-node item-' + mark + '" id="' + node.id + '" ><div class="workflow-nodeico"></div><div class="workflow-nodetext">' + node.name + '</div><div class="workflow-nodeassemble" ></div></div>');
            }
            $node.find('.workflow-nodeassemble').append('<div class="workflow-nodeclose"></div>');
            $node.find('.workflow-nodeassemble').append('<div class="workflow-nodespot left"><div class="workflow-nodespotc"></div></div>');
            $node.find('.workflow-nodeassemble').append('<div class="workflow-nodespot top"><div class="workflow-nodespotc"></div></div>');
            $node.find('.workflow-nodeassemble').append('<div class="workflow-nodespot right"><div class="workflow-nodespotc"></div></div>');
            $node.find('.workflow-nodeassemble').append('<div class="workflow-nodespot bottom"><div class="workflow-nodespotc"></div></div>');

            $node.css({ 'top': node.top + 'px', 'left': node.left + 'px', 'width': node.width + 'px', 'height': node.height + 'px' });

            if (node.state != undefined && (node.type == 'startround' || node.type == 'auditornode' || node.type == 'stepnode' || node.type == 'confluencenode')) {
                $node.css({ 'padding-left': '0', 'color': '#fff' }).find('.workflow-nodeico').remove();
                switch (node.state) {//0正在处理 1 已处理同意 2 已处理不同意 3 未处理 4 当前需要处理的节点
                    case '0':
                        $node.css({ 'background': '#5bc0de', 'border': '0' });
                        break;
                    case '1':
                        $node.css({ 'background': '#5cb85c', 'border': '0' });
                        break;
                    case '2':
                        $node.css({ 'background': '#d9534f', 'border': '0' });
                        break;
                    case '3':
                        $node.css({ 'background': '#999', 'border': '0' });
                        break;
                    case '4':
                        $node.css({ 'background': '#f0ad4e', 'border': '0' });
                        break;
                }
            }

            // 初始化节点的配置信息
            if (!isold) {
                switch (node.type) {
                    case 'startround':
                        node.wfForms = [];
                        node.authorizeFields = [];
                        node.iocName = '';
                        node.dbSuccessId = '';
                        node.dbSuccessSql = '';
                        break;
                    case 'stepnode':
                        node.dbFailId = '';
                        node.dbFailSql = '';
                    case 'auditornode':
                        node.auditors = [];
                        node.wfForms = [];
                        node.authorizeFields = [];
                        node.iocName = '';
                        node.dbSuccessId = '';
                        node.dbSuccessSql = '';

                        node.timeoutAction = 48;// 超时流转时间
                        node.timeoutNotice = 24;// 超时通知时间
                        break;
                    case 'processnode':
                        node.dbFailId = '';
                        node.dbFailSql = '';
                        node.iocName = '';
                        break;
                    case 'confluencenode':// 会签
                        node.confluenceType = '1';
                        node.confluenceRate = '100';
                        node.iocName = '';
                        node.dbSuccessId = '';
                        node.dbSuccessSql = '';
                        node.dbFailId = '';
                        node.dbFailSql = '';
                        break;
                    case 'conditionnode':// 条件
                        node.conditions = [];
                        node.dbConditionId = "";
                        node.conditionSql = "";
                        break;
                }
            }



            $node[0].wfdata = node;
            $node[0].DefaultOption = DefaultOption;

            $workArea.append($node);
            DefaultOption.node.push(node);
        },
        //删除结点
        delNode: function (DefaultOption, nodeData) {
            var tmplines = [];
            for (var i = 0, l = DefaultOption.line.length; i < l; i++) {
                var tmpLine = DefaultOption.line[i];
                if (tmpLine.from != nodeData.id && tmpLine.to != nodeData.id) {
                    tmplines.push(tmpLine);
                }
                else {
                    $('#' + tmpLine.id).remove();
                }
            }
            $('#' + nodeData.id).remove();
            DefaultOption.line = tmplines;
            DefaultOption.node.splice(DefaultOption.node.indexOf(nodeData), 1);
            if (nodeData.type == 'startround') {
                DefaultOption.hasStartround = false;
            }
            else if (nodeData.type == 'endround') {
                DefaultOption.hasEndround = false;
            }

            DefaultOption.focusId = "";
        },
        //移动结点到一个新的位置
        moveNode: function (id, left, top) {
            if (left < 0) left = 0;
            if (top < 0) top = 0;
            var $node = $("#" + id);
            $node.css({ left: left + "px", top: top + "px" });
            var nodedata = $node[0].wfdata;
            var DefaultOption = $node[0].DefaultOption;
            nodedata.left = left;
            nodedata.top = top;
            //重画转换线
            this.resetLines(id, DefaultOption);
        },

        // 更新节点名字
        updateNodeName: function ($workArea, nodeId) {
            var $node = $workArea.find('#' + nodeId);
            var nodeData = $node[0].wfdata;
            $node.find('.workflow-nodetext').html(nodeData.name);
        },
        initNodeEvent: function ($workArea) {
            var DefaultOption = $workArea[0].DefaultOption;

            //节点双击事件
            $workArea.delegate(".workflow-node", "dblclick", { $workArea: $workArea }, function (e) {
                var $workArea = e.data.$workArea;
                var DefaultOption = $workArea[0].DefaultOption;
                var $node = $(this);
                var nodeData = $node[0].wfdata;
                DefaultOption.openNode(nodeData);
            });
            if (!DefaultOption.isPreview) {
                //绑定点击事件
                $workArea.delegate(".workflow-node", "click", function (e) {
                    $.dfworkflow.focusItem(this.id);
                    return false;
                });
                //绑定右击事件
                $workArea.delegate(".workflow-node", "contextmenu", function (e) {
                    //$.dfworkflow.focusItem(this.id);
                    //return false;
                });
                //绑定用鼠标移动事件(节点拖动)
                $workArea.delegate(".workflow-nodeico", "mousedown", { $workArea: $workArea }, function (e) {
                    var $node = $(this).parents(".workflow-node");
                    var DefaultOption = $node[0].DefaultOption;
                    var nodeData = $node[0].wfdata;

                    e = e || window.event;
                    if (DefaultOption.$nowType == "direct") {
                        return;
                    }
                    var id = $node.attr("id");
                    var $workArea = e.data.$workArea;
                    $.dfworkflow.focusItem(id);
                    var ev = $.dfworkflow.mousePosition(e), t = $.dfworkflow.getElCoordinate($workArea[0]);
                    var $ghost = $('#' + DefaultOption.id).find('.workflow-rsghost');
                    if (nodeData.type == "endround" || nodeData.type == "startround" || nodeData.type == "conditionnode") {
                        $ghost.css({ 'padding-left': '0px' });

                    }
                    else {
                        $ghost.css({ 'padding-left': '48px' });
                    }

                    $node.children().clone().prependTo($ghost);
                    if (nodeData.type == "conditionnode") {
                        $ghost.find('b').css({ 'width': '100%', 'height': '100%', 'position': 'absolute', 'z-index': '-1' });

                    }

                    $ghost.find('.workflow-nodeclose').remove();
                    var X, Y;

                    X = ev.x - t.left;
                    Y = ev.y - t.top;
                    var vX = X - nodeData.left + 32, vY = Y - nodeData.top + 32;
                    var isMove = false;
                    var hack = 1;
                    if (navigator.userAgent.indexOf("8.0") != -1) hack = 0;
                    document.onmousemove = function (e) {
                        if (!e) e = window.event;
                        var ev = $.dfworkflow.mousePosition(e);
                        if (X == ev.x - vX && Y == ev.y - vY) return false;
                        X = ev.x - vX; Y = ev.y - vY - 47;
                        if (isMove && $ghost.css("display") == "none") {
                            $ghost.css({
                                display: "table",
                                width: $('#' + id).css('width'), height: $('#' + id).css('height'),
                                top: nodeData.top + "px",
                                left: nodeData.left + t.left + "px", cursor: "move"
                            });
                        }
                        if (X < 60) {
                            X = 60;
                        }
                        else if (X + nodeData.width > t.left + $workArea.width()) {
                            X = t.left + $workArea.width() - nodeData.width;
                        }

                        if (Y < 0) {
                            Y = 0;
                        }
                        else if (Y + nodeData.height > t.top + $workArea.height() - 47) {
                            Y = $workArea.height() - nodeData.height + t.top - 47;
                        }

                        $ghost.css({ left: X + "px", top: Y + "px" });
                        isMove = true;
                    }
                    document.onmouseup = function (e) {
                        if (isMove) $.dfworkflow.moveNode(id, X - t.left + 32, Y + 47 - t.top + 32);
                        $ghost.empty().hide();
                        document.onmousemove = null;
                        document.onmouseup = null;
                    }
                    return false;
                });
                //绑定鼠标覆盖/移出事件
                $workArea.delegate(".workflow-node", "mouseenter", { $workArea: $workArea }, function (e) {
                    var DefaultOption = e.data.$workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    $(this).addClass("workflow-nodemark");
                });
                $workArea.delegate(".workflow-node", "mouseleave", { $workArea: $workArea }, function (e) {
                    var DefaultOption = e.data.$workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    $(this).removeClass("workflow-nodemark");
                });

                $workArea.delegate(".workflow-nodespot", "mouseenter", { $workArea: $workArea }, function (e) {
                    var DefaultOption = e.data.$workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    $(this).addClass("workflow-nodespotmark");
                });
                $workArea.delegate(".workflow-nodespot", "mouseleave", { $workArea: $workArea }, function (e) {
                    var DefaultOption = e.data.$workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    $(this).removeClass("workflow-nodespotmark");
                });

                //绑定连线时确定初始点
                $workArea.delegate(".workflow-nodespot", "mousedown", { $workArea: $workArea }, function (e) {
                    var DefaultOption = e.data.$workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    var $this = $(this);
                    var $node = $this.parents('.workflow-node');
                    var nodeData = $node[0].wfdata;
                    var X, Y;
                    X = nodeData.left;
                    Y = nodeData.top;
                    var position = 'left';
                    if ($this.hasClass('left')) {
                        position = 'left';
                        Y += nodeData.height / 2;
                    }
                    else if ($this.hasClass('top')) {
                        position = 'top';
                        X += nodeData.width / 2;
                    }
                    else if ($this.hasClass('right')) {
                        position = 'right';
                        X += nodeData.width;
                        Y += nodeData.height / 2;
                    }
                    else if ($this.hasClass('bottom')) {
                        position = 'bottom';
                        X += nodeData.width / 2;
                        Y += nodeData.height;
                    }
                    e.data.$workArea.data("lineStart", { "x": X, "y": Y, "id": nodeData.id, "position": position }).css("cursor", "crosshair");
                    var line = $.dfworkflow.drawLine('1', "workflow_tmp_line", [X, Y], [X, Y], true, true);
                    var $draw = $('#' + DefaultOption.id).find('svg');
                    $draw.append(line);
                });
                //划线时用的绑定
                $workArea.mousemove(function (e) {
                    var $workArea = $(this);
                    var DefaultOption = $workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    var lineStart = $workArea.data("lineStart");
                    if (!lineStart) return;
                    var ev = $.dfworkflow.mousePosition(e), t = $.dfworkflow.getElCoordinate(this);
                    var X, Y;
                    X = ev.x - t.left;
                    Y = ev.y - t.top;
                    var line = document.getElementById("workflow_tmp_line");
                    line.childNodes[0].setAttribute("d", "M " + lineStart.x + " " + lineStart.y + " L " + X + " " + Y);
                    line.childNodes[1].setAttribute("d", "M " + lineStart.x + " " + lineStart.y + " L " + X + " " + Y);
                    if (line.childNodes[1].getAttribute("marker-end") == "url(\"#arrow2\")")
                        line.childNodes[1].setAttribute("marker-end", "url(#arrow3)");
                    else line.childNodes[1].setAttribute("marker-end", "url(#arrow3)");
                });
                $workArea.mouseup(function (e) {
                    var $workArea = $(this);
                    var DefaultOption = $workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    $(this).css("cursor", "auto").removeData("lineStart");
                    $("#workflow_tmp_line").remove();
                });
                //绑定连线时确定结束点
                $workArea.delegate(".workflow-nodespot", "mouseup", { $workArea: $workArea }, function (e) {
                    var $workArea = e.data.$workArea;
                    var DefaultOption = $workArea[0].DefaultOption;
                    if (DefaultOption.currentBtn != "direct") return;
                    var $this = $(this);
                    var $node = $this.parents('.workflow-node');
                    var nodeData = $node[0].wfdata;
                    var lineStart = $workArea.data("lineStart");
                    var position = 'left';
                    if ($this.hasClass('left')) {
                        position = 'left';
                    }
                    else if ($this.hasClass('top')) {
                        position = 'top';
                    }
                    else if ($this.hasClass('right')) {
                        position = 'right';
                    }
                    else if ($this.hasClass('bottom')) {
                        position = 'bottom';
                    }
                    if (lineStart) $.dfworkflow.addLine(DefaultOption, { id: luckyu.utility.newGuid(), from: lineStart.id, to: nodeData.id, sp: lineStart.position, ep: position, name: "" });
                });
                //绑定结点的删除功能
                $workArea.delegate(".workflow-nodeclose", "click", function () {
                    var $node = $(this).parents('.workflow-node');
                    var nodeData = $node[0].wfdata;
                    var DefaultOption = $node[0].DefaultOption;
                    $.dfworkflow.delNode(DefaultOption, nodeData);
                    return false;
                });
            }
        },
        initLineEvent: function ($workArea) {
            var DefaultOption = $workArea[0].DefaultOption;
            if (!DefaultOption.isPreview) {
                $workArea.delegate('g', "click", function (e) {
                    $.dfworkflow.focusItem(this.id);
                });
                $workArea.delegate('g', "dblclick", { $workArea: $workArea }, function (e) {
                    var $workArea = e.data.$workArea;
                    var DefaultOption = $workArea[0].DefaultOption;

                    var lineData = $.dfworkflow.getLine(DefaultOption, this.id);
                    DefaultOption.openLine(lineData);
                });
            }
        },
        // 获取线条数据
        getLine: function (DefaultOption, lineId) {
            for (var i = 0, l = DefaultOption.line.length; i < l; i++) {
                if (lineId == DefaultOption.line[i].id) {
                    return DefaultOption.line[i];
                }
            }
        },
        // 获取线条端点坐标
        getLineSpotXY: function (nodeId, DefaultOption, type) {
            var nodeData;
            for (var i = 0, l = DefaultOption.node.length; i < l; i++) {
                if (nodeId == DefaultOption.node[i].id) {
                    nodeData = DefaultOption.node[i];
                    break;
                }
            }
            var X, Y;
            X = nodeData.left;
            Y = nodeData.top;
            switch (type) {
                case 'left':
                    Y += nodeData.height / 2;
                    break;
                case 'top':
                    X += nodeData.width / 2;
                    break;
                case 'right':
                    X += nodeData.width;
                    Y += nodeData.height / 2;
                    break;
                case 'bottom':
                    X += nodeData.width / 2;
                    Y += nodeData.height;
                    break;
            }
            return [X, Y];
        },
        // 绘制一条箭头线，并返回线的DOM
        drawLine: function (linecondition, id, sp, ep, mark, dash, cursor) {
            var line;
            line = document.createElementNS("http://www.w3.org/2000/svg", "g");
            var hi = document.createElementNS("http://www.w3.org/2000/svg", "path");
            var path = document.createElementNS("http://www.w3.org/2000/svg", "path");

            if (id != "") line.setAttribute("id", id);
            line.setAttribute("from", sp[0] + "," + sp[1]);
            line.setAttribute("to", ep[0] + "," + ep[1]);
            hi.setAttribute("visibility", "hidden");
            hi.setAttribute("stroke-width", 9);
            hi.setAttribute("fill", "none");
            hi.setAttribute("stroke", "white");
            hi.setAttribute("d", "M " + sp[0] + " " + sp[1] + " L " + ep[0] + " " + ep[1]);
            hi.setAttribute("pointer-events", "stroke");
            path.setAttribute("d", "M " + sp[0] + " " + sp[1] + " L " + ep[0] + " " + ep[1]);
            path.setAttribute("stroke-width", 3.0);
            path.setAttribute("stroke-linecap", "round");
            path.setAttribute("fill", "none");
            if (dash) path.setAttribute("style", "stroke-dasharray:6,5");

            if (mark) {
                path.setAttribute("stroke", "#3498DB");//ff3300
                path.setAttribute("marker-end", "url(#arrow3)");
            }
            else if (linecondition == '2') {
                path.setAttribute("stroke", "#ff0000");//ff3300
                path.setAttribute("marker-end", "url(#arrow2)");
            }
            else {
                path.setAttribute("stroke", "gray");
                path.setAttribute("marker-end", "url(#arrow1)");
            }
            ///console.log(linecondition);

            line.appendChild(hi);
            line.appendChild(path);
            line.style.cursor = "crosshair";
            if (id != "" && id != "workflow_tmp_line") {
                var text = document.createElementNS("http://www.w3.org/2000/svg", "text");
                //text.textContent=id;
                line.appendChild(text);
                var x = (ep[0] + sp[0]) / 2;
                var y = (ep[1] + sp[1]) / 2;
                text.setAttribute("text-anchor", "middle");
                text.setAttribute("x", x);
                text.setAttribute("y", y - 5);
                line.style.cursor = "pointer";
                text.style.cursor = "text";
            }
            return line;
        },
        //画一条只有两个中点的折线
        drawPoly: function (linecondition, id, sp, m1, m2, ep, mark) {
            var poly, strPath;
            poly = document.createElementNS("http://www.w3.org/2000/svg", "g");
            var hi = document.createElementNS("http://www.w3.org/2000/svg", "path");
            var path = document.createElementNS("http://www.w3.org/2000/svg", "path");
            if (id != "") poly.setAttribute("id", id);
            poly.setAttribute("from", sp[0] + "," + sp[1]);
            poly.setAttribute("to", ep[0] + "," + ep[1]);
            hi.setAttribute("visibility", "hidden");
            hi.setAttribute("stroke-width", 9);
            hi.setAttribute("fill", "none");
            hi.setAttribute("stroke", "white");
            strPath = "M " + sp[0] + " " + sp[1];
            if (m1[0] != sp[0] || m1[1] != sp[1])
                strPath += " L " + m1[0] + " " + m1[1];
            if (m2[0] != ep[0] || m2[1] != ep[1])
                strPath += " L " + m2[0] + " " + m2[1];
            strPath += " L " + ep[0] + " " + ep[1];
            hi.setAttribute("d", strPath);
            hi.setAttribute("pointer-events", "stroke");
            path.setAttribute("d", strPath);
            path.setAttribute("stroke-width", 3.0);
            path.setAttribute("stroke-linecap", "round");
            path.setAttribute("fill", "none");

            if (mark) {
                path.setAttribute("stroke", "#3498DB");//ff3300
                path.setAttribute("marker-end", "url(#arrow3)");
            }
            else if (linecondition == '2') {
                path.setAttribute("stroke", "#ff0000");//ff3300
                path.setAttribute("marker-end", "url(#arrow2)");
            }
            else {
                path.setAttribute("stroke", "gray");
                path.setAttribute("marker-end", "url(#arrow1)");
            }
            poly.appendChild(hi);
            poly.appendChild(path);
            var text = document.createElementNS("http://www.w3.org/2000/svg", "text");
            //text.textContent=id;
            poly.appendChild(text);
            var x = (m2[0] + m1[0]) / 2;
            var y = (m2[1] + m1[1]) / 2;
            text.setAttribute("text-anchor", "middle");
            text.setAttribute("x", x);
            text.setAttribute("y", y - 5);
            text.style.cursor = "text";
            poly.style.cursor = "pointer";
            return poly;
        },
        // 计算两个结点间要连折线的话，连线的所有坐标
        calcPolyPoints: function (SP, EP, type, M) {
            var m1 = [], m2 = [], m;
            if (type == "lr") {
                var m = M || (SP[0] + EP[0]) / 2;
                //粗略计算2个中点
                m1 = [m, SP[1]];
                m2 = [m, EP[1]];
            }
            //如果是允许中段可上下移动的折线,则参数M为可移动中段线的Y坐标
            else if (type == "tb") {
                var m = M || (SP[1] + EP[1]) / 2;
                //粗略计算2个中点
                m1 = [SP[0], m];
                m2 = [EP[0], m];
            }
            return { start: SP, m1: m1, m2: m2, end: EP };
        },
        // 增加一条线
        addLine: function (DefaultOption, line) {
            var $line;
            if (line.from == line.to) return;
            // 避免两个节点间不能有一条以上同向接连线
            for (var i = 0, l = DefaultOption.line.length; i < l; i++) {
                if ((line.from == DefaultOption.line[i].from && line.to == DefaultOption.line[i].to)) {
                    return;
                }
            }
            // 获取开始和结束节点的坐标
            var sxy = $.dfworkflow.getLineSpotXY(line.from, DefaultOption, line.sp);
            var exy = $.dfworkflow.getLineSpotXY(line.to, DefaultOption, line.ep);
            line.name = line.name || '';
            line.linecondition = line.linecondition || '1';
            DefaultOption.line.push(line);

            if (line.type && line.type != "sl") {
                var res = $.dfworkflow.calcPolyPoints(sxy, exy, line.type, line.M);
                $line = $.dfworkflow.drawPoly(line.linecondition, line.id, res.start, res.m1, res.m2, res.end, line.mark);
            }
            else {
                line.type = "sl";//默认为直线
                $line = $.dfworkflow.drawLine(line.linecondition, line.id, sxy, exy, line.mark);
            }
            var $draw = $('#' + DefaultOption.id).find('svg');

            $($line)[0].DefaultOption = DefaultOption;
            if (line.name != "") {
                $($line).find('text').html(line.name);
            }
            $draw.append($line);
        },
        // 重构所有连向某个结点的线的显示，传参结构为$nodeData数组的一个单元结构
        resetLines: function (nodeId, DefaultOption) {
            var $line;
            for (var i = 0, l = DefaultOption.line.length; i < l; i++) {
                var sxy = [];
                var exy = [];
                var line = DefaultOption.line[i];
                if (line.from == nodeId || line.to == nodeId) {

                    sxy = $.dfworkflow.getLineSpotXY(line.from, DefaultOption, line.sp);
                    exy = $.dfworkflow.getLineSpotXY(line.to, DefaultOption, line.ep);

                    $('#' + line.id).remove();

                    if (line.type == "sl") {
                        $line = $.dfworkflow.drawLine(line.linecondition, line.id, sxy, exy, line.mark);
                    }
                    else {
                        var res = $.dfworkflow.calcPolyPoints(sxy, exy, line.type, line.M);
                        $line = $.dfworkflow.drawPoly(line.linecondition, line.id, res.start, res.m1, res.m2, res.end, line.mark);
                    }
                    var $draw = $('#' + DefaultOption.id).find('svg');
                    $($line)[0].DefaultOption = DefaultOption;
                    $draw.append($line);

                    var lineId = $($line).attr('id');
                    var lineData = $.dfworkflow.getLine(DefaultOption, lineId);
                    $($line).find('text').html(lineData.name);
                }
            }
        },
        //重新设置连线的样式 newType= "sl":直线, "lr":中段可左右移动型折线, "tb":中段可上下移动型折线
        setLineType: function (id, newType) {
            var $line = $('#' + id);
            var DefaultOption = $line[0].DefaultOption;

            var lineData = $.dfworkflow.getLine(DefaultOption, id);

            if (!newType || newType == null || newType == "" || newType == lineData.type) return false;
            var from = lineData.from;
            var to = lineData.to;
            lineData.type = newType;

            var sxy = $.dfworkflow.getLineSpotXY(from, DefaultOption, lineData.sp);
            var exy = $.dfworkflow.getLineSpotXY(to, DefaultOption, lineData.ep);

            var res;
            // 如果是变成折线
            if (newType != "sl") {
                var res = $.dfworkflow.calcPolyPoints(sxy, exy, lineData.type, lineData.M);
                $.dfworkflow.setLineM(id, $.dfworkflow.getMValue(sxy, exy, newType), true);
            }
            // 如果是变回直线
            else {
                delete lineData.M;
                var $lineMove = $('.workflow-linemover');
                $lineMove.hide().removeData("type").removeData("tid");

                $line.remove();
                $line = $.dfworkflow.drawLine(lineData.linecondition, lineData.id, sxy, exy, lineData.mark);
                var $draw = $('#' + DefaultOption.id).find('svg');
                $($line)[0].DefaultOption = DefaultOption;
                $draw.append($line);
                var lineData = $.dfworkflow.getLine(DefaultOption, id);
                $($line).find('text').html(lineData.name);

            }



            if (DefaultOption.focusId == id) {
                $.dfworkflow.focusItem(id);
            }
        },
        //设置折线中段的X坐标值（可左右移动时）或Y坐标值（可上下移动时）
        setLineM: function (id, M, noStack) {

            var DefaultOption = $('#' + id)[0].DefaultOption;
            var lineData = $.dfworkflow.getLine(DefaultOption, id);

            if (!lineData || M < 0 || !lineData.type || lineData.type == "sl") return false;
            var from = lineData.from;
            var to = lineData.to;
            lineData.M = M;
            var sxy = $.dfworkflow.getLineSpotXY(from, DefaultOption, lineData.sp);
            var exy = $.dfworkflow.getLineSpotXY(to, DefaultOption, lineData.ep);
            var ps = $.dfworkflow.calcPolyPoints(sxy, exy, lineData.type, lineData.M);


            $('#' + id).remove();
            console.log(lineData);
            var $line = $.dfworkflow.drawPoly(lineData.linecondition, id, ps.start, ps.m1, ps.m2, ps.end, lineData.marked || DefaultOption.focusId == id);
            var $draw = $('#' + DefaultOption.id).find('svg');

            $($line)[0].DefaultOption = DefaultOption;
            $draw.append($line);
            $($line).find('text').html(lineData.name);
        },
        //初始化折线中段的X/Y坐标,mType='rb'时为X坐标,mType='tb'时为Y坐标
        getMValue: function (sxy, exy, mType) {
            if (mType == "lr") {
                return (sxy[0] + exy[0]) / 2;
            }
            else if (mType == "tb") {
                return (sxy[1] + exy[1]) / 2;
            }
        },
        // 删除线条
        delLine: function (lineId) {
            var $line = $('#' + lineId)
            var DefaultOption = $line[0].DefaultOption;
            for (var i = 0, l = DefaultOption.line.length; i < l; i++) {
                if (lineId == DefaultOption.line[i].id) {
                    DefaultOption.line.splice(i, 1);
                    break;
                }
            }
            DefaultOption.focusId = "";
            $line.remove();
        },
        updateLineName: function ($workArea, lineId) {
            var $line = $('#' + lineId);
            var DefaultOption = $workArea[0].DefaultOption;
            var lineData = $.dfworkflow.getLine(DefaultOption, lineId);
            $line.find('text').html(lineData.name);
            if (lineData.linecondition == '2') {
                $line[0].childNodes[1].setAttribute("stroke", "#ff3300");
                $line[0].childNodes[1].setAttribute("marker-end", "url(#arrow2)");
            }
            else {
                $line[0].childNodes[1].setAttribute("stroke", "gray");
                $line[0].childNodes[1].setAttribute("marker-end", "url(#arrow1)");
            }
        }
    };
    $.fn.dfworkflow = function (op) {
        var DefaultOption = {
            openNode: function () { },
            openLine: function () { },
            toolBtns: ["startround", "endround", "stepnode", "confluencenode", "conditionnode", "auditornode","processnode"],//"childwfnode"
            nodeRemarks: {
                cursor: "选择",
                direct: "连接线",
                startround: "开始",
                endround: "结束",
                stepnode: "审批",
                confluencenode: "会签",
                conditionnode: "条件判断",
                auditornode: "传阅",
                processnode: "执行"
                //childwfnode: "子流程节点"
            },
            node: [],
            line: [],
            hasStartround: false,
            hasEndround: false,
            focusId: ''
        };
        $.extend(DefaultOption, op);
        var $self = $(this);
        DefaultOption.id = $self.attr("id");
        $self[0].DefaultOption = DefaultOption;
        $.dfworkflow.render($self);
    };

    $.fn.dfworkflowGet = function () {
        var $self = $(this);
        var $workArea = $self.find(".workflow-workinner");
        var DefaultOption = $workArea[0].DefaultOption;

        var data = {
            nodes: DefaultOption.node,
            lines: DefaultOption.line
        };

        return data;
    }

    $.fn.dfworkflowSet = function (name, op) {
        var $self = $(this);
        var $workArea = $self.find(".workflow-workinner");
        switch (name) {
            case 'updateNodeName':
                $.dfworkflow.updateNodeName($workArea, op.nodeId);
                break;
            case 'updateLineName':
                $.dfworkflow.updateLineName($workArea, op.lineId);
                break;
            case 'set':
                var DefaultOption = $workArea[0].DefaultOption;
                for (var i = 0, l = op.data.nodes.length; i < l; i++) {
                    var node = op.data.nodes[i];
                    $.dfworkflow.addNode($workArea, DefaultOption, node, true);
                }
                for (var i = 0, l = op.data.lines.length; i < l; i++) {
                    var line = op.data.lines[i];
                    $.dfworkflow.addLine(DefaultOption, line);
                }
                break;
        }
    }

})(jQuery, top.luckyu);
