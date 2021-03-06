﻿/**
 * 日历
 * @param obj eles 日期输入框
 * @param boolean dobubble 是否为双日期（true）
 * @param boolean secondNot 有无时分秒（有则true）
 * @return none
 */
function setDate(eles, dobubble, secondNot) {
    let singleNot, formatDate, pickerFormatDate;
    singleNot = dobubble ? false : true;
    if (secondNot) {
        pickerFormatDate = "YYYY-MM-DD HH:mm:ss";
        formatDate = "yyyy-MM-dd HH:mm:ss";
    } else {
        pickerFormatDate = "YYYY-MM-DD";
        formatDate = "yyyy-MM-dd";
    }
    eles.daterangepicker({
        "singleDatePicker": singleNot,//是否为单日期
        //"showDropdowns": true,
        "startDate": new Date().addDay(-1).formater(formatDate),
        "endDate": new Date().formater(formatDate),
        //"autoUpdateInput": false,   //是否初始化空值
        "autoApply": true,
        "timePicker24Hour": true,   //24小时
        "timePickerSeconds": true,  //显示S
        "timePicker": true,         //显示时间
        "opens": "left",
        "locale": {
            "format": pickerFormatDate,
            "separator": " - ",
            "applyLabel": "确定",
            "cancelLabel": "清除",
            "fromLabel": "起始时间",
            "toLabel": "结束时间'",
            "customRangeLabel": "自定义",
            "weekLabel": "W",
            "daysOfWeek": ["日", "一", "二", "三", "四", "五", "六"],
            "monthNames": ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            "firstDay": 1
        },
    }, function (start, end, label) {
        if (secondNot === true && dobubble === true) {
            $(eles).val($.trim(start.format('YYYY-MM-DD HH:mm:ss') + '~' + end.format('YYYY-MM-DD HH:mm:ss')));
        } else if (secondNot === false && dobubble === true) {
            $(eles).val($.trim(start.format('YYYY-MM-DD') + '~' + end.format('YYYY-MM-DD')));
        } else if (secondNot === false && dobubble === false) {
            $(eles).val(start.format('YYYY-MM-DD'));
        } else if (secondNot === true && dobubble === false) {
            $(eles).val(start.format('YYYY-MM-DD HH:mm:ss'));
        }
    }).on('cancel.daterangepicker', function (ev, picker) {
        //清空值
        $(this).val('');
    });;
}