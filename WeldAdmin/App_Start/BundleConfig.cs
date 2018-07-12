using System.Web;
using System.Web.Optimization;

namespace SireusRR
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                                        "~/Content/Site.css",
                                        "~/Content/Custom.css",
                                        "~/Content/ColumnsOfSameHeight.css",
                                        "~/Content/Frame.css",
                                        "~/Content/bootstrap.css"));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}