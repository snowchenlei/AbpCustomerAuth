function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit,   //页面大小
        skipCount: params.offset //跳过条数 //params.offset / params.limit  //页码
    };
}
(function () {
    var columns = [
        { checkbox: true },
        { field: 'userId', title: 'userId', visible: false },
        { field: 'userName', title: app.localize('UserName') },
        { field: 'serviceName', title: app.localize('ServiceName') },
        { field: 'methodName', title: app.localize('MethodName') },
    ];
    $(function () {
        //1、初始化表格
        table.init(columns);

        $('#create').click(function () {
            createOrEdit(app.localize('CreateNewUser'));
        });
    });
})();