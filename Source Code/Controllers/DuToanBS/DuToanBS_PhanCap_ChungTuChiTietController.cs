using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBS_PhanCapChungTuChiTietController : Controller
    {
        //
        // GET: /DuToan_PhanCapChungTuChiTiet/
        public string sViewPath = "~/Views/DuToanBS/ChungTuChiTiet/";

        [Authorize]
        public ActionResult Index(String iID_MaChungTu)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            return View(sViewPath + "PhanCap_ChungTuChiTiet_Index.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTiet_Frame(String sLNS, String MaLoai)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaChungTu = Request.Form["DuToan_iID_MaChungTu"];
            MaLoai = Request.Form["DuToan_MaLoai"];
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["MaLoai"] = MaLoai;
            return View(sViewPath + "PhanCap_ChungTuChiTiet_Index_DanhSach_Frame.aspx", new { sLNS = sLNS });
            //return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, LoadLai = "1" });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ChiNganSach, String iID_MaChungTu, String sLNS)
        {
            DataTable dtCT = DuToanBS_ChungTuChiTietModels.Get_RowChungTuChiTiet(iID_MaChungTu);
            DataRow data = dtCT.Rows[0];
            String MaND = User.Identity.Name;
            String TenBangChiTiet = "DTBS_ChungTuChiTiet_PhanCap";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idMaMucLucNganSach = Request.Form["idMaMucLucNganSach"];

            String[] arrMaMucLucNganSach = idMaMucLucNganSach.Split(',');
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { CapPhat_BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaCapPhatChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachHang }, StringSplitOptions.None);


            String iKyThuat = Convert.ToString(data["iKyThuat"]);
            String MaLoai = Convert.ToString(data["MaLoai"]);
            String iID_MaPhongBan = "", sTenPhongBan="";
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                dtPhongBan.Dispose();
            }
            #region Nganh Ky thuat
            if (iKyThuat == "1" && MaLoai!="2")
            {
                for (int i = 0; i < arrMaHang.Length; i++)
                {
                    TenBangChiTiet = "DTBS_ChungTuChiTiet_PhanCap";
                    if (arrMaHang[i] != "")
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                        //Lay ma don vi vua nhap
                        String iID_MaDonVi = "";
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                            {
                                iID_MaDonVi = arrGiaTri[j];
                                break;
                            }
                        }


                        iID_MaCapPhatChiTiet = arrMaHang[i].Split('_')[0];
                        //lay ma don vi truong hop sua
                        if (!String.IsNullOrEmpty(iID_MaCapPhatChiTiet))
                        {
                            iID_MaDonVi = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaCapPhatChiTiet, "iID_MaDonVi"));
                        }
                        if (DuToanBS_ChungTuChiTietModels.CheckDonViBaoDam2Lan(iID_MaDonVi))
                        {
                            TenBangChiTiet = "DTBS_ChungTuChiTiet";
                        }
                        if (arrHangDaXoa[i] == "1")
                        {
                            //Lưu các hàng đã xóa
                            if (iID_MaCapPhatChiTiet != "")
                            {
                                //Dữ liệu đã có
                                Bang bang = new Bang(TenBangChiTiet);
                                bang.DuLieuMoi = false;
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                                bang.MaNguoiDungSua = User.Identity.Name;
                                bang.IPSua = Request.UserHostAddress;
                                bang.Save();
                            }
                        }
                        else
                        {
                            Boolean okCoThayDoi = false;
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {

                                if (arrThayDoi[j] == "1")
                                {
                                    if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                                    {
                                        if (arrGiaTri[j] == "")
                                        {
                                            if (iID_MaCapPhatChiTiet == "")
                                            {
                                                okCoThayDoi = false;
                                                break;
                                            }
                                            else
                                            {
                                                okCoThayDoi = true;
                                                break;

                                            }
                                        }

                                    }
                                    okCoThayDoi = true;

                                }

                            }
                            if (okCoThayDoi)
                            {

                                Bang bang = new Bang(TenBangChiTiet);
                                if (iID_MaCapPhatChiTiet == "")
                                {
                                    //Du Lieu Moi
                                    bang.DuLieuMoi = true;
                                    String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                                    DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                                    //Dien thong tin cua Muc luc ngan sach
                                    NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                                    dtMucLuc.Dispose();

                                    if (DuToanBS_ChungTuChiTietModels.CheckDonViBaoDam2Lan(iID_MaDonVi))
                                    {
                                        bang.CmdParams.Parameters.AddWithValue("@iKyThuat", 1);
                                        bang.CmdParams.Parameters.AddWithValue("@MaLoai", 1);
                                        if (bang.CmdParams.Parameters.IndexOf("@sLNS") >= 0)
                                        {
                                            bang.CmdParams.Parameters["@sLNS"].Value = DuToanBS_ChungTuChiTietModels.sLNSBaoDam;
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue("@sLNS", DuToanBS_ChungTuChiTietModels.sLNSBaoDam);
                                        }
                                        if (bang.CmdParams.Parameters.IndexOf("@sXauNoiMa") >= 0)
                                        {
                                            bang.CmdParams.Parameters["@sXauNoiMa"].Value = DuToanBS_ChungTuChiTietModels.sLNSBaoDam + "-" + data["sL"] + "-" + data["sK"] + "-" + data["sM"] + "-" + data["sTM"] + "-" + data["sTTM"] + "-" + data["sNG"];
                                        }
                                        else
                                        {
                                            bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", DuToanBS_ChungTuChiTietModels.sLNSBaoDam + "-" + data["sL"] + "-" + data["sK"] + "-" + data["sM"] + "-" + data["sTM"] + "-" + data["sTTM"] + "-" + data["sNG"]);
                                        }
                                    }
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                    //Them cac tham so tu bang CP_CapPhat

                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguon", data["iID_MaNguon"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                                    bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", data["iID_MaTrangThaiDuyet"]);
                                }
                                else
                                {
                                    //Du Lieu Da Co
                                    bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                    bang.DuLieuMoi = false;
                                }
                                bang.MaNguoiDungSua = User.Identity.Name;
                                bang.IPSua = Request.UserHostAddress;

                                //Them tham so
                                for (int j = 0; j < arrMaCot.Length; j++)
                                {
                                    if (arrThayDoi[j] == "1")
                                    {
                                        if (arrMaCot[j].EndsWith("_ConLai") == false)
                                        {
                                            String Truong = "@" + arrMaCot[j];
                                            //doi lai ten truong
                                            if (arrMaCot[j] == "sTenDonVi_BaoDam")
                                            {
                                                Truong = "@sTenDonVi";
                                            }
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
                                            else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
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
                                String iID_MaChungTuNew = Convert.ToString(bang.Save());
                                //Ngan sách ngành sẽ thêm 1 dòng ngân sách sử dung o bảng phân cấp
                                if (TenBangChiTiet == "DTBS_ChungTuChiTiet")
                                {
                                    String SQL = String.Format(@"INSERT INTO DTBS_ChungTuChiTiet_PhanCap
(
      [iID_MaDotNganSach]
      ,[iID_MaChungTu]
      ,[iID_MaPhongBan]
      ,[sTenPhongBan]
      ,[iID_MaPhongBanDich]
      ,[iID_MaTrangThaiDuyet]
      ,[iNamLamViec]
      ,[iID_MaNguonNganSach]
      ,[iID_MaNamNganSach]
      ,[bChiNganSach]
      ,[iID_MaDuyetChungTuChiTiet]
      ,[bDongY]
      ,[sLyDo]
      ,[sGhiChu]
      ,[iID_MaDonVi]
      ,[sTenDonVi]
      ,[iID_MaMucLucNganSach]
      ,[iID_MaMucLucNganSach_Cha]
      ,[sXauNoiMa]
      ,[bLaHangCha]
      ,[sLNS]
      ,[sL]
      ,[sK]
      ,[sM]
      ,[sTM]
      ,[sTTM]
      ,[sNG]
      ,[sTNG]
      ,[sMoTa]
      ,[rTongSoNamTruoc]
      ,[rUocThucHien]
      ,[sMaCongTrinh]
      ,[sTenCongTrinh]
      ,[rNgay]
      ,[rSoNguoi]
      ,[rChiTaiKhoBac]
      ,[rTonKho]
      ,[rTuChi]
      ,[rChiTapTrung]
      ,[rHangNhap]
      ,[rHangMua]
      ,[rHienVat]
      ,[rDuPhong]
      ,[rPhanCap]
      ,[rTienThu]
      ,[rTongSo]
      ,[rNgay_DonVi]
      ,[rSoNguoi_DonVi]
      ,[rChiTaiKhoBac_DonVi]
      ,[rTonKho_DonVi]
      ,[rTuChi_DonVi]
      ,[rChiTapTrung_DonVi]
      ,[rHangNhap_DonVi]
      ,[rHangMua_DonVi]
      ,[rHienVat_DonVi]
      ,[rDuPhong_DonVi]
      ,[rPhanCap_DonVi]
      ,[rTienThu_DonVi]
      ,[rTongSo_DonVi]
      ,[bsMaCongTrinh]
      ,[bsTenCongTrinh]
      ,[brNgay]
      ,[brSoNguoi]
      ,[brChiTaiKhoBac]
      ,[brTonKho]
      ,[brTuChi]
      ,[brChiTapTrung]
      ,[brHangNhap]
      ,[brHangMua]
      ,[brHienVat]
      ,[brDuPhong]
      ,[brPhanCap]
      ,[brTienThu]
      ,[bsMaCongTrinh_DonVi]
      ,[bsTenCongTrinh_DonVi]
      ,[brNgay_DonVi]
      ,[brSoNguoi_DonVi]
      ,[brChiTaiKhoBac_DonVi]
      ,[brTonKho_DonVi]
      ,[brTuChi_DonVi]
      ,[brChiTapTrung_DonVi]
      ,[brHangNhap_DonVi]
      ,[brHangMua_DonVi]
      ,[brHienVat_DonVi]
      ,[brDuPhong_DonVi]
      ,[brPhanCap_DonVi]
      ,[brTienThu_DonVi]
      ,[iSTT]
      ,[iTrangThai]
      ,[bPublic]
      ,[iID_MaNhomNguoiDung_Public]
      ,[iID_MaNhomNguoiDung_DuocGiao]
      ,[sID_MaNguoiDung_DuocGiao]
      ,[dNgayTao]
      ,[sID_MaNguoiDungTao]
      ,[iSoLanSua]
      ,[dNgaySua]
      ,[sIPSua]
      ,[sID_MaNguoiDungSua]
      ,[MaLoai])
     
       
      SELECT 
      [iID_MaDotNganSach]
      ,iID_MaChungTuChiTiet
      ,[iID_MaPhongBan]
      ,[sTenPhongBan]
      ,[iID_MaPhongBanDich]
      ,[iID_MaTrangThaiDuyet]
      ,[iNamLamViec]
      ,[iID_MaNguonNganSach]
      ,[iID_MaNamNganSach]
      ,[bChiNganSach]
      ,[iID_MaDuyetChungTuChiTiet]
      ,[bDongY]
      ,[sLyDo]
      ,[sGhiChu]
      ,[iID_MaDonVi]
      ,[sTenDonVi]
      ,[iID_MaMucLucNganSach]
      ,[iID_MaMucLucNganSach_Cha]
      ,[sXauNoiMa]
      ,[bLaHangCha]
      ,sLNS='1020100'
      ,[sL]
      ,[sK]
      ,[sM]
      ,[sTM]
      ,[sTTM]
      ,[sNG]
      ,[sTNG]
      ,[sMoTa]
      ,[rTongSoNamTruoc]
      ,[rUocThucHien]
      ,[sMaCongTrinh]
      ,[sTenCongTrinh]
      ,[rNgay]
      ,[rSoNguoi]
      ,[rChiTaiKhoBac]
      ,[rTonKho]
      ,[rTuChi]
      ,[rChiTapTrung]
      ,[rHangNhap]
      ,[rHangMua]
      ,[rHienVat]
      ,[rDuPhong]
      ,[rPhanCap]
      ,[rTienThu]
      ,[rTongSo]
      ,[rNgay_DonVi]
      ,[rSoNguoi_DonVi]
      ,[rChiTaiKhoBac_DonVi]
      ,[rTonKho_DonVi]
      ,[rTuChi_DonVi]
      ,[rChiTapTrung_DonVi]
      ,[rHangNhap_DonVi]
      ,[rHangMua_DonVi]
      ,[rHienVat_DonVi]
      ,[rDuPhong_DonVi]
      ,[rPhanCap_DonVi]
      ,[rTienThu_DonVi]
      ,[rTongSo_DonVi]
      ,[bsMaCongTrinh]
      ,[bsTenCongTrinh]
      ,[brNgay]
      ,[brSoNguoi]
      ,[brChiTaiKhoBac]
      ,[brTonKho]
      ,[brTuChi]
      ,[brChiTapTrung]
      ,[brHangNhap]
      ,[brHangMua]
      ,[brHienVat]
      ,[brDuPhong]
      ,[brPhanCap]
      ,[brTienThu]
      ,[bsMaCongTrinh_DonVi]
      ,[bsTenCongTrinh_DonVi]
      ,[brNgay_DonVi]
      ,[brSoNguoi_DonVi]
      ,[brChiTaiKhoBac_DonVi]
      ,[brTonKho_DonVi]
      ,[brTuChi_DonVi]
      ,[brChiTapTrung_DonVi]
      ,[brHangNhap_DonVi]
      ,[brHangMua_DonVi]
      ,[brHienVat_DonVi]
      ,[brDuPhong_DonVi]
      ,[brPhanCap_DonVi]
      ,[brTienThu_DonVi]
      ,[iSTT]
      ,[iTrangThai]
      ,[bPublic]
      ,[iID_MaNhomNguoiDung_Public]
      ,[iID_MaNhomNguoiDung_DuocGiao]
      ,[sID_MaNguoiDung_DuocGiao]
      ,[dNgayTao]
      ,[sID_MaNguoiDungTao]
      ,[iSoLanSua]
      ,[dNgaySua]
      ,[sIPSua]
      ,[sID_MaNguoiDungSua]
      ,MaLoai=2
       FROM DTBS_ChungTuChiTiet
WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet");
                                    SqlCommand cmd = new SqlCommand(SQL);
                                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuNew);
                                    Connection.UpdateDatabase(cmd);
                                }
                            }
                        }
                    }
                }
            }

            #endregion
            #region Nganh khac
            else
            {

                for (int i = 0; i < arrMaHang.Length; i++)
                {
                    if (arrMaHang[i] != "")
                    {
                        iID_MaCapPhatChiTiet = arrMaHang[i].Split('_')[0];
                        if (arrHangDaXoa[i] == "1")
                        {
                            //Lưu các hàng đã xóa
                            if (iID_MaCapPhatChiTiet != "")
                            {
                                //Dữ liệu đã có
                                Bang bang = new Bang(TenBangChiTiet);
                                bang.DuLieuMoi = false;
                                bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                                bang.MaNguoiDungSua = User.Identity.Name;
                                bang.IPSua = Request.UserHostAddress;
                                bang.Save();
                            }
                        }
                        else
                        {
                            String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                            String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { PhanBo_ChiTieu_BangDuLieu.DauCachO }, StringSplitOptions.None);
                            Boolean okCoThayDoi = false;
                            for (int j = 0; j < arrMaCot.Length; j++)
                            {

                                if (arrThayDoi[j] == "1")
                                {
                                    if (arrMaCot[j].StartsWith("iID_MaDonVi"))
                                    {
                                        if (arrGiaTri[j] == "")
                                        {
                                            if (iID_MaCapPhatChiTiet == "")
                                            {
                                                okCoThayDoi = false;
                                                break;
                                            }
                                            else
                                            {
                                                okCoThayDoi = true;
                                                break;

                                            }
                                        }


                                    }
                                    okCoThayDoi = true;
                                }

                            }
                            if (okCoThayDoi)
                            {

                                Bang bang = new Bang(TenBangChiTiet);
                                if (iID_MaCapPhatChiTiet == "")
                                {
                                    //Du Lieu Moi
                                    bang.DuLieuMoi = true;
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                    //Them cac tham so tu bang CP_CapPhat
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguon", data["iID_MaNguon"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                                    bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", data["bChiNganSach"]);
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", data["iID_MaTrangThaiDuyet"]);
                                    bang.CmdParams.Parameters.AddWithValue("@MaLoai", MaLoai);
                                }
                                else
                                {
                                    //Du Lieu Da Co
                                    bang.GiaTriKhoa = iID_MaCapPhatChiTiet;
                                    bang.DuLieuMoi = false;
                                }
                                bang.MaNguoiDungSua = User.Identity.Name;
                                bang.IPSua = Request.UserHostAddress;

                                if (iID_MaCapPhatChiTiet == "")
                                {
                                    //Xác định xâu mã nối


                                    String iID_MaMucLucNganSach = arrMaHang[i].Split('_')[1];

                                    DataTable dtMucLuc = MucLucNganSachModels.dt_ChiTietMucLucNganSach(iID_MaMucLucNganSach);
                                    //Dien thong tin cua Muc luc ngan sach
                                    NganSach_HamChungModels.ThemThongTinCuaMucLucNganSach(dtMucLuc.Rows[0], bang.CmdParams.Parameters);
                                    dtMucLuc.Dispose();
                                }


                                //Them tham so
                                for (int j = 0; j < arrMaCot.Length; j++)
                                {
                                    if (arrThayDoi[j] == "1")
                                    {
                                        if (arrMaCot[j].EndsWith("_ConLai") == false)
                                        {
                                            String Truong = "@" + arrMaCot[j];
                                            //doi lai ten truong
                                            if (arrMaCot[j] == "sTenDonVi_BaoDam")
                                            {
                                                Truong = "@sTenDonVi";
                                            }
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
                                            else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
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
                }
            }
            #endregion
         
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaChungTu, string iLoai)
        {
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sLNS = Request.Form[ParentID + "_sLNS"];
            String sL = Request.Form[ParentID + "_sL"];
            String sK = Request.Form[ParentID + "_sK"];
            String sM = Request.Form[ParentID + "_sM"];
            String sTM = Request.Form[ParentID + "_sTM"];
            String sTTM = Request.Form[ParentID + "_sTTM"];
            String sNG = Request.Form[ParentID + "_sNG"];
            String sTNG = Request.Form[ParentID + "_sTNG"];

            return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }

    }
}
