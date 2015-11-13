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
namespace VIETTEL.Controllers.QuyetToan
{
    public class QuyetToan_ChungTuController : Controller
    {
        //
        // GET: /QuyetToan_ChungTu/
        public string sViewPath = "~/Views/QuyetToan/ChungTu/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "QuyetToan_ChungTu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String Loai)
        {
            String MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String sThangQuy = "";
            if (Loai == "1")
            {
                sThangQuy = Request.Form[ParentID + "_iQuy"];
            }
            else
            {
                sThangQuy = Request.Form[ParentID + "_iQuy"];
            }

            NameValueCollection arrLoi = new NameValueCollection();
            if (HamChung.isDate(TuNgay) == false && TuNgay != "")
            {
                arrLoi.Add("err_dTuNgay", "Ngày nhập sai");
            }

            if (HamChung.isDate(DenNgay) == false && DenNgay != "")
            {
                arrLoi.Add("err_dDenNgay", "Ngày nhập sai");
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["Loai"] = Loai;
                return View(sViewPath + "QuyetToan_ChungTu_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "QuyetToan_ChungTu", new { Loai = Loai, MaDonVi = MaDonVi, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sThangQuy = sThangQuy });
            }
        }

        [Authorize]
        public ActionResult Edit(String iID_MaChungTu, String Loai)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "DT_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["Loai"] = Loai;
            ViewData["MaChungTu"] = iID_MaChungTu;
         
                return View(sViewPath + "QuyetToan_ChungTu_Edit.aspx");
            
            //else if (Loai == "2")
            //{
            //    return View(sViewPath + "QuyetToan_ChungTu_NhaNuoc_Edit.aspx");
            //}
            //else
            //{
            //    return View(sViewPath + "QuyetToan_ChungTuNghiepVu_Edit.aspx");
            //}
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditSubmit(String ParentID, String MaChungTu, String Loai)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            String MaChungTuAddNew = "";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("QTA_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang_Quy"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            if (HamChung.isDate(NgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayChungTu", "Ngày chứng từ nhập sai!");
            }

            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (iThang == string.Empty || iThang == "" || iThang == null)
            {
                arrLoi.Add("err_iThang_Quy", "Bạn chưa chọn tháng!");
            }
            if (iID_MaDonVi == string.Empty || iID_MaDonVi == "" || iID_MaDonVi == null)
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["Loai"] = Loai;
                ViewData["MaChungTu"] = MaChungTu;
                ViewData["NgayChungTu"] = NgayChungTu;
                ViewData["iThang"] = iThang;
                ViewData["iID_MaDonVi"] = iID_MaDonVi;
                ViewData["sLNS"] = sLNS;
                ViewData["sNoiDung"] = sNoiDung;
                return View(sViewPath + "QuyetToan_ChungTu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                String iID_MaPhongBan = "", sTenPhongBan = "";
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                    sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                    dtPhongBan.Dispose();
                }
                
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
               
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan));
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", Loai);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(QuyetToanModels.iID_MaPhanHeQuyetToan));
                     MaChungTuAddNew = Convert.ToString(bang.Save());
                    //QuyetToan_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                   // QuyetToan_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTu;
                    bang.Save();

                    DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(MaChungTu);

                    SqlCommand cmd;
                    String SQL = "UPDATE QTA_ChungTuChiTiet SET iID_MaPhongBan=@iID_MaPhongBan,sTenPhongBan=@sTenPhongBan, iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, " +
                            "iNamLamViec=@iNamLamViec,iID_MaNguonNganSach=@iID_MaNguonNganSach,iID_MaNamNganSach=@iID_MaNamNganSach,bChiNganSach=@bChiNganSach, " +
                            "iThang_Quy=@iThang_Quy,bLoaiThang_Quy=@bLoaiThang_Quy,iID_MaDonVi=@iID_MaDonVi " +
                            "WHERE iID_MaChungTu=@iID_MaChungTu";
                    cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                    cmd.Parameters.AddWithValue("@sTenPhongBan", dtChungTu.Rows[0]["sTenPhongBan"]);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                    cmd.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                    cmd.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                    cmd.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", dtChungTu.Rows[0]["bLoaiThang_Quy"]);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    cmd.CommandText = SQL;
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    dtChungTu.Dispose();

                    //Update lại các trường chỉ tiêu lấy từ cấp phát sang
                    //DataTable dtChungTuChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtChungTuChiTiet(MaChungTu);
                   // QuyetToan_ChungTuChiTietModels.Update_TruongChiTieu(dtChungTuChiTiet);
                    //QuyetToan_ChungTuChiTietModels.Update_TruongDaQuyetToan(dtChungTuChiTiet);
                }
                dtCauHinh.Dispose();
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                return RedirectToAction("Index", "QuyetToan_ChungTuChiTiet", new {iID_MaChungTu = MaChungTuAddNew});
            }
            else
            {
                return RedirectToAction("Index", "QuyetToan_ChungTu", new { Loai = Loai });
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Fast_Submit(String ParentID, String MaChungTu, String Loai)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("QTA_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String optThangQuy = Convert.ToString(Request.Form[ParentID + "_ThangQuy"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (sLNS == string.Empty || sLNS == "" || sLNS == null)
            {
                arrLoi.Add("err_sDSLNS", "Bạn chưa chọn loại ngân sách!");
            }
            if (HamChung.isDate(NgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayChungTu", "Ngày chứng từ nhập sai!");
            }
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            switch (optThangQuy)
            {
                case "Thang":
                    if (iThang == string.Empty || iThang == "-1" || iThang == "" || iThang == null)
                    {
                        arrLoi.Add("err_ThangQuy", "Bạn chưa chọn tháng!");
                    }
                    break;
                case "Quy":
                    if (iQuy == string.Empty || iQuy == "" || iQuy == null)
                    {
                        arrLoi.Add("err_ThangQuy", "Bạn chưa chọn quý!");
                    }
                    break;
                case "Nam":
                    break;
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["Loai"] = Loai;
                ViewData["MaChungTu"] = MaChungTu;
                ViewData["NgayChungTu"] = NgayChungTu;
                return View(sViewPath + "QuyetToan_ChungTuNghiepVu_Edit.aspx");
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
                    switch (optThangQuy)
                    {
                        case "Thang":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang);
                            break;
                        case "Quy":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iQuy);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                            break;
                    }
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan));
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", Loai);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(QuyetToanModels.iID_MaPhanHeQuyetToan));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //QuyetToan_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    QuyetToan_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    switch (optThangQuy)
                    {
                        case "Thang":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 0);
                            break;
                        case "Quy":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iQuy);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                            break;
                    }

                    bang.GiaTriKhoa = MaChungTu;
                    bang.Save();

                    DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(MaChungTu);

                    SqlCommand cmd;
                    String SQL = "UPDATE QTA_ChungTuChiTiet SET iID_MaPhongBan=@iID_MaPhongBan, iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, " +
                            "iNamLamViec=@iNamLamViec,iID_MaNguonNganSach=@iID_MaNguonNganSach,iID_MaNamNganSach=@iID_MaNamNganSach,bChiNganSach=@bChiNganSach, " +
                            "iThang_Quy=@iThang_Quy,bLoaiThang_Quy=@bLoaiThang_Quy,iID_MaDonVi=@iID_MaDonVi " +
                            "WHERE iID_MaChungTu=@iID_MaChungTu";
                    cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                    cmd.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                    cmd.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                    cmd.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", dtChungTu.Rows[0]["bLoaiThang_Quy"]);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    cmd.CommandText = SQL;
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    dtChungTu.Dispose();

                    //Update lại các trường chỉ tiêu lấy từ cấp phát sang
                    DataTable dtChungTuChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtChungTuChiTiet(MaChungTu);
                    QuyetToan_ChungTuChiTietModels.Update_TruongChiTieu(dtChungTuChiTiet);
                    QuyetToan_ChungTuChiTietModels.Update_TruongDaQuyetToan(dtChungTuChiTiet);
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "QuyetToan_ChungTu", new { Loai = Loai });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_NhaMuoc_Submit(String ParentID, String MaChungTu, String Loai)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("QTA_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String optThangQuy = Convert.ToString(Request.Form[ParentID + "_ThangQuy"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            String iID_MaPhongBan_Chon = Convert.ToString(Request.Form[ParentID + "_iID_MaPhongBan_Chon"]);
            iID_MaPhongBan_Chon = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            if (sLNS == string.Empty || sLNS == "" || sLNS == null)
            {
                arrLoi.Add("err_sDSLNS", "Bạn chưa chọn loại ngân sách!");
            }
            if (HamChung.isDate(NgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayChungTu", "Ngày chứng từ nhập sai!");
            }
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            switch (optThangQuy)
            {
                case "Thang":
                    if (iThang == string.Empty || iThang == "" || iThang == null)
                    {
                        arrLoi.Add("err_ThangQuy", "Bạn chưa chọn tháng!");
                    }
                    break;
                case "Quy":
                    if (iQuy == string.Empty || iQuy == "" || iQuy == null)
                    {
                        arrLoi.Add("err_ThangQuy", "Bạn chưa chọn quý!");
                    }
                    break;
                case "Nam":
                    break;
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["Loai"] = Loai;
                ViewData["MaChungTu"] = MaChungTu;
                ViewData["NgayChungTu"] = NgayChungTu;
                return View(sViewPath + "QuyetToan_ChungTuNghiepVu_Edit.aspx");
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
                    switch (optThangQuy)
                    {
                        case "Thang":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang);
                            break;
                        case "Quy":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iQuy);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                            break;
                    }
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan));
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", Loai);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan_Chon);
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(QuyetToanModels.iID_MaPhanHeQuyetToan));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //QuyetToan_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, Request.UserHostAddress);
                    QuyetToan_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    switch (optThangQuy)
                    {
                        case "Thang":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iThang);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 0);
                            break;
                        case "Quy":
                            bang.CmdParams.Parameters.AddWithValue("@iThang_Quy", iQuy);
                            bang.CmdParams.Parameters.AddWithValue("@bLoaiThang_Quy", 1);
                            break;
                    }

                    bang.GiaTriKhoa = MaChungTu;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan_Chon);
                    bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                    bang.Save();

                    DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(MaChungTu);

                    SqlCommand cmd;
                    String SQL = "UPDATE QTA_ChungTuChiTiet SET iID_MaPhongBan=@iID_MaPhongBan, iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, " +
                            "iNamLamViec=@iNamLamViec,iID_MaNguonNganSach=@iID_MaNguonNganSach,iID_MaNamNganSach=@iID_MaNamNganSach,bChiNganSach=@bChiNganSach, " +
                            "iThang_Quy=@iThang_Quy,bLoaiThang_Quy=@bLoaiThang_Quy,iID_MaDonVi=@iID_MaDonVi " +
                            "WHERE iID_MaChungTu=@iID_MaChungTu";
                    cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                    cmd.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                    cmd.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                    cmd.Parameters.AddWithValue("@iThang_Quy", dtChungTu.Rows[0]["iThang_Quy"]);
                    cmd.Parameters.AddWithValue("@bLoaiThang_Quy", dtChungTu.Rows[0]["bLoaiThang_Quy"]);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                    String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]));
                    cmd.Parameters.AddWithValue("@strTenDonVi",strTenDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    cmd.CommandText = SQL;
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    dtChungTu.Dispose();

                    //Update lại các trường chỉ tiêu lấy từ cấp phát sang
                    DataTable dtChungTuChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtChungTuChiTiet(MaChungTu);
                    QuyetToan_ChungTuChiTietModels.Update_TruongChiTieu(dtChungTuChiTiet);
                    QuyetToan_ChungTuChiTietModels.Update_TruongDaQuyetToan(dtChungTuChiTiet);
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "QuyetToan_ChungTu", new { Loai = Loai });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChungTu, String Loai)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = QuyetToan_ChungTuModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "QuyetToan_ChungTu", new { Loai = Loai });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return View(sViewPath + "QuyetToan_ChungTu_Duyet.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID, String Loai)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Duyet", "QuyetToan_ChungTu", new { Loai = Loai, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        public JsonResult get_List_QuyetToan_ChungTu(String Loai, String MaND, String iSoChungTu, String dTuNgay, String dDenNgay, String iID_MaTrangThaiDuyet, String page)
        {
            QuyetToanListModels ModelData = new QuyetToanListModels(Loai, MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet, page);

            String strList = "";
            strList = RenderPartialViewToStringLoad("~/Views/QuyetToan/ChungTu/QuyetToan_ChungTu_List_Partial.ascx", ModelData);

            return Json(strList, JsonRequestBehavior.AllowGet);
        }

        public string RenderPartialViewToStringLoad(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            QuyetToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = QuyetToan_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            QuyetToan_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            return RedirectToAction("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            QuyetToan_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            QuyetToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = QuyetToan_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            QuyetToan_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        //TuChoi khi da duyet
        [Authorize]
        public ActionResult TuChoiDuyet(String iID_MaChungTu, String Loai)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            QuyetToan_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            QuyetToan_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = QuyetToan_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            QuyetToan_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Duyet", "QuyetToan_ChungTu", new { iID_MaChungTu = iID_MaChungTu,Loai=Loai });
        }

        [Authorize]
        public JsonResult get_TongGiaTriQuyetToan(String sLNS, String iID_MaDonVi, String Thang_Quy, String LoaiThang_Quy)
        {
            String rSoQuyetToanTrongKy_TuChi = "0", rSoQuyetToanDenKy_TuChi = "0", rTongSoQuyetToan_TuChi = "0";
            String rSoQuyetToanTrongKy_HienVat = "0", rSoQuyetToanDenKy_HienVat = "0", rTongSoQuyetToan_HienVat = "0";
            DataTable vR, vR1, vR2;

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];

            vR = QuyetToan_ChungTuModels.SoQuyetToanTrongKy(sLNS, iID_MaDonVi, Convert.ToString(R["iNamLamViec"]), Thang_Quy, LoaiThang_Quy, Convert.ToString(User.Identity.Name));
            vR1 = QuyetToan_ChungTuModels.SoQuyetToanDenKy(sLNS, iID_MaDonVi, Convert.ToString(R["iNamLamViec"]), Thang_Quy, LoaiThang_Quy, Convert.ToString(User.Identity.Name));
            vR2 = QuyetToan_ChungTuModels.ThongKeTongSoQuyetToan(sLNS, iID_MaDonVi, Convert.ToString(R["iNamLamViec"]), User.Identity.Name);
            if (vR.Rows.Count > 0)
            {
                rSoQuyetToanTrongKy_TuChi = CommonFunction.DinhDangSo(Convert.ToString(vR.Rows[0]["TuChi"]));
                rSoQuyetToanDenKy_TuChi = CommonFunction.DinhDangSo(Convert.ToString(vR1.Rows[0]["TuChi"]));
                rTongSoQuyetToan_TuChi = CommonFunction.DinhDangSo(Convert.ToString(vR2.Rows[0]["TuChi"]));
                rSoQuyetToanTrongKy_HienVat = CommonFunction.DinhDangSo(Convert.ToString(vR.Rows[0]["HienVat"]));
                rSoQuyetToanDenKy_HienVat = CommonFunction.DinhDangSo(Convert.ToString(vR1.Rows[0]["HienVat"]));
                rTongSoQuyetToan_HienVat = CommonFunction.DinhDangSo(Convert.ToString(vR2.Rows[0]["HienVat"]));
            }
            vR.Dispose();
            vR1.Dispose();
            vR2.Dispose();
            dtCauHinh.Dispose();
            Object item = new
            {
                rSoQuyetToanTrongKy_TuChi = rSoQuyetToanTrongKy_TuChi,
                rSoQuyetToanDenKy_TuChi = rSoQuyetToanDenKy_TuChi,
                rTongSoQuyetToan_TuChi = rTongSoQuyetToan_TuChi,
                rSoQuyetToanTrongKy_HienVat = rSoQuyetToanTrongKy_HienVat,
                rSoQuyetToanDenKy_HienVat = rSoQuyetToanDenKy_HienVat,
                rTongSoQuyetToan_HienVat = rTongSoQuyetToan_HienVat
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_dtMucLucNganSach_NghiepVu(String ParentID, String iID_MaPhongBan, String sLNS)
        {
            return Json(get_objMucLucNganSach_NghiepVu(ParentID, iID_MaPhongBan, sLNS), JsonRequestBehavior.AllowGet);
        }

        public static String get_objMucLucNganSach_NghiepVu(String ParentID, String iID_MaPhongBan, String sLNS)
        {
            String[] arrsLNS = null;
            if (sLNS != "")
            {
                    arrsLNS = sLNS.Split(',');
            }

            DataTable dt = DanhMucModels.NS_LoaiNganSachNghiepVuKhac_PhongBan(iID_MaPhongBan);
            String strTenLNS = "", MaLNS = "", strChecked = "";
            StringBuilder builder = new StringBuilder();
            if (dt != null)
            {
                builder.Append("<table class='mGrid'>");
                builder.Append("<tr>");
                builder.Append("<th align=\"left\"><input type='checkbox' name='abc' id='abc' onclick='CheckAll(this.checked);' /></th>");
                builder.Append("<th>Danh sách loại ngân sách nghiệp vụ khác</th>");
                builder.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strChecked = "";
                    strTenLNS = Convert.ToString(dt.Rows[i]["TenHT"]);
                    MaLNS = Convert.ToString(dt.Rows[i]["sLNS"]);
                    if (arrsLNS != null && arrsLNS.Length > 0)
                    {
                        for (int j = 0; j < arrsLNS.Length; j++)
                        {
                            if (MaLNS.Equals(arrsLNS[j]))
                            {
                                strChecked = "checked=\"checked\"";
                                break;
                            }
                        }
                    }
                    builder.Append("<tr>");
                    builder.Append("<td align=\"left\">");
                    builder.Append("<input type='checkbox' value='" + MaLNS + "' " + strChecked + "' onclick='ChonLNSNhapLieu();' check-group='MaMucLucNganSach' id='" + ParentID + "_sLNS' name='" + ParentID + "_sLNS' />");
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(String.Format("{0}", strTenLNS));
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }
                builder.Append("</table>");
            }
            return builder.ToString(); ;
        }

        public JsonResult get_dtMucLucNganSach_NhaNuoc(String ParentID, String iID_MaPhongBan, String sLNS)
        {
            return Json(get_objMucLucNganSach_NhaNuoc(ParentID, iID_MaPhongBan, sLNS), JsonRequestBehavior.AllowGet);
        }

        public static String get_objMucLucNganSach_NhaNuoc(String ParentID, String iID_MaPhongBan, String sLNS)
        {
            String[] arrsLNS = null;
            if (sLNS != "")
            {
                arrsLNS = sLNS.Split(',');
            }

            DataTable dt = DanhMucModels.NS_LoaiNganSachNhaNuoc_PhongBan(iID_MaPhongBan);
            String strTenLNS = "", MaLNS = "", strChecked = "";
            StringBuilder builder = new StringBuilder();
            if (dt != null)
            {
                builder.Append("<table class='mGrid'>");
                builder.Append("<tr>");
                builder.Append("<th align=\"left\"><input type='checkbox' name='abc' id='abc' onclick='CheckAll(this.checked);' /></th>");
                builder.Append("<th>Danh sách loại ngân sách nhà nước</th>");
                builder.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strChecked = "";
                    strTenLNS = Convert.ToString(dt.Rows[i]["TenHT"]);
                    MaLNS = Convert.ToString(dt.Rows[i]["sLNS"]);
                    if (arrsLNS != null && arrsLNS.Length > 0)
                    {
                        for (int j = 0; j < arrsLNS.Length; j++)
                        {
                            if (MaLNS.Equals(arrsLNS[j]))
                            {
                                strChecked = "checked=\"checked\"";
                                break;
                            }
                        }
                    }
                    builder.Append("<tr>");
                    builder.Append("<td align=\"left\">");
                    builder.Append("<input type='checkbox' value='" + MaLNS + "' " + strChecked + "' onclick='ChonLNSNhapLieu();' check-group='MaMucLucNganSach' id='" + ParentID + "_sLNS' name='" + ParentID + "_sLNS' />");
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(String.Format("{0}", strTenLNS));
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }
                builder.Append("</table>");
            }
            return builder.ToString(); ;
        }

        #region Chuyển năm sau
        [Authorize]
        public ActionResult ChuyenNamSau()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "QuyetToan_ChuyenNamSau.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ChuyenNamSau()
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            int MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan);
            NganSach_HamChungModels.ChuyenNamSau(MaND, IPSua, MaTrangThaiDuyet, "QTA_ChungTu", "QTA_ChungTuChiTiet", false);
            return RedirectToAction("Index");

        }
        #endregion
    }
}
