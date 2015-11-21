using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.CapPhat
{
    public class CapPhat_ChungTu_DonViController : Controller
    {
        // GET: /CapPhat_ChungTu_DonVi/
        public static readonly string VIEW_ROOTPATH = "~/Views/CapPhat/ChungTu_DonVi/";
        public static readonly string VIEW_CAPPHAT_CHUNGTU_DONVI_INDEX = "CapPhat_ChungTu_DonVi_Index.aspx";
        public static readonly string VIEW_CAPPHAT_DONVI_INDEX_DANHSACH_FRAME = "CapPhat_DonVi_Index_DanhSach_Frame.aspx";
        public static readonly string VIEW_CAPPHAT_CHUNGTU_DONVI_DUYET = "CapPhat_ChungTu_DonVi_Duyet.aspx";
        public static readonly string VIEW_CHUNGTU_DONVI_EDIT = "CapPhat_ChungTu_DonVi_Edit.aspx";
        public static readonly string VIEW_CHUNGTU_DONVI_CHUNGTUCHITIET_INDEX = "CapPhat_DonVi_ChungTuChiTiet_Index.aspx";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
                return View(VIEW_ROOTPATH + VIEW_CAPPHAT_CHUNGTU_DONVI_INDEX);
        }
        /// <summary>
        /// Hiển thị view lưới Cấp phát chi tiết
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        public ActionResult CapPhatChiTiet_Frame(String iID_MaCapPhat)
        {
            return View(VIEW_ROOTPATH + VIEW_CAPPHAT_DONVI_INDEX_DANHSACH_FRAME);
        }
        /// <summary>
        /// tìm chứng từ
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="DonVi"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TimKiemChungTu(String ParentID, String DonVi)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoCapPhat = Request.Form[ParentID + "_iSoCapPhat"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iDM_MaLoaiCapPhat = Request.Form[ParentID + "_iDM_MaLoaiCapPhat"];
            String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(User.Identity.Name);

            return RedirectToAction("Index", "CapPhat_ChungTu_Donvi", new { MaPhongBan = MaPhongBan, SoCapPhat = SoCapPhat, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, DonVi = DonVi });
        }

        /// <summary>
        /// HÀm xử lý hoạt động thêm mới hoặc sửa chứng từ 
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="Loai"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SuaChungTu(String iID_MaCapPhat, String Loai)
        {
            String MaND = User.Identity.Name;

            //Phải có quyền thêm chứng từ
            if (String.IsNullOrEmpty(iID_MaCapPhat) && LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            //Phải có quyền thêm chứng từ            
            if (BaoMat.ChoPhepLamViec(MaND, "CP_CapPhat", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            //Dữ liệu tạo mới
            if (String.IsNullOrEmpty(iID_MaCapPhat))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            //Dữ liệu update    
            else {
                ViewData["Loai"] = "1";
                ViewData["DuLieuMoi"] = "0";
            }

            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;

            return View(VIEW_ROOTPATH + VIEW_CHUNGTU_DONVI_EDIT);
        }
        /// <summary>
        /// Hàm xử lý hoạt động lưu chứng từ
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="Loai"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LuuChungTu(String ParentID, String iID_MaCapPhat, String Loai)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" 
                && LuongCongViecModel.NguoiDung_DuocThemChungTu(CapPhatModels.iID_MaPhanHe, MaND) == false)
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

            NameValueCollection arrLoi = new NameValueCollection();
            //Loại cấp phát
            String iDM_MaLoaiCapPhat = Convert.ToString(Request.Form[ParentID + "_iDM_MaLoaiCapPhat"]);
            //Tính chất cấp thu
            String iID_MaTinhChatCapThu = Convert.ToString(Request.Form[ParentID + "_iID_MaTinhChatCapThu"]);
            //Đơn vị
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            //Chi tiết đến
            String sLoai = Convert.ToString(Request.Form[ParentID + "_iID_Loai"]);
            //Ngày chứng từ
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayCapPhat"]);
            //Loại ngân sách
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);

            if (iDM_MaLoaiCapPhat == Convert.ToString(Guid.Empty) || String.IsNullOrEmpty(iDM_MaLoaiCapPhat))
            {
                arrLoi.Add("err_iDM_MaLoaiCapPhat", "Bạn chưa chọn danh mục cấp phát!");
            }

            if (iID_MaTinhChatCapThu == Convert.ToString(Guid.Empty) || String.IsNullOrEmpty(iID_MaTinhChatCapThu) 
                || iID_MaTinhChatCapThu == "-1")
            {
                arrLoi.Add("err_iID_MaTinhChatCapThu", "Bạn chưa chọn tính chất cấp thu!");
            }

            if (iID_MaDonVi == Convert.ToString(Guid.Empty) ||  String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị cấp phát!");
            }

            if (HamChung.isDate(NgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayCapPhat", "Ngày không đúng");
            }

            if (String.IsNullOrEmpty(NgayChungTu))
            {
                arrLoi.Add("err_dNgayCapPhat", "Bạn chưa nhập ngày chứng từ!");
            }

            //VungNV: 2015/10/21 If has error then redirect to Edit Page
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
                ViewData["DuLieuMoi"] = "0";

                if (String.IsNullOrEmpty(iID_MaCapPhat))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                
                return View(VIEW_ROOTPATH + VIEW_CHUNGTU_DONVI_EDIT);
            }   
            //VungNV: 2015/10/21 Execute insert or update
            else 
                {

                    String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(User.Identity.Name);
                    DataTable dtNguoiDungCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    //VungNV: 2015/10/21: If is new record then execute insert to [CP_CapPhat] and [CP_DuyetCapPhat] table
                    if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtNguoiDungCauHinh.Rows[0]["iNamLamViec"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtNguoiDungCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtNguoiDungCauHinh.Rows[0]["iID_MaNamNganSach"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeCapPhat));
                        bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS);
                        bang.CmdParams.Parameters.AddWithValue("@iLoai", Loai);
                        bang.CmdParams.Parameters.AddWithValue("@sLoai", sLoai);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeCapPhat));
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                        String MaChungTuAddNew = Convert.ToString(bang.Save());
                        
                        CapPhat_ChungTuModels.CapNhatBangDuyetChungTu(MaChungTuAddNew, "Tạo mới", User.Identity.Name, Request.UserHostAddress);
                    }
                    //VungNV: 2015/10/21: If is old record then execute update to [CP_CapPhat] and [CP_CapPhatChiTiet] table
                    else
                    {
                        bang.GiaTriKhoa = iID_MaCapPhat;
                        bang.Save();
                        CapPhat_ChungTuChiTiet_DonViModels.DongBoChungTuChiTiet(iID_MaCapPhat);
                    }
                }
            return RedirectToAction("Index", "CapPhat_ChungTu_DonVi", new {Loai = Loai });
        }
        /// <summary>
        /// trình duyệt
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaCapPhat)
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


            return RedirectToAction("CapPhatChiTiet_Frame", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat = iID_MaCapPhat });
        }
        /// <summary>
        /// từ chối
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TuChoi(String iID_MaCapPhat)
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

            return RedirectToAction("CapPhatChiTiet_Frame", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat=iID_MaCapPhat});
        }
        /// <summary>
        /// Hiển thị chứng từ chi tiết view
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChungTuChiTiet()
        {
            return View(VIEW_ROOTPATH + VIEW_CHUNGTU_DONVI_CHUNGTUCHITIET_INDEX);
        }
        /// <summary>
        /// lưu chứng từ chi tiết
        /// 
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LuuChungTuChiTiet(String iID_MaCapPhat)
        {
            String TenBangChiTiet = "CP_CapPhatChiTiet";

            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauLaHangCha = Request.Form["idXauLaHangCha"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrLaHangCha = idXauLaHangCha.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            DataTable dtChungTu = CapPhat_ChungTuModels.LayToanBoThongTinChungTu(iID_MaCapPhat);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                if (arrMaHang[i] != "")
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;

                        String MaChungTuChiTiet = arrMaHang[i].Split('_')[0];

                        if (MaChungTuChiTiet == "")
                        {
                            String strTenPhongBan = Convert.ToString(PhongBanModels.Get_Table(Convert.ToString(dtChungTu.Rows[0]["iID_MaPhongBan"])).Rows[0]["sTen"]);
                            String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]));
                            String strTenTinhChatCapThu = "";
                            strTenTinhChatCapThu = Convert.ToString(TinhChatCapThuModels.Get_RowTinhChatCapThu(Convert.ToString(dtChungTu.Rows[0]["iID_MaTinhChatCapThu"])).Rows[0]["sTen"]);
                            Boolean bLoaiCapThu = Convert.ToBoolean(TinhChatCapThuModels.Get_RowTinhChatCapThu(Convert.ToString(dtChungTu.Rows[0]["iID_MaTinhChatCapThu"])).Rows[0]["bLoai"]);

                            //Them vao bang cua phan cap phat
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                            bang.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", dtChungTu.Rows[0]["iDM_MaLoaiCapPhat"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", strTenPhongBan);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);                            
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", strTenDonVi);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTinhChatCapThu", dtChungTu.Rows[0]["iID_MaTinhChatCapThu"]);
                            bang.CmdParams.Parameters.AddWithValue("@bLoai", bLoaiCapThu);
                            bang.CmdParams.Parameters.AddWithValue("@dNgayCapPhat", dtChungTu.Rows[0]["dNgayCapPhat"]);

                            String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                            DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                            //Dien thong tin cua Muc luc ngan sach
                            NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                            //Xet rieng ngan sach thuong xuyen
                            dtMucLuc.Dispose();
                        }
                        else
                        {
                            bang.GiaTriKhoa = MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].EndsWith("_ConLai") == false)
                                {
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1")
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                        }
                                    }
                                    else if (arrMaCot[j].StartsWith("r") || arrMaCot[j].StartsWith("i"))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }
                                }
                            }
                        }
                        bang.Save();
                    }
                }
            }

            dtChungTu.Dispose();
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat = iID_MaCapPhat });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat = iID_MaCapPhat });
            }
            return RedirectToAction("CapPhatChiTiet_Frame", new { iID_MaCapPhat = iID_MaCapPhat });
        }
        /// <summary>
        /// get_dtDonVi_PhongBan
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public JsonResult get_dtDonVi_PhongBan(String ParentID, String MaND, String iID_MaDonVi, String iNamLamViec)
        {
            return Json(LayDoiTuongDonVi_PhongBan(ParentID, MaND, iID_MaDonVi, iNamLamViec), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// LayDoiTuongDonVi_PhongBan
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static String LayDoiTuongDonVi_PhongBan(String ParentID, String MaND, String iID_MaDonVi, String iNamLamViec)
        {
            String strDanhMucDuAn = string.Empty;
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            if (string.IsNullOrEmpty(MaND) == false)
            {
                String strSQL = String.Format(@"SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaPhongBan = @iID_MaPhongBan AND iTrangThai = 1 AND iID_MaDonVi IN (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_DonVi AND  sMaNguoiDung = @sMaNguoiDung) ORDER BY iID_MaDonVi");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSQL;
                cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
               // DataTable dt = Connection.GetDataTable(cmd);
                DataTable dt = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTen"] = "--- Danh sách đơn vị ---";
                dt.Rows.InsertAt(R, 0);

                SelectOptionList slDanhMucDuan = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
                strDanhMucDuAn = MyHtmlHelper.DropDownList(ParentID, slDanhMucDuan, iID_MaDonVi, "iID_MaDonVi", null, "class=\"input1_2\"");
            }
            return strDanhMucDuAn;

        }
    }
}
