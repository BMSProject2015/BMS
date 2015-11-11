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
    public class KTCS_LoaiTaiSanController : Controller
    {
        //
        // GET: /KTCS_LoaiTaiSan/
        public string sViewPath = "~/Views/CongSan/LoaiTaiSan/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KeToanCongSan_LoaiTaiSan_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", "KTCS_LoaiTaiSan");
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_Ma)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_Ma))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_Ma"] = iID_Ma;
            return View(sViewPath + "KeToanCongSan_LoaiTaiSan_Edit.aspx");
        }
        [Authorize]
        public ActionResult Delete(String iID_Ma)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("KTCS_LoaiTaiSan");
            bang.IPSua = Request.UserHostAddress;
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.GiaTriKhoa = iID_Ma;
            if (HamChung.Checked_Delete("KTCS_NhomTaiSan", "iID_MaLoaiTaiSan", iID_Ma))
            {
                bang.Delete();
            }
            return View(sViewPath + "KeToanCongSan_LoaiTaiSan_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaLoaiTaiSan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_LoaiTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            NameValueCollection arrLoi = new NameValueCollection();
            String siID_MaLoaiTaiSan = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiTaiSan"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            Boolean bDuLieuMoi=false;
            if(DuLieuMoi=="1")
                bDuLieuMoi=true;

            if (siID_MaLoaiTaiSan == string.Empty || siID_MaLoaiTaiSan == "")
            {
                arrLoi.Add("err_iID_MaLoaiTaiSan", MessageModels.sKyHieu);
            }
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", MessageModels.sTen);
            }




            if (HamChung.Check_Trung("KTCS_LoaiTaiSan","iID_MaLoaiTaiSan",siID_MaLoaiTaiSan,"iID_MaLoaiTaiSan",siID_MaLoaiTaiSan,bDuLieuMoi))
            {
                arrLoi.Add("err_iID_MaLoaiTaiSan", "Mã loại tài sản đã tồn tại!");
            }
            
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaLoaiTaiSan"] = iID_MaLoaiTaiSan;
                return View(sViewPath + "KeToanCongSan_LoaiTaiSan_Edit.aspx");
            }
            else
            {
               
                    Bang bang = new Bang("KTCS_LoaiTaiSan");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);

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

        //public Boolean CheckMaTaiKhoan(String iID_MaLoaiTaiSan)
        //{
        //    Boolean vR = false;
        //    DataTable dt = KTCS_LoaiTaiSanModels.getChiTietTK(iID_MaLoaiTaiSan);
        //    if (dt.Rows.Count > 0)
        //    {
        //        vR = true;
        //    }
        //    return vR;
        //}
    }
}
