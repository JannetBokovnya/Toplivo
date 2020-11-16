using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Toplivo.Web.Common;
using Toplivo.Web.DAL;

namespace Toplivo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.Initialise();

            //Инициализация БД путем выполнения кода в классе инициализатора с использование методов EF
            Database.SetInitializer(new ToplivoDbInitializer());

            using (var db = new ToplivoContext())
            {
                db.Database.Initialize(true);
            }
        }
    }
}
