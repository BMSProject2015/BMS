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

namespace VIETTEL.Controllers.Luong
{
    public class Luong_CauHinhLuongController : Controller
    {
        //
        // GET: /CauHinhLuong/

        public string sViewPath = "~/Views/Luong/CauHinhLuong/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "Luong_CauHinhLuong_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            int iNamLamViec = Convert.ToInt16(Request.Form[ParentID + "_iNamLamViec"]);
            int iThangLamViec = Convert.ToInt16(Request.Form[ParentID + "_iThangLamViec"]);
            String LoaiNhap = Request.Form[ParentID + "_LoaiNhap"];

            CauHinhLuongModels.SuaCauHinh(User.Identity.Name, new { iNamLamViec = iNamLamViec, iThangLamViec = iThangLamViec });
            switch(LoaiNhap)
            {
                case "1":
                    return RedirectToAction("Index", "Luong_BangLuong", new { iNamBangLuong = iNamLamViec, iThangBangLuong = iThangLamViec });
                break;

                case "2":
                return RedirectToAction("Index", "Luong_ThueThuNhapCaNhan", new { iNamBangLuong = iNamLamViec, iThangBangLuong = iThangLamViec });
                break;

                case "3":
                return RedirectToAction("Index", "Luong_KhauTruThue", new { iNamBangLuong = iNamLamViec, iThangBangLuong = iThangLamViec });
                break;

                default:
                return RedirectToAction("Index", "Luong_BangLuong", new { iNamBangLuong = iNamLamViec, iThangBangLuong = iThangLamViec });
                break;
            }
        }

    }
}
