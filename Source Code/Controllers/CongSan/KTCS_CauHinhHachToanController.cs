using System;
using System.IO;
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
using System.Text;

namespace VIETTEL.Controllers.CongSan
{
    public class KTCS_CauHinhHachToanController : Controller
    {
        //
        // GET: /KTCS_KyHieuHachToanChiTiet/
        public string sViewPath = "~/Views/CongSan/CauHinhHachToan/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KyHieuHachToanChiTiet", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KeToanCongSan_CauHinhHachToan_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaKyHieuHachToan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KyHieuHachToanChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaKyHieuHachToan"] = iID_MaKyHieuHachToan;
            return View(sViewPath + "KeToanCongSan_CauHinhHachToan_Edit.aspx");
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaKyHieuHachToanChiTiet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KyHieuHachToanChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaKyHieuHachToanChiTiet))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaKyHieuHachToanChiTiet"] = iID_MaKyHieuHachToanChiTiet;
            return View(sViewPath + "KeToanCongSan_CauHinhHachToan_Edit.aspx");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaKyHieuHachToanChiTiet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KyHieuHachToanChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("KTCS_KyHieuHachToanChiTiet");
            bang.IPSua = Request.UserHostAddress;
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.GiaTriKhoa = iID_MaKyHieuHachToanChiTiet;
            bang.Delete();            
            return View(sViewPath + "KeToanCongSan_CauHinhHachToan_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaKyHieuHachToanChiTiet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KyHieuHachToanChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            NameValueCollection arrLoi = new NameValueCollection();
          
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            Boolean bDuLieuMoi=false;
            if(DuLieuMoi=="1")
                bDuLieuMoi=true;

            
            



            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaKyHieuHachToanChiTiet))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaKyHieuHachToanChiTiet"] = iID_MaKyHieuHachToanChiTiet;
                return View(sViewPath + "KeToanCongSan_CauHinhHachToan_Edit.aspx");
            }
            else
            {
               
                    Bang bang = new Bang("KTCS_KyHieuHachToanChiTiet");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    bang.TruongKhoaKieuSo = true;
                    bang.Save();
               
                
                return RedirectToAction("Index");
            }
        }
      

        /// <summary>
        /// Tìm kiếm loại tài khoản
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String sTaiKhoan = Request.Form[ParentID + "_sTaiKhoan"];
            String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
            return RedirectToAction("Index", "KTCS_LoaiTaiKhoan", new { ParentID = ParentID, Ten = sTaiKhoan, KyHieu = sKyHieu });
        }

        //public Boolean CheckMaTaiKhoan(String iID_MaKyHieuHachToanChiTiet)
        //{
        //    Boolean vR = false;
        //    DataTable dt = KTCS_KyHieuHachToanChiTietModels.getChiTietTK(iID_MaKyHieuHachToanChiTiet);
        //    if (dt.Rows.Count > 0)
        //    {
        //        vR = true;
        //    }
        //    return vR;
        //}
    }
}
