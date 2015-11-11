using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanChiNganSachQuocPhongDoanhNghiepController : Controller
    {
        //
        // GET: /DuToanChiNganSachSuDung/
        public string sViewPath = "~/Report_Views/DuToan/";
        public ActionResult Index()
        {
            return View(sViewPath + "rptDuToanChiNganSachQuocPhongDoanhNghiep.aspx");
        }

    }
}
