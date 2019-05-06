function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit,   //页面大小
        skipCount: params.offset / params.limit  //页码
    };
}
(function () {
    var _userService = abp.services.app.user;
    var dialog;
    var model;
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            createOrEdit(app.localize('EditUser', row.name), row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: app.localize('Delete', row.name),
                message: abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), row.name),
                callback: function (result) {
                    if (result) {
                        _userService.delete({
                            id: row.id
                        }).done(function () {
                            var $table = $('#tb-body');
                            $table.bootstrapTable('remove',
                                {
                                    field: 'id',
                                    values: [row.id]
                                });
                            //refreshUserList();
                        });
                    }
                }
            });
        }
    };

    function operateFormater(value, row, index) {
        var htmlArr = [];
        htmlArr.push('<div class="btn-group" role="group" aria-label="Row Operation">');
        if (abp.auth.isGranted('Pages.Administration.Users.Edit')) {
            htmlArr.push(
                '<button type="button" class="btn btn-sm btn-warning edit" title="edit"><i class="fas fa-edit"></i>' +
                app.localize('Edit') +
                '</button>');
        }
        if (abp.auth.isGranted('Pages.Administration.Users.Delete')) {
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
        { field: 'userName', title: app.localize('UserName') },
        { field: 'name', title: app.localize('Name') },
        { field: 'emailAddress', title: app.localize('EmailAddress') },
        { title: app.localize('Operation'), formatter: operateFormater, events: operateEvents }
    ];

    function save() {
        abp.ui.setBusy(dialog);
        //手动验证
        var $e = $("#modelForm");
        if (!$e.valid()) {
            abp.ui.clearBusy(dialog);
            return false;
        }
        var assignedRoleNames = _findAssignedRoleNames();
        var user = $e.serializeFormToObject();
        if (user.SetRandomPassword) {
            user.Password = null;
        }
        var _$roleCheckboxes = $("input[name='role']:checked");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                user.roleNames.push(_$roleCheckbox.val());
            }
        }
        _userService.createOrEdit({
            user,
            assignedRoleNames
        }).done(function (result) {
            abp.notify.info(app.localize('SavedSuccessfully'));
            dialog.modal('hide');
            refreshTable();
        }).always(function () {
            abp.ui.clearBusy(dialog);
        });
    }

    function _findAssignedRoleNames() {
        var assignedRoleNames = [];

        dialog.find('.user-role-checkbox-list input[type=checkbox]')
            .each(function () {
                if ($(this).is(':checked')) {
                    assignedRoleNames.push($(this).attr('name'));
                }
            });

        return assignedRoleNames;
    }
    //搜索
    function createOrEdit(title, id) {
        dialog = bootbox.dialog({
            title: title,
            message: '<p><i class="fa fa-spin fa-spinner"></i> ' + app.localize('Loading')+'</p>',
            size: 'large',
            buttons: {
                cancel: {
                    label: app.localize('Cancel'),
                    className: 'btn-danger'
                },
                confirm: {//ok、confirm会在加载完成后获取焦点
                    label: app.localize('OK'),
                    className: 'btn-success',
                    callback: function(result) {
                        if (result) {
                            save();
                            return false;
                        }
                    }
                }
            }
        });
        dialog.init(function () {
            //$('.ladda-button').attr('data-style', 'zoom-in');
            $.get(abp.appPath + 'Users/CreateOrEditModal', { userId: id }, function (data) {
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }
    $(function () {
        //1、初始化表格
        table.init(columns);

        $('#create').click(function () {
            createOrEdit(app.localize('CreateNewUser'));
        });
        $('#batch-delete').click(function () {
            var arr = $('#tb-body').bootstrapTable('getSelections');
            if (arr.length <= 0) {
                app.localize('PleaseSelectAtLeastOneItem');
                return false;
            }
            var names = arr.map(a => a.name).join(',');
            bootbox.confirm({
                size: 'small',
                title: app.localize('Delete', names),
                message: abp.utils.formatString(app.localize('AreYouSureWantToDelete'), names),
                callback: function (result) {
                    if (result) {
                        var ids = arr.map(a => a.id);
                        _userService.batchDelete(ids).done(function () {
                            var $table = $('#tb-body');
                            $table.bootstrapTable('remove',
                                {
                                    field: 'id',
                                    values: ids
                                });
                        });
                    }
                }
            });
            return true;
        });
    });
})();