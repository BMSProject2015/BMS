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
namespace VIETTEL.Controllers.BaoHiem
{
    public class BaoHiem_PhaiThuController : Controller
    {
        //
        // GET: /BaoHiem_ChungTu/

        public string sViewPath = "~/Views/BaoHiem/PhaiThu/";

        [Authorize]
        public ActionResult Index(String bChi)
        {
            ViewData["bChi"] = bChi;
            return View(sViewPath + "BaoHiem_PhaiThu_Index.aspx");
        }
        [Authorize]
        public ActionResult LaySoLieu()
        {
            return View(sViewPath + "BaoHiem_PhaiThu_LaySoLieu.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String bChi)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_sSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            return RedirectToAction("Index", "BaoHiem_PhaiThu", new { bChi = bChi, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iThang_Quy = iThang_Quy, iID_MaDonVi = iID_MaDonVi });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaBaoHiemPhaiThu,String bChi)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaBaoHiemPhaiThu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "BH_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaBaoHiemPhaiThu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaBaoHiemPhaiThu"] = iID_MaBaoHiemPhaiThu;
            ViewData["bChi"] = bChi;
            return View(sViewPath + "BaoHiem_PhaiThu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaBaoHiemPhaiThu, String bChi)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("BH_PhaiThuChungTu");
            
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            String bLoaiThang_Quy = Request.Form[ParentID + "_bLoaiThang_Quy"];
            String iThang = Request.Form[ParentID + "_iThang_Quy"];            
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();                        
            
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaBaoHiemPhaiThu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaBaoHiemPhaiThu"] = MaBaoHiemPhaiThu;                
                return View(sViewPath + "BaoHiem_PhaiThuMaBaoHiemPhaiThu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.CmdParams.Parameters["@iThang_Quy"].Value = iThang;
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeBaoHiem));
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);                    
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                      
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeBaoHiem));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                   
                    BaoHiem_PhaiThuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);

                    BaoHiem_PhaiThuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = MaBaoHiemPhaiThu;
                    bang.Save();
                    BaoHiem_PhaiThuChiTietModels.UpdateBangChiTiet(User.Identity.Name, Request.UserHostAddress, MaBaoHiemPhaiThu, iID_MaDonVi, sTenDonVi, iThang, bLoaiThang_Quy);
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "BaoHiem_PhaiThu", new {bChi=bChi });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_LaySLieu(String ParentID, String iID_MaPhongBan)
        {
            String TuThang = Request.Form[ParentID + "_iTuThang_Quy"];
            String DenThang = Request.Form[ParentID + "_iDenThang_Quy"];
            int iTuThang = Convert.ToInt16(TuThang);
            int iDenThang = Convert.ToInt16(DenThang);
            DataTable dtDV;
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            String iNamLamViec=Convert.ToString(R["iNamLamViec"]);
            for (int i = iTuThang; i <= iDenThang; i++)
            {
                dtDV = BaoHiem_PhaiThuChiTietModels.Get_dtDonViQuyetToan(i, iNamLamViec);
                for (int j = 0; j < dtDV.Rows.Count; j++)
                {
                    String iID_MaDonVi=Convert.ToString(dtDV.Rows[j]["iID_MaDonVi"]);
                    String TenTruong="iID_MaDonVi,iThang_Quy,iNamLamViec";
                    String GiaTri = iID_MaDonVi + "," + i.ToString()+","+iNamLamViec;
                    if (HamChung.Check_Trung("BH_PhaiThuChungTu", "iID_MaBaoHiemPhaiThu", "", TenTruong, GiaTri, true) == false)
                    {
                        int iSoChungTu = BaoHiem_PhaiThuModels.GetMaxChungTu() + 1;
                        Bang bang = new Bang("BH_PhaiThuChungTu");
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.TruyenGiaTri(ParentID, Request.Form);
                        bang.CmdParams.Parameters.AddWithValue("@iThang_Quy",i);

                        bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeBaoHiem));
                        bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
                        bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtDV.Rows[j]["iID_MaDonVi"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", DonViModels.Get_TenDonVi(iID_MaDonVi));
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeBaoHiem));

                        String MaChungTuAddNew = Convert.ToString(bang.Save());

                        BaoHiem_PhaiThuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);

                        BaoHiem_PhaiThuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                    }
                }
            }
            return RedirectToAction("Index", "BaoHiem_PhaiThu");
        }

        [Authorize]
        public ActionResult Delete(String iID_MaBaoHiemPhaiThu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "BH_PhaiThuChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = BaoHiem_PhaiThuModels.Delete_ChungTu(iID_MaBaoHiemPhaiThu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "BaoHiem_PhaiThu");
        }

        [Authorize]
        public ActionResult Duyet(String bChi)
        {
            return RedirectToAction("Index", "BaoHiem_PhaiThu");
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaBaoHiemPhaiThu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = BaoHiem_PhaiThuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaBaoHiemPhaiThu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            BaoHiem_PhaiThuModels.Update_iID_MaTrangThaiDuyet(iID_MaBaoHiemPhaiThu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = BaoHiem_PhaiThuModels.InsertDuyetChungTu(iID_MaBaoHiemPhaiThu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetBaoHiemPhaiThuCuoiCung", MaDuyetChungTu);
            BaoHiem_PhaiThuModels.UpdateRecord(iID_MaBaoHiemPhaiThu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = BaoHiem_PhaiThuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaBaoHiemPhaiThu);
            return RedirectToAction("Index", "BaoHiem_PhaiThuChiTiet", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu});
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaBaoHiemPhaiThu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = BaoHiem_PhaiThuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaBaoHiemPhaiThu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua

            BaoHiem_DuyetChungTuChiModels.CapNhapLaiTruong_sSua(iID_MaBaoHiemPhaiThu);

            ///Update trạng thái cho bảng chứng từ
            BaoHiem_PhaiThuModels.Update_iID_MaTrangThaiDuyet(iID_MaBaoHiemPhaiThu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = BaoHiem_PhaiThuModels.InsertDuyetChungTu(iID_MaBaoHiemPhaiThu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetBaoHiemPhaiThuCuoiCung", MaDuyetChungTu);
            BaoHiem_PhaiThuModels.UpdateRecord(iID_MaBaoHiemPhaiThu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "BaoHiem_PhaiThuChiTiet", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu});
        }
    }
}
