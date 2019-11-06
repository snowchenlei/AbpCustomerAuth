function queryParams(params) {
    return { //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit //页面大小
        , skipCount: params.offset //跳过条数 //params.offset / params.limit  //页码
        , parameterTypeId: $('#sel_search_parameter_type_id option:selected').val()
    };
}
(function () {
    var _parameterService = abp.services.app.parameter;

    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            createOrEdit(app.localize('EditParameter', row.value), row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: app.localize('Delete'),
                message: abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), row.value),
                callback: function (result) {
                    if (result) {
                        _parameterService.delete({
                            id: row.id
                        }).done(function () {
                            var $table = $('#tb-body');
                            $table.bootstrapTable('remove',
                                {
                                    field: 'id',
                                    values: [row.id]
                                });
                        });
                    }
                }
            });
        }
    };
    function operateFormater(value, row, index) {
        var htmlArr = [];
        htmlArr.push('<div class="btn-group" role="group" aria-label="Row Operation">');
        if (abp.auth.isGranted('Pages.Administration.Parameters.Edit')) {
            htmlArr.push(
                '<button type="button" class="btn btn-sm btn-warning edit" title="edit"><i class="fas fa-edit"></i>' +
                app.localize('Edit') +
                '</button>');
        }
        if (abp.auth.isGranted('Pages.Administration.Parameters.Delete')) {
            htmlArr.push(
                '<button type="button" class="btn btn-sm btn-danger remove" title="remove"><i class="fas fa-trash"></i>' +
                app.localize('Delete') +
                '</button>');
        }
        htmlArr.push('</div>');
        return htmlArr.join('');
    }
    var columns = [
        { checkbox: true },
        { field: 'id', title: 'Id', visible: false },
        { field: 'value', title: app.localize('Value') },
        { field: 'typeName', title: app.localize('TypeName') },
        { title: app.localize('Operation'), formatter: operateFormater, events: operateEvents }
    ];

    function createOrEdit(title, id) {
        dialog = bootbox.dialog({
            title: title,
            message: '<p><i class="fa fa-spin fa-spinner"></i> ' + app.localize('Loading') + '</p>',
            size: 'large',
            buttons: {
                cancel: {
                    label: app.localize('Cancel'),
                    className: 'btn-danger'
                },
                confirm: {//ok、confirm会在加载完成后获取焦点
                    label: app.localize('OK'),
                    className: 'btn-success',
                    callback: function (result) {
                        if (result) {
                            save();
                            return false;
                        }
                    }
                }
            }
        });
        dialog.init(function () {
            $.get(abp.appPath + 'Parameters/CreateOrEditModal', { id: id }, function (data) {
                dialog.removeAttr('tabindex');
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }

    function save() {
        abp.ui.setBusy(dialog);
        //手动验证
        var $e = $("#modelForm");
        if (!$e.valid()) {
            abp.ui.clearBusy(dialog);
            return false;
        }
        var parameter = $e.serializeFormToObject();
        _parameterService.createOrEdit({
            parameter
        }).done(function (result) {
            abp.notify.info(app.localize('SavedSuccessfully'));
            dialog.modal('hide');
            refreshTable();
        }).always(function () {
            abp.ui.clearBusy(dialog);
        });
    };

    $(function () {
        //1、初始化表格
        table.init('api/services/app/Parameter/GetPaged', columns);
        $('#sel_search_parameter_type_id').select2({
            language: "zh-CN",// 指定语言为中文，国际化才起效
            placeholder: app.localize('PleaseChoose', app.localize('ParameterType')),//'请选择参数类型',
            allowClear: true,
            width: '100%',
            ajax: {
                url: 'api/services/app/Parameter/GetAllParameterTypes',
                dataType: 'json',
                processResults: function (data) {
                    // Tranforms the top-level key of the response object from 'items' to 'results'
                    return {
                        results: data.result.items
                    };
                }
                // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
            }
        });
        $('#create').click(function () {
            createOrEdit(app.localize('CreateNewParameter'));
        });
    });
})();