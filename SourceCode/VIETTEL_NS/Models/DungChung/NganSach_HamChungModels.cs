using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class NganSach_HamChungModels
    {

        /// <summary>        
        /// Lấy danh sach phòng ban của người dùng
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable DSPhongBanCuaNguoiDung(String MaND)
        {
            String SQL = "SELECT NS_NguoiDung_PhongBan.iID_MaPhongBan,NS_PhongBan.sTen FROM NS_NguoiDung_PhongBan INNER JOIN NS_PhongBan ON NS_PhongBan.iID_MaPhongBan=NS_NguoiDung_PhongBan.iID_MaPhongBan";
            SQL += " WHERE NS_NguoiDung_PhongBan.iTrangThai=1 AND NS_NguoiDung_PhongBan.sMaNguoiDung=@sMaNguoiDung";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sach phòng ban của người dùng theo ma
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable DSBQLCuaNguoiDung(String MaND)
        {
            String SQL = "SELECT NS_PhongBan.sKyHieu,NS_PhongBan.sKyHieu + ' - ' + NS_PhongBan.sTen as sTen FROM NS_NguoiDung_PhongBan INNER JOIN NS_PhongBan ON NS_PhongBan.iID_MaPhongBan=NS_NguoiDung_PhongBan.iID_MaPhongBan";
            SQL += " WHERE NS_NguoiDung_PhongBan.iTrangThai=1 AND NS_NguoiDung_PhongBan.sMaNguoiDung=@sMaNguoiDung";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách loại ngân sách mà phòng ban quản lý
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable DSLNSCuaPhongBan(String MaND)
        {
            String MaPhongBan = MaPhongBanCuaMaND(MaND);
            String SQL = String.Format(@"SELECT a.sLNS,a.sLNS+'-'+sMoTa as sTen,b.sNhapTheoTruong
                                        FROM (SELECT sLNS FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND iID_MaPhongBan=@iID_MaPhongBan AND iTrangThai=1) as a
                                        INNER JOIN NS_MucLucNganSach b ON a.sLNS=b.sLNS
                                        WHERE b.iTrangThai=1 AND b.sL=''
                                        GROUP BY a.sLNS,sMoTa,sNhapTheoTruong");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách loại ngân sách mà phòng ban quản lý
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable DSLNS_LocCuaPhongBan(String MaND,String sLNS)
        {
            String[] arrLNS = sLNS.Split(',');
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DK += "  a.sLNS LIKE @sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i] + "%");
            }
            if (!String.IsNullOrEmpty(DK))
                DK = " AND (" + DK + ")";
            String MaPhongBan = MaPhongBanCuaMaND(MaND);
            String SQL = String.Format(@"SELECT a.sLNS,a.sLNS+'-'+sMoTa as sTen,b.sNhapTheoTruong
                                        FROM (SELECT sLNS FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND iID_MaPhongBan=@iID_MaPhongBan AND iTrangThai=1) as a
                                        INNER JOIN NS_MucLucNganSach b ON a.sLNS=b.sLNS
                                        WHERE b.iTrangThai=1 AND b.sL='' {0}
                                        GROUP BY a.sLNS,sMoTa,sNhapTheoTruong",DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lay loai ngan sach theo ma ngan sach
        /// </summary>
        /// <param name="LNS"></param>
        /// <returns></returns>
        public static DataTable DSLNSTheoLNS(String MaND, String LNS)
        {
            String MaPhongBan = MaPhongBanCuaMaND(MaND);
            String SQL =
                String.Format(
                    @"SELECT a.sLNS,a.sLNS+'-'+sMoTa as sTen,b.sNhapTheoTruong
                                        FROM (SELECT sLNS FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND iID_MaPhongBan=@iID_MaPhongBan AND iTrangThai=1) as a
                                        INNER JOIN NS_MucLucNganSach b ON a.sLNS=b.sLNS
                                        WHERE b.iTrangThai=1 AND b.sL='' AND b.sLNS LIKE @sLNS
                                        GROUP BY a.sLNS,sMoTa,sNhapTheoTruong");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@sLNS", LNS + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }

        /// <summary>
        /// Lấy xau dieu kien danh sách loại ngân sách mà phòng ban quản lý
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static String XauDieuKien_DSLNSCuaPhongBan(String MaND)
        {
            DataTable dt = DSLNSCuaPhongBan(MaND);
            String vR="0=1";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vR += String.Format(" OR sLNS='{0}'", dt.Rows[i]["sLNS"]);
            }
            vR = String.Format("({0})", vR);
            return vR;
        }

        /// <summary>
        /// Lấy danh sách đơn vị của phòng ban
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable DSDonViCuaPhongBan(String MaND)
        {
            String MaPhongBan = MaPhongBanCuaMaND(MaND);
            String SQL = "SELECT NS_DonVi.* FROM NS_PhongBan_DonVi INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi) NS_DonVi ON (NS_PhongBan_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi) WHERE NS_PhongBan_DonVi.iTrangThai=1 AND NS_DonVi.iTrangThai=1 AND iID_MaPhongBan=@iID_MaPhongBan";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy ra danh sách đơn vị mà người dùng quản lý
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable DSDonViCuaNguoiDung(String MaND)
        {
            String SQL = @"SELECT DISTINCT NS_DonVi.iID_MaDonVi+' - '+NS_DonVi.sTen as TenHT, NS_DonVi.* FROM (SELECT * FROM NS_NguoiDung_DonVi
		                  WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec_DonVi AND sMaNguoiDung=@sMaNguoiDung) AS NS_NguoiDung_DonVi 
                          INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi) NS_DonVi ON (NS_NguoiDung_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi) WHERE NS_NguoiDung_DonVi.iTrangThai=1 AND NS_NguoiDung_DonVi.sMaNguoiDung=@sMaNguoiDung ORDER BY NS_DonVi.iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        
        /// <summary>
        /// Kiểm tra phòng ban có quản lý loại ngân sách này không
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static Boolean PhongBan_LNS(String MaND, String sLNS)
        {
            String MaPhongBan = MaPhongBanCuaMaND(MaND);
            String SQL = "SELECT Count(*) FROM NS_PhongBan_LoaiNganSach WHERE iTrangThai=1 AND iID_MaPhongBan=@iID_MaPhongBan AND sLNS=@sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            Int32 count = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Kiểm tra phòng ban có được làm việc với đơn vị này không
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static Boolean PhongBan_DonVi(String MaND, String iID_MaDonVi)
        {
            String MaPhongBan = MaPhongBanCuaMaND(MaND);

            String SQL = "SELECT Count(*) FROM NS_PhongBan_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaPhongBan=@iID_MaPhongBan AND iID_MaDonVi=@iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            Int32 count = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Kiểm tra ngừoi dùng có được làm việc với đon vị này không
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static Boolean NguoiDung_DonVi(String MaND, String iID_MaDonVi)
        {
            String SQL = "SELECT Count(*) FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sMaNguoiDung=@sMaNguoiDung AND iID_MaDonVi=@iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            Int32 count = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Lấy iID_MaPhongBan của người dùng
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns>=Guid.Empty.ToString() =0000-00000-00000-0000: MaND không thuộc phòng ban nào</returns>
        public static String MaPhongBanCuaMaND(String MaND)
        {
            String vR = "";
            String SQL = "SELECT iID_MaPhongBan FROM NS_NguoiDung_PhongBan WHERE iTrangThai=1 AND sMaNguoiDung=@sMaNguoiDung";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            vR = Connection.GetValueString(cmd, Guid.Empty.ToString());
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy mục lục ngân sách của loại ngân sách và người dùng
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="MaND"></param>
        /// <returns>Datatable</returns>
        public static DataTable DT_MucLucNganSach_sLNS(String sLNS)
        {
            DataTable vR = null;
            String SQL = String.Format("SELECT * FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS=@sLNS ORDER BY {0}", MucLucNganSachModels.strDSTruongSapXep);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy mục lục ngân sách của loại ngân sách và người dùng
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="MaND"></param>
        /// <returns>Datatable</returns>
        public static DataTable DT_MucLucNganSach_sLNS(String sLNS,String sLoai)
        {
            String DKLoai = "";
            if (String.IsNullOrEmpty(sLoai) == false)
            {
                switch (sLoai)
                {
                    case "sTM":
                        DKLoai = " AND sTM <>'' AND sTTM='' ";
                        break;
                    case "sM":
                        DKLoai = "AND sM <>'' AND sTM='' ";
                        break;
                    case "sLNS":
                        DKLoai = "AND sL='' ";
                        break;
                }
            }
            DataTable vR = null;
            String SQL = String.Format("SELECT * FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS=@sLNS {0} ORDER BY {1}",DKLoai, MucLucNganSachModels.strDSTruongSapXep);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable DT_MucLucNganSach_sLNS_TheoDau(String sLNS)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucNganSach WHERE iTrangThai=1 AND LEFT(sLNS,1) = @sLNS ORDER By iSTT");
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Thêm thông tin chi tiết mục lục ngân sách từ RMucLucNganSach vào params
        /// </summary>
        /// <param name="RMucLucNganSach"></param>
        /// <param name="Params"></param>
        public static void ThemThongTinCuaMucLucNganSach(DataRow RMucLucNganSach, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String[] arrDSDuocNhapTruongTien = MucLucNganSachModels.strDSDuocNhapTruongTien.Split(',');
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String sXauNoiMa = "";
            if (Params.IndexOf("@iID_MaMucLucNganSach") >= 0)
            {
                Params["@iID_MaMucLucNganSach"].Value = RMucLucNganSach["iID_MaMucLucNganSach"];
                Params["@iID_MaMucLucNganSach_Cha"].Value = RMucLucNganSach["iID_MaMucLucNganSach_Cha"];
                Params["@bLaHangCha"].Value = RMucLucNganSach["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaMucLucNganSach", RMucLucNganSach["iID_MaMucLucNganSach"]);
                Params.AddWithValue("@iID_MaMucLucNganSach_Cha", RMucLucNganSach["iID_MaMucLucNganSach_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucNganSach["bLaHangCha"]);
            }
            for (int i = 0; i < arrDSDuocNhapTruongTien.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSDuocNhapTruongTien[i]) >= 0)
                {
                    Params["@" + arrDSDuocNhapTruongTien[i]].Value = RMucLucNganSach[arrDSDuocNhapTruongTien[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSDuocNhapTruongTien[i], RMucLucNganSach[arrDSDuocNhapTruongTien[i]]);
                }
            }
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucNganSach[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucNganSach[arrDSTruong[i]]);
                }
                if (i < arrDSTruong.Length-1 && String.IsNullOrEmpty(Convert.ToString(RMucLucNganSach[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucNganSach[arrDSTruong[i]]);
                }
            }
            if (Params.IndexOf("@sXauNoiMa") >= 0)
            {
                Params["@sXauNoiMa"].Value = sXauNoiMa;
            }
            else
            {
                Params.AddWithValue("@sXauNoiMa", sXauNoiMa);
            }
        }

        /// <summary>
        /// Thêm thông tin chi tiết mục lục ngân sách từ RMucLucNganSach vào params không lấy các cột nhập trường tiền
        /// </summary>
        /// <param name="RMucLucNganSach"></param>
        /// <param name="Params"></param>
        public static void ThemThongTinCuaMucLucNganSachKhongLayTruongTien(DataRow RMucLucNganSach, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String sXauNoiMa = "";
            if (Params.IndexOf("@iID_MaMucLucNganSach") >= 0)
            {
                Params["@iID_MaMucLucNganSach"].Value = RMucLucNganSach["iID_MaMucLucNganSach"];
                Params["@iID_MaMucLucNganSach_Cha"].Value = RMucLucNganSach["iID_MaMucLucNganSach_Cha"];
                Params["@bLaHangCha"].Value = RMucLucNganSach["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaMucLucNganSach", RMucLucNganSach["iID_MaMucLucNganSach"]);
                Params.AddWithValue("@iID_MaMucLucNganSach_Cha", RMucLucNganSach["iID_MaMucLucNganSach_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucNganSach["bLaHangCha"]);
            }
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucNganSach[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucNganSach[arrDSTruong[i]]);
                }
                if (i < arrDSTruong.Length - 1 && String.IsNullOrEmpty(Convert.ToString(RMucLucNganSach[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucNganSach[arrDSTruong[i]]);
                }
            }
            if (Params.IndexOf("@sXauNoiMa") >= 0)
            {
                Params["@sXauNoiMa"].Value = sXauNoiMa;
            }
            else
            {
                Params.AddWithValue("@sXauNoiMa", sXauNoiMa);
            }
        }
        /// <summary>
        /// Thêm thông tin chi tiết mục lục quân số từ RMucLucQuanSo vào params không lấy các cột nhập trường tiền
        /// </summary>
        /// <param name="RMucLucNganSach"></param>
        /// <param name="Params"></param>
        public static void ThemThongTinCuaMucLucQuanSoKhongLayTruongTien(DataRow RMucLucQuanSo, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "sKyHieu,sMoTa";
            String[] arrDSTruong = strDSTruong.Split(',');
            String sXauNoiMa = "";
            if (Params.IndexOf("@iID_MaMucLucQuanSo") >= 0)
            {
                Params["@iID_MaMucLucQuanSo"].Value = RMucLucQuanSo["iID_MaMucLucQuanSo"];
                Params["@iID_MaMucLucQuanSo_Cha"].Value = RMucLucQuanSo["iID_MaMucLucQuanSo_Cha"];
                Params["@bLaHangCha"].Value = RMucLucQuanSo["bLaHangCha"];
            }
            else
            {
                Params.AddWithValue("@iID_MaMucLucQuanSo", RMucLucQuanSo["iID_MaMucLucQuanSo"]);
                Params.AddWithValue("@iID_MaMucLucQuanSo_Cha", RMucLucQuanSo["iID_MaMucLucQuanSo_Cha"]);
                Params.AddWithValue("@bLaHangCha", RMucLucQuanSo["bLaHangCha"]);
            }
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucQuanSo[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucQuanSo[arrDSTruong[i]]);
                }
                if (i < arrDSTruong.Length - 1 && String.IsNullOrEmpty(Convert.ToString(RMucLucQuanSo[arrDSTruong[i]])) == false)
                {
                    if (sXauNoiMa != "") sXauNoiMa += "-";
                    sXauNoiMa += Convert.ToString(RMucLucQuanSo[arrDSTruong[i]]);
                }
            }
            //if (Params.IndexOf("@sXauNoiMa") >= 0)
            //{
            //    Params["@sXauNoiMa"].Value = sXauNoiMa;
            //}
            //else
            //{
            //    Params.AddWithValue("@sXauNoiMa", sXauNoiMa);
            //}
        }
        /// <summary>
        /// Hàm chuyển dữ liệu năm sau
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaTrangThaiDuyet"></param>
        /// <param name="TenBangChungTu"></param>
        /// <param name="TenBangChiTiet"></param>
        /// <param name="TenBangDot"></param>
        /// <param name="TenTruongNgayDot"></param>
        public static void ChuyenNamSau(String MaND, String IPSua, int MaTrangThaiDuyet, String TenBangChungTu = "PB_PhanBo", String TenBangChiTiet = "PB_PhanBoChiTiet", Boolean CoBangDot = true, String TenBangDot = "PB_DotPhanBo", String TenTruongNgayDot = "dNgayDotPhanBo")
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            int iNamLamViec = Convert.ToInt16(dtCauHinh.Rows[0]["iNamLamViec"]);
            //Lấy dt chứng từ đã duyệt
            String SQL = String.Format("SELECT * FROM {0} WHERE iTrangThai=1 AND iID_MaTrangThaiDuyet={1} AND  iID_MaNamNganSach=3 AND iNamLamViec=@iNamLamViec", TenBangChungTu, MaTrangThaiDuyet);
            DataTable dtChungTu;
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            dtChungTu = Connection.GetDataTable(cmd);
            cmd.Dispose();

            Bang bang = new Bang(TenBangChungTu);
            Bang bangChiTiet = new Bang(TenBangChiTiet);

            String iID_MaChungTu, iID_ChungTuGoc;
            DataTable dtChiTiet;
            DataTable dtNguonNS = DanhMucModels.NS_NguonNganSach();
            DataTable dtDot;
            String iID_MaNguonNganSach, iID_MaDotNganSach="";
            String TruongKhoaDot = "";
            DateTime d = new DateTime(iNamLamViec + 1, 1, 1);
            for (int i = 0; i < dtChungTu.Rows.Count; i++)
            {
                #region Đợt ngân sách
                if (CoBangDot)
                {
                    Bang bangDot = new Bang(TenBangDot);
                    TruongKhoaDot = bangDot.TruongKhoa;
                    iID_MaNguonNganSach = Convert.ToString(dtChungTu.Rows[i]["iID_MaNguonNganSach"]);

                    SQL = String.Format("SELECT * FROM {0} WHERE iTrangThai=1 AND iID_MaNamNganSach=1 AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iNamLamViec=@iNamLamViec", bangDot.TenBang);
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec + 1);
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    dtDot = Connection.GetDataTable(cmd);
                    if (dtDot.Rows.Count > 0)
                    {
                        iID_MaDotNganSach = Convert.ToString(dtDot.Rows[0][bangDot.TruongKhoa]);
                    }
                    else
                    {
                        DataTable dtLNS = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
                        String sDSLNS = "";
                        for (int j = 0; j < dtLNS.Rows.Count; j++)
                        {
                            sDSLNS += Convert.ToString(dtLNS.Rows[j]["sLNS"]) + ";";
                        }
                        iID_MaDotNganSach = Guid.NewGuid().ToString();
                        
                        bangDot.MaNguoiDungSua = MaND;
                        bangDot.IPSua = IPSua;
                        bangDot.CmdParams.Parameters.AddWithValue("@sDSLNS", sDSLNS);
                        bangDot.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec + 1);
                        bangDot.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", 1);
                        bangDot.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                        bangDot.CmdParams.Parameters.AddWithValue("@" + TenTruongNgayDot, d);
                        bangDot.CmdParams.Parameters.AddWithValue("@" + bangDot.TruongKhoa, iID_MaDotNganSach);
                        bangDot.Save();
                    }
                }
                #endregion
                //Lấy dt chi tiết của chứng từ
                iID_ChungTuGoc = Convert.ToString(dtChungTu.Rows[i][bang.TruongKhoa]);
                SQL = String.Format("SELECT * FROM {0} WHERE iTrangThai=1  AND iID_MaTrangThaiDuyet={1} AND  {2}=@{2}", TenBangChiTiet, MaTrangThaiDuyet, bang.TruongKhoa);
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@" + bang.TruongKhoa, iID_ChungTuGoc);
                dtChiTiet = Connection.GetDataTable(cmd);
                cmd.Dispose();
                //add du liệu chi tiết các cột vào param     
                for (int j = 0; j < dtChungTu.Columns.Count; j++)
                {
                    String TenCot = dtChungTu.Columns[j].ColumnName;
                    String GiaTri = Convert.ToString(dtChungTu.Rows[i][j]);
                    if (TenCot == "iID_MaNamNganSach")
                    {
                        GiaTri = "1";//chuyển mã năm ngân sách thành 1 (Năm nay)
                    }
                    if (TenCot == "iNamLamViec")
                    {
                        GiaTri = Convert.ToString(iNamLamViec + 1);
                    }
                    //Trường hợp có bảng đợt
                    if (CoBangDot && TenCot == TruongKhoaDot)
                    {
                        GiaTri = iID_MaDotNganSach;
                    }
                   // if (i == 0)
                   // {
                        bang.CmdParams.Parameters.AddWithValue("@" + TenCot, GiaTri);
                   // }
                   // else
                   // {
                        bang.CmdParams.Parameters["@" + TenCot].Value = GiaTri;
                   // }
                }
                iID_MaChungTu = Guid.NewGuid().ToString();
                //Remove trường tự tăng iSoChungTu
                bang.CmdParams.Parameters.RemoveAt(bang.CmdParams.Parameters.IndexOf("@iSoChungTu"));
                bang.CmdParams.Parameters["@" + bang.TruongKhoa].Value = iID_MaChungTu;
                bang.Save();

                for (int h = 0; h < dtChiTiet.Rows.Count; h++)
                {
                    for (int j = 0; j < dtChiTiet.Columns.Count; j++)
                    {
                        Type _type = dtChiTiet.Columns[j].DataType;
                        String TenCot = dtChiTiet.Columns[j].ColumnName;
                        Object GiaTri = dtChiTiet.Rows[h][j];

                        if (TenCot == "iID_MaNamNganSach")
                        {
                            GiaTri = "1";//chuyển mã năm ngân sách thành 1 (Năm nay)
                        }
                        if (TenCot == "iNamLamViec")
                        {
                            GiaTri = iNamLamViec + 1;
                        }
                        if (TenCot == bang.TruongKhoa)
                        {
                            GiaTri = iID_MaChungTu;
                        }
                        //Trường hợp có bảng đợt
                        if (CoBangDot && TenCot == TruongKhoaDot)
                        {
                            GiaTri = iID_MaDotNganSach;
                        }
                        //Lấy tất cả trường ngày là ngày 1/1/năm
                        if (TenCot.StartsWith("d"))
                        {
                            GiaTri = d;
                        }
                        //if (h == 0)
                       // {
                            bangChiTiet.CmdParams.Parameters.AddWithValue("@" + TenCot, GiaTri);
                       // }
                        //else
                       // {
                          //  bangChiTiet.CmdParams.Parameters["@" + TenCot].Value = GiaTri;
                        //}

                    }
                    //Trường hợp trường khóa bảng chi tiết không phải kiểu số
                    if (bangChiTiet.TruongKhoaKieuSo == false)
                    {
                        bangChiTiet.CmdParams.Parameters["@" + bangChiTiet.TruongKhoa].Value = Guid.NewGuid();
                    }
                    else
                    {
                        int cs=bangChiTiet.CmdParams.Parameters.IndexOf("@" + bangChiTiet.TruongKhoa);
                        if(cs>=0)
                            bangChiTiet.CmdParams.Parameters.RemoveAt(cs);
                    }
                    bangChiTiet.Save();
                }

            }
        }
    }
}