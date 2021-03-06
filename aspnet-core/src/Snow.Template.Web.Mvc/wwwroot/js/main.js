﻿(function ($) {
    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        abp.notifications.showUiNotifyForUserNotification(userNotification);

        //Desktop notification
        Push.create("Template", {
            body: userNotification.notification.data.message,
            icon: abp.appPath + 'images/app-logo-small.png',
            timeout: 6000,
            onClick: function () {
                window.focus();
                this.close();
            }
        });
    });

    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });
        //map to object
        var obj = {};
        //data.map(function (x) { obj[x.name] = x.value; });
        data.map(function (x) {
            let key = x.name.split('.').pop();
            if (obj[key] !== undefined) {
                //避免微软mvc生成checkbox多了个hidden
                return;
                //obj[key] = obj[key] + ',' + x.value;
            } else {
                obj[key] = x.value;
            }
        });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }
})(jQuery);
