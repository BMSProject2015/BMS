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
    public class BaoHiem_ChungTuThuController : Controller
    {
        //
        // GET: /BaoHiem_ChungTu/

        public string sViewPath = "~/Views/BaoHiem/ChungTuThu/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "BaoHiem_ChungTuThu_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String bChi)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_sSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Index", "BaoHiem_ChungTuThu", new { SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaChungTuThu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTuThu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND) == false)
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
            if (String.IsNullOrEmpty(iID_MaChungTuThu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaChungTuThu"] = iID_MaChungTuThu;

            return View(sViewPath + "BaoHiem_ChungTuThu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaChungTuThu)
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
            Bang bang = new Bang("BH_ChungTuThu");

            String LoaiBaoHiem = Request.Form[ParentID + "_iLoaiBaoHiem"];
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String sTenLoaiBaoHiem = "";
            switch (Convert.ToInt16(LoaiBaoHiem ))
            {
                case 1:
                    sTenLoaiBaoHiem = "BHXH";
                    break;
                case 2:
                    sTenLoaiBaoHiem = "BHYT";
                    break;
                case 3:
                    sTenLoaiBaoHiem = "BHTN";
                    break;
            }
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
                if (String.IsNullOrEmpty(MaChungTuThu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["MaChungTuThu"] = MaChungTuThu;
                return View(sViewPath + "BaoHiem_ChungTuThu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];


                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeBaoHiem));
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenLoaiBaoHiem", sTenLoaiBaoHiem);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeBaoHiem));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    BaoHiem_ChungTuThuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    BaoHiem_ChungTuThuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTuThu;
                    bang.Save();
                    BaoHiem_ChungTuThuChiTietModels.UpdateBangChiTiet(User.Identity.Name, Request.UserHostAddress, MaChungTuThu,Convert.ToString(LoaiBaoHiem), sTenLoaiBaoHiem, iThang_Quy, "0");
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "BaoHiem_ChungTuThu");
        }


  

        [Authorize]
        public ActionResult Delete(String iID_MaChungTuThu, String iLoai)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "BH_ChungTuThu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = BaoHiem_ChungTuThuModels.Delete_ChungTu(iID_MaChungTuThu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "BaoHiem_ChungTuThu", new { iLoai = iLoai });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return RedirectToAction("Index", "BaoHiem_ChungTuThu");
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTuThu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = BaoHiem_ChungTuThuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTuThu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            BaoHiem_ChungTuThuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTuThu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = BaoHiem_ChungTuThuModels.InsertDuyetChungTu(iID_MaChungTuThu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetBaoHiemThuCuoiCung", MaDuyetChungTu);
            BaoHiem_ChungTuThuModels.UpdateRecord(iID_MaChungTuThu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = BaoHiem_ChungTuThuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTuThu);
            return RedirectToAction("Index", "BaoHiem_ChungTuThuChiTiet", new { iID_MaChungTuThu = iID_MaChungTuThu });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTuThu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = BaoHiem_ChungTuThuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTuThu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua

            BaoHiem_DuyetChungTuThuModels.CapNhapLaiTruong_sSua(iID_MaChungTuThu);

            ///Update trạng thái cho bảng chứng từ
            BaoHiem_ChungTuThuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTuThu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = BaoHiem_ChungTuThuModels.InsertDuyetChungTu(iID_MaChungTuThu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetBaoHiemThuCuoiCung", MaDuyetChungTu);
            BaoHiem_ChungTuThuModels.UpdateRecord(iID_MaChungTuThu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "BaoHiem_ChungTuThuChiTiet", new { iID_MaChungTuThu = iID_MaChungTuThu });
        }
    }
}
