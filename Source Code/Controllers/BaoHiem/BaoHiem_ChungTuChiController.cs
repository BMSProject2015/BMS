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
    public class BaoHiem_ChungTuChiController : Controller
    {
        //
        // GET: /BaoHiem_ChungTu/

        public string sViewPath = "~/Views/BaoHiem/ChungTuChi/";

        [Authorize]
        public ActionResult Index(String bChi)
        {
            ViewData["bChi"] = bChi;
            return View(sViewPath + "BaoHiem_ChungTuChi_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String bChi)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String iThang = Request.Form[ParentID + "_iThang"];
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String iThang_Quy="";
            String bLoaiThang_Quy = Request.Form[ParentID + "_bLoaiThang_Quy"];
            if (bLoaiThang_Quy == "0")
            {
                iThang_Quy = iThang;
            }
            else
            {
                iThang_Quy = iQuy;
            }
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String SoChungTu = Request.Form[ParentID + "_sSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Index", "BaoHiem_ChungTuChi", new { bChi = bChi, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iThang_Quy = iThang_Quy,bLoaiThang_Quy=bLoaiThang_Quy, iID_MaDonVi = iID_MaDonVi });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaChungTuChi,String bChi)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTuChi) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND) == false)
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
            if (String.IsNullOrEmpty(iID_MaChungTuChi))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaChungTuChi"] = iID_MaChungTuChi;
            ViewData["bChi"] = bChi;
            return View(sViewPath + "BaoHiem_ChungTuChi_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaChungTuChi,String bChi)
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
            Bang bang = new Bang("BH_ChungTuChi");

            String LoaiBaoHiem=Request.Form[ParentID+"_iLoaiBaoHiem"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
            String bLoaiThang_Quy = Request.Form[ParentID + "_bLoaiThang_Quy"];
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String iThang = Request.Form[ParentID + "_iThang_Quy"];
            if (bLoaiThang_Quy == "1")
                iThang = Convert.ToString(Convert.ToInt16(iQuy)*3);
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();            
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTuChi))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["MaChungTu"] = MaChungTuChi;
                ViewData["bChi"] = bChi;
                return View(sViewPath + "BaoHiem_ChungTuChi_Edit.aspx");
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
                    bang.CmdParams.Parameters.AddWithValue("@bChi", bChi);     
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeBaoHiem));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //if(bChi.Trim()=="1"){
                    //BaoHiem_ChungTuChiChiTietModels.ThemChiTiet2(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    //}
                    //else
                    //{
                    //    BaoHiem_ChungTuChiChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    //}
                    BaoHiem_ChungTuChiChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);

                    BaoHiem_ChungTuChiModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTuChi;
                    bang.Save();
                    BaoHiem_ChungTuChiChiTietModels.UpdateBangChiTiet(User.Identity.Name, Request.UserHostAddress, MaChungTuChi, iID_MaDonVi, sTenDonVi, iThang, bLoaiThang_Quy);
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "BaoHiem_ChungTuChi", new {bChi=bChi });
        }

   

        [Authorize]
        public ActionResult Delete(String iID_MaChungTuChi, String bChi)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "BH_ChungTuChi", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = BaoHiem_ChungTuChiModels.Delete_ChungTu(iID_MaChungTuChi, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "BaoHiem_ChungTuChi", new { bChi = bChi });
        }

        [Authorize]
        public ActionResult Duyet(String bChi)
        {
            return RedirectToAction("Index", "BaoHiem_ChungTuChi", new { bChi = bChi });
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTuChi,String bChi)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = BaoHiem_ChungTuChiChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTuChi);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            BaoHiem_ChungTuChiModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTuChi, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = BaoHiem_ChungTuChiModels.InsertDuyetChungTu(iID_MaChungTuChi, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuChiCuoiCung", MaDuyetChungTu);
            BaoHiem_ChungTuChiModels.UpdateRecord(iID_MaChungTuChi, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = BaoHiem_ChungTuChiChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTuChi);
            return RedirectToAction("Index", "BaoHiem_ChungTuChiChiTiet", new { iID_MaChungTuChi = iID_MaChungTuChi, bChi = bChi });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTuChi,String bChi)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = BaoHiem_ChungTuChiChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTuChi);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua

            BaoHiem_DuyetChungTuChiModels.CapNhapLaiTruong_sSua(iID_MaChungTuChi);

            ///Update trạng thái cho bảng chứng từ
            BaoHiem_ChungTuChiModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTuChi, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = BaoHiem_ChungTuChiModels.InsertDuyetChungTu(iID_MaChungTuChi, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuChiCuoiCung", MaDuyetChungTu);
            BaoHiem_ChungTuChiModels.UpdateRecord(iID_MaChungTuChi, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "BaoHiem_ChungTuChiChiTiet", new { iID_MaChungTuChi = iID_MaChungTuChi, bChi = bChi });
        }
    }
}
