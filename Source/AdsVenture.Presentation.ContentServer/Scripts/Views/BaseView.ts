///<reference path='../Helpers/Notifications.ts'/>
///<reference path='../Helpers/Api.ts'/>
///<reference path='../App.ts'/>
declare var app: App;

module Views {

    export class BaseView {

        notifications: Helpers.Notifications;
        api: Helpers.Api;

        constructor(notifications: Helpers.Notifications = null) {
            if (notifications) {
                // Use specified notifications
                this.notifications = notifications;
                this.api = new Helpers.Api(<CustomApiSettings>{
                    notifications: this.notifications
                });
            } else {
                // Use app defaults
                this.notifications = app.notifications;
                this.api = app.api;
            }
        }

    }
}