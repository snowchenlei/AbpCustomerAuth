function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit,   //页面大小
        skipCount: params.offset //跳过条数 //params.offset / params.limit  //页码
    };
}
(function () {
    var _tenantService = abp.services.app.tenant;

    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            debugger
            edit(app.localize('EditTenant', row.name), row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: app.localize('Delete'),
                message: abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), row.name),
                callback: function (result) {
                    if (result) {
                        _tenantService.delete({
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
        if (abp.auth.isGranted('Pages.Administration.Tenants.Edit')) {
            htmlArr.push(
                '<button type="button" class="btn btn-sm btn-warning edit" title="edit"><i class="fas fa-edit"></i>' +
                app.localize('Edit') +
                '</button>');
        }
        if (abp.auth.isGranted('Pages.Administration.Tenants.Delete')) {
            htmlArr.push(
                '<button type="button" class="btn btn-sm btn-danger remove" title="remove"><i class="fas fa-trash"></i>' +
                app.localize('Delete') +
                '</button>');
        }
        htmlArr.push('</div>');
        return htmlArr.join('');
    }
    function activeFormatter(data) {
        if (data) {
            return '<input class="ichk" type="checkbox" checked disabled>';
        } else {
            return '<input type="checkbox" disabled/>';
        }
    }
    var columns = [
        { checkbox: true },
        { field: 'id', title: 'Id', visible: false },
        { field: 'tenancyName', title: app.localize('TenancyName') },
        { field: 'name', title: app.localize('Name') },
        { field: 'isActive', title: app.localize('isActive'), formatter: activeFormatter },
        { title: app.localize('Operation'), formatter: operateFormater, events: operateEvents }
    ];

    function create(title) {
        dialog = createOrEdit(title, createSave);
        dialog.init(function () {
            $.get(abp.appPath + 'tenants/createModal', function (data) {
                dialog.removeAttr('tabindex');
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }
    function edit(title, id) {
        dialog = createOrEdit(title, editSave);
        dialog.init(function () {
            $.get(abp.appPath + 'tenants/editModal', { Id: id }, function (data) {
                dialog.removeAttr('tabindex');
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }
    function createOrEdit(title, callback) {
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
                            callback();
                            return false;
                        }
                    }
                }
            }
        });
        return dialog;
    }

    function createSave() {
        abp.ui.setBusy(dialog);
        //手动验证
        var $e = $("#modelForm");
        if (!$e.valid()) {
            abp.ui.clearBusy(dialog);
            return false;
        }
        var tenant = $e.serializeFormToObject();
        _tenantService.create(tenant).done(function (result) {
            abp.notify.info(app.localize('SavedSuccessfully'));
            dialog.modal('hide');
            refreshTable();
        }).always(function () {
            abp.ui.clearBusy(dialog);
        });
    };
    function editSave() {
        abp.ui.setBusy(dialog);
        //手动验证
        var $e = $("#modelForm");
        if (!$e.valid()) {
            abp.ui.clearBusy(dialog);
            return false;
        }
        var tenant = $e.serializeFormToObject();
        _tenantService.update(tenant).done(function (result) {
            abp.notify.info(app.localize('SavedSuccessfully'));
            dialog.modal('hide');
            refreshTable();
        }).always(function () {
            abp.ui.clearBusy(dialog);
        });
    };
    $(function () {
        //1、初始化表格
        table.init('api/services/app/tenant/GetPaged', columns);
        $('#create').click(function () {
            create(app.localize('CreateNewTenant'));
        });
    });
})();