using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DomainModel;
using VIETTEL.Models;
using System.Web.Configuration;
using System.Web.Security;

namespace VIETTEL
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
           

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN"); // đặt trang hiện tại là tiếng việt
            //Lấy xâu kết nối
            Connection.ConnectionString = WebConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            RegisterRoutes(RouteTable.Routes);
            HamRiengModels.SSODomain = WebConfigurationManager.AppSettings["SSODomain"];
            HamRiengModels.SSOTimeout = Convert.ToInt32(WebConfigurationManager.AppSettings["SSOTimeout"]);

            //String MaND = User.Identity.Name;
            //NguoiDungCauHinhModels.iNamLamViec=NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
            //Lấy số bản ghi trong một trang
            Globals.PageSize = 20;
        }

        protected void Session_Start()
        {
            Session.Timeout = 500000;
        }
    }
}