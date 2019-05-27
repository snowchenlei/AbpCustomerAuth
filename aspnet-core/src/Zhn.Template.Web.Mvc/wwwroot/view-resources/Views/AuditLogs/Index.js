function queryParams(params) {
    return { //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit, //页面大小
        skipCount: params.offset, //跳过条数 //params.offset / params.limit  //页码
        methodName: $('#txt_search_method_name').val(),
        serviceName: $('#txt_search_service_name').val(),
        userName: $('#txt_search_user_name').val(),
        browserInfo: $('#txt_search_browser').val(),
        minExecutionDuration: $('#txt_search_min_execution_duration').val(),
        maxExecutionDuration: $('#txt_search_max_execution_duration').val(),
        hasException: $('#sel_search_state').val(),
        dateRange: $('#txt_search_range_time').val()
    };
}

(function () {
    var columns = [
        { checkbox: true },
        { field: 'userId', title: 'userId', visible: false },
        {
            field: 'exception',
            //title: app.localize('exception'),
            formatter: function (value, row, index) {
                var $div = $("<div/>").addClass("text-center");
                if (value) {
                    $div.append($("<i/>").addClass("fa fa-exclamation-triangle m--font-warning")
                        .attr("title", app.localize("HasError")));
                } else {
                    $div.append($("<i/>").addClass("fa fa-check-circle m--font-success")
                        .attr("title", app.localize("Success")));
                }
                return $div[0].outerHTML;
            }
        },
        {
            field: 'executionTime',
            title: app.localize('Time'),
            formatter: function (value, row, index) {
                return moment(value).format('YYYY-MM-DD HH:mm:ss');
            }
        },
        { field: 'userName', title: app.localize('UserName') },
        { field: 'serviceName', title: app.localize('Service') },
        { field: 'methodName', title: app.localize('Action') },
        {
            field: 'executionDuration',
            title: app.localize('Duration'),
            formatter: function (value, row, index) {
                return app.localize('Xms', value);
            }
        },
        { field: 'clientIpAddress', title: app.localize('IpAddress') },
        { field: 'clientName', title: app.localize('Client') },
        {
            field: 'browserInfo',
            title: app.localize('Browser'),
            formatter: function (value, row, index) {
                return $("<span/>").text(abp.utils.truncateStringWithPostfix(value, 32))
                    .attr("title", value)[0].outerHTML;
            }
        },
    ];
    $(function () {
        //1、初始化表格
        table.init('api/services/app/AuditLog/GetAuditLogs', columns);
        setDate($('#txt_search_range_time'), true, true);
    });
})();