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
    public class TCDN_HoSo_DoanhNghiepController : Controller
    {
        public string sViewPath = "~/Views/TCDN/DoanhNghiep/";
        [Authorize]
        public ActionResult Index()
        {
           
            return View(sViewPath + "TCDN_HoSo_DoanhNghiep_Index.aspx");
        }

        [Authorize]
        public ActionResult Edit(string iID_MaDoanhNghiep, String iLoai)
        {
            if (string.IsNullOrEmpty(iID_MaDoanhNghiep))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            else
            {
                ViewData["DuLieuMoi"] = "0";
            }
            ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
            ViewData["iLoai"] = iLoai;
            if(iLoai=="1")
                return View(sViewPath + "TCDN_HoSo_DoanhNghiep_Edit.aspx");
             //linh vuc hoat dong
            else if (iLoai == "2")
            {
                return View(sViewPath + "LinhVuc/TCDN_HoSo_DoanhNghiep_Edit_LinhVuc.aspx");
            }
             //Đơn vị thành viên
            else if (iLoai == "3")
            {
                return View(sViewPath + "DonViThanhVien/TCDN_HoSo_DoanhNghiep_Edit_DonViThanhVien.aspx");
            }
            //Cty lien doanh lien ket
            else if (iLoai == "4")
            {
                return View(sViewPath + "CongTyLDLK/TCDN_HoSo_DoanhNghiep_Edit_CongTyLDLK.aspx");
            }
            //Dự án đang đầu tư
            else if (iLoai == "5")
            {
                return View(sViewPath + "DuAnDangDauTu/TCDN_HoSo_DoanhNghiep_Edit_DuAnDangDauTu.aspx");
            }
            else
            {
                return View(sViewPath + "TCDN_HoSo_DoanhNghiep_Edit.aspx");
            }
        }
        [Authorize]
        public ActionResult Delete(String iID_MaDoanhNghiep)
        {
            Bang bang = new Bang("TCDN_DoanhNghiep");
            bang.GiaTriKhoa = iID_MaDoanhNghiep;
            bang.Delete();
            return RedirectToAction("Index", "TCDN_HoSo_DoanhNghiep");
        }
        /// <summary>
        /// xử lý thêm hoặc sửa hồ sơ doanh nghiệp
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="iID_MaDoanhNghiep">Mã doanh nghiệp</param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(string ParentId, string iID_MaDoanhNghiep)
        {
            string MaND = User.Identity.Name;
            string sChucNang = "Edit";

            // Kiểm tra quyền
            if (Request.Form[ParentId + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("TCDN_DoanhNghiep");

            // Kiểm tra quyền của người dùng với chứng năng
            if (!BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            NameValueCollection arrLoi = new NameValueCollection();

            string sTenDoanhNghiep = Convert.ToString(Request.Form[ParentId + "_sTenDoanhNghiep"]);
            string sTenGiaoDich = Convert.ToString(Request.Form[ParentId + "_sTenGiaoDich"]);
            string sTenVietTat = Convert.ToString(Request.Form[ParentId + "_sTenVietTat"]);
            string iID_MaLoaiHinhDoanhNghiep = Convert.ToString(Request.Form[ParentId + "_iID_MaLoaiHinhDoanhNghiep"]);
            string iID_MaHinhThucHoatDong = Convert.ToString(Request.Form[ParentId + "_iID_MaHinhThucHoatDong"]);
            string iID_MaKhoi = Convert.ToString(Request.Form[ParentId + "_iID_MaKhoi"]);
            string iID_MaNhom = Convert.ToString(Request.Form[ParentId + "_iID_MaNhom"]);

            if (string.IsNullOrEmpty(sTenDoanhNghiep))
            {
                arrLoi.Add("err_sTenDoanhNghiep", "Bạn chưa nhập tên doanh nghiệp");
            }
           
            if (arrLoi != null && arrLoi.Count > 0)
            {
                for (int i = 0; i < arrLoi.Count; i++)
                {
                    ModelState.AddModelError(ParentId + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
                return View(sViewPath + "TCDN_HoSo_DoanhNghiep_Edit.aspx");
            }
            else
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentId, Request.Form);

                if (Request.Form[ParentId + "_DuLieuMoi"] == "1")
                {
                }
                else
                {
                    bang.GiaTriKhoa = iID_MaDoanhNghiep;
                }
                bang.CmdParams.Parameters.AddWithValue("@iTrangThai", true);
                bang.Save();

                // Lưu chức danh quản lý
                var dtChucDanh = DanhMucModels.DT_DanhMuc("TCDN_ChucDanhQL", true, "");
               
                if (dtChucDanh!=null && dtChucDanh.Rows!=null && dtChucDanh.Rows.Count>0)
                {
                    for (int i = 0; i < dtChucDanh.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(HamChung.ConvertToString(dtChucDanh.Rows[i]["sTen"])))
                        {
                            string sHoTen = HamChung.ConvertToString(Request.Form[ParentId + "_sHoTen[" + i.ToString() + "]"]);
                            if (string.IsNullOrEmpty(sHoTen))
                            {
                                continue;
                            }
                            string iID_MaQuanLy = HamChung.ConvertToString(Request.Form[ParentId + "_iID_MaQuanLy[" + i.ToString() + "]"]);
                             Bang bangChucDanh = new Bang("TCDN_DN_Quanly");
                             if (!string.IsNullOrEmpty(iID_MaQuanLy))
                             {
                                 bangChucDanh.DuLieuMoi = false;
                                 bangChucDanh.GiaTriKhoa = iID_MaQuanLy;
                             }
                             else
                             {
                                 bangChucDanh.DuLieuMoi = true;
                             }
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@iTrangThai", true);
                             
                             string sCapBac = HamChung.ConvertToString(Request.Form[ParentId + "_sCapBac[" + i.ToString() + "]"]);
                             string sSoDT = HamChung.ConvertToString(Request.Form[ParentId + "_sSoDT[" + i.ToString() + "]"]);
                             string sDiDong = HamChung.ConvertToString(Request.Form[ParentId + "_sDiDong[" + i.ToString() + "]"]);
                             string iID_MaChucDanh = HamChung.ConvertToString(Request.Form[ParentId + "_iID_MaChucDanh[" + i.ToString() + "]"]);
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@sHoTen", sHoTen);
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@sCapBac", sCapBac);
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@sSoDT", sSoDT);
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@sDiDong", sDiDong);
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@iID_MaChucDanh", iID_MaChucDanh);
                             bangChucDanh.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);

                             bangChucDanh.Save();
                        }
                       
                    } 
                }
            }
            if (Request.Form[ParentId + "_DuLieuMoi"] == "1")
            {
                return RedirectToAction("Index", "TCDN_HoSo_DoanhNghiep");
            }

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 1 });
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_DuAnDangDauTu(string ParentID, string iID_MaDoanhNghiep,String iID_MaDuAn)
        {
            string MaND = User.Identity.Name;
            Bang bang = new Bang("TCDN_DuAnDauTu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu_Cha", 286);
                    //Tinh sKyHieu
                    String SQL = String.Format("SELECT MAX(iID_MaDuAn) FROM TCDN_DuAnDauTu");
                    SqlCommand cmd= new SqlCommand(SQL);
                    int iKyHieu = Convert.ToInt32(Connection.GetValue(cmd, 0))+1;
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@sKyHieu", "341"+iKyHieu);
                }
                else
                {
                    bang.GiaTriKhoa = iID_MaDuAn;
                    bang.DuLieuMoi = false;
                }
                bang.Save();
                return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep ,iLoai=5});
        }
        [Authorize]
        public ActionResult Search_DuAnDangDauTu(String ParentID, String iID_MaDoanhNghiep)
        {
            string sTen = Request.Form[ParentID + "_sTenDoanhNghiep_search"];

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 5, sTen = sTen });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_DonViThanhVien(string ParentID, string iID_MaDoanhNghiep, String iID_Ma)
        {
            string MaND = User.Identity.Name;
            Bang bang = new Bang("TCDN_DonViThanhVien");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            String iLoai = Request.Form[ParentID + "_iLoai"];
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                //Tinh sKyHieu
                String SQL = String.Format("SELECT MAX(iID_Ma) FROM TCDN_DonViThanhVien WHERE iTrangThai=1");
                SqlCommand cmd = new SqlCommand(SQL);
                int iKyHieu = Convert.ToInt32(Connection.GetValue(cmd, 0)) + 1;
                cmd.Dispose();
                bang.CmdParams.Parameters.AddWithValue("@sKyHieu", "4142" + iKyHieu);
            }
            else
            {
                bang.GiaTriKhoa = iID_Ma;
                bang.DuLieuMoi = false;
            }
            bang.Save();
            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 3 });
        }

        [Authorize]
        public ActionResult Delete_DonViThanhVien(String iID_MaDoanhNghiep, String iID_Ma)
        {

            Bang bang = new Bang("TCDN_DonViThanhVien");
                bang.GiaTriKhoa = iID_Ma;
            bang.Delete();
          
            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 3 });
        }
        [Authorize]
        public ActionResult Search_DonViThanhVien(String ParentID, String iID_MaDoanhNghiep)
        {
            string sTen = Request.Form[ParentID + "_sTenCongTy_search"];

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 3, sTen = sTen });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_CongTyLDLK(string ParentID, string iID_MaDoanhNghiep, String iID_Ma)
        {
            string MaND = User.Identity.Name;
            Bang bang = new Bang("TCDN_CongTyLienDoanhLienKet");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            }
            else
            {
                bang.GiaTriKhoa = iID_Ma;
                bang.DuLieuMoi = false;
            }
            bang.Save();
            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 4 });
        }

        [Authorize]
        public ActionResult Delete_CongTyLDLK(String iID_MaDoanhNghiep, String iID_Ma)
        {

            Bang bang = new Bang("TCDN_CongTyLienDoanhLienKet");
            bang.GiaTriKhoa = iID_Ma;
            bang.Delete();

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 4 });
        }
        [Authorize]
        public ActionResult Search_CongTyLDLK(String ParentID, String iID_MaDoanhNghiep)
        {
            string sTen = Request.Form[ParentID + "_sTenCongTy_search"];
            string iID_MaHinhThucHoatDong = Request.Form[ParentID + "_iID_MaHinhThucHoatDong"];

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 4, sTen = sTen, iID_MaHinhThucHoatDong = iID_MaHinhThucHoatDong });
        }
        [Authorize]


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_LinhVuc(string ParentID, string iID_MaDoanhNghiep, String iID_Ma)
        {
            string MaND = User.Identity.Name;
            Bang bang = new Bang("TCDN_LinhVuc");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            }
            else
            {
                bang.GiaTriKhoa = iID_Ma;
                bang.DuLieuMoi = false;
            }
            bang.Save();
            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 2 });
        }

        [Authorize]
        public ActionResult Delete_LinhVuc(String iID_MaDoanhNghiep, String iID_Ma)
        {

            Bang bang = new Bang("TCDN_LinhVuc");
            bang.GiaTriKhoa = iID_Ma;
            bang.Delete();

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 2 });
        }
        [Authorize]
        public ActionResult Search_LinhVuc(String ParentID, String iID_MaDoanhNghiep)
        {
            string sTen = Request.Form[ParentID + "_sTenCongTy_search"];
            string iID_MaHinhThucHoatDong = Request.Form[ParentID + "_iID_MaHinhThucHoatDong"];

            return RedirectToAction("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iLoai = 2, sTen = sTen, iID_MaHinhThucHoatDong = iID_MaHinhThucHoatDong });
        }
        [Authorize]
        public ActionResult SearchSubmit(string ParentID)
        {
            string sTenDoanhNghiep = Request.Form[ParentID + "_sTenDoanhNghiep"];
            string sTenThuongGoi = Request.Form[ParentID + "_sTenThuongGoi"];
            string sTenVietTat = Request.Form[ParentID + "_sTenVietTat"];
            string iID_MaLoaiHinhDoanhNghiep = Request.Form[ParentID + "_iID_MaLoaiHinhDoanhNghiep"];
            string iID_MaHinhThucHoatDong = Request.Form[ParentID + "_iID_MaHinhThucHoatDong"];
            string sTenGiaoDich = Request.Form[ParentID + "_sTenGiaoDich"];
            string iID_MaKhoi = Request.Form[ParentID + "_iID_MaKhoi"];
            string iID_MaNhom = Request.Form[ParentID + "_iID_MaNhom"];
            return RedirectToAction("Index", "TCDN_HoSo_DoanhNghiep", new
                                                                          {
                                                                              sTenDoanhNghiep = sTenDoanhNghiep,
                                                                              sTenThuongGoi = sTenThuongGoi,
                                                                              sTenGiaoDich = sTenGiaoDich,
                                                                              iID_MaLoaiHinhDoanhNghiep = iID_MaLoaiHinhDoanhNghiep,
                                                                              iID_MaHinhThucHoatDong = iID_MaLoaiHinhDoanhNghiep,
                                                                              iID_MaKhoi = iID_MaLoaiHinhDoanhNghiep,
                                                                              iID_MaNhom = iID_MaLoaiHinhDoanhNghiep
                                                                          });
        }

       
    }
}