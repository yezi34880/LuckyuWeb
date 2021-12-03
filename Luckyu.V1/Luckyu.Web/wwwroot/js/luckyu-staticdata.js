(function () {

    window.luckyu_staticdata = {
        wf_resultshow: [
            { value: 1, name: '<span class="label label-success">通过</span>' },
            { value: 2, name: '<span class="label label-error">退回</span>' },
            { value: 3, name: '<span class="label label-primary">申请会签办理</span>' },
            { value: 4, name: '<span class="label label-success">已阅</span>' },
            { value: 5, name: '<span class="label label-primary">调整</span>' },
            { value: 6, name: '<span class="label label-primary">申请转发查看</span>' },
            { value: 100, name: '<span class="label label-info">当前待办</span>' },
        ],
        // 说明
        wf_description: '【转发查看】选择其他用户协助审批，转发查看用户可以填写意见建议，无法通过或是退回，审批节点也不会移动，后续审批人仅仅能够看到转发查看用户审批意见<br />【会签办理】选择其他用户会签办理审批，相当于把当前步审批权让渡给会签办理人，会签办理用户可以通过或是退回，操作后审批节点会移动<br /><b>注：1、申请转发查看后当前审批人仍然可以自行处理，或者等待选择人处理<br />2、会签办理选择多人时，只有当多个会签办理人全部通过，节点才会向后移动，而只需要任意一个会签办理人拒绝，当前节点任务就会结束，类似于会签。</b>',
    };
})();