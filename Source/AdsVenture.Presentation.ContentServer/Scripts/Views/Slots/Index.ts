///<reference path='../BaseView.ts'/>
///<reference path='../ModalView.ts'/>

module Views.Slots {

    export class Index extends BaseView {

        dataTable;
        delete: Shared.ConfirmMassiveActionModal;
       
        constructor(dataUrl: string) {
            super();

            this.dataTable= DataTables.init("#datatable", {
                url: dataUrl,
                html: true,
                pageLength: 25
            });
            this.delete = new Shared.ConfirmMassiveActionModal(
                "#modal_deleteMassive",
                "delete-items-template",
                this.dataTable,
                this.notifications
            );
        }
        deleteMassive() {
            this.api.delete("Slots", {
                action: "DeleteMassive",
                data: { IDs: this.dataTable.getCheckedIds() },
                success: () => {
                    this.notifications.clear().showSuccess(Resources.Success_Delete);
                    this.dataTable.fnReloadAjax(null, null, true);
                }
            });
        }
	}
} 