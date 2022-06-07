/**
 *  审批日志
 */
var bootstrap = function (layui) {

    var page = {
        init: function () {
            page.initData();
        },
        initData: function () {
            $("li[luckyu-id]").each(function (item, index) {
                var self = $(this);
                var instanceId = self.attr("luckyu-id");
                luckyu.ajax.getv2(luckyu.rootUrl + '/WorkflowModule/Task/GetFormData', { instanceId: instanceId }, function (data) {

                    var shceme = JSON.parse(data.Instance.schemejson);

                    for (var i = 0, l = shceme.nodes.length; i < l; i++) {
                        var node = shceme.nodes[i];
                        if (node.type === "startround" || node.type === "endround") {
                            continue;
                        }
                        node.state = '3';
                        var history = data.History.filter(r => r.node_id === node.id);
                        if (!!history && history.length > 0) {
                            if (history[0].result == 1) {
                                node.state = '1';
                            }
                            else if (history[0].result == 2) {
                                node.state = '2';
                            }
                        }
                        if (data.CurrentNode.id == node.id) {
                            node.state = '0';
                        }
                    }

                    $('#flow_' + instanceId).dfworkflow({
                        isPreview: true,
                        openNode: function (node) {

                        }
                    });

                    $('#flow_' + instanceId).parent().height(window.innerHeight - 70).width(window.innerWidth - 550);
                    $('#ultimeline_' + instanceId).parent().height(window.innerHeight - 70).width(window.innerWidth - 900);
                    $(window).resize(function () {
                        $('#flow_' + instanceId).parent().height(window.innerHeight - 70).width(window.innerWidth - 550);
                        $('#ultimeline_' + instanceId).parent().height(window.innerHeight - 70).width(window.innerWidth - 900);
                    });
                    $('#flow_' + instanceId).dfworkflowSet('set', { data: shceme });

                    // 历史记录
                    var htmlHistory = '';
                    for (var i = 0; i < data.History.length; i++) {
                        var his = data.History[i];
                        htmlHistory += '\
<li class="layui-timeline-item">\
    <i class="layui-icon layui-timeline-axis">&#xe63f;</i>\
    <div class="layui-timeline-content layui-text">\
        <h3 class="layui-timeline-title">'+ luckyu.utility.toEnum(his.result, luckyu_staticdata.wf_resultshow) + ' ' + his.nodename + '</h3>\
        <p>';
                        if (his.result != 0) {
                            htmlHistory += "审批人：" + his.create_username;

                            if (his.result != 100) {
                                htmlHistory += "<br/>提交时间：" + new Date(his.tasktime).format("yyyy-MM-dd HH:mm")
                                htmlHistory += "<br/>办结时间：" + new Date(his.apptime).format("yyyy-MM-dd HH:mm")
                                htmlHistory += (!his.opinion ? '' : ('<br/>意见建议：' + his.opinion));
                            }
                            if (!!his.annex && his.annex != '[]') {
                                var list = JSON.parse(his.annex);
                                htmlHistory += "<br/>附件：";
                                for (var j = 0; j < list.length; j++) {
                                    htmlHistory += ' <br/><a style="color:blue;cursor:pointer;text-decoration:underline;" href="/SystemModule/Annex/ShowFile?keyValue=' + list[j].Key + '" target="_blank"> ' + list[j].Value + '</a>';
                                }
                            }
                        }

                        htmlHistory += '</p></div></li>';
                    }
                    $('#ultimeline_' + instanceId).html(htmlHistory);

                });

            });
        },
    };
    page.init();

};