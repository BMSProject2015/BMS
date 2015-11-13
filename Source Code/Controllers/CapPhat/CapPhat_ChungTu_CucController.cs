using System;
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

namespace VIETTEL.Controllers.DuToan
{
    public class CapPhat_ChungTu_CucController : Controller
    {
        //
        // GET: /CapPhat_ChungTu/
        public string sViewPath = "~/Views/CapPhat/ChungTu_Cuc/";
        /// <summary>
        /// Action Index Cấp phát chứng từ
        /// </summary>
        /// <param name="MaDotNganSach"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(String MaDotNganSach, String sLNS, String DonVi)
        {
            ViewData["MaDotNganSach"] = MaDotNganSach;
            ViewData["sLNS"] = sLNS;
            ViewData["DonVi"] = DonVi;
            return View(sViewPath + "CapPhat_ChungTu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String DonVi)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoCapPhat = Request.Form[ParentID + "_iSoCapPhat"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iDM_MaLoaiCapPhat = Request.Form[ParentID + "_iDM_MaLoaiCapPhat"];
            String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(User.Identity.Name);
            return RedirectToAction("Index", "CapPhat_ChungTu_Cuc", new { MaPhongBan = MaPhongBan, SoCapPhat = SoCapPhat, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, DonVi = DonVi });
        }

        [Authorize]
        public ActionResult Edit(String iID_MaCapPhat, String sLNS, String DonVi)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaCapPhat) && LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "CP_CapPhat", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaCapPhat))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            ViewData["DonVi"] = DonVi;
            ViewData["sLNS"] = sLNS;
            return View(sViewPath + "CapPhat_ChungTu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaCapPhat, String sLNS,String DonVi)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("CP_CapPhat");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String sLoai = "";
            String iDM_MaLoaiCapPhat = Convert.ToString(Request.Form[ParentID + "_iDM_MaLoaiCapPhat"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayCapPhat"]);
            if (iDM_MaLoaiCapPhat == Convert.ToString(Guid.Empty) || iDM_MaLoaiCapPhat == "" || iDM_MaLoaiCapPhat == null)
            {
                arrLoi.Add("err_iDM_MaLoaiCapPhat", "Bạn chưa chọn danh mục cấp phát!");
            }
            if (HamChung.isDate(NgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayCapPhat", "Ngày không đúng");
            }
            if (String.IsNullOrEmpty(NgayChungTu))
            {
                arrLoi.Add("err_dNgayCapPhat", "Bạn chưa nhập ngày chứng từ!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DonVi"] = DonVi;
                ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
                return View(sViewPath + "CapPhat_ChungTu_Edit.aspx");
            }
            else
            {

                DataTable dtNguoiDungCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtNguoiDungCauHinh.Rows[0]["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtNguoiDungCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtNguoiDungCauHinh.Rows[0]["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeCapPhat));
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS + ";");
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeCapPhat));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    CapPhat_ChungTuChiTiet_CucModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress, sLoai);
                    CapPhat_ChungTu_CucModels.InsertDuyetChungTu(MaChungTuAddNew, "Tạo mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = iID_MaCapPhat;
                    bang.Save();
                }
            }
            return RedirectToAction("Index", "CapPhat_ChungTu_Cuc", new { DonVi = DonVi });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaCapPhat,String DonVi)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "CP_CapPhat", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Xóa bảng cấp phát và chi tiết
            CapPhat_ChungTuModels.Delete_ChungTu(iID_MaCapPhat, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "CapPhat_ChungTu_Cuc", new { MaDotNganSach = iID_MaCapPhat, DonVi = DonVi });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return View(sViewPath + "CapPhat_ChungTu_Duyet.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID, String ChiNganSach)
        {
            String MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iDM_MaLoaiCapPhat = Request.Form[ParentID + "_iDM_MaLoaiCapPhat"];

            return RedirectToAction("Duyet", "CapPhat_ChungTu_Cuc", new { MaPhongBan = MaPhongBan, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat });
        }

        [Authorize]
        public ActionResult TrinhDuyet(String ChiNganSach, String iID_MaCapPhat)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = CapPhat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaCapPhat);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            CapPhat_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaCapPhat, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = CapPhat_ChungTuModels.InsertDuyetChungTu(iID_MaCapPhat, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetCapPhatCuoiCung", MaDuyetChungTu);
            CapPhat_ChungTuModels.UpdateRecord(iID_MaCapPhat, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();


            return RedirectToAction("Duyet", "CapPhat_ChungTu_Cuc", new { ChiNganSach = ChiNganSach });
        }

        [Authorize]
        public ActionResult TuChoi(String ChiNganSach, String iID_MaCapPhat)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = CapPhat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaCapPhat);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            CapPhat_ChungTuModels.CapNhapLaiTruong_sSua(iID_MaCapPhat);

            ///Update trạng thái cho bảng chứng từ
            CapPhat_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaCapPhat, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = CapPhat_ChungTuModels.InsertDuyetChungTu(iID_MaCapPhat, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetCapPhatCuoiCung", MaDuyetChungTu);
            CapPhat_ChungTuModels.UpdateRecord(iID_MaCapPhat, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Duyet", "CapPhat_ChungTu_Cuc", new { ChiNganSach = ChiNganSach });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ThongTri(String ParentID, String iID_MaCapPhat)
        {

            return RedirectToAction("Index", "CapPhat_Report", new { iID_MaCapPhat = iID_MaCapPhat });
        }

    }
}
