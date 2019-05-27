//function dateFormat(format, date) {
//    if (!date) {
//        date = new Date();
//    }
//    let Week = ['日', '一', '二', '三', '四', '五', '六'];
//    let o = {
//        "y+": date.getYear(), //year
//        "M+": date.getMonth() + 1, //month
//        "d+": date.getDate(), //day
//        "h+": date.getHours(), //hour
//        "H+": date.getHours(), //hour
//        "m+": date.getMinutes(), //minute
//        "s+": date.getSeconds(), //second
//        "q+": Math.floor((date.getMonth() + 3) / 3), //quarter
//        "S": date.getMilliseconds(), //millisecond
//        "w": Week[date.getDay()]
//    };
//    if (/(y+)/.test(format)) {
//        format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
//    }
//    for (var k in o) {
//        if (o.hasOwnProperty(k)) {
//            if (new RegExp("(" + k + ")").test(format)) {
//                format = format.replace(RegExp.$1,
//                    RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
//            }
//        }
//    }
//    return format;
//}

//function addDay(currentDate, day) {
//    return new Date(currentDate.getTime() + day * 24 * 60 * 60 * 1000);
//}
(function () {
    Date.prototype.formater = function (format) {
        var date = this;
        let Week = ['日', '一', '二', '三', '四', '五', '六'];
        let o = {
            "y+": date.getYear(), //year
            "M+": date.getMonth() + 1, //month
            "d+": date.getDate(), //day
            "h+": date.getHours(), //hour
            "H+": date.getHours(), //hour
            "m+": date.getMinutes(), //minute
            "s+": date.getSeconds(), //second
            "q+": Math.floor((date.getMonth() + 3) / 3), //quarter
            "S": date.getMilliseconds(), //millisecond
            "w": Week[date.getDay()]
        };
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (o.hasOwnProperty(k)) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1,
                        RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
        }
        return format;
    };
    Date.prototype.addDay = function (day) {
        return new Date(this.getTime() + day * 24 * 60 * 60 * 1000);
    };
})();