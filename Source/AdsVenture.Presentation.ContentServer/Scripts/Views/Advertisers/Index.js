///<reference path='../BaseView.ts'/>
///<reference path='../ModalView.ts'/>
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Views;
(function (Views) {
    (function (Advertisers) {
        var Index = (function (_super) {
            __extends(Index, _super);
            function Index(dataUrl) {
                _super.call(this);

                this.dataTable = DataTables.init("#datatable", {
                    url: dataUrl,
                    html: true,
                    pageLength: 25
                });
                this.delete = new Views.Shared.ConfirmMassiveActionModal("#modal_deleteMassive", "delete-items-template", this.dataTable, this.notifications);
            }
            Index.prototype.deleteMassive = function () {
                var _this = this;
                this.api.delete("Advertisers", {
                    action: "DeleteMassive",
                    data: { IDs: this.dataTable.getCheckedIds() },
                    success: function () {
                        _this.notifications.clear().showSuccess(Resources.Success_Delete);
                        _this.dataTable.fnReloadAjax(null, null, true);
                    }
                });
            };
            return Index;
        })(Views.BaseView);
        Advertisers.Index = Index;
    })(Views.Advertisers || (Views.Advertisers = {}));
    var Advertisers = Views.Advertisers;
})(Views || (Views = {}));
//# sourceMappingURL=Index.js.map
