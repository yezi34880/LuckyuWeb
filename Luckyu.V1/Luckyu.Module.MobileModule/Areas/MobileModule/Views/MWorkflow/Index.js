var bootstrap = function () {
    "use strict";

    var paras = bui.history.getParams("url");
    var tasktype = !paras.tasktype ? 1 : parseInt(paras.tasktype);

    var page = {
        init: function () {
            page.bind();
            page.bindList();
        },
        bind: function () {
            if (tasktype === 1) {
                $("#headerTitle").html("待办工作");
            }
            else if (tasktype == 2) {
                $("#headerTitle").html("已办工作");
            }
            else {
                $("#headerTitle").html("我的提交");
            }

        },
        bindList: function () {
            var uiScroll = bui.scroll({
                id: "#scroll",
                children: ".bui-list", //循环遍历的数据的父层,如果不对,会出现无限滚动的情况
                page: 1,
                pageSize: 30,
                onBeforeRefresh: function () {
                },
                onBeforeLoad: function () {
                },
                onRefresh: function () {
                    getData.call(this, 1, 30, "html");
                },
                onLoad: getData,
                callback: function (argument) {

                }
            });

            function getData(page, pagesize, command) {
                var _self = this;
                //跟刷新共用一套数据
                var command = command || "append";

                bui.ajax({
                    url: "/WorkflowModule/Task/Page",
                    data: {
                        tasktype: tasktype,
                        page: page,
                        rows: pagesize
                    }
                }).done(function (res) {
                    //生成html
                    var html = template(res.rows);
                    $("#scrollList")[command](html);
                    bui.btn({ id: "#scrollList", handle: ".bui-btn" }).load();

                    // 更新分页信息,如果高度不足会自动请求下一页
                    _self.updatePage(page, res.rows);
                    // 刷新的时候返回位置
                    _self.reverse();
                }).fail(function (res) {
                    // 可以点击重新加载
                    _self.fail(page, pagesize, res);
                })
            }
            //生成模板
            function template(data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    var row = data[i];
                    html += '\
<li class="bui-btn bui-box" href="/MobileModule/MWorkflow/Form?taskId=' + row.task_id + '&instanceId=' + row.instance_id + '&processId=' + row.process_id + '&historyId=' + row.history_id + '">\
    <div class="span1">\
        <h3 class="item-title"><span>【'+ row.flowname + '】</span>' + row.processname + '</h3>\
        <p class="item-text">'+ row.submit_username + ' ' + row.createtime + '</p>\
     </div >\
     <i class="icon-listright"></i>\
</li>';
                }
                return html;
            }
        }
    };
    page.init();
};