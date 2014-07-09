///<reference path='../Helpers/Notifications.ts'/>
///<reference path='../Helpers/Api.ts'/>
///<reference path='../App.ts'/>

var Views;
(function (Views) {
    var BaseView = (function () {
        function BaseView(notifications) {
            if (typeof notifications === "undefined") { notifications = null; }
            if (notifications) {
                // Use specified notifications
                this.notifications = notifications;
                this.api = new Helpers.Api({
                    notifications: this.notifications
                });
            } else {
                // Use app defaults
                this.notifications = app.notifications;
                this.api = app.api;
            }
        }
        return BaseView;
    })();
    Views.BaseView = BaseView;
})(Views || (Views = {}));
//# sourceMappingURL=BaseView.js.map
