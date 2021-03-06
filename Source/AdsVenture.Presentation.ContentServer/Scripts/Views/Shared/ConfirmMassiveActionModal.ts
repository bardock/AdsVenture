///<reference path='../ModalView.ts'/>

module Views.Shared {

    export class ConfirmMassiveActionModal extends ModalView {

        dataTable;
        templateId: string;

        constructor(selector, templateId: string,
                    dataTable, notifications: Helpers.Notifications = null) {
            super(selector, notifications);

            this.templateId = templateId;
            this.dataTable = dataTable;
        }

        show(id: any = null) {
            this.notifications.clear();
            var items = this.dataTable.getCheckedDescriptions();
            if (!items || items.length == 0) {
                this.notifications.showError(Resources.Error_MustSelectAnyItem);
                return;
            }
            this.$modal.find(".confirm-description")
                .empty()
                .mustache(this.templateId, items)
                .show();
            this.$modal.modal();
        }
    }
}