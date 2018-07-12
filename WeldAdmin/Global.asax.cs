using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SireusRR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Random GlobalRandom = new Random();
        public static bool WaitFlag = false;
        public static int RandomAlbumID = 0;
        public static int Random100000(int seed = 100000)
        {
            return GlobalRandom.Next(seed);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ReactConfig.Configure();
        }
    }
}
