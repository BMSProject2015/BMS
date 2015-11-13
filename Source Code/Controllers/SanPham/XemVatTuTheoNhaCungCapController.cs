using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VIETTEL.Controllers
{
    public class XemVatTuTheoNhaCungCapController : Controller
    {
        //
        // GET: /XemVatTuTheoNhaCungCap/
        public string sViewPath = "~/Views/SanPham/XemVatTuTheoNhaCungCap/";
        public ActionResult Index(int? XemVatTuTheoNhaCungCap_page, String iID_MaNhaCungCap)
        {
            ViewData["XemVatTuTheoNhaCungCap_page"] = XemVatTuTheoNhaCungCap_page;
            ViewData["iID_MaNhaCungCap"] = iID_MaNhaCungCap;
            return View(sViewPath + "Index.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            String iID_MaNhaCungCap = Request.Form[ParentID + "_iID_MaNhaCungCap"];
            return RedirectToAction("Index", new { iID_MaNhaCungCap = iID_MaNhaCungCap });
        }

    }
}
