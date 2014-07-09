///<reference path='BaseView.ts'/>
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Views;
(function (Views) {
    var ModalView = (function (_super) {
        __extends(ModalView, _super);
        function ModalView(selector, notifications) {
            if (typeof notifications === "undefined") { notifications = null; }
            this.$modal = $(selector);
            this.notifications = notifications || this.$modal.size() && new Helpers.Notifications(this.getNotificationsContainer());

            // Call super constructor, overriding notifications instance
            _super.call(this, this.notifications);

            this.$modal.modal({ show: false }).on("show", function () {
                $("body").addClass("modal-open");
            }).on("hide", function () {
                $("body").removeClass("modal-open");
            });
            ;
        }
        ModalView.prototype.getNotificationsContainer = function () {
            var container = this.$modal.find(".notifications-container");
            if (container.size() == 0)
                throw "Modal must have a notifications container with class 'notifications-container'";
            return container;
        };

        ModalView.prototype.show = function (param) {
            if (typeof param === "undefined") { param = null; }
            this.$modal.modal("show");
        };

        ModalView.prototype.hide = function () {
            this.$modal.modal("hide");
        };
        return ModalView;
    })(Views.BaseView);
    Views.ModalView = ModalView;

    var ModalForm = (function (_super) {
        __extends(ModalForm, _super);
        function ModalForm(modalSelector, formSelector, notifications) {
            if (typeof formSelector === "undefined") { formSelector = "form"; }
            if (typeof notifications === "undefined") { notifications = null; }
            _super.call(this, modalSelector, notifications);

            this.$form = $(formSelector, this.$modal);
        }
        ModalForm.prototype.show = function (param) {
            if (typeof param === "undefined") { param = null; }
            this.$form.clearForm();
            _super.prototype.show.call(this, param);
        };
        return ModalForm;
    })(ModalView);
    Views.ModalForm = ModalForm;
})(Views || (Views = {}));
//# sourceMappingURL=ModalView.js.map
