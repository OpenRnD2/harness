using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace OpenRnD.Harness.IISExpress.Tests.Target
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapRoute(null, "{controller}/{action}", defaults: new { controller = "Home", action = "Index" });
        }
    }
}