using System.Web;
using System.Web.Optimization;

namespace VillaMouzinho.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Backoffice
            bundles.Add(new StyleBundle("~/Content/backoffice-css").Include(
                    "~/Content/Backoffice/bootstrap.min.css",
                    "~/Content/Backoffice/default-css.css",
                    "~/Content/Backoffice/font-awesome.min.css",
                    "~/Content/Backoffice/metisMenu.css",
                    "~/Content/Backoffice/owl.carousel.min.css",
                    "~/Content/Backoffice/responsive.css",
                    "~/Content/Backoffice/slicknav.min.css",
                    "~/Content/Backoffice/styles.css",
                    "~/Content/Backoffice/themify-icons.css",
                    "~/Content/Backoffice/typography.css"));

            bundles.Add(new ScriptBundle("~/Scripts/backoffice-js").Include(
                  "~/Scripts/Backoffice/vendor/jquery-2.2.4.min.js",
                  "~/Scripts/Backoffice/popper.min.js",
                  "~/Scripts/Backoffice/bootstrap.min.js",
                  "~/Scripts/Backoffice/owl.carousel.min.js",
                  "~/Scripts/Backoffice/metisMenu.min.js",
                  "~/Scripts/Backoffice/jquery.slimscroll.min.js",
                  "~/Scripts/Backoffice/jquery.slicknav.min.js",
                  "~/Scripts/Backoffice/datatable.js",
                  "~/Scripts/Backoffice/plugins.js",
                  "~/Scripts/Backoffice/scripts.js"));
        }
    }
}
