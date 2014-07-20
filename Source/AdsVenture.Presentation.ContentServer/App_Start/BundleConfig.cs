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
                        "~/Scripts/ThirdParty/jquery-{version}.js",
                        "~/Scripts/ThirdParty/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/bootstrap-theme.css",
                      "~/Content/css/site.css"));
        }
    }
}
