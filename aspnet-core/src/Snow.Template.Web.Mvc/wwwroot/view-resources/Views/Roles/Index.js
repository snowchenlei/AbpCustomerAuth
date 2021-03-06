﻿function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit,   //页面大小
        skipCount: params.offset //跳过条数 //params.offset / params.limit  //页码
    };
}
(function () {
    var _roleService = abp.services.app.role;
    var _permissionsTree;
    var dialog;
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            createOrEdit(app.localize('EditRole', row.displayName), row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: app.localize('Delete'),
                message: abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), row.displayName),
                callback: function (result) {
                    if (result) {
                        _roleService.delete({
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
        if (abp.auth.isGranted('Pages.Administration.Roles.Edit')) {
            htmlArr.push(
                '<button type="button" class="btn btn-sm btn-warning edit" title="edit"><i class="fas fa-edit"></i>' +
                app.localize('Edit') +
                '</button>');
        }
        if (abp.auth.isGranted('Pages.Administration.Roles.Delete')) {
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
        { field: 'displayName', title: app.localize('DisplayName') },
        { field: 'creationTime', title: app.localize('CreationTime') },
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
        var role = $e.serializeFormToObject();
        _roleService.createOrUpdate({
            role,
            GrantedPermissions: getSelectedPermissionNames()
        }).done(function (result) {
            abp.notify.info(app.localize('SavedSuccessfully'));
            dialog.modal('hide');
            refreshTable();
        }).always(function () {
            abp.ui.clearBusy(dialog);
        });
    }

    function getSelectedPermissionNames() {
        _permissionsTree = $.fn.zTree.getZTreeObj("permissionTree");
        var checkedNodes = _permissionsTree.getCheckedNodes(true);
        var selectedPermissionNames = [];
        for (var i = 0; i < checkedNodes.length; i++) {
            selectedPermissionNames.push(checkedNodes[i].name);
        }
        return selectedPermissionNames;
    }

    //搜索
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
            $.get(abp.appPath + 'Roles/CreateOrEditModal', { Id: id }, function (data) {
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }
    $(function () {
        //1、初始化表格
        table.init('api/services/app/role/getPaged', columns);

        $('#create').click(function () {
            createOrEdit(app.localize('CreateNewRole'));
        });
    });
})();