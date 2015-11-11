using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToanChiNganSachQuocPhongController : Controller
    {
        //
        // GET: /rptDuToanChiNganSachQuocPhong/
        public string sViewPath = "~/Report_Views/DuToan/";
        public ActionResult Index()
        {
            return View(sViewPath + "rptDuToanChiNgansachQuocPhong.aspx");
        }
        public ActionResult EditSubmit(String ParentID,String sAction)
        {
            String NamLamViec = Request.Form[ParentID + "_iNamLamViec"];
            return RedirectToAction(sAction, new { NamLamViec = NamLamViec });
        }
        public ActionResult DanhSach()
        {
            return View(sViewPath + "ReportList.aspx");
        }

        public ActionResult XDCB()
        {
            return View(sViewPath + "rptChiNganSachQuocPhong_XDCB.aspx");
        }

        public ActionResult Print()
        {
            return View(sViewPath + "FromPrint.aspx");
        }

    }
}
