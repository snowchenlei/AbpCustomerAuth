$(window).resize(function () {
    ChangeIFrame();
});
$(function () {
    changeNav();
});
function ChangeIFrame() {
    var height = getHeight();
    $('.contentFrame').css('height', height);
}
//创建导航
function changeNav() {
    $('.mt-2 .deepNav').click(function () {
        $(this).parent('li').addClass('active');
        $(this).parents('.open').addClass('active');
        var para = {
            "id": $(this).data('id'),
            "title": $(this).data('name'),
            "close": true,
            "url": $(this).data('href'),
            "height": getHeight()
        };
        addTabs(para);
    });
}

//获取主体内容(iframe)的高度
function getHeight() {
    return document.documentElement.clientHeight
        - (51 + parseInt($($('.content-header')[0]).css('height')) + 70) - 5;
}