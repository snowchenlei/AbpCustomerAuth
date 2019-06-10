var operate = {
    //新增
    add: function () {
        createOrEdit('添加');
    },
    //编辑
    edit: function (name, id) {
        createOrEdit('修改' + name, id);
    },
    //删除
    del: function (name, id) {
        bootbox.confirm({
            size: 'small',
            title: '删除',
            message: '确定要删除"' + name + '"吗？',
            callback: function (result) {
                if (result) {
                    $.post(absoluteUrl + '/Delete',
                        { id: id },
                        function (result) {
                            requestCallBack(result,
                                function () {
                                    refreshTable();
                                });
                        });
                }
            }
        });
    }
};