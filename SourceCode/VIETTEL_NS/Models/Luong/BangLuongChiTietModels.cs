using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class BangLuongChiTietModels
    {
        /// <summary>
        /// Dong bo tu phan he nhan su
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="UserID"></param>
        /// <param name="IPSua"></param>
        public static void ThemChiTiet(String iID_MaBangLuong, String UserID, String IPSua, int iNamLamViec)
        {
            DataTable dtLuong = LuongModels.Get_ChiTietBangLuong(iID_MaBangLuong);
            if (dtLuong.Rows.Count > 0)
            {
                DataTable dtDonVi = LuongModels.Get_DSDonViCuaBangLuong(iID_MaBangLuong, iNamLamViec);
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    String iID_MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]);
                    DataTable dtCanBo = LuongModels.Get_dtCanBoDangCongTacCuaDonVi(iID_MaDonVi);
                    if (dtCanBo != null && dtCanBo.Rows.Count > 0)
                    {
                        for (int iCanBo = 0; iCanBo < dtCanBo.Rows.Count; iCanBo++)
                        {
                            String iID_MaCanBo = Convert.ToString(dtCanBo.Rows[iCanBo]["iID_MaCanBo"]);
                            BangLuongChiTiet_CaNhanModels.ThemLuongChiTiet_CanBo(iID_MaBangLuong, iID_MaCanBo, UserID,
                                                                                 IPSua);
                        }
                    }
                    else
                    {
                        int iThangBangLuong = Convert.ToInt32(dtLuong.Rows[0]["iThangBangLuong"]);
                        int iNamBangLuong = Convert.ToInt32(dtLuong.Rows[0]["iNamBangLuong"]);
                        DateTime ThoiGian = new DateTime(iNamBangLuong, iThangBangLuong, 1);
                        int iSoNgayTrongThang = CDate.LaySoNgayTrongThang(ThoiGian);

                        //Lấy thông tin bảng lương tháng trước
                        DateTime ThoiGian_BangLuongTruoc = ThoiGian.AddMonths(-1);
                        int iThangBangLuong_Truoc = ThoiGian_BangLuongTruoc.Month;
                        int iNamBangLuong_Truoc = ThoiGian_BangLuongTruoc.Year;
                        String iID_MaBangLuong_ThangTruoc = LuongModels.Get_iID_MaBangLuong(iID_MaDonVi, iNamBangLuong_Truoc, iThangBangLuong_Truoc);
                        if (!String.IsNullOrEmpty(iID_MaBangLuong_ThangTruoc))
                        {
                            BangLuongChiTiet_CaNhanModels.ThemLuongChiTiet_CanBo(iID_MaBangLuong, "", UserID,
                                                                                 IPSua);
                        }

                    }
                    if (dtCanBo != null) dtCanBo.Dispose();
                }
                dtDonVi.Dispose();
            }
            dtLuong.Dispose();
        }

        public static DataTable Get_DataTable(String iID_MaBangLuong, String iID_MaDonVi = "", String iID_MaCanBo = "")
        {
            DataTable dt = null;
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            DK = "iID_MaBangLuong=@iID_MaBangLuong";
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iID_MaCanBo) == false)
            {
                DK += " AND iID_MaCanBo=@iID_MaCanBo";
                cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            }
            String SQL = "SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_DataTable_PhuCap(String iID_MaBangLuong="", String iID_MaDonVi = "", String iID_MaCanBo = "", String iID_MaBangLuongChiTiet="")
        {
            DataTable dt = null;
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaBangLuong) == false)
            {
                DK = " AND iID_MaBangLuong=@iID_MaBangLuong";
                cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            }
            if (String.IsNullOrEmpty(iID_MaBangLuongChiTiet) == false)
            {
                DK = " AND iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet";
                cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
            }
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi=@iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iID_MaCanBo) == false)
            {
                DK += " AND iID_MaCanBo=@iID_MaCanBo";
                cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            }
            String SQL = "SELECT * FROM L_BangLuongChiTiet_PhuCap WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        
        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaBangLuong)
        {
            int vR = -1;
            DataTable dt =LuongModels.Get_ChiTietBangLuong(iID_MaBangLuong);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeLuong, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM L_BangLuongChiTiet WHERE iID_MaBangLuong=@iID_MaBangLuong AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaBangLuong)
        {
            int vR = -1;
            DataTable dt = LuongModels.Get_ChiTietBangLuong(iID_MaBangLuong);

            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeLuong, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static void TrichLuong(String iID_MaBangLuong, String iTrichLuong_Loai, String rTrichLuong_SoLuong, String UserID, String IPSua)
        {
            DataTable dt = LuongModels.Get_dtBangLuongChiTiet(iID_MaBangLuong, null, Luong_BangDuLieu.iLoaiBangLuong_BangChiTiet);
            DataTable dtPhuCap = BangLuongChiTietModels.Get_DataTable_PhuCap(iID_MaBangLuong);
            //Update trích lương vào bảng lương;
            UpdateTrichLuong(iID_MaBangLuong, iTrichLuong_Loai, rTrichLuong_SoLuong, UserID, IPSua);
            //Cập nhật lại bảng lương
            String iID_MaBangLuongChiTiet;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
               iID_MaBangLuongChiTiet=Convert.ToString(dt.Rows[i]["iID_MaBangLuongChiTiet"]);
               BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, dt.Rows[i],dtPhuCap);
            }
        }

        public static void UpdateTrichLuong(String iID_MaBangLuong,String iTrichLuong_Loai, String rTrichLuong_SoLuong, String UserID, String IPSua)
        {
            String SQL = "UPDATE L_BangLuongChiTiet SET iTrichLuong_Loai=@iTrichLuong_Loai,rTrichLuong_SoLuong=@rTrichLuong_SoLuong";            
            SQL += " WHERE iID_MaBangLuong=@iID_MaBangLuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            cmd.Parameters.AddWithValue("@iTrichLuong_Loai", iTrichLuong_Loai);
            cmd.Parameters.AddWithValue("@rTrichLuong_SoLuong", rTrichLuong_SoLuong);
            Connection.UpdateDatabase(cmd, UserID, IPSua);
            cmd.Dispose();                             
        }

        public static void DieuChinhTienAn1Ngay(String iID_MaBangLuong, String iID_MaDonVi, String rTienAn1Ngay, String UserID, String IPSua)
        {
            DataTable dt = LuongModels.Get_dtBangLuongChiTiet(iID_MaBangLuong, null, -1);
            DataTable dtPhuCap = BangLuongChiTietModels.Get_DataTable_PhuCap(iID_MaBangLuong, "", "");
            //Update tiền ăn một ngày vào bảng lương;
            UpdateTienAn(iID_MaBangLuong, iID_MaDonVi, rTienAn1Ngay, UserID, IPSua);
            //Cập nhật lại bảng lương
            String iID_MaBangLuongChiTiet;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iID_MaBangLuongChiTiet = Convert.ToString(dt.Rows[i]["iID_MaBangLuongChiTiet"]);
                BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, dt.Rows[i], dtPhuCap);
            }
        }

        private static void UpdateTienAn_DonVi(String iID_MaBangLuong, String iID_MaDonVi, String rTienAn1Ngay, String UserID, String IPSua)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                String SQL = "UPDATE L_BangLuongChiTiet SET rTienAn1Ngay=@rTienAn1Ngay";
                SQL += " WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaDonVi=@iID_MaDonVi";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@rTienAn1Ngay", rTienAn1Ngay);
                Connection.UpdateDatabase(cmd, UserID, IPSua);
                cmd.Dispose(); 
            }
        }

        public static void UpdateTienAn(String iID_MaBangLuong, String iID_MaDonVi, String rTienAn1Ngay, String UserID, String IPSua)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                UpdateTienAn_DonVi(iID_MaBangLuong, iID_MaDonVi, rTienAn1Ngay, UserID, IPSua);
            }
            else
            {
                DataTable dtDonVi = LuongModels.LayDanhSachDonViCuaBangLuong(iID_MaBangLuong);
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    UpdateTienAn_DonVi(iID_MaBangLuong, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]), rTienAn1Ngay, UserID, IPSua);
                }
                dtDonVi.Dispose();
            }
            
        }

        public static void HeSoKhuVuc(String iID_MaBangLuong, String iID_MaDonVi, String rPhuCap_KhuVuc_HeSo, String UserID, String IPSua)
        {
            DataTable dt = LuongModels.Get_dtBangLuongChiTiet(iID_MaBangLuong, null, -1);
            DataTable dtPhuCap = BangLuongChiTietModels.Get_DataTable_PhuCap(iID_MaBangLuong, "", "", "");
            //Update Hệ số khu vực vào bảng lương;
            UpdateHeSoKhuVuc(iID_MaBangLuong, iID_MaDonVi, rPhuCap_KhuVuc_HeSo, UserID, IPSua);
            //Cập nhật lại bảng lương
            String iID_MaBangLuongChiTiet;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iID_MaBangLuongChiTiet = Convert.ToString(dt.Rows[i]["iID_MaBangLuongChiTiet"]);
                BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, dt.Rows[i], dtPhuCap);
            }
        }

        private static void UpdateHeSoKhuVuc_DonVi(String iID_MaBangLuong, String iID_MaDonVi, String rPhuCap_KhuVuc_HeSo, String UserID, String IPSua)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                String SQL = "UPDATE L_BangLuongChiTiet SET rPhuCap_KhuVuc_HeSo=@rPhuCap_KhuVuc_HeSo";
                SQL += " WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaDonVi=@iID_MaDonVi";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@rPhuCap_KhuVuc_HeSo", rPhuCap_KhuVuc_HeSo);
                Connection.UpdateDatabase(cmd, UserID, IPSua);
                cmd.Dispose();

                double rGiaTri = CommonFunction.IsNumeric(rPhuCap_KhuVuc_HeSo) ? Convert.ToDouble(rPhuCap_KhuVuc_HeSo) : 0;
                cmd = new SqlCommand("UPDATE L_BangLuongChiTiet_PhuCap SET rHeSo=@rHeSo WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaDonVi=@iID_MaDonVi AND sMaTruongHeSo_BangLuong=@sMaTruongHeSo_BangLuong");
                cmd.Parameters.AddWithValue("@rHeSo", rGiaTri);
                cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@sMaTruongHeSo_BangLuong", "rPhuCap_KhuVuc_HeSo");
                Connection.UpdateDatabase(cmd, UserID, IPSua);
            }
        }

        public static void UpdateHeSoKhuVuc(String iID_MaBangLuong, String iID_MaDonVi, String rPhuCap_KhuVuc_HeSo, String UserID, String IPSua)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                UpdateHeSoKhuVuc_DonVi(iID_MaBangLuong, iID_MaDonVi, rPhuCap_KhuVuc_HeSo, UserID, IPSua);
            }
            else
            {
                DataTable dtDonVi = LuongModels.LayDanhSachDonViCuaBangLuong(iID_MaBangLuong);
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    UpdateHeSoKhuVuc_DonVi(iID_MaBangLuong, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]), rPhuCap_KhuVuc_HeSo, UserID, IPSua);
                }
                dtDonVi.Dispose();
            }
        }

        public static void HuyTapThe(String iID_MaBangLuong, String iID_MaDonVi, String iID_MaNgachLuong_CanBo, String sHieuTangGiam, String sKyHieu_MucLucQuanSo_HieuTangGiam, String UserID, String IPSua)
        {
            DataTable dt = LuongModels.Get_dtBangLuongChiTiet(iID_MaNgachLuong_CanBo, null, -1);
            DataTable dtPhuCap = BangLuongChiTietModels.Get_DataTable_PhuCap(iID_MaBangLuong);
            //Update hủy tập thể vào bảng lương;
            Update_HuyTapThe(iID_MaBangLuong, iID_MaDonVi, iID_MaNgachLuong_CanBo, sHieuTangGiam, sKyHieu_MucLucQuanSo_HieuTangGiam, UserID, IPSua);
            //Cập nhật lại bảng lương
            String iID_MaBangLuongChiTiet;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iID_MaBangLuongChiTiet = Convert.ToString(dt.Rows[i]["iID_MaBangLuongChiTiet"]);
                BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, dt.Rows[i], dtPhuCap);
            }
        }

        private static void Update_HuyTapThe_DonVi(String iID_MaBangLuong, String iID_MaDonVi, String iID_MaNgachLuong_CanBo, String sHieuTangGiam, String sKyHieu_MucLucQuanSo_HieuTangGiam, String UserID, String IPSua)
        {
            String SQL = "UPDATE L_BangLuongChiTiet SET sHieuTangGiam=@sHieuTangGiam,sKyHieu_MucLucQuanSo_HieuTangGiam=sKyHieu_MucLucQuanSo_HieuTangGiam";
            SQL += " WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaDonVi=@iID_MaDonVi AND iID_MaNgachLuong_CanBo=@iID_MaNgachLuong_CanBo";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iID_MaNgachLuong_CanBo", iID_MaNgachLuong_CanBo);
            cmd.Parameters.AddWithValue("@sKyHieu_MucLucQuanSo_HieuTangGiam", sKyHieu_MucLucQuanSo_HieuTangGiam);
            cmd.Parameters.AddWithValue("@sHieuTangGiam", sHieuTangGiam);
            Connection.UpdateDatabase(cmd, UserID, IPSua);
            cmd.Dispose();
        }

        public static void Update_HuyTapThe(String iID_MaBangLuong, String iID_MaDonVi, String iID_MaNgachLuong_CanBo, String sHieuTangGiam, String sKyHieu_MucLucQuanSo_HieuTangGiam, String UserID, String IPSua)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                Update_HuyTapThe_DonVi(iID_MaBangLuong, iID_MaDonVi,iID_MaNgachLuong_CanBo,sHieuTangGiam,sKyHieu_MucLucQuanSo_HieuTangGiam, UserID, IPSua);
            }
            else
            {
                DataTable dtDonVi = LuongModels.LayDanhSachDonViCuaBangLuong(iID_MaBangLuong);
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    Update_HuyTapThe_DonVi(iID_MaBangLuong, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]), iID_MaNgachLuong_CanBo, sHieuTangGiam, sKyHieu_MucLucQuanSo_HieuTangGiam, UserID, IPSua);
                }
                dtDonVi.Dispose();
            }
        }

        public static String Lay_iID_MaBangLuongChiTiet_Tiep(String iID_MaBangLuong, String iID_MaBangLuongChiTiet)
        {
            String vR = "";
            Dictionary<String, String> dcTimKiem=null;
            DataTable dt = LuongModels.Get_dtBangLuongChiTiet(iID_MaBangLuong, dcTimKiem, Luong_BangDuLieu.iLoaiBangLuong_BangChiTiet);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (iID_MaBangLuongChiTiet == Convert.ToString(dt.Rows[i]["iID_MaBangLuongChiTiet"]))
                {
                    if (i+1 < dt.Rows.Count)
                    {
                        vR = Convert.ToString(dt.Rows[i+1]["iID_MaBangLuongChiTiet"]);
                    }
                    break;
                }
            }
            return vR;
        }

        /// <summary>
        /// Điều chỉnh lại bảng LuongChiTiet 
        ///     - Nếu bOmDaiNgay
        ///         + 1: Ốm ngắn ngày không tính được cho bằng 0
        ///         + 0: Bảo hiểm chi được cho hết bằng 0
        /// </summary>
        /// <param name="bang"></param>
        public static void DieuChinhLaiBangTruocKhiGhi(Bang bang, int iNamBangLuong, int iThangBangLuong)
        {
            if (bang.CmdParams.Parameters.IndexOf("@iID_MaLyDoTangGiam") >= 0 && Convert.ToBoolean(bang.CmdParams.Parameters["@bOmDaiNgay"].Value))
            {
            }
            if (bang.CmdParams.Parameters.IndexOf("@bOmDaiNgay") >=0 && Convert.ToBoolean(bang.CmdParams.Parameters["@bOmDaiNgay"].Value))
            {
                //if (bang.CmdParams.Parameters.IndexOf("@iSoNgayNghiOm") >= 0)
                //{
                //    bang.CmdParams.Parameters["@iSoNgayNghiOm"].Value = 0;
                //}
                //else
                //{
                //    bang.CmdParams.Parameters.AddWithValue("@iSoNgayNghiOm", 0);
                //}
            }
            if (bang.CmdParams.Parameters.IndexOf("@bOmDaiNgay") >= 0 && Convert.ToBoolean(bang.CmdParams.Parameters["@bOmDaiNgay"].Value)==false)
            {
                List<String> arrTruong = bang.bangCSDL.DanhSachTruong();
                for (int i = 0; i < arrTruong.Count; i++)
                {
                    if (arrTruong[i].StartsWith("rBaoHiemChi_"))
                    {
                        String ThamSo = "@" + arrTruong[i];
                        if (bang.CmdParams.Parameters.IndexOf(ThamSo) >= 0)
                        {
                            bang.CmdParams.Parameters[ThamSo].Value = 0;
                        }
                        else
                        {
                            bang.CmdParams.Parameters.AddWithValue(ThamSo, 0);
                        }
                    }
                }
            }

            //<--Tính Số năm thâm niên                    
            if (bang.CmdParams.Parameters.IndexOf("@dNgayNhapNgu_CanBo") >= 0)
            {
                object dNgayNhapNgu = bang.CmdParams.Parameters["@dNgayNhapNgu_CanBo"].Value;
                object dNgayXuatNgu = bang.CmdParams.Parameters["@dNgayXuatNgu_CanBo"].Value;
                object dNgayTaiNgu = bang.CmdParams.Parameters["@dNgayTaiNgu_CanBo"].Value;

                Object sNXTNgu_Cu = CommonFunction.ThemGiaTriVaoThamSo(bang.CmdParams.Parameters, "@sNXTNgu", LuongModels.get_sNXTNgu(dNgayNhapNgu, dNgayXuatNgu, dNgayTaiNgu));
                Object iSoNamThamNien_Cu = CommonFunction.ThemGiaTriVaoThamSo(bang.CmdParams.Parameters, "@iSoNamThamNien", LuongModels.get_NamThamNien(iThangBangLuong, iNamBangLuong, dNgayNhapNgu, dNgayXuatNgu, dNgayTaiNgu));
            }
            //-->Tính Số năm thâm niên
        }
        /// <summary>
        /// Cập nhập bảng phụ cấp: iID_MaDonVi, iID_MaNgachLuong_CanBo, iID_MaBacLuong_CanBo
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        private static void QuyetToan_CapNhapBangPhuCap(int iThangBangLuong, int iNamBangLuong, String MaND, String IPSua)
        {
            //ref double rNguoi,
            SqlCommand cmd = new SqlCommand("SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong");
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            cmd = new SqlCommand("UPDATE L_BangLuongChiTiet_PhuCap " + 
                                 "SET iID_MaDonVi=@iID_MaDonVi, " +
                                     "iID_MaNgachLuong_CanBo=@iID_MaNgachLuong_CanBo, "+
                                     "iID_MaBacLuong_CanBo=@iID_MaBacLuong_CanBo, "+
                                     "sID_MaNguoiDungSua=@sID_MaNguoiDungSua, " +
                                     "sIPSua=@sIPSua " +
                                 "WHERE iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet");
            cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", null);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", null);
            cmd.Parameters.AddWithValue("@iID_MaNgachLuong_CanBo", null);
            cmd.Parameters.AddWithValue("@iID_MaBacLuong_CanBo", null);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", MaND);
            cmd.Parameters.AddWithValue("@sIPSua", IPSua);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmd.Parameters["@iID_MaBangLuongChiTiet"].Value = dt.Rows[i]["iID_MaBangLuongChiTiet"];
                cmd.Parameters["@iID_MaDonVi"].Value = dt.Rows[i]["iID_MaDonVi"];
                cmd.Parameters["@iID_MaNgachLuong_CanBo"].Value = dt.Rows[i]["iID_MaNgachLuong_CanBo"];
                cmd.Parameters["@iID_MaBacLuong_CanBo"].Value = dt.Rows[i]["iID_MaBacLuong_CanBo"];
                Connection.UpdateDatabase(cmd);
            }
            cmd.Dispose();
            dt.Dispose();
        }
        /// <summary>
        /// Quyết toán bảng lương chi tiết
        /// </summary>
        /// <param name="sMaTruong"></param>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNgachLuong"></param>
        /// <param name="iID_MaBacLuong"></param>
        /// <param name="rNguoi"></param>
        /// <param name="rTien"></param>
        private static void QuyetToan_LuongChiTiet(String sMaTruong, 
                                                    int iThangBangLuong,
                                                    int iNamBangLuong,
                                                    String iID_MaDonVi,
                                                    String iID_MaNgachLuong,
                                                    String iID_MaBacLuong,
                                                    ref double rNguoi,
                                                    ref double rTien)
        {
            SqlCommand cmd = new SqlCommand();
            String DK = String.Format("iTrangThai=1 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong AND iID_MaDonVi=@iID_MaDonVi AND {0}<>0", sMaTruong);
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);

            if (string.IsNullOrEmpty(iID_MaNgachLuong) == false)
            {
                DK += " AND iID_MaNgachLuong_CanBo=@iID_MaNgachLuong";
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
            }

            if (string.IsNullOrEmpty(iID_MaBacLuong) == false)
            {
                DK += " AND iID_MaBacLuong_CanBo=@iID_MaBacLuong";
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
            }

            String SQL = String.Format("SELECT COUNT(*), SUM({0}) FROM L_BangLuongChiTiet WHERE {1}",sMaTruong, DK);
            
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (CommonFunction.IsNumeric(dt.Rows[0][0]))
                {
                    rNguoi = Convert.ToDouble(dt.Rows[0][0]);
                }
                if (CommonFunction.IsNumeric(dt.Rows[0][1]))
                {
                    rTien = Convert.ToDouble(dt.Rows[0][1]);
                }
            }
            dt.Dispose();
        }
        /// <summary>
        /// Quyết toán bảng phụ cấp
        /// </summary>
        /// <param name="sMaTruong"></param>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaNgachLuong"></param>
        /// <param name="iID_MaBacLuong"></param>
        /// <param name="rNguoi"></param>
        /// <param name="rTien"></param>
        private static Boolean QuyetToan_LuongPhuCap(String sMaTruong,
                                                    int iThangBangLuong,
                                                    int iNamBangLuong,
                                                    String iID_MaDonVi,
                                                    String iID_MaNgachLuong,
                                                    String iID_MaBacLuong,
                                                    ref double rNguoi,
                                                    ref double rTien)
        {
            Boolean vR = false;
            //ref double rNguoi,
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iThangBangLuong=@iThangBangLuong AND iThangBangLuong=@iThangBangLuong AND iID_MaDonVi=@iID_MaDonVi AND sMaTruong=@sMaTruong AND rSoTien<>0";
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sMaTruong", "PhuCap_"+sMaTruong);

            if (string.IsNullOrEmpty(iID_MaNgachLuong) == false)
            {
                DK += " AND iID_MaNgachLuong_CanBo=@iID_MaNgachLuong";
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
            }

            if (string.IsNullOrEmpty(iID_MaBacLuong) == false)
            {
                DK += " AND iID_MaBacLuong_CanBo=@iID_MaBacLuong";
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
            }

            String SQL = String.Format("SELECT COUNT(*), SUM(rSoTien) FROM L_BangLuongChiTiet_PhuCap WHERE {0}", DK);

            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            if (dt.Rows.Count > 0)
            {
                if (CommonFunction.IsNumeric(dt.Rows[0][0]))
                {
                    rNguoi = Convert.ToDouble(dt.Rows[0][0]);
                }
                if (CommonFunction.IsNumeric(dt.Rows[0][1]))
                {
                    rTien = Convert.ToDouble(dt.Rows[0][1]);
                }
                vR = true;
            }
            dt.Dispose();
            return vR;
        }

        public static Boolean ChuyenBangQuyetToan(int iThangBangLuong, int iNamBangLuong, String MaND, String IPSua)
        {
            BangCSDL bangCSDL_BangLuongChiTiet = new BangCSDL("L_BangLuongChiTiet");
            SqlCommand cmd;

            //Xóa dữ liệu trong tháng của bảng L_BangLuong_QuyetToan
            cmd = new SqlCommand("DELETE FROM L_BangLuong_QuyetToan WHERE iThangBangLuong=@iThangBangLuong AND iNamBangLuong=@iNamBangLuong");
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Thêm các thông tin phụ vào bảng L_BangLuongChiTiet_PhuCap
            QuyetToan_CapNhapBangPhuCap(iThangBangLuong, iNamBangLuong, MaND, IPSua);

            //Lấy tất cả đơn vị 
            DataTable dtDonVi = DonViModels.Get_dtDonVi();

            //Lấy danh mục trường
            cmd = new SqlCommand("SELECT * FROM L_DanhMucTruong_MucLucNganSach WHERE iTrangThai=1");
            DataTable dtDMTruong = Connection.GetDataTable(cmd);
            cmd.Dispose();

            for (int iDMTruong = 0; iDMTruong < dtDMTruong.Rows.Count; iDMTruong++)
            {
                String sMaTruong = Convert.ToString(dtDMTruong.Rows[iDMTruong]["sMaTruong"]);
                String iID_MaNgachLuong = Convert.ToString(dtDMTruong.Rows[iDMTruong]["iID_MaNgachLuong"]);
                String iID_MaBacLuong = Convert.ToString(dtDMTruong.Rows[iDMTruong]["iID_MaBacLuong"]);
                

                for (int iDonVi = 0; iDonVi < dtDonVi.Rows.Count; iDonVi++)
                {
                    String iID_MaDonVi = Convert.ToString(dtDonVi.Rows[iDonVi]["iID_MaDonVi"]);
                    Bang bang = new Bang("L_BangLuong_QuyetToan");
                    Boolean okCoGiaTri = false;
                    bang.DuLieuMoi = true;
                    bang.MaNguoiDungSua = MaND;
                    bang.IPSua = IPSua;
                    //Điền các thông tin chung
                    bang.CmdParams.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
                    bang.CmdParams.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    //Điền các trường mục lục ngân sách
                    for(int j=dtDMTruong.Columns.IndexOf("iID_MaMucLucNganSach");j<dtDMTruong.Columns.IndexOf("sMoTa");j++)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@" + dtDMTruong.Columns[j].ColumnName, dtDMTruong.Rows[iDMTruong][j]);
                    }
                    double rNguoi = 0, rTien = 0;
                    if (bangCSDL_BangLuongChiTiet.CoTruong(sMaTruong))
                    {
                        //Nếu sMaTruong trong bảng L_BangLuongChiTiet
                        QuyetToan_LuongChiTiet(sMaTruong, iThangBangLuong, iNamBangLuong, iID_MaDonVi, iID_MaNgachLuong, iID_MaBacLuong, ref rNguoi, ref rTien);
                        okCoGiaTri = true;
                    }
                    else
                    {
                        //Nếu không có trong bảng L_BangLuongChiTiet
                        //sMaTruong = String.Fo
                        okCoGiaTri = QuyetToan_LuongPhuCap(sMaTruong, iThangBangLuong, iNamBangLuong, iID_MaDonVi, iID_MaNgachLuong, iID_MaBacLuong, ref rNguoi, ref rTien);
                    }

                    if (okCoGiaTri)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@rNguoi", rNguoi);
                        bang.CmdParams.Parameters.AddWithValue("@rTien", rTien);
                        bang.Save();
                    }
                }
            }

            dtDMTruong.Dispose();
            return true;
        }

        /// <summary>
        /// Lấy datatable lý do tăng giảm
        /// </summary>
        /// <param name="sHieuTangGiam">"T" - "G"</param>
        /// <returns></returns>
        public static DataTable Get_dtLyDoTangGiam(String sHieuTangGiam)
        {
            String MaMucLucQuanSoCha=Guid.Empty.ToString();            
            if (sHieuTangGiam == "T")
            {
                DataTable dtHieuTangGiamCha = QuyetToan_QuanSo_MucLucModels.GetChiTietMucLucQuanSo("2");
                MaMucLucQuanSoCha=Convert.ToString(dtHieuTangGiamCha.Rows[0]["iID_MaMucLucQuanSo"]);
                dtHieuTangGiamCha.Dispose();
            }
            else if (sHieuTangGiam == "G")
            {

                DataTable dtHieuTangGiamCha = QuyetToan_QuanSo_MucLucModels.GetChiTietMucLucQuanSo("3");
                MaMucLucQuanSoCha = Convert.ToString(dtHieuTangGiamCha.Rows[0]["iID_MaMucLucQuanSo"]);
                dtHieuTangGiamCha.Dispose();
            }
            
            return QuyetToan_QuanSo_MucLucModels.List_MucLucQuanSo(MaMucLucQuanSoCha);
           
        }
    }
}