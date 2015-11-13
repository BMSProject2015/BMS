using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Controllers
{
    public class LichSuGiaoDichController : Controller
    {
        //
        // GET: /LichSuGiaoDich/
        public string sViewPath = "~/Views/SanPham/LichSuGiaoDich/";
        public ActionResult Index(int? LichSuGiaoDich_page, int? TimKiemVatTu_page, String Searchid, String sMaVatTu, String sTen, String sTenGoc, String sQuyCach, String cbsMaVatTu, String cbsTen, String cbsTenGoc, String cbsQuyCach, String MaNhomLoaiVatTu, String MaNhomChinh, String MaNhomPhu, String MaChiTietVatTu, String MaXuatXu, String iTrangThai, String sMaVatTuHT, String sMaYeuCauHT)
        {
            ViewData["TimKiemVatTu_page"] = TimKiemVatTu_page;
            ViewData["Searchid"] = Searchid;
            ViewData["sMaVatTu"] = sMaVatTu;
            ViewData["sTen"] = sTen;
            ViewData["sTenGoc"] = sTenGoc;
            ViewData["sQuyCach"] = sQuyCach;
            ViewData["cbsMaVatTu"] = cbsMaVatTu;
            ViewData["cbsTen"] = cbsTen;
            ViewData["cbsTenGoc"] = cbsTenGoc;
            ViewData["cbsQuyCach"] = cbsQuyCach;

            ViewData["MaNhomLoaiVatTu"] = MaNhomLoaiVatTu;
            ViewData["MaNhomChinh"] = MaNhomChinh;
            ViewData["MaNhomPhu"] = MaNhomPhu;
            ViewData["MaChiTietVatTu"] = MaChiTietVatTu;
            ViewData["MaXuatXu"] = MaXuatXu;
            ViewData["iTrangThai"] = iTrangThai;

            ViewData["LichSuGiaoDich_page"] = LichSuGiaoDich_page;
            ViewData["sMaVatTuHT"] = sMaVatTuHT; 
            ViewData["sMaYeuCauHT"] = sMaYeuCauHT;
            return View(sViewPath + "Index.aspx");
        }

        public ActionResult Detail(String iID_MaLichSuGiaoDich)
        {
            ViewData["iID_MaLichSuGiaoDich"] = iID_MaLichSuGiaoDich;
            return View(sViewPath + "Detail.aspx");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID, int Searchid)
        {
            if (Searchid == 1)
            {
                String sMaVatTu = Request.Form[ParentID + "_sMaVatTu"];
                String sTen = Request.Form[ParentID + "_sTen"];
                String sTenGoc = Request.Form[ParentID + "_sTenGoc"];
                String sQuyCach = Request.Form[ParentID + "_sQuyCach"];
                String cbsMaVatTu = Request.Form[ParentID + "_cbsMaVatTu"];
                String cbsTen = Request.Form[ParentID + "_cbsTen"];
                String cbsTenGoc = Request.Form[ParentID + "_cbsTenGoc"];
                String cbsQuyCach = Request.Form[ParentID + "_cbsQuyCach"];

                String sMaVatTuHT = Request.Form[ParentID + "_sMaVatTuHT"];
                String sMaYeuCauHT = Request.Form[ParentID + "_sMaYeuCauHT"];

                return RedirectToAction("Index", new { Searchid = Searchid, sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach, cbsMaVatTu = cbsMaVatTu, cbsTen = cbsTen, cbsTenGoc = cbsTenGoc, cbsQuyCach = cbsQuyCach, sMaVatTuHT = sMaVatTuHT, sMaYeuCauHT = sMaYeuCauHT });
            }
            else
            {
                String iDM_MaNhomLoaiVatTu = Request.Form[ParentID + "_iDM_MaNhomLoaiVatTu"]; ;
                String iDM_MaNhomChinh = Request.Form[ParentID + "_iDM_MaNhomChinh"];
                String iDM_MaNhomPhu = Request.Form[ParentID + "_iDM_MaNhomPhu"];
                String iDM_MaChiTietVatTu = Request.Form[ParentID + "_iDM_MaChiTietVatTu"];
                String iDM_MaXuatXu = Request.Form[ParentID + "_iDM_MaXuatXu"];
                String iTrangThai = Request.Form[ParentID + "_iTrangThai"];

                String sMaVatTuHT = Request.Form[ParentID + "_sMaVatTuHT"];
                String sMaYeuCauHT = Request.Form[ParentID + "_sMaYeuCauHT"];
                return RedirectToAction("Index", new { Searchid = Searchid, MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, MaNhomChinh = iDM_MaNhomChinh, MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTu = iDM_MaChiTietVatTu, MaXuatXu = iDM_MaXuatXu, iTrangThai = iTrangThai, sMaVatTuHT = sMaVatTuHT, sMaYeuCauHT = sMaYeuCauHT });
            }
        }
    } 
}
