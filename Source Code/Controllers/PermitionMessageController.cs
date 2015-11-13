using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oneres.Controllers
{
    public class PermitionMessageController : Controller
    {
        //
        // GET: /PermitionMessage/

        public string sViewPath = "~/Views/Shared/";
        public ActionResult Index()
        {
            return View(sViewPath + "PermitionMessage.aspx");
            //return View();
        }

    }
}
