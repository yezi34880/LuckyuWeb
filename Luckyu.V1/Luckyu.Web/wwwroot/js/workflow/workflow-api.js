/*
 * 描 述：工作流api
 */
(function ($, luckyu, layui) {
    "use strict";

    layui.use(['layer'], function () { });

    var api = {
        create: luckyu.rootUrl + '/WorkFlowModule/Task/Create',
        approve: luckyu.rootUrl + '/WorkFlowModule/Task/Approve',
        adduser: luckyu.rootUrl + '/WorkFlowModule/Task/AddUser',
        finish: luckyu.rootUrl + '/WorkFlowModule/Task/Finish',
    };

    luckyu.workflowapi = {
        /**
         * 创建流程
         * @param {any} op
         */
        create: function (op) {
            var DefaultOption = {
                processId: '',
                flowCode: '',
                processName: '',
                submitUserId: '',
                callback: null,
            }
            $.extend(DefaultOption, op);
            var req = {
                processId: DefaultOption.processId,
                flowCode: DefaultOption.flowCode,
                processName: DefaultOption.processName,
            };
            luckyu.ajax.post(api.create, req, function (res) {
                if (res.code == 200) {
                    top.layui.notice.success("创建流程成功！");
                    if (!!callback) {
                        op.callback(true);
                    }
                }
                else {
                    top.layui.notice.error(res.info);
                    if (!!callback) {
                        op.callback(false);
                    }
                }
            });
        },

        /**
         * 审批
         * @param {any} op
         */
        approve: function (op) {
            var DefaultOption = {
                taskId: '',
                result: 1,
                opinion: '',
                callback: null,
            }
            $.extend(DefaultOption, op);
            var req = {
                taskId: DefaultOption.taskId,
                result: DefaultOption.result,
                opinion: DefaultOption.opinion,
            };
            luckyu.ajax.post(api.audit, req, function (res) {
                if (res.code == 200) {
                    if (!!callback) {
                        op.callback(true);
                    }
                }
                else {
                    top.layui.notice.error(res.info);
                    if (!!callback) {
                        op.callback(false);
                    }
                }
            });
        },

        /**
         * 加签
         * @param {any} op
         */
        adduser: function (op) {
            var DefaultOption = {
                taskId: '',
                userId: '',
                callback: null,
            }
            $.extend(DefaultOption, op);
            var req = {
                taskId: DefaultOption.taskId,
                userId: DefaultOption.userId,
            };
            luckyu.ajax.post(api.adduser, req, function (res) {
                if (res.code == 200) {
                    if (!!callback) {
                        op.callback(true);
                    }
                }
                else {
                    top.layui.notice.error(res.info);
                    if (!!callback) {
                        op.callback(false);
                    }
                }
            });

        },

        /**
         * 强制结束流程
         * @param {any} op
         */
        finish: function (op) {
            var req = {
                instanceId: op.instanceId,
            };
            luckyu.ajax.post(api.adduser, req, function (res) {
                if (res.code == 200) {
                    top.layui.notice.success(res.info);
                }
                else {
                    top.layui.notice.error(res.info);
                }
            });

        }

    };

})(jQuery, luckyu, layui);
