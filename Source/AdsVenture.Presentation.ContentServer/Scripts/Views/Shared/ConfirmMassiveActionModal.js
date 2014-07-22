///<reference path='../ModalView.ts'/>
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Views;
(function (Views) {
    (function (Shared) {
        var ConfirmMassiveActionModal = (function (_super) {
            __extends(ConfirmMassiveActionModal, _super);
            function ConfirmMassiveActionModal(selector, templateId, dataTable, notifications) {
                if (typeof notifications === "undefined") { notifications = null; }
                _super.call(this, selector, notifications);

                this.templateId = templateId;
                this.dataTable = dataTable;
            }
            ConfirmMassiveActionModal.prototype.show = function (id) {
                if (typeof id === "undefined") { id = null; }
                this.notifications.clear();
                var items = this.dataTable.getCheckedDescriptions();
                if (!items || items.length == 0) {
                    this.notifications.showError(Resources.Error_MustSelectAnyItem);
                    return;
                }
                this.$modal.find(".confirm-description").empty().mustache(this.templateId, [items]).show();
                this.$modal.modal();
            };
            return ConfirmMassiveActionModal;
        })(Views.ModalView);
        Shared.ConfirmMassiveActionModal = ConfirmMassiveActionModal;
    })(Views.Shared || (Views.Shared = {}));
    var Shared = Views.Shared;
})(Views || (Views = {}));
//# sourceMappingURL=ConfirmMassiveActionModal.js.map
