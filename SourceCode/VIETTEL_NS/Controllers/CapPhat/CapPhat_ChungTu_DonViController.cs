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
        public string sViewPath = "~/Views/CapPhat/ChungTu_DonVi/";

        [Authorize]
        public ActionResult Index()
        {
                return View(sViewPath + "CapPhat_ChungTu_DonVi_Index.aspx");
        }

        public ActionResult CapPhatChiTiet_Frame(String iID_MaCapPhat)
        {
            return View(sViewPath + "CapPhat_DonVi_Index_DanhSach_Frame.aspx");
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

            return RedirectToAction("Index", "CapPhat_ChungTu_Donvi", new { MaPhongBan = MaPhongBan, SoCapPhat = SoCapPhat, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, DonVi = DonVi });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return View(sViewPath + "CapPhat_ChungTu_DonVi_Duyet.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID, String DonVi)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoCapPhat = Request.Form[ParentID + "_iSoCapPhat"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iDM_MaLoaiCapPhat = Request.Form[ParentID + "_iDM_MaLoaiCapPhat"];
            String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(User.Identity.Name);
            return RedirectToAction("Duyet", "CapPhat_ChungTu_Donvi", new { MaPhongBan = MaPhongBan, SoCapPhat = SoCapPhat, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, DonVi = DonVi });
        }

        [Authorize]
        public ActionResult Edit(String iID_MaCapPhat, String Loai)
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

            return View(sViewPath + "CapPhat_ChungTu_DonVi_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaCapPhat, String Loai)
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
                
                return View(sViewPath + "CapPhat_ChungTu_DonVi_Edit.aspx");
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
                        
                        CapPhat_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Tạo mới", User.Identity.Name, Request.UserHostAddress);
                    }
                    //VungNV: 2015/10/21: If is old record then execute update to [CP_CapPhat] and [CP_CapPhatChiTiet] table
                    else
                    {
                        bang.GiaTriKhoa = iID_MaCapPhat;
                        bang.Save();

                        DataTable dtChungTu = CapPhat_ChungTuChiTiet_DonViModels.GetChungTu(iID_MaCapPhat);

                        String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dtChungTu.Rows[0]["iID_MaDonVi"]));

                        SqlCommand cmd;
                        String SQL = "UPDATE CP_CapPhatChiTiet SET iDM_MaLoaiCapPhat=@iDM_MaLoaiCapPhat, iID_MaPhongBan=@iID_MaPhongBan, iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, " +
                                "iNamLamViec=@iNamLamViec,iID_MaNguonNganSach=@iID_MaNguonNganSach,iID_MaNamNganSach=@iID_MaNamNganSach,bChiNganSach=@bChiNganSach, " +
                                "iID_MaDonVi=@iID_MaDonVi, sTenDonVi=@sTenDonVi " +
                                "WHERE iID_MaCapPhat=@iID_MaCapPhat";

                        cmd = new SqlCommand();
                        cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", dtChungTu.Rows[0]["iDM_MaLoaiCapPhat"]);
                        cmd.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                        cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                        cmd.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                        cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                        cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                        cmd.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                        cmd.Parameters.AddWithValue("@sTenDonVi", strTenDonVi);
                        cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                        cmd.CommandText = SQL;
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                        dtChungTu.Dispose();
                    }
                }
            return RedirectToAction("Index", "CapPhat_ChungTu_DonVi", new {Loai = Loai });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchChiTietSubmit(String ParentID, String iID_MaCapPhat)
        {
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Detail", new { iID_MaCapPhat = iID_MaCapPhat  , sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaCapPhat)
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


            return RedirectToAction("CapPhatChiTiet_Frame", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat = iID_MaCapPhat });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaCapPhat)
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

            return RedirectToAction("CapPhatChiTiet_Frame", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat=iID_MaCapPhat});
        }

        [Authorize]
        public ActionResult Detail()
        {
            return View(sViewPath + "CapPhat_DonVi_ChungTuChiTiet_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaCapPhat)
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

            DataTable dtChungTu = CapPhat_ChungTuChiTiet_DonViModels.GetChungTu(iID_MaCapPhat);
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

                        //Bang bang1 = new Bang("KTTG_ChungTuChiTietCapThu");
                        //bang1.MaNguoiDungSua = User.Identity.Name;
                        //bang1.IPSua = Request.UserHostAddress;

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

                            //Them vao bang cua phan cap thu
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaCapPhat);
                            //bang1.CmdParams.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", dtChungTu.Rows[0]["iDM_MaLoaiCapPhat"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@sTenPhongBan", strTenPhongBan);
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@bChiNganSach", dtChungTu.Rows[0]["bChiNganSach"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", dtChungTu.Rows[0]["iID_MaDonVi"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@sTenDonVi", strTenDonVi);
                            //bang1.CmdParams.Parameters.AddWithValue("@iID_MaTinhChatCapThu", dtChungTu.Rows[0]["iID_MaTinhChatCapThu"]);
                            //bang1.CmdParams.Parameters.AddWithValue("@sTinhChatCapThu", strTenTinhChatCapThu);
                            //bang1.CmdParams.Parameters.AddWithValue("@bLoai", bLoaiCapThu);
                            //bang1.CmdParams.Parameters.AddWithValue("@iNgay", Convert.ToDateTime(dtChungTu.Rows[0]["dNgayCapPhat"]).Day);
                            //bang1.CmdParams.Parameters.AddWithValue("@iThang", Convert.ToDateTime(dtChungTu.Rows[0]["dNgayCapPhat"]).Month);

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

        public JsonResult get_dtDonVi_PhongBan(String ParentID, String MaND, String iID_MaDonVi, String iNamLamViec)
        {
            return Json(get_objDonVi_PhongBan(ParentID, MaND, iID_MaDonVi, iNamLamViec), JsonRequestBehavior.AllowGet);
        }

        public static String get_objDonVi_PhongBan(String ParentID, String MaND, String iID_MaDonVi, String iNamLamViec)
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
