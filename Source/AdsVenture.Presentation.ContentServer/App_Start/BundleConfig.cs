using System.Web;
using System.Web.Optimization;

namespace AdsVenture.Presentation.ContentServer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/ThirdParty/mustache.js",
                "~/Scripts/ThirdParty/jquery-{version}.js",
                "~/Scripts/ThirdParty/bootstrap.js",
                "~/Scripts/ThirdParty/history.js",
                "~/Scripts/ThirdParty/jquery.globalize.js",
                "~/Scripts/ThirdParty/jquery.globalize.culture.es.js",
                "~/Scripts/ThirdParty/jquery.mustache.js",
                "~/Scripts/ThirdParty/jquery.numeric.js",
                "~/Scripts/ThirdParty/jquery.select2.js",
                "~/Scripts/ThirdParty/jquery.validate.js",
                "~/Scripts/ThirdParty/jquery.validate.unobtrusive.js",
                "~/Scripts/ThirdParty/jquery.dataTables/jquery.dataTables.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.filteringDelay.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.bootstrap.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.reloadAjax.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.sorting.numbersWithHtml.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.typeDetection.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.sorting.dataValue.js",
                "~/Scripts/ThirdParty/jquery.dataTables/plugin.sorting.globalize.js",
                "~/Scripts/Plugins/fileicon.js",
                "~/Scripts/Plugins/fileinput.js",
                "~/Scripts/Plugins/fileupload.js",
                "~/Scripts/Plugins/jquery.clearForm.js",
                "~/Scripts/Plugins/jquery.disable.js",
                "~/Scripts/Plugins/jquery.focusForm.js",
                "~/Scripts/Plugins/jquery.focusForm.redirect.js",
                "~/Scripts/Plugins/jquery.focusForm.select2.js",
                "~/Scripts/Plugins/jquery.isScreenVisible.js",
                "~/Scripts/Plugins/loading.js",
                "~/Scripts/Plugins/serialize-object.js",
                "~/Scripts/Plugins/thousandsSeparator.js",
                "~/Scripts/Extensions/jquery.numeric.js",
                "~/Scripts/Extensions/jquery.validate.globalize.js",
                "~/Scripts/Extensions/jquery.validate.unobtrusive.js",
                "~/Scripts/Helpers/DataTables.js",
                "~/Scripts/Helpers/Event.js",
                "~/Scripts/Helpers/mimetype.js",
                "~/Scripts/Helpers/Notifications.js",
                "~/Scripts/Helpers/Numbers.js",
                "~/Scripts/Helpers/Api.js",
                "~/Scripts/App.js",
                "~/Scripts/Views/BaseView.js",
                "~/Scripts/Views/ModalView.js",
                "~/Scripts/Views/Shared/ConfirmMassiveActionModal.js",
                "~/Scripts/Views/Shared/ChainedCheckboxList.js",
                "~/Scripts/Views/Publishers/Index.js",
                "~/Scripts/Views/Advertisers/Index.js",
                "~/Scripts/Views/Contents/Index.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/bootstrap-theme.css",
                "~/Content/css/dataTables.bootstrap.css",
                "~/Content/css/site.css"
            ));
        }
    }
}
