///<reference path='BaseView.ts'/>

module Views {

    export class ModalView extends BaseView {

        public $modal;

        constructor(selector, notifications: Helpers.Notifications = null) {

            this.$modal = $(selector);            
            this.notifications = notifications || this.$modal.size() && new Helpers.Notifications(this.getNotificationsContainer());

            // Call super constructor, overriding notifications instance
            super(this.notifications);
            
            this.$modal.modal({ show: false })
                .on("show", function () {
                    $("body").addClass("modal-open");
                })
                .on("hide", function () {
                    $("body").removeClass("modal-open");
                });;
        }

        private getNotificationsContainer() {
            var container = this.$modal.find(".notifications-container");
            if (container.size() == 0)
                throw "Modal must have a notifications container with class 'notifications-container'";
            return container;
        }

        show(param: any = null) {
            this.$modal.modal("show");
        }

        hide() {
            this.$modal.modal("hide");
        }
    }

    export class ModalForm extends ModalView {

        $form;

        constructor(
            modalSelector,
            formSelector = "form",
            notifications: Helpers.Notifications = null) {

            super(modalSelector, notifications);

            this.$form = $(formSelector, this.$modal);
        }

        show(param: any = null) {
            this.$form.clearForm();
            super.show(param);
        }
    }
}