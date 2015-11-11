using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.NhanSu
{
    public class NhanSu_ReportController : Controller
    {
        //
        // GET: /KeToan_Report/

        public string sViewPath = "~/Report_Views/NhanSu/";
        public ActionResult Index()
        {
            return View(sViewPath + "rptNhanSu_Index.aspx");
        }

    }
}
