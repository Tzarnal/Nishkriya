using System.Web.Optimization;

namespace Nishkriya
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js"));

            //Unified scripts bundle
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/jquery-{version}.js", 
                "~/Scripts/bootstrap.js"));

            //Unified css bundle
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/site.css", 
                "~/Content/css/bootstrap.css", 
                "~/Content/css/bootstrap-responsive.css"));
        }
    }
}