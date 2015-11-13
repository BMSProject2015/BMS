using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.IO;
using System.Data.OleDb;
namespace VIETTEL.Controllers.DuToan
{
    public class DuToan_ChungTu_ChiTapTrungController : Controller
    {
        //
        // GET: /ChungTu/
        public string sViewPath = "~/Views/DuToan/ChiTapTrung/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "ChungTu_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iLoai, String iID_MaChungTu)
        {
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]; 
            return RedirectToAction("Index", "DuToan_ChungTu_ChiTapTrung", new { iLoai = iLoai, iID_MaChungTu = iID_MaChungTu, iID_MaDonVi = iID_MaDonVi, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG });
        }
        [Authorize]
        public ActionResult Edit(String ParentID,String MaND)
        {
          //  DuToan_ChungTuModels.TaoDanhSachChungTuChiTapTrung(MaND, Request.UserHostAddress);
            return RedirectToAction("InDex");
        }
    }
}
