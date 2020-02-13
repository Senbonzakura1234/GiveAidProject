using System.Web;
using System.Web.Optimization;

namespace GiveAidCharity
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/root-script").Include(
                "~/Content/DashBoardTheme/vendors/js/vendor.bundle.base.js",
                "~/Content/DashBoardTheme/vendors/chart.js/Chart.min.js",
                "~/Content/DashBoardTheme/js/off-canvas.js", "~/Content/DashBoardTheme/js/hoverable-collapse.js",
                "~/Content/DashBoardTheme/js/misc.js", "~/Content/DashBoardTheme/js/dashboard.js",
                "~/Content/DashBoardTheme/js/todolist.js"));
        }
    }
}
