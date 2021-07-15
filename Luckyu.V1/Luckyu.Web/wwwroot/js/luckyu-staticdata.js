(function () {

    window.luckyu_staticdata = {
        wf_resultshow: [
            { value: 1, name: '<span class="label label-success">通过</span>' },
            { value: 2, name: '<span class="label label-error">驳回</span>' },
            { value: 3, name: '<span class="label label-primary">申请代办</span>' },
            { value: 4, name: '<span class="label label-success">已阅</span>' },
            { value: 5, name: '<span class="label label-primary">调整</span>' },
            { value: 6, name: '<span class="label label-primary">申请协办</span>' },
            { value: 100, name: '<span class="label label-info">当前待办</span>' },
        ],
        // 说明
        wf_description: '【协办】选择其他用户协助审批，其他用户审批后流程节点不会移动，后续审批人仅仅能够看到协办用户审批意见<br />【代办】选择其他用户代办审批，其他用户审批后节点会移动，相当于把当前步审批让渡给代办人<br /><b>注：1、申请协办、代办后当前审批人仍然可以自行处理，或者等待选择人处理<br />2、协办选择多人后，相当于生成多条待办事项，每个选择的用户都可以填写意见；代办选择多人后，依旧处于当前待办人步骤，当前待办人与其选择代办人（一个或多个）任意一人审批流程节点都会移动</b>',
    };
})();