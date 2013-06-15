using System.Web.Optimization;

namespace Nishkriya
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts.js")
                            .Include("~/Scripts/jquery-{version}.js")
                            .Include("~/Scripts/bootstrap.js")
                            .Include("~/Scripts/spoiler.js")
                            .Include("~/Scripts/nishkriya.js"));

            bundles.Add(new StyleBundle("~/bundles/styles.css")
                            .Include("~/Content/css/bootstrap.css")
                            .Include("~/Content/css/bootstrap-responsive.css")
                            .Include("~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}