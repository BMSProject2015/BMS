using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Controls;
using DomainModel.Abstract;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Data;
using VIETTEL.CacBang;
using VIETTEL.Models;

namespace VIETTEL.Controllers.DuToan
{
    public class DuToanController : Controller
    {
        //
        // GET: /DuToan/
        public string sViewPath = "~/Views/DuToan/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "DuToan_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            int iNamLamViec = Convert.ToInt16(Request.Form[ParentID + "_MaNam"]);
            int iID_MaNamNganSach = Convert.ToInt16(Request.Form[ParentID + "_iID_MaNamNganSach"]);
            int iID_MaNguonNganSach = Convert.ToInt16(Request.Form[ParentID + "_iID_MaNguonNganSach"]);
            NguoiDungCauHinhModels.SuaCauHinh(User.Identity.Name, new { iNamLamViec = iNamLamViec, iID_MaNamNganSach = iID_MaNamNganSach, iID_MaNguonNganSach = iID_MaNguonNganSach });
            return RedirectToAction("Index", "Home");
        }
    }
}
