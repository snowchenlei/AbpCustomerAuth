
(function() {
    var setting = {
        check: {
            enable: true,
            //chkboxType: { "Y": "ps", "N": "ps" }
        },
        data: {
            key: {
                name: 'displayName',
                checked: 'isSelected'
            },
            simpleData: {
                enable: true,
                idKey: 'name',
                pIdKey: 'parentName',
                rootPId: null
            }
        }
        //async: {
        //    enable: true,
        //    url: "../asyncData/getNodes.php",
        //    autoParam: ["id", "name=n", "level=lv"],
        //    otherParam: { "otherParam": "zTreeAsyncTest" },
        //    //dataFilter: filter
        //}
    };
    $(function () {
        $.fn.zTree.init($("#permissionTree"), setting, permissionNodes);
    });
})();