using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.TinDung
{
    public class TinDUng_ReportController : Controller
    {
        //
        // GET: /PhanBo_Report/

        public string sViewPath = "~/Report_Views/TinDung/";
        public ActionResult Index()
        {
            return View(sViewPath + "Report_Index.aspx");
        }

    }
}
