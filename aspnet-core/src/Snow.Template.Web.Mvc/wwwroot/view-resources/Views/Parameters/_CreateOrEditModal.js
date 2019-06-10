(function () {
    $(function () {
        $('#parameterType').select2({
            language: "zh-CN",// 指定语言为中文，国际化才起效
            placeholder: '请选择',
            allowClear: true,
            width: '100%'
        });
    });
})();