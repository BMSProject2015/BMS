using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
namespace VIETTEL.Models
{
    public class DanhMucModels
    {
        /// <summary>
        /// Lấy danh sách loại ngân sách
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSach(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7  AND sL = '' ORDER By sXauNoiMa");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSach_PhongBan(Boolean All, String sLNS)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = "SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sL = ''";
            if (String.IsNullOrEmpty(sLNS) == false)
            {
                sql += " and sLNS like @sLNS";
                cmd.Parameters.AddWithValue("@sLNS", sLNS + "%");
            }
            sql += " ORDER By sXauNoiMa";
            DataTable dt;
            cmd.CommandText = sql;
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách loại ngân sách
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSach_Ma()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sL = '' ORDER By sXauNoiMa");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách loại ngân sách
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSach_Ma_Con(String sLNS)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaMucLucNganSach, sLNS +' - '+ sL +' - '+ sK +' - '+ sM +' - '+ sTM +' - '+ sTTM +' - '+ sNG +' - '+ sTNG +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER By sXauNoiMa");
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách danh muc ngach luong
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable L_DanhMucNgachLuong(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1 ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaNgachLuong"] = "";
                R["sTenNgachLuong"] = "---Tất cả---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách loại ngân sách bỏ đi loại ngân sách 1010000
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNghiepVu(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sLNS <> '1010000' AND sL = '' ORDER By sLNS");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách loại ngân sách bỏ đi loại ngân sách 1010000
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNhaNuoc()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sLNS LIKE '2%' AND sL = '' ORDER By sLNS");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSachAll(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7 AND SUBSTRING(A.sLNS,1,1) <>'8' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSachFull(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7  AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSachQuocPhong(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7 AND A.sLNS LIKE '1%' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSachNhaNuoc_PhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7 AND (A.sLNS LIKE '2%' OR A.sLNS LIKE '3%') AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);            
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSachKhac_PhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7 AND A.sLNS NOT LIKE '1%'  AND A.sLNS NOT LIKE '2%' AND A.sLNS NOT LIKE '8%'  AND A.sLNS NOT LIKE '3%' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách loại ngân sách bỏ đi loại ngân sách 1010000
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNghiepVuKhac()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND LEFT(sLNS,1) <> 2 AND sLNS <> '1010000' AND sL = '' ORDER By sLNS");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable NS_LoaiNganSachNghiepVuKhac_PhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS as sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7 AND LEFT(A.sLNS,1) <> 2 AND A.sLNS <> '1010000' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSachNghiepVuNhaNuoc_PhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS as sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7  AND A.sLNS <> '1010000' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable NS_LoaiNganSach_PhongBan(String iID_MaPhongBan)
        {
            DataTable dt;
            String SQL = "SELECT A.sLNS as sLNS, A.sLNS +' - '+ A.sMoTa AS TenHT FROM NS_MucLucNganSach as A INNER JOIN NS_PhongBan_LoaiNganSach AS B ON A.sLNS = B.sLNS WHERE B.iID_MaPhongBan=@iID_MaPhongBan AND B.iTrangThai=1 AND LEN(A.sLNS)=7 AND SUBSTRING(A.sLNS,1,1)<>'8' AND A.sL = '' ORDER By A.sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách danh muc ngach luong
        /// </summary>
        /// <param name="All"> true: hiển thị dòng ---Tất cả---; false: ko hiển thị</param>
        /// <returns></returns>
        public static DataTable NS_DonVi_DenCap(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT CAST(iCap AS nvarchar(50)) as iCap, CAST(iCap AS nvarchar(50)) as sCap FROM NS_DonVi WHERE iTrangThai=1 Group by iCap ORDER By iCap");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iCap"] = "0";
                R["sCap"] = "---Tất cả---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy danh sách mục lục ngân sách người có công
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNguoiCoCong(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND LEFT(sLNS,3) = '206' AND sL = '' ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// lấy mục lục ngân sách nhà nước
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNhaNuoc(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND LEFT(sLNS,1) = '2' AND sL = '' ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// lấy ngân sách khác
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachKhac(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND LEFT(sLNS,3) = '109' AND sL = '' ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy tên loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static String NS_TenLoaiNganSach(String sLNS)
        {
            String vR = "";
            DataTable dt = NS_LoaiNganSach(false);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToString(dt.Rows[i]["sLNS"]) == sLNS)
                {
                    vR = Convert.ToString(dt.Rows[i]["TenHT"]);
                    break;
                }
            }
            dt.Dispose();
            return vR;
        }
        /// <summary>
        /// Danh sách nguồn ngân sách
        /// </summary>
        /// <returns></returns>
        public static DataTable NS_NguonNganSach()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_NguonNganSach WHERE iTrangThai=1 ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách năm ngân sách
        /// </summary>
        /// <returns></returns>
        public static DataTable NS_NamNganSach()
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_NamNganSach WHERE iTrangThai=1 ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách đợt ngân sách
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_DotNganSach(String NamLamViec)
        {
            DataTable dt;
            String SQL = String.Format("SELECT * FROM DT_DotNganSach WHERE iTrangThai=1 AND iNamLamViec={0} ORDER By iSTT", NamLamViec);
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách ngày đợt ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_NgayDotNganSach(String sLNS, String NamLamViec)
        {
            String SQL = "SELECT Convert(varchar(10),dNgayDotNganSach,103) WHERE iNamLamViec=@iNamLamViec AND iID_MaDotNganSach in (SELECT iID_MaDotNganSach FROM DT_ChungTuChiTiet WHERE sLNS=@sLNS)";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public static DataTable NS_DonVi()
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaDonVi, iID_MaDonVi+' - '+sTen AS sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi ORDER By iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable VN_NoiDung()
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaNoiDung, iID_MaNoiDung+' - '+sTenNoiDung AS sTenNoiDung FROM DC_DanhMucNoiDung WHERE iTrangThai = 1  ORDER By sTenNoiDung asc");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable GetDonVi(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi";

            String SQL = String.Format("SELECT * FROM NS_DonVi WHERE {0}", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            vR = CommonFunction.dtData(cmd, "iID_MaDonVi", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int GetDonVi_Count()
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_DonVi WHERE {0}", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_OneRow_DonVi(String MaDonVi)
        {
            DataTable vR;            
            String SQL = String.Format("SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaDonVi = @iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }

        public static DataTable GetRow_DonVi(String MaDonVi)
        {
            DataTable vR;
            String SQL = String.Format("SELECT iID_MaDonVi, iID_MaDonVi+' - '+ sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaDonVi = @iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }

        public static DataTable NS_PhongBan()
        {
            DataTable dt;
            String SQL = String.Format("SELECT * FROM NS_PhongBan WHERE iTrangThai=1 ORDER By iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable GetRow_PhongBan(String MaPhongBan)
        {
            DataTable dt;
            String SQL = String.Format("SELECT * FROM NS_PhongBan WHERE iID_MaPhongBan = @iID_MaPhongBan AND iTrangThai=1");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable DT_DanhMuc(String TenLoaiDanhMuc, Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt = CommonFunction.Lay_dtDanhMuc(TenLoaiDanhMuc);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDanhMuc"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }
        public static DataTable DT_DanhMuc_All(String TenLoaiDanhMuc)
        {
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang='{0}') ORDER BY iSTT", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
        public static DataTable GetRow_DanhMuc(String MaDanhMuc)
        {
            DataTable vR;
            String SQL = String.Format("SELECT * FROM DC_DanhMuc WHERE iID_MaDanhMuc = @iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDanhMuc", MaDanhMuc);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }

        //phuonglt15
        public static DataTable GetPhongBan(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 ";

            String SQL = String.Format("SELECT * FROM NS_PhongBan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int GetPhongBan_Count()
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 ";

            String SQL = String.Format("SELECT COUNT(*) FROM NS_PhongBan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static string getTenPB(string MaPB)
        {
            String SQL = String.Format("SELECT sTen FROM NS_PhongBan WHERE iID_MaPhongBan=@MaPhongBan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPB);
            cmd.CommandText = SQL;
            String sValue = Convert.ToString(Connection.GetValue(cmd, String.Empty));
            cmd.Dispose();
            return sValue;
        }
        /// <summary>
        /// Lấy tên phòng ban bởi ký hiệu
        /// </summary>
        /// <param name="sKyHieu"></param>
        /// <returns></returns>
        public static string GetTenPB_KyHieu(string sKyHieu)
        {
            String SQL = String.Format("SELECT sTen FROM NS_PhongBan WHERE sKyHieu=@sKyHieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            cmd.CommandText = SQL;
            String sValue = Convert.ToString(Connection.GetValue(cmd, String.Empty));
            cmd.Dispose();
            return sValue;
        }
        public static DataTable getPhongBanByCombobox(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaPhongBan, sKyHieu + ' - ' + sTen AS sTen FROM NS_PhongBan WHERE iTrangThai=1 ORDER By iSTT ASC");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaPhongBan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_PhongBan_KyHieu(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT sKyHieu, sKyHieu + ' - ' + sTen AS sTen FROM NS_PhongBan WHERE iTrangThai=1 ORDER By sTen ASC");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["sKyHieu"] = String.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lây danh sách đơn vị
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>
        public static DataTable getDonViByCombobox(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaDonVi, iID_MaDonVi + ' - ' + sTen AS sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi  ORDER By sTen ASC");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = string.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lây danh sách đơn vị
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>
        public static DataTable getDonViByComboboxGroup(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt = GetDonViGroup();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string strValue= getLevelString(Convert.ToInt32(dt.Rows[i]["iCap"]));
                    dt.Rows[i]["sTen"] = strValue + Convert.ToString(dt.Rows[i]["sTen"]);
                }
                catch (Exception)
                {
                    
                }
            }

            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = string.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }

        public static DataTable GetDonViByMaDonViCha(String sMaDonViCha)
        {
            DataTable dt;

            String SQL = "SELECT iID_MaDonVi, iID_MaDonVi + ' - ' + sTen AS sTen,iCap FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi";
            if(sMaDonViCha != "")
            {
                SQL += " AND iID_MaDonViCha = @iID_MaDonViCha";
            }
            else
            {
                SQL += " AND iID_MaDonViCha is null";
            }
            SQL += " ORDER By sTen ASC";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            if (sMaDonViCha != "")
            {
                cmd.Parameters.AddWithValue("iID_MaDonViCha", sMaDonViCha);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public  static  DataTable GetDonViGroup()
        {
            DataTable dtDonVi = new DataTable();

            DataTable dtDonViParent = GetDonViByMaDonViCha("");
            dtDonVi = dtDonViParent.Clone();
            if (dtDonViParent.Rows.Count > 0)
            {
                for (int i = 0; i < dtDonViParent.Rows.Count; i++)
                {
                    string sLevel = "";
                    try
                    {
                        sLevel = getLevelString(Convert.ToInt32(dtDonViParent.Rows[i]["iCap"]));
                    }
                    catch (Exception)
                    {
                        sLevel = "";
                    }
                    dtDonVi.ImportRow(dtDonViParent.Rows[i]);
                    GetDonViChild(ref dtDonVi, Convert.ToString(dtDonViParent.Rows[i]["iID_MaDonVi"]), sLevel); 
                }
            }
            return dtDonVi;
        }

        /// <summary>
        /// lay danh sach don vi con
        /// </summary>
        /// <param name="dt">table nhan gia tri tra ve</param>
        public static void GetDonViChild(ref DataTable dt, string sMaDonViCha, string sLevel)
        {
            DataTable dtDonVi = new DataTable();
            dtDonVi = GetDonViByMaDonViCha(sMaDonViCha);
            if (dtDonVi.Rows.Count > 0)
            {
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    dtDonVi.Rows[i]["sTen"] = sLevel + Convert.ToString(dtDonVi.Rows[i]["sTen"]);
                    dt.ImportRow(dtDonVi.Rows[i]);
                    GetDonViChild(ref dt, Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]),"..." + sLevel);
                }
            }
        }

        private static string getLevelString(int iLevel)
        {
            string sLevel = "";
            for (int i = 1; i < iLevel; i++)
            {
                sLevel += "...";
            }
            return sLevel;
        }

        /// <summary>
        /// Lấy danh sách đơn vị theo phòng ban
        /// </summary>
        /// <param name="MaPhongBan"></param>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_DonVi_PhongBan(String MaPhongBan, Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format(@"SELECT DV.iID_MaDonVi, DV.iID_MaDonVi + ' - ' + DV.sTen AS sTen 
            FROM NS_PhongBan_DonVi AS PBDV 
            INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi ) DV ON PBDV.iID_MaDonVi = DV.iID_MaDonVi 
            WHERE PBDV.iTrangThai=1 AND DV.iTrangThai=1 AND PBDV.iID_MaPhongBan=@MaPhongBan ORDER BY DV.sTen ASC");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaPhongBan", MaPhongBan);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = string.Empty;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static DataTable getNguoiDung(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT sID_MaNguoiDung, sHoTen FROM QT_NguoiDung WHERE  bHoatDong=1");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["sID_MaNguoiDung"] = string.Empty;
                R["sHoTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// danh sách ngoại tệ
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>
        public static DataTable QLDA_NgoaiTe(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaNgoaiTe, sTen FROM QLDA_NgoaiTe WHERE  iTrangThai=1");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaNgoaiTe"] = 0;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
         
        public static int ThangLamViec(string MaND)
        {
            String SQL = String.Format("SELECT iThangLamViec FROM DC_NguoiDungCauHinh WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            cmd.CommandText = SQL;
            int sValue = Convert.ToInt32(Connection.GetValue(cmd, DateTime.Now.Month));
            cmd.Dispose();
            return sValue;
        }
        public static int NamLamViec(string MaND)
        {
            String SQL = String.Format("SELECT iNamLamViec FROM DC_NguoiDungCauHinh WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            cmd.CommandText = SQL;
            int sValue = Convert.ToInt32(Connection.GetValue(cmd, DateTime.Now.Year));
            cmd.Dispose();
            return sValue;
        }
        /// <summary>
        /// Hàm trả về datatable 31 ngày trong tháng
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Ngay()
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaNgay", typeof(String));
            vR.Columns.Add("TenNgay", typeof(String));
            DataRow Row;
            for (int i = 1; i < 32; i++)
            {
                Row = vR.NewRow();
                vR.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            vR.Rows.InsertAt(vR.NewRow(), 0);
            vR.Rows[0]["MaNgay"] = "";
            vR.Rows[0]["TenNgay"] = "-- Ngày --";
            return vR;
        }
        /// <summary>
        /// Lấy số ngày của tháng và năm
        /// </summary>
        /// <param name="iThang"></param>
        /// <param name="iNam"></param>
        /// <returns>dt ngày của tháng</returns>
        public static DataTable DT_Ngay(int iThang, int iNam, Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaNgay", typeof(String));
            vR.Columns.Add("TenNgay", typeof(String));
            int SoNgayTrongThang = DateTime.DaysInMonth(iNam, iThang);
            DataRow Row;
            for (int i = 1; i < SoNgayTrongThang + 1; i++)
            {
                Row = vR.NewRow();
                vR.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaNgay"] = "";
                vR.Rows[0]["TenNgay"] = "-- Ngày --";
            }
            return vR;
        }


        /// <summary>
        /// Hàm trả về datatable các tháng trong năm: 1 --> 12
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Thang(Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaThang", typeof(String));
            vR.Columns.Add("TenThang", typeof(String));
            DataRow Row;
            for (int i = 1; i < 13; i++)
            {
                Row = vR.NewRow();
                vR.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaThang"] = "-1";
                vR.Rows[0]["TenThang"] = "-- Tháng --";
            }
            return vR;
        }
        public static DataTable DT_Thang_ThuNop(Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaThang", typeof(String));
            vR.Columns.Add("TenThang", typeof(String));
            DataRow Row;
            for (int i = 1; i < 25; i++)
            {
                Row = vR.NewRow();
                vR.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaThang"] = "-1";
                vR.Rows[0]["TenThang"] = "-- Tháng --";
            }
            return vR;
        }
        /// <summary>
        /// Hàm trả về datatable các tháng trong năm: 0 --> 12
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Thang_CoThangKhong()
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaThang", typeof(String));
            vR.Columns.Add("TenThang", typeof(String));
            DataRow Row;
            for (int i = 0; i < 13; i++)
            {
                Row = vR.NewRow();
                vR.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            return vR;
        }
        /// <summary>
        /// Hàm trả về datatable các quý trong năm: 1 --> 4
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Quy(Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaQuy", typeof(String));
            vR.Columns.Add("TenQuy", typeof(String));
            DataRow Row;
            for (int i = 1; i < 5; i++)
            {
                Row = vR.NewRow();
                vR.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaQuy"] = "-1";
                vR.Rows[0]["TenQuy"] = "--Quý--";
            }
            return vR;
        }
        /// <summary>
        /// Hàm trả về datatable các quý trong năm: 1 --> 4
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Quy_QuyetToan(Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaQuy", typeof(String));
            vR.Columns.Add("TenQuy", typeof(String));

            DataRow R = vR.NewRow();
            R["MaQuy"] = "1";
            R["TenQuy"] = "Quý I";
            vR.Rows.InsertAt(R, 1);

            DataRow R1 = vR.NewRow();
            R1["MaQuy"] = "2";
            R1["TenQuy"] = "Quý II";
            vR.Rows.InsertAt(R1, 2);

            DataRow R2 = vR.NewRow();
            R2["MaQuy"] = "3";
            R2["TenQuy"] = "Quý III";
            vR.Rows.InsertAt(R2, 3);

            DataRow R4 = vR.NewRow();
            R4["MaQuy"] = "4";
            R4["TenQuy"] = "Quý IV";
            vR.Rows.InsertAt(R4, 4);

            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaQuy"] = "-1";
                vR.Rows[0]["TenQuy"] = "--Quý--";
            }
            return vR;
        }
        /// <summary>
        /// Hàm trả về datatable các đợt trong năm
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Dot_DuToan(Boolean All = true)
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("MaDot", typeof(String));
            vR.Columns.Add("TenDot", typeof(String));

            DataRow R = vR.NewRow();
            R["MaDot"] = "1";
            R["TenDot"] = "6-9-2069";
            vR.Rows.InsertAt(R, 1);

            DataRow R1 = vR.NewRow();
            R1["MaDot"] = "2";
            R1["TenDot"] = "9-6-2069";
            vR.Rows.InsertAt(R1, 2);

            if (All)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["MaDot"] = "-1";
                vR.Rows[0]["TenDot"] = "-- Đợt --";
            }
            return vR;
        }
        /// <summary>
        /// Hàm trả về datatable các năm từ 2000-2020
        /// </summary>
        /// <param name="CoHangChon">true: hien thi hang chon</param>
        /// <param name="sTenHangChon">Ten hang chon: -- Bạn chọn năm ngân sách --</param>
        /// <returns></returns>
        public static DataTable DT_Nam(Boolean CoHangChon = true, string sTenHangChon = "-- Bạn chọn năm --")
        {
            DateTime dNgayHienTai = DateTime.Now;
            int NamLamViec = Convert.ToInt32(dNgayHienTai.Year); ;
            if (NguoiDungCauHinhModels.iNamLamViec != null)
            {
                NamLamViec = Convert.ToInt32(NguoiDungCauHinhModels.iNamLamViec);
            }
            
            int NamMin = NamLamViec - 50;                
            int NamMax = NamLamViec + 50;
      
            DataTable dtNam = new DataTable();
            dtNam.Columns.Add("MaNam", typeof(String));
            dtNam.Columns.Add("TenNam", typeof(String));
            DataRow R;
            for (int i = NamMin; i < NamMax; i++)
            {
                R = dtNam.NewRow();
                dtNam.Rows.Add(R);
                R[0] = Convert.ToString(i);
                R[1] = Convert.ToString(i);
            }
            if (CoHangChon)
            {
                dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
                dtNam.Rows[0]["TenNam"] = sTenHangChon;
            }
            return dtNam;
        }

        /// <summary>
        /// Hàm trả về datatable các tuổi từ 18-60
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_Tuoi(Boolean CoHangChon = false, int iTuTuoi = 18, int iDenTuoi = 65)
        {
            DataTable dtTuoi = new DataTable();
            dtTuoi.Columns.Add("MaTuoi", typeof(String));
            dtTuoi.Columns.Add("TenTuoi", typeof(String));
            DataRow R;
            for (int i = iTuTuoi; i <= iDenTuoi; i++)
            {
                R = dtTuoi.NewRow();
                dtTuoi.Rows.Add(R);
                R[0] = Convert.ToString(i);
                R[1] = Convert.ToString(i);
            }
            if (CoHangChon)
            {
                dtTuoi.Rows.InsertAt(dtTuoi.NewRow(), 0);
                dtTuoi.Rows[0]["TenTuoi"] = "-- Bạn chọn tuổi --";

            }
            return dtTuoi;
        }


        public static DataTable KT_GetDanhSachTaiKhoan(String sKyHieu="", Boolean All = false, String Tieude = "---Tất cả---")
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_DanhMucThamSo WHERE iTrangThai=1 AND sKyHieu = @sKyHieu");
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            DataTable dtKyHieu = Connection.GetDataTable(cmd);
            String sTK = "";
            if (dtKyHieu != null && dtKyHieu.Rows.Count > 0)
            {
                sTK = Convert.ToString(dtKyHieu.Rows[0]["sThamSo"]);
            }
            if (sTK !="")
            {
                //sTK =sTK.Replace(",", "','");
                String sSql =
                    "Select iID_MaTaiKhoan,(iID_MaTaiKhoan + '-' +sTen) as sTen from KT_TaiKhoan " +
                    " where iTrangThai = 1 ";
                string sDSTaiKhoan1 = "";
                string[] aTK = sTK.Split(',');
                for (int i = 0; i < aTK.Length; i++)
                {
                    if (sDSTaiKhoan1 == "")
                    {
                        sDSTaiKhoan1 = " iID_MaTaiKhoan = @sdk" + i.ToString();
                    }
                    else
                    {
                        sDSTaiKhoan1 += " or iID_MaTaiKhoan = @sdk" + i.ToString();
                    }
                }
                sDSTaiKhoan1 = " And (" + sDSTaiKhoan1 + ")";

                sSql += sDSTaiKhoan1;
                sSql += " group by iID_MaTaiKhoan,sTen";
                cmd = new SqlCommand(sSql);
                //'11211','11212','11213'
                //add parameter
                for (int i = 0; i < aTK.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sdk" + i.ToString(), aTK[i]);
                }
                dt = Connection.GetDataTable(cmd);

                if (All)
                {
                    DataRow R = dt.NewRow();
                    R["iID_MaTaiKhoan"] = "0";
                    R["sTen"] = Tieude;
                    dt.Rows.InsertAt(R, 0);
                }
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lay ra danh sách đối tượng
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_DoiTuong(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDT", typeof(String));
            dt.Columns.Add("TenDT", typeof(String));

            DataRow R = dt.NewRow();
            R["MaDT"] = "1";
            R["TenDT"] = "Trợ lý phòng ban";
            dt.Rows.InsertAt(R, 0);

            DataRow R1 = dt.NewRow();
            R1["MaDT"] = "2";
            R1["TenDT"] = "Trợ lý tổng hợp";
            dt.Rows.InsertAt(R1, 1);

            DataRow R2 = dt.NewRow();
            R2["MaDT"] = "3";
            R2["TenDT"] = "Trưởng phòng";
            dt.Rows.InsertAt(R2, 2);

            DataRow R4 = dt.NewRow();
            R4["MaDT"] = "4";
            R4["TenDT"] = "Trưởng phòng (kiêm thủ trưởng)";
            dt.Rows.InsertAt(R4, 4);

            DataRow R5 = dt.NewRow();
            R5["MaDT"] = "5";
            R5["TenDT"] = "Trợ lý tổng hợp Cục";
            dt.Rows.InsertAt(R5, 5);

            if (ThemDongTieuDe)
            {
                DataRow R6 = dt.NewRow();
                R6["MaDT"] = Guid.Empty;
                R6["TenDT"] = sDongTieuDe;
                dt.Rows.InsertAt(R6, 0);
            }
            return dt;
        }

        public static DataTable getQuy()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_Quy", typeof(int));
            dt.Columns.Add("sTen", typeof(String));
            for (int i = 1; i <= 4; i++)
            {
                DataRow dr = dt.NewRow();
                dr["iID_Quy"] = i;
                dr["sTen"] = "Quý " + i;
                dt.Rows.Add(dr);
                dr = null;

            }
            return dt;
        }
        public static DataTable getLoaiThuChi()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iIDLoai", typeof(int));
            dt.Columns.Add("sTen", typeof(String));

            DataRow dr0 = dt.NewRow();
            dr0["iIDLoai"] = 0;
            dr0["sTen"] = "Tất cả";
            dt.Rows.Add(dr0);
            dr0 = null;

            DataRow dr = dt.NewRow();
            dr["iIDLoai"] = 1;
            dr["sTen"] = "1 - Thu";
            dt.Rows.Add(dr);
            dr = null;
            DataRow dr1 = dt.NewRow();
            dr1["iIDLoai"] = 2;
            dr1["sTen"] = "2 - Chi";
            dt.Rows.Add(dr1);
            dr1 = null;

            return dt;
        }

    }
}
