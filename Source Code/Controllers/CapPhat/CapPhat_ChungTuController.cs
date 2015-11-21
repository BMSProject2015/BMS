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

//HungPX QUP: sửa namespace thành cấp phát
namespace VIETTEL.Controllers.CapPhat
{
    public class CapPhat_ChungTuController : Controller
    {
        //
        // GET: /CapPhat_ChungTu/
        public static readonly string CREATE = "Create";
        public static readonly string EDIT = "Edit";
        public static readonly string DELETE = "Delete";
        public static readonly string VIEW_ROOTPATH = "~/Views/CapPhat/ChungTu/";
        public static readonly string VIEW_CAPPHAT_CHUNGTU_INDEX = "CapPhat_ChungTu_Index.aspx";
        public static readonly string VIEW_CAPPHAT_CHUNGTU_CHUYENNAMSAU = "CapPhat_ChungTu_ChuyenNamSau.aspx";
        public static readonly string VIEW_CAPPHAT_CHUNGTU_EDIT = "CapPhat_ChungTu_Edit.aspx";
        public static readonly string VIEW_CAPPHAT_CHUNGTU_DUYET = "CapPhat_ChungTu_Duyet.aspx";

        /// <summary>
        /// Action Index Cấp phát chứng từ
        /// </summary>
        /// <param name="MaDotNganSach"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(String MaDotNganSach, String sLNS, String DonVi)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["MaDotNganSach"] = MaDotNganSach;
                ViewData["sLNS"] = sLNS;
                ViewData["DonVi"] = DonVi;
                return View(VIEW_ROOTPATH + VIEW_CAPPHAT_CHUNGTU_INDEX);
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// chưa rõ nghiệp vụ
        /// </summary>
        /// <param name="MaDotNganSach"></param>
        /// <param name="sLNS"></param>
        /// <param name="DonVi"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChuyenNamSau(String MaDotNganSach, String sLNS, String DonVi)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["MaDotNganSach"] = MaDotNganSach;
                ViewData["sLNS"] = sLNS;
                ViewData["DonVi"] = DonVi;
                return View(VIEW_ROOTPATH + VIEW_CAPPHAT_CHUNGTU_CHUYENNAMSAU);
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TimKiemChungTu(String ParentID, String DonVi, String Loai)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoCapPhat = Request.Form[ParentID + "_iSoCapPhat"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iDM_MaLoaiCapPhat = Request.Form[ParentID + "_iDM_MaLoaiCapPhat"];
            String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(User.Identity.Name);
            return RedirectToAction("Index", "CapPhat_ChungTu", new { MaPhongBan = MaPhongBan, SoCapPhat = SoCapPhat, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, DonVi = DonVi, Loai = Loai });
        }
        /// <summary>
        /// Hàm sửa chứng từ cấp phát khi người dùng click sửa chứng từ
        /// Trong trường hợp tạo mới, mã cấp phát bằng null
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="Loai">Loại ngân sách cấp phát</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SuaChungTu(String iID_MaCapPhat, String Loai)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaCapPhat) && LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "CP_CapPhat", EDIT) == false)
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
            ViewData["Loai"] = Loai;
            return View(VIEW_ROOTPATH + VIEW_CAPPHAT_CHUNGTU_EDIT);
        }
        /// <summary>
        /// Lưu chứng từ sau khi thực hiện sửa hoặc tạo mới
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="DonVi"></param>
        /// <param name="Loai"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LuuChungTu(String ParentID, String iID_MaCapPhat, String DonVi, String Loai)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }
            Bang bang = new Bang("CP_CapPhat");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iDM_MaLoaiCapPhat = Convert.ToString(Request.Form[ParentID + "_iDM_MaLoaiCapPhat"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayCapPhat"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String sLoai = Convert.ToString(Request.Form[ParentID + "_iID_Loai"]);
            String iID_MaTinhChatCapThu = Convert.ToString(Request.Form[ParentID + "_iID_MaTinhChatCapThu"]);
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
            if (String.IsNullOrEmpty(iID_MaTinhChatCapThu) || iID_MaTinhChatCapThu == "-1")
            {
                arrLoi.Add("err_iID_MaTinhChatCapThu", "Bạn chưa chọn tính chất cấp thu");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DonVi"] = DonVi;
                ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
                ViewData["DuLieuMoi"] = Request.Form[ParentID + "_DuLieuMoi"];
                return View(VIEW_ROOTPATH + VIEW_CAPPHAT_CHUNGTU_EDIT);
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
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeCapPhat));
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", Loai);
                    bang.CmdParams.Parameters.AddWithValue("@sLoai", sLoai);
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    CapPhat_ChungTuModels.CapNhatBangDuyetChungTu(MaChungTuAddNew, "Tạo mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = iID_MaCapPhat;
                    bang.Save();

                    // HungPX QUP: Update bảng chứng từ chi tiết tương ứng với chứng từ vừa edit
                    CapPhat_ChungTuChiTietModels.DongBoChungTuChiTiet(iID_MaCapPhat);
                }
            }
            return RedirectToAction("Index", "CapPhat_ChungTu", new { DonVi = DonVi, Loai = Loai });
        }
        /// <summary>
        /// xóa chứng từ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="DonVi"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult XoaChungTu(String iID_MaCapPhat, String DonVi)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "CP_CapPhat", DELETE) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Xóa bảng cấp phát và chi tiết
            CapPhat_ChungTuModels.XoaChungTu(iID_MaCapPhat, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "CapPhat_ChungTu", new { MaDotNganSach = iID_MaCapPhat, DonVi = DonVi });
        }

        /// <summary>
        /// Xử lý hoạt động trình duyệt (duyệt) chứng từ của người dùng
        /// </summary>
        /// <param name="ChiNganSach"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TrinhDuyet(String ChiNganSach, String iID_MaCapPhat)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = CapPhat_ChungTuChiTietModels.LayMaTrangThaiDuyetTrinhDuyet(MaND, iID_MaCapPhat);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            CapPhat_ChungTuModels.CapNhatMaTrangThaiDuyet(iID_MaCapPhat, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = CapPhat_ChungTuModels.CapNhatBangDuyetChungTu(iID_MaCapPhat, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetCapPhatCuoiCung", MaDuyetChungTu);
            CapPhat_ChungTuModels.CapNhatBanGhi(iID_MaCapPhat, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("CapPhatChiTiet_Frame", "CapPhat_ChungTuChiTiet", new { ChiNganSach = ChiNganSach, iID_MaCapPhat = iID_MaCapPhat });
        }
        /// <summary>
        /// Xử lý hoạt động từ chối chứng từ của người dùng
        /// </summary>
        /// <param name="ChiNganSach"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TuChoi(String ChiNganSach, String iID_MaCapPhat)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = CapPhat_ChungTuChiTietModels.LayMaTrangThaiDuyetTuChoi(MaND, iID_MaCapPhat);
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
            CapPhat_ChungTuModels.CapNhatMaTrangThaiDuyet(iID_MaCapPhat, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = CapPhat_ChungTuModels.CapNhatBangDuyetChungTu(iID_MaCapPhat, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetCapPhatCuoiCung", MaDuyetChungTu);
            CapPhat_ChungTuModels.CapNhatBanGhi(iID_MaCapPhat, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("CapPhatChiTiet_Frame", "CapPhat_ChungTuChiTiet", new { ChiNganSach = ChiNganSach, iID_MaCapPhat = iID_MaCapPhat });
        }
        /// <summary>
        /// chưa rõ nghiệp vụ
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ThongTri(String ParentID, String iID_MaCapPhat)
        {
            return RedirectToAction("Index", "CapPhat_Report", new { iID_MaCapPhat = iID_MaCapPhat });
        }
        /// <summary>
        /// chưa rõ nghiệp vụ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ChuyenNamSau()
        {
            String MaND = User.Identity.Name;
            CapPhatModels.ChuyenNamSau(MaND);
            return RedirectToAction("Index");
        }
    }
}
