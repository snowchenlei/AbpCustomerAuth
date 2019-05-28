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
    let detailHtml;
    window.operateEvents = {
        'click .detail': function (e, value, auditLog, index) {
            $('#AuditLogDetailModal_UserName').html(auditLog.userName);
            $('#AuditLogDetailModal_ClientIpAddress').html(auditLog.clientIpAddress);
            $('#AuditLogDetailModal_ClientName').html(auditLog.clientName);
            $('#AuditLogDetailModal_BrowserInfo').html(auditLog.browserInfo);
            $('#AuditLogDetailModal_ServiceName').html(auditLog.serviceName);
            $('#AuditLogDetailModal_MethodName').html(auditLog.methodName);
            $('#AuditLogDetailModal_ExecutionTime').html(moment(auditLog.executionTime).fromNow() + ' (' + moment(auditLog.executionTime).format('YYYY-MM-DD hh:mm:ss') + ')');
            $('#AuditLogDetailModal_Duration').html(app.localize('Xms', auditLog.executionDuration));
            $('#AuditLogDetailModal_Parameters').html(getFormattedParameters(auditLog.parameters));

            if (auditLog.impersonatorUserId) {
                $('#AuditLogDetailModal_ImpersonatorInfo').show();
            } else {
                $('#AuditLogDetailModal_ImpersonatorInfo').hide();
            }

            if (auditLog.exception) {
                $('#AuditLogDetailModal_Success').hide();
                $('#AuditLogDetailModal_Exception').show();
                $('#AuditLogDetailModal_Exception').html(auditLog.exception);
            } else {
                $('#AuditLogDetailModal_Exception').hide();
                $('#AuditLogDetailModal_Success').show();
            }

            if (auditLog.customData) {
                $('#AuditLogDetailModal_CustomData_None').hide();
                $('#AuditLogDetailModal_CustomData').show();
                $('#AuditLogDetailModal_CustomData').html(auditLog.customData);
            } else {
                $('#AuditLogDetailModal_CustomData').hide();
                $('#AuditLogDetailModal_CustomData_None').show();
            }
            bootbox.dialog({
                title: app.localize("AuditLogDetail"),
                message: $('#AuditLogDetailBody > #AuditLogDetailContent')[0],
                size: 'large',
                buttons: {
                    cancel: {
                        label: app.localize('Cancel'),
                        className: 'btn-danger',
                        callback: function () {
                            initModalDetail();
                        }
                    }
                },
                onEscape: function () {
                    initModalDetail();
                }
            });
        }
    };
    function initModalDetail() {
        $('#AuditLogDetailBody').append(detailHtml);
        $('#AuditLogDetailContent li:first-child a').tab('show');
    }
    function operateFormater(value, row, index) {
        var htmlArr = [];
        htmlArr.push('<div class="btn-group" role="group" aria-label="Row Operation">');
        htmlArr.push(
            '<button type="button" class="btn btn-sm btn-warning detail" title="' + app.localize("AuditLogDetail") + '"><i class="fas fa-detail"></i>' +
            app.localize("AuditLogDetail") +
            '</button>');
        htmlArr.push('</div>');
        return htmlArr.join('');
    }
    var columns = [
        { checkbox: true },
        { field: 'userId', title: 'userId', visible: false },
        { title: app.localize('Operation'), formatter: operateFormater, events: operateEvents },
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
    function getFormattedParameters(parameters) {
        try {
            var json = JSON.parse(parameters);
            return JSON.stringify(json, null, 4);
        } catch (e) {
            return parameters;
        }
    }
    $(function () {
        //1、初始化表格
        table.init('api/services/app/AuditLog/GetAuditLogs', columns);
        setDate($('#txt_search_range_time'), true, true);
        detailHtml = $('#AuditLogDetailBody > #AuditLogDetailContent')[0];
    });
})();