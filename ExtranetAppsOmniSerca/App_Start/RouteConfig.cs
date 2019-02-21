using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExtranetAppsOmniSerca
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Medicos", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CuentaCorriente",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CuentaCorriente", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Facturacion",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Facturacion", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "OperativaClientes",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "OperativaClientes", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Coordinacion",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Coordinacion", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Liquidaciones",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Liquidaciones", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EmpleadosLiquidaciones",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EmpleadosLiquidaciones", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Afiliaciones",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Afiliaciones", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AfiliacionesPopUp",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AfiliacionesPopUp", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}