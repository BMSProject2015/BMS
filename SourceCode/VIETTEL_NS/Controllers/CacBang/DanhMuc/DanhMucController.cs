using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using DomainModel.Abstract;
using System.Data.SqlClient;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace Oneres.Controllers.DanhMuc
{
    public class DanhMucController : Controller
    {
        public string sViewPath = "~/Views/CacBang/DanhMuc/DanhMuc/";

        [Authorize]
        public ActionResult Index(string MaLoaiDanhMuc, int? DanhMuc_page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_DanhMuc");
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData["DanhMuc_page"] = DanhMuc_page;
                dicData["iID_MaLoaiDanhMuc"] = MaLoaiDanhMuc;
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string MaDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.GiaTriKhoa = MaDanhMuc;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
                if (dicData != null)
                {
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Detail.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(string MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_DanhMuc");
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection) dicData["data"];
                dicData["iID_MaLoaiDanhMuc"] = MaLoaiDanhMuc;
                dicData["LoaiDanhMuc"] =
                    (string) (CommonFunction.LayTruong("DC_LoaiDanhMuc", "iID_MaLoaiDanhMuc", MaLoaiDanhMuc, "sTen"));

                dicData["DuLieuMoi"] = "1";
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string MaDanhMuc, string MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = MaDanhMuc;
                bang.Delete();
                return RedirectToAction("Detail", "LoaiDanhMuc", new {MaLoaiDanhMuc = MaLoaiDanhMuc});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string MaDanhMuc, string MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.GiaTriKhoa = MaDanhMuc;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection) dicData["data"];
                dicData["iID_MaLoaiDanhMuc"] = MaLoaiDanhMuc;
                dicData["LoaiDanhMuc"] =
                    (string) (CommonFunction.LayTruong("DC_LoaiDanhMuc", "iID_MaLoaiDanhMuc", MaLoaiDanhMuc, "sTen"));
                if (dicData != null)
                {
                    dicData["DuLieuMoi"] = "0";
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID, String MaLoaiDanhMuc)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    bang.Save();
                    return RedirectToAction("Detail", "LoaiDanhMuc", new {MaLoaiDanhMuc = MaLoaiDanhMuc});
                }
                else
                {
                    Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                    dicData["DuLieuMoi"] = "1";
                    dicData["iID_MaLoaiDanhMuc"] = MaLoaiDanhMuc;
                    dicData["LoaiDanhMuc"] =
                        (string)
                        (CommonFunction.LayTruong("DC_LoaiDanhMuc", "iID_MaLoaiDanhMuc", MaLoaiDanhMuc, "sTen"));
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
