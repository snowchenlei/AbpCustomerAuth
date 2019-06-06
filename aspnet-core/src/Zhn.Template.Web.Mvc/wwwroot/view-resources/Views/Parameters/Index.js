function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        maxResultCount: params.limit,   //页面大小
        skipCount: params.offset //跳过条数 //params.offset / params.limit  //页码
    };
}
(function () {
    var columns = [
        { checkbox: true },
        { field: 'id', title: 'Id', visible: false },
        { field: 'value', title: app.localize('Value') },
        { field: 'typeName', title: app.localize('TypeName') }
    ];
    //#region zTree
    var setting = {
        async: {
            enable: true,
            type: 'get',
            url: 'api/services/app/Parameter/GetAllParameterTypes'
        },
        view: {
            addHoverDom: addHoverDom,
            removeHoverDom: removeHoverDom,
            selectedMulti: false
        },
        edit: {
            enable: true,//开启编辑
            editNameSelectAll: true,//编辑时全选文本
            //showRemoveBtn: showRemoveBtn,
            //showRenameBtn: showRenameBtn
        },
        //check: {
        //    enable: true,
        //    //chkboxType: { "Y": "ps", "N": "ps" }
        //},
        data: {
            key: {
                name: 'name'
            },
            simpleData: {
                enable: true,
                idKey: 'id',
                pIdKey: 'parentId',
                rootPId: null
            }
        },
        callback: {
            beforeRemove: beforeRemove,
            beforeRename: beforeRename
        }
    };
    var log, className = "dark", treeObj = 'navTree';
    function beforeRemove(treeId, treeNode) {
        className = (className === "dark" ? "" : "dark");
        var zTree = $.fn.zTree.getZTreeObj(treeObj);
        zTree.selectNode(treeNode);
        //return confirm(abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), treeNode.name));
        return bootbox.confirm({
            size: 'small',
            title: app.localize('Delete', treeNode.name),
            message: abp.utils.formatString(abp.localization.localize('AreYouSureWantToDelete', 'Template'), treeNode.name),
            callback: function (result) {
                return result;
            }
        });
    }
    function beforeRename(treeId, treeNode, newName, isCancel) {
        className = (className === "dark" ? "" : "dark");
        if (newName.length === 0) {
            setTimeout(function () {
                var zTree = $.fn.zTree.getZTreeObj(treeObj);
                zTree.cancelEditName();
                abp.notify.error('节点名称不能为空');
            }, 0);
            return false;
        }
        return true;
    }
    var newCount = 1;
    function addHoverDom(treeId, treeNode) {
        var sObj = $("#" + treeNode.tId + "_span");
        if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) return;
        var addStr = "<span class='button add' id='addBtn_" + treeNode.tId
            + "' title='add node' onfocus='this.blur();'></span>";
        sObj.after(addStr);
        var btn = $("#addBtn_" + treeNode.tId);
        if (btn) btn.bind("click", function () {
            var zTree = $.fn.zTree.getZTreeObj(treeObj);
            zTree.addNodes(treeNode, { id: (100 + newCount), pId: treeNode.id, name: "new node" + (newCount++) });
            return false;
        });
    };
    function removeHoverDom(treeId, treeNode) {
        $("#addBtn_" + treeNode.tId).unbind().remove();
    };
    //#endregion zTree

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
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }

    $(function () {
        //1、初始化表格
        table.init('api/services/app/Parameter/GetParameters', columns);
        $.fn.zTree.init($("#" + treeObj), setting);
        $('#create').click(function () {
            createOrEdit(app.localize('CreateNewParameter'));
        });
    });
})();