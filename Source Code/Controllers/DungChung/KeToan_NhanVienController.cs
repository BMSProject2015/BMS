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

namespace VIETTEL.Controllers.DungChung
{
    public class KeToan_NhanVienController : Controller
    {
        //
        // GET: /KeToan_NhanVien/


        public string sViewPath = "~/Views/DungChung/KeToanNhanVien/";

        [Authorize]
        public ActionResult Index(int? MaNhanVien_page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("KT_NhanVien");
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                ViewData["MaNhanVien_page"] = MaNhanVien_page;
                return View(sViewPath + "KeToan_NhanVien_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string iID_MaNhanVien)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("KT_NhanVien");
                bang.GiaTriKhoa = iID_MaNhanVien;
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
        public ActionResult Delete(string iID_MaNhanVien)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("KT_NhanVien");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = iID_MaNhanVien;
                bang.Delete();
                return RedirectToAction("Index", "KeToan_NhanVien");

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaNhanVien)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["iID_MaNhanVien"] = iID_MaNhanVien;

                NameValueCollection data = new NameValueCollection();
                if (String.IsNullOrEmpty(iID_MaNhanVien) == false)
                {

                    ViewData["DuLieuMoi"] = "0";
                    data = KeToanNhanVienModels.LayThongTinNhanVien(iID_MaNhanVien);
                }
                else
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["data"] = data;
                return View(sViewPath + "KeToan_NhanVien_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("KT_NhanVien");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

                String iID_MaNhanVien = Request.Form[ParentID + "_iID_MaNhanVien"];
                iID_MaNhanVien = iID_MaNhanVien.Trim();
                String sTen = Request.Form[ParentID + "_sTen"];
                if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaNhanVien, "iID_MaNhanVien", iID_MaNhanVien,
                                         bang.DuLieuMoi))
                {
                    arrLoi.Add("err_iID_MaNhanVien", "Không được nhập trùng ký hiệu");
                }

                if (String.IsNullOrEmpty(iID_MaNhanVien))
                {
                    arrLoi.Add("err_iID_MaNhanVien", "Bạn chưa nhập mã nhân viên");
                }

                if (String.IsNullOrEmpty(sTen))
                {
                    arrLoi.Add("err_sTen", "Bạn chưa nhập tên nhân viên");
                }


                if (arrLoi.Count == 0)
                {
                    bang.GiaTriKhoa = iID_MaNhanVien;
                    bang.Save();
                    return RedirectToAction("Index", "KeToan_NhanVien");
                }
                else
                {

                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }

                    Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                    ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                    ViewData["iID_MaNhanVien"] = iID_MaNhanVien;
                    ViewData["data"] = dicData["data"];
                    return View(sViewPath + "KeToan_NhanVien_Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
