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
    public class KeToan_NgoaiTeController : Controller
    {
        //
        // GET: /KeToan_NhanVien/


        public string sViewPath = "~/Views/KeToanChiTiet/KhoBac/DanhMucNgoaiTe/";

        [Authorize]
        public ActionResult Index(int? MaNhanVien_page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("KTKB_NgoaiTe");
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                ViewData["MaNhanVien_page"] = MaNhanVien_page;
                return View(sViewPath + "KeToan_NgoaiTe_Index.aspx");
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
                Bang bang = new Bang("KTKB_NgoaiTe");
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
                Bang bang = new Bang("KTKB_NgoaiTe");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = iID_MaNhanVien;
                bang.Delete();
                return RedirectToAction("Index", "KeToan_NgoaiTe");

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
                    data = KeToanNgoaiTeModels.LayThongTinNhanVien(iID_MaNhanVien);
                }
                else
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["data"] = data;
                return View(sViewPath + "KeToan_NgoaiTe_Edit.aspx");
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
                Bang bang = new Bang("KTKB_NgoaiTe");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

                String iID_MaNhanVien = Request.Form[ParentID + "_iID_MaNgoaiTe"];
                iID_MaNhanVien = iID_MaNhanVien.Trim();
                String sTen = Request.Form[ParentID + "_sTen"];
                String rTyGia = Request.Form[ParentID + "_rTyGia"];
                if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaNhanVien, "iID_MaNgoaiTe", iID_MaNhanVien,
                                         bang.DuLieuMoi))
                {
                    arrLoi.Add("err_iID_MaNhanVien", "Không được nhập trùng ký hiệu");
                }

                if (String.IsNullOrEmpty(iID_MaNhanVien))
                {
                    arrLoi.Add("err_iID_MaNhanVien", "Bạn chưa nhập mã ngoại tệ");
                }

                if (String.IsNullOrEmpty(sTen))
                {
                    arrLoi.Add("err_sTen", "Bạn chưa nhập tên ngoại tệ");
                }
                if (String.IsNullOrEmpty(rTyGia))
                {
                    arrLoi.Add("err_rTyGia", "Bạn chưa nhập tỷ giá");
                }

                if (arrLoi.Count == 0)
                {
                  
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    if (!String.IsNullOrEmpty(iID_MaNhanVien))
                    {
                        String MaND = User.Identity.Name;
                        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

                        int iNam = DateTime.Now.Year;
                        int iThang = DateTime.Now.Month;
                        if (dtCauHinh.Rows.Count > 0 && dtCauHinh!=null)
                        {
                            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
                            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
                        }
                        if (dtCauHinh!=null)
                        {
                            dtCauHinh.Dispose();
                        }
                    
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNgoaiTe", iID_MaNhanVien);
                        bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
                        bang.CmdParams.Parameters.AddWithValue("@iThangLamViec", iThang);
                    }
                    else
                    {
                        bang.GiaTriKhoa = iID_MaNhanVien; 
                    }

                    bang.Save();
                    return RedirectToAction("Index", "KeToan_NgoaiTe");
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
                    return View(sViewPath + "KeToan_NgoaiTe_Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
