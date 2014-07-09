///<reference path='../Includes.ts'/>
var Helpers;
(function (Helpers) {
    var Notifications = (function () {
        function Notifications(container) {
            if (typeof container === "undefined") { container = "#notifications"; }
            this.$container = $(container);
        }
        Notifications.prototype.clear = function (id) {
            if (typeof id === "undefined") { id = null; }
            var elems = this.$container.find(".alert");
            if (id != null)
                elems = elems.filter("[id='" + id + "']");
            elems.remove();
            return this;
        };

        Notifications.prototype.show = function (data, level, id) {
            if (typeof id === "undefined") { id = null; }
            var $elem = $("<div>");

            if (typeof data == "string")
                data = { message: data };

            $elem.mustache(data.template || 'notifications-template', $.extend({ level: level, notificationId: id }, data));
            $elem = $elem.children().appendTo(this.$container);

            if ($elem.isScreenVisible(50) == false)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return this;
        };

        Notifications.prototype.showInfo = function (data, id) {
            if (typeof id === "undefined") { id = null; }
            return this.show(data, Notifications.LEVEL.INFO, id);
        };

        Notifications.prototype.showSuccess = function (data, id) {
            if (typeof id === "undefined") { id = null; }
            return this.show(data, Notifications.LEVEL.SUCCESS, id);
        };

        Notifications.prototype.showError = function (data, id) {
            if (typeof id === "undefined") { id = null; }
            return this.show(data, Notifications.LEVEL.ERROR, id);
        };

        Notifications.prototype.showWarning = function (data, id) {
            if (typeof id === "undefined") { id = null; }
            return this.show(data, Notifications.LEVEL.WARNING, id);
        };

        Notifications.prototype.exists = function (id) {
            return this.$container.find(".alert[id='" + id + "']").size() > 0;
        };
        Notifications.LEVEL = {
            INFO: "info",
            SUCCESS: "success",
            ERROR: "danger",
            WARNING: "warning"
        };

        Notifications.delayMsecs = 10000;
        return Notifications;
    })();
    Helpers.Notifications = Notifications;
})(Helpers || (Helpers = {}));
//# sourceMappingURL=Notifications.js.map
