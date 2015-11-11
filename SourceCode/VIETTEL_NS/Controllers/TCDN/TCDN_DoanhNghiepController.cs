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

namespace VIETTEL.Controllers.TCDN
{
    public class TCDN_DoanhNghiepController : Controller
    {
        //
        // GET: /TCDN_DoanhNghiep/
        public string sViewPath = "~/Views/TCDN/DoanhNghiep/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "TCDN_DoanhNghiep_Index.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaDoanhNghiep)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaDoanhNghiep) && LuongCongViecModel.NguoiDung_DuocThemChungTu(ThuNopModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "TCDN_DoanhNghiep", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaDoanhNghiep))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
            return View(sViewPath + "TCDN_DoanhNghiep_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaDoanhNghiep)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            //if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(ThuNopModels.iID_MaPhanHe, MaND) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("TCDN_DoanhNghiep");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String sTenDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_sTenDoanhNghiep"]);
            String sTenThuongGoi = Convert.ToString(Request.Form[ParentID + "_sTenThuongGoi"]);
            String sTenGiaoDich = Convert.ToString(Request.Form[ParentID + "_sTenGiaoDich"]);
            String sTenTheoQuocPhong = Convert.ToString(Request.Form[ParentID + "_sTenTheoQuocPhong"]);
            String iID_MaLoaiDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiDoanhNghiep"]);
            String iID_MaNhomDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomDoanhNghiep"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị quản lý!");
            }
            if (sTenDoanhNghiep == string.Empty || sTenDoanhNghiep == "" || sTenDoanhNghiep == null)
            {
                arrLoi.Add("err_sTenDoanhNghiep", "Bạn chưa nhập tên doanh nghiệp!");
            }
            if (sTenThuongGoi == string.Empty || sTenThuongGoi == "" || sTenThuongGoi == null)
            {
                arrLoi.Add("err_sTenThuongGoi", "Bạn chưa nhập tên thường gọi doanh nghiệp!");
            }
            if (sTenGiaoDich == string.Empty || sTenGiaoDich == "" || sTenGiaoDich == null)
            {
                arrLoi.Add("err_sTenGiaoDich", "Bạn chưa nhập tên giao dịch doanh nghiệp!");
            }
            if (sTenTheoQuocPhong == string.Empty || sTenTheoQuocPhong == "" || sTenTheoQuocPhong == null)
            {
                arrLoi.Add("err_sTenTheoQuocPhong", "Bạn chưa nhập tên theo quốc phòng doanh nghiệp!");
            }
            if (iID_MaLoaiDoanhNghiep == Guid.Empty.ToString()  ||iID_MaLoaiDoanhNghiep == string.Empty || iID_MaLoaiDoanhNghiep == "" || iID_MaLoaiDoanhNghiep == null)
            {
                arrLoi.Add("err_iID_MaLoaiDoanhNghiep", "Bạn chưa chọn loại hình doanh nghiệp!");
            }
            if (iID_MaNhomDoanhNghiep == Guid.Empty.ToString() || iID_MaNhomDoanhNghiep == string.Empty || iID_MaNhomDoanhNghiep == "" || iID_MaNhomDoanhNghiep == null)
            {
                arrLoi.Add("err_iID_MaNhomDoanhNghiep", "Bạn chưa chọn nhóm doanh nghiệp!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaDoanhNghiep))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
                return View(sViewPath + "TCDN_DoanhNghiep_Edit.aspx");
            }
            else
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {}
                else
                {
                    bang.GiaTriKhoa = iID_MaDoanhNghiep;
                }
                bang.Save();
            }
            return RedirectToAction("Index", "TCDN_DoanhNghiep");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
           String  sTenDoanhNghiep = Request.Form[ParentID+ "_sTenDoanhNghiep"];
           String sTenThuongGoi = Request.Form[ParentID + "_sTenThuongGoi"];
           String sTenGiaoDich = Request.Form[ParentID + "_sTenGiaoDich"];
           String sTenTheoQuocPhong = Request.Form[ParentID + "_sTenTheoQuocPhong"];
           String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
           String iNamLamViec = Request.Form[ParentID + "_iNamLamViec"];
           String iID_MaLoaiDoanhNghiep = Request.Form[ParentID + "_iID_MaLoaiDoanhNghiep"];
           String iID_MaNhomDoanhNghiep = Request.Form[ParentID + "_iID_MaNhomDoanhNghiep"];
            return RedirectToAction("Index", "TCDN_DoanhNghiep", new
            {
                sTenDoanhNghiep = sTenDoanhNghiep,
                sTenThuongGoi = sTenThuongGoi,
                sTenGiaoDich = sTenGiaoDich,
                sTenTheoQuocPhong = sTenTheoQuocPhong,
                iID_MaDonVi = iID_MaDonVi,
                iNamLamViec = iNamLamViec,
                iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep,
                iID_MaNhomDoanhNghiep = iID_MaNhomDoanhNghiep
            });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaDoanhNghiep)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_DoanhNghiep", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = TCSN_DoanhNghiepModels.Delete(iID_MaDoanhNghiep);
            return RedirectToAction("Index", "TCDN_DoanhNghiep");
        }

        [Authorize]
        public ActionResult Duyet(String iLoai)
        {
            return RedirectToAction("Index", "TCDN_DoanhNghiep");
        }
    }
}
