///<reference path='../Includes.ts'/>

module Helpers {

    export class Notifications {

        static LEVEL = {
            INFO: "info",
            SUCCESS: "success",
            ERROR: "danger",
            WARNING: "warning"
        }

        static delayMsecs = 10000;

        private $container;

        constructor(container = "#notifications") {
            this.$container = $(container);
        }

        clear(id = null) {
            var elems = this.$container.find(".alert");
            if (id != null)
                elems = elems.filter("[id='" + id + "']");
            elems.remove();
            return this;
        }

        show(data, level, id = null) {
            var $elem = $("<div>");

            if (typeof data == "string")
                data = { message: data };

            $elem.mustache(
                data.template || 'notifications-template',
                $.extend({ level: level, notificationId: id }, data)
            );
            $elem = $elem.children().appendTo(this.$container);

            if ($elem.isScreenVisible(50) == false)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return this;
        }

        showInfo(data, id = null) {
            return this.show(data, Notifications.LEVEL.INFO, id);
        }

        showSuccess(data, id = null) {
            return this.show(data, Notifications.LEVEL.SUCCESS, id);
        }

        showError(data, id = null) {
            return this.show(data, Notifications.LEVEL.ERROR, id);
        }

        showWarning(data, id = null) {
            return this.show(data, Notifications.LEVEL.WARNING, id);
        }

        exists(id) {
            return this.$container.find(".alert[id='" + id + "']").size() > 0;
        }
    }

}