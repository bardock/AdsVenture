///<reference path='../BaseView.ts'/>
///<reference path='../ModalView.ts'/>

module Views.Advertisers {

    export class Index extends BaseView {

        dataTable;
       
        constructor(dataUrl: string) {
            super();

            this.dataTable= DataTables.init("#datatable", {
                url: dataUrl,
                html: true,
                pageLength: 25
            });
        }
	}
} 