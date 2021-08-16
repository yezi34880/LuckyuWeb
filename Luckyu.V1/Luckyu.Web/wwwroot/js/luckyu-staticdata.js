(function () {

    window.luckyu_staticdata = {
        wf_resultshow: [
            { value: 1, name: '<span class="label label-success">通过</span>' },
            { value: 2, name: '<span class="label label-error">退回</span>' },
            { value: 3, name: '<span class="label label-primary">申请加签</span>' },
            { value: 4, name: '<span class="label label-success">已阅</span>' },
            { value: 5, name: '<span class="label label-primary">调整</span>' },
            { value: 6, name: '<span class="label label-primary">申请协办</span>' },
            { value: 100, name: '<span class="label label-info">当前待办</span>' },
        ],
        // 说明
        wf_description: '【协办】选择其他用户协助审批，协办用户可以填写意见建议，无法通过或是驳回，审批节点也不会移动，后续审批人仅仅能够看到协办用户审批意见<br />【加签】选择其他用户加签审批，相当于把当前步审批权让渡给加签人，加签用户可以通过或是驳回，操作后审批节点会移动<br /><b>注：1、申请协办后当前审批人仍然可以自行处理，或者等待选择人处理<br />2、加签选择多人时，只有当多个加签人全部通过，节点才会向后移动，而只需要任意一个加签人拒绝，当前节点任务就会结束，类似于会签。</b>',
    };
})();