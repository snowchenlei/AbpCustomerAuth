var absoluteUrl;
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        MaxResultCount: params.limit,   //页面大小
        SkipCount: params.offset / params.limit,  //页码
    };
}
function operateFormater(value, row, index) {
    var htmlArr = [];
    htmlArr.push('<div class="btn-group" role="group" aria-label="Row Operation">');
    htmlArr.push('<button type="button" class="btn btn-sm btn-warning edit" title="edit"><i class="fas fa-edit"></i>修改</button>');
    htmlArr.push('<button type="button" class="btn btn-sm btn-danger remove" title="remove"><i class="fas fa-trash"></i>删除</button>');
    htmlArr.push('</div>');
    return htmlArr.join('');
}
function createOrEdit(title, id) {
    var dialog = bootbox.dialog({
        title: title,
        message: '<p><i class="fa fa-spin fa-spinner"></i> 加载中...</p>',
        size: 'large',
        buttons: {
            cancel: {
                label: "取消",
                className: 'btn-danger ladda-button'
            },
            ok: {
                label: "提交",
                className: 'btn-success ladda-button',
                callback: function (result) {
                    //var l = Ladda.create(result.target);
                    //l.start();
                    //手动验证
                    var $e = $("#modelForm");
                    if (!$e.valid()) {
                        //l.stop();
                        return false;
                    }
                    var s = $e.serializeArray();
                    $.post(abp.appPath + 'Users/CreateOrEditModal',
                        s,
                        function (result) {
                            //l.stop();
                            requestCallBack(result,
                                function () {
                                    refreshTable();
                                });
                            dialog.modal('hide');
                        });
                    return false;
                }
            }
        }
    });
    dialog.init(function () {
        //$('.ladda-button').attr('data-style', 'zoom-in');
        $.get(abp.appPath + 'Users/CreateOrEditModal', { userId: id }, function (data) {
            dialog.find('.bootbox-body').html(data);
        });
    });
}
(function () {
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            createOrEdit('修改用户' + row.name, row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: '删除' + row.name,
                message: abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), row.name),
                callback: function (result) {
                    if (result) {
                        var _userService = abp.services.app.user;
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
    var columns = [
        { checkbox: true },
        { field: 'id', title: 'Id', visible: false },
        { field: 'userName', title: '用户名' },
        { field: 'name', title: '名称' },
        { field: 'emailAddress', title: '邮箱' },
        { title: '操作', formatter: operateFormater, events: operateEvents }
    ];

    $(function () {
        //1、初始化表格
        table.init(columns);

        $('#create').click(function () {
            createOrEdit('添加用户', 0);
        });

        var _userService = abp.services.app.user;
        var _$modal = $('#UserCreateModal');
        var _$form = _$modal.find('form');

        _$form.validate({
            rules: {
                Password: "required",
                ConfirmPassword: {
                    equalTo: "#Password"
                }
            }
        });

        $('#RefreshButton').click(function () {
            refreshUserList();
        });

        $('.delete-user').click(function () {
            var userId = $(this).attr("data-user-id");
            var userName = $(this).attr('data-user-name');

            deleteUser(userId, userName);
        });

        $('.edit-user').click(function (e) {
            var userId = $(this).attr("data-user-id");

            e.preventDefault();
            $.ajax({
                url: abp.appPath + 'Users/EditUserModal?userId=' + userId,
                type: 'POST',
                contentType: 'application/html',
                success: function (content) {
                    $('#UserEditModal div.modal-content').html(content);
                },
                error: function (e) { }
            });
        });

        _$form.find('button[type="submit"]').click(function (e) {
            e.preventDefault();

            if (!_$form.valid()) {
                return;
            }

            var user = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js
            user.roleNames = [];
            var _$roleCheckboxes = $("input[name='role']:checked");
            if (_$roleCheckboxes) {
                for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                    var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                    user.roleNames.push(_$roleCheckbox.val());
                }
            }

            abp.ui.setBusy(_$modal);
            _userService.create(user).done(function () {
                _$modal.modal('hide');
                location.reload(true); //reload page to see new user!
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });
        });

        _$modal.on('shown.bs.modal', function () {
            _$modal.find('input:not([type=hidden]):first').focus();
        });

        function refreshUserList() {
            location.reload(true); //reload page to see new user!
        }

        function deleteUser(userId, userName) {
            abp.message.confirm(
                abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), userName),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _userService.delete({
                            id: userId
                        }).done(function () {
                            refreshUserList();
                        });
                    }
                }
            );
        }
    });
})();