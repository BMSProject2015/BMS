using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
namespace VIETTEL.Models
{
    public class VayNoModels
    {
        public static DataTable LayDanhSachNoiDung(string maNoiDung, string tenNoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            String sqlQuery = String.Format("SELECT * FROM DC_DanhMucNoiDung WHERE  (iTrangThai=1)");
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(maNoiDung) && maNoiDung != "")
            {
                sqlQuery += " AND (iID_MaNoiDung=@ID_MaNoiDung)";
                cmd.Parameters.AddWithValue("@ID_MaNoiDung", maNoiDung);
            }
            if (!String.IsNullOrEmpty(tenNoiDung) && tenNoiDung != "")
            {
                sqlQuery += " AND (sTenNoiDung like @TenNoiDung)";
                cmd.Parameters.AddWithValue("@TenNoiDung", "%" + tenNoiDung + "%");
            }
            cmd.CommandText = sqlQuery;
           // cmd.Parameters.Add("@ID_MaNoiDung", maNoiDung);
            //cmd.Parameters.Add("@TenNoiDung", tenNoiDung);
           // DataTable result = Connection.GetDataTable(cmd);
            DataTable result = CommonFunction.dtData(cmd, "iID_MaNoiDung", Trang, SoBanGhi);
            return result;
        }
        public static DataTable LayDanhSachTaiKhoan(string MaND)
        {
           int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
           String sqlQuery = String.Format(@"SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' + sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1  AND iNam=@iNam
                                         AND (SELECT COUNT(iID_MaTaiKhoan) AS c from KT_TaiKhoan AS KT WHERE iID_MaTaiKhoan LIKE (KT_TaiKhoan.iID_MaTaiKhoan + '%')
                                                AND LEN(iID_MaTaiKhoan) > LEN(KT_TaiKhoan.iID_MaTaiKhoan)   )= 0
                                         ORDER BY iID_MaTaiKhoan");
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@iNam", iNamLamViec);
            cmd.CommandText = sqlQuery;
            DataTable result = Connection.GetDataTable(cmd);
            return result;
        }
        public static DataTable LayThongTinNoiDung(string ID)
        {
            String sqlQuery = String.Format(@"SELECT * FROM DC_DanhMucNoiDung
                                            WHERE iID_MaNoiDung = @ID");
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.Parameters.Add("@ID", ID);
            DataTable result = Connection.GetDataTable(cmd);
            return result;
        }
        public static DataTable LayThongTinVayVon(string iID_Vay)
        {
            String sqlQuery = String.Format(@"SELECT * FROM VN_Vay
                                            WHERE iID_Vay = @iID_Vay");
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.Parameters.Add("@iID_Vay", iID_Vay);
            DataTable result = Connection.GetDataTable(cmd);
            return result;
        }
        public static int XoaThongTinNoiDung(string ID)
        {
            return Connection.DeleteRecord("DC_DanhMucNoiDung", "iID_MaNoiDung", ID);
        }
        public static int XoaThongTinVayVon(string ID)
        {
            return Connection.DeleteRecord("VN_VayChiTiet", "iID_VayChiTiet", ID);
        }
        public static int LaySoBanGhiNoiDung(string maNoiDung, string tenNoiDung)
        {
            String sqlQuery =
                string.Format(
                    @"SELECT COUNT(*) AS ROWS_NUMBER
                    FROM DC_DanhMucNoiDung
                    WHERE (iTrangThai=1) ");
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(maNoiDung) && maNoiDung != "")
            {
                sqlQuery += " AND (iID_MaNoiDung=@ID_MaNoiDung)";
                cmd.Parameters.AddWithValue("@ID_MaNoiDung", maNoiDung);
            }
            if (!String.IsNullOrEmpty(tenNoiDung) && tenNoiDung != "")
            {
                sqlQuery += " AND (sTenNoiDung like @TenNoiDung)";
                cmd.Parameters.AddWithValue("@TenNoiDung", "%" + tenNoiDung + "%");
            }
            cmd.CommandText = sqlQuery;
            string rows_number = Connection.GetValueString(cmd, "0");
            return Convert.ToInt32(rows_number);
        }
        public static bool CheckExistNoiDung(string maNoiDung)
        {
            String sqlQuery = String.Format(@"SELECT * FROM DC_DanhMucNoiDung
                                            WHERE iID_MaNoiDung = @iID_MaNoiDung");
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.Parameters.Add("@iID_MaNoiDung", maNoiDung);
            DataTable result = Connection.GetDataTable(cmd);
            return result.Rows.Count == 0 ? false : true;

        }
        public static bool ThemMoiNoiDung(string strNoiDung)
        {
            char[] spliter = new char[] { ',' };
            string[] arrNoiDung = strNoiDung.Split(spliter, StringSplitOptions.None);
            string sqlQuery = string.Format(@"INSERT INTO DC_DanhMucNoiDung (iID_MaNoiDung,sTenNoiDung,sMoTaChung,iID_Loai,bPublic)
                                            VALUES( @iID_MaNoiDung,@sTenNoiDung,@sMoTaChung,@iLoai,@bPublic)
                                            WHERE NOT EXISTS (SELECT * FROM DC_DanhMucNoiDung WHERE iID_MaNoiDung = @iID_MaNoiDung)");
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.Parameters.Add("@iID_MaNoiDung", arrNoiDung[0]);
            cmd.Parameters.Add("@sTenNoiDung", arrNoiDung[1]);
            cmd.Parameters.Add("@sMoTaChung", arrNoiDung[2]);
            cmd.Parameters.Add("@iLoai", arrNoiDung[3]);
            //DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            //dtfi.ShortDatePattern = "dd/MM/yyyy";
            //dtfi.DateSeparator = "/";
            //DateTime objDate = Convert.ToDateTime(arrNoiDung[4], dtfi);
            //  cmd.Parameters.Add("@dNgayTao", objDate);
            cmd.Parameters.Add("@bPublic", arrNoiDung[5]);
            int affectedRow = 0;
            try
            {
                affectedRow = Connection.InsertRecord("DC_DanhMucNoiDung", cmd);

            }
            catch (Exception)
            {
                return false;
            }
            return affectedRow == 0 ? false : true;
        }
        public static bool SuaNoiDung(string strNoiDung)
        {
            char[] spliter = new char[] { ',' };
            string[] arrNoiDung = strNoiDung.Split(spliter, StringSplitOptions.None);
            string sqlQuery = string.Format(@"UPDATE DC_DanhMucNoiDung
                                            SET iID_MaNoiDung = @iID_MaNoiDung, sTenNoiDung = @sTenNoiDung,sMoTaChung = @sMoTaChung, iID_Loai = @iLoai,bPublic = @bPublic
                                            WHERE NOT EXISTS (SELECT * FROM DC_DanhMucNoiDung WHERE iID_MaNoiDung = @iID_MaNoiDung AND ID <> @ID) AND ID = @ID");
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.Parameters.Add("@ID", arrNoiDung[0]);
            cmd.Parameters.Add("@iID_MaNoiDung", arrNoiDung[1]);
            cmd.Parameters.Add("@sTenNoiDung", arrNoiDung[2]);
            cmd.Parameters.Add("@sMoTaChung", arrNoiDung[3]);
            cmd.Parameters.Add("@iLoai", arrNoiDung[4]);
            //DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            //dtfi.ShortDatePattern = "dd/MM/yyyy";
            //dtfi.DateSeparator = "/";
            //DateTime objDate = Convert.ToDateTime(arrNoiDung[5], dtfi);
            // cmd.Parameters.Add("@dNgayTao", objDate);
            cmd.Parameters.Add("@bPublic", arrNoiDung[6]);
            int affectedRow = 0;
            try
            {
                affectedRow = Connection.UpdateDatabase(cmd);

            }
            catch (Exception)
            {
                return false;
            }
            return affectedRow == 0 ? false : true;
        }
        public static DataTable getListVayVon(String iID_Vay,
            String MaNoiDung, String MaDonVi, string dFromNgayTao, string dToNgayTao, string dFromNgayTra, string dToNgayTra, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = string.Format(@"SELECT  DT1.*, DT3.sTen sTenPhongBan,DT4.sTen sTenDonVi, DT5.sTenNoiDung  FROM VN_VayChiTiet DT1
                                        LEFT JOIN VN_DonVi_BQuanLy DT2 ON DT1.iID_MaDonVi = DT2.iID_MaDonVi
                                        LEFT JOIN NS_PhongBan DT3 ON DT3.iID_MaPhongBan = DT2.iID_MaPhongBan
                                        LEFT JOIN NS_DonVi DT4 ON DT4.iID_MaDonVi = DT1.iID_MaDonVi
                                        LEFT JOIN DC_DanhMucNoiDung DT5 ON DT5.iID_MaNoiDung = DT1.iID_MaNoiDung
                                        WHERE 1 = 1");
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";
            if (iID_Vay != Guid.Empty.ToString() && iID_Vay != string.Empty)
            {
                SQL += " AND DT1.iID_Vay= @iID_Vay";
                cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            }
            if (MaDonVi != Guid.Empty.ToString() && MaDonVi != string.Empty)
            {
                SQL += " AND DT1.iID_MaDonVi= @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            }
            if (MaNoiDung != Guid.Empty.ToString() && MaNoiDung != string.Empty)
            {
                SQL += " AND DT1.iID_MaNoiDung= @iID_MaNoiDung";
                cmd.Parameters.AddWithValue("@iID_MaNoiDung", MaNoiDung);
            }
            if (dFromNgayTao != null && dFromNgayTao != string.Empty)
            {
                SQL += " AND DT1.dNgayVay >= @dFromNgayVay";

                DateTime objDate = Convert.ToDateTime(dFromNgayTao, dtfi);
                cmd.Parameters.AddWithValue("@dFromNgayVay", objDate);
            }
            if (dToNgayTao != null && dToNgayTao != string.Empty)
            {
                SQL += " AND DT1.dNgayVay <= @dToNgayVay";
                DateTime objDate = Convert.ToDateTime(dToNgayTao, dtfi);
                cmd.Parameters.AddWithValue("@dToNgayVay", objDate);
            }

            if (dFromNgayTra != null && dFromNgayTra != string.Empty)
            {
                SQL += " AND DT1.dHanPhaiTra >= @dFromHanTra";
                DateTime objDate = Convert.ToDateTime(dFromNgayTra, dtfi);
                cmd.Parameters.AddWithValue("@dFromHanTra", objDate);
            }
            if (dToNgayTra != null && dToNgayTra != string.Empty)
            {
                SQL += " AND DT1.dHanPhaiTra <= @dToNgayTra";
                DateTime objDate = Convert.ToDateTime(dToNgayTra, dtfi);
                cmd.Parameters.AddWithValue("@dToNgayTra", objDate);
            }
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayVay DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static DataTable getListVayVon(String Thang, String Nam, String MaNoiDung, String MaDonVi, string dFromNgayTao, string dToNgayTao, string dFromNgayTra, string dToNgayTra, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL =
                "SELECT  DT1.iID_VayChiTiet, DT1.rVayTrongThang, DT1.dNgayVay, DT1.dHanPhaiTra, DT1.bPublic, DT1.iID_MaDonVi, DT1.iID_MaKhoanVay, DT3.sTen sTenPhongBan,DT4.sTen sTenDonVi, DT5.sTenNoiDung  FROM VN_VayChiTiet DT1";
            SQL += " LEFT JOIN VN_DonVi_BQuanLy DT2 ON DT1.iID_MaDonVi = DT2.iID_MaDonVi";
            SQL += " LEFT JOIN NS_PhongBan DT3 ON DT3.iID_MaPhongBan = DT2.iID_MaPhongBan";
            SQL += " LEFT JOIN (SELECT * FROM  NS_DonVi WHERE iNamLamViec_DonVi={0}) DT4 ON DT4.iID_MaDonVi = DT1.iID_MaDonVi";
            SQL += "  LEFT JOIN DC_DanhMucNoiDung DT5 ON DT5.iID_MaNoiDung = DT1.iID_MaNoiDung WHERE 1 = 1";
            SQL = String.Format(SQL, NguoiDungCauHinhModels.iNamLamViec);
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";

            if (String.IsNullOrEmpty(Thang) == false && Thang != "")
            {
                SQL += " AND DT1.iThang = @Thang";
                cmd.Parameters.AddWithValue("@Thang", Thang);
            }
            if (String.IsNullOrEmpty(Nam) == false && Nam != "")
            {
                SQL += " AND DT1.iNam = @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }

            if (MaDonVi != Guid.Empty.ToString() && MaDonVi != string.Empty)
            {
                SQL += " AND DT1.iID_MaDonVi= @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            }
            if (MaNoiDung != Guid.Empty.ToString() && MaNoiDung != string.Empty)
            {
                SQL += " AND DT1.iID_MaNoiDung= @iID_MaNoiDung";
                cmd.Parameters.AddWithValue("@iID_MaNoiDung", MaNoiDung);
            }
            if (dFromNgayTao != null && dFromNgayTao != string.Empty)
            {
                SQL += " AND DT1.dNgayVay >= @dFromNgayVay";

                DateTime objDate = Convert.ToDateTime(dFromNgayTao, dtfi);
                cmd.Parameters.AddWithValue("@dFromNgayVay", objDate);
            }
            if (dToNgayTao != null && dToNgayTao != string.Empty)
            {
                SQL += " AND DT1.dNgayVay <= @dToNgayVay";
                DateTime objDate = Convert.ToDateTime(dToNgayTao, dtfi);
                cmd.Parameters.AddWithValue("@dToNgayVay", objDate);
            }

            if (dFromNgayTra != null && dFromNgayTra != string.Empty)
            {
                SQL += " AND DT1.dHanPhaiTra >= @dFromHanTra";
                DateTime objDate = Convert.ToDateTime(dFromNgayTra, dtfi);
                cmd.Parameters.AddWithValue("@dFromHanTra", objDate);
            }
            if (dToNgayTra != null && dToNgayTra != string.Empty)
            {
                SQL += " AND DT1.dHanPhaiTra <= @dToNgayTra";
                DateTime objDate = Convert.ToDateTime(dToNgayTra, dtfi);
                cmd.Parameters.AddWithValue("@dToNgayTra", objDate);
            }
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "DT1.dNgayVay DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int getListVayVon_Count(String Thang, String Nam, String MaNoiDung = "", String MaDonVi = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT COUNT(*) ROWNUMBER FROM VN_VayChiTiet WHERE 1  = 1";
            if (String.IsNullOrEmpty(Thang) == false && Thang != "")
            {
                SQL += " AND iThang = @Thang";
                cmd.Parameters.AddWithValue("@Thang", Thang);
            }
            if (String.IsNullOrEmpty(Nam) == false && Nam != "")
            {
                SQL += " AND iNam = @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            if (MaDonVi != "")
            {
                SQL += " AND iID_MaDonVi= @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            }

            if (MaNoiDung != "")
            {
                SQL += " AND iID_MaNoiDung= @iID_MaNoiDung";
                cmd.Parameters.AddWithValue("@iID_MaNoiDung", MaNoiDung);
            }
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable ListNoiDung()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = string.Format(@"SELECT iID_MaNoiDung, iID_MaNoiDung + '-'+ sTenNoiDung as sMaTen FROM DC_DanhMucNoiDung 
                                        WHERE bPublic = 1
                                        ORDER BY iID_MaNoiDung ASC");

            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable getTongThuVonLai(string iID_VayVonChiTiet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = string.Format(@" SELECT SUM(DT1.rThuVon) AS rThuVon, SUM(DT1.rThuLai) rThuLai FROM VN_ThuVonChiTiet DT1
                                        WHERE DT1.iID_VayChiTiet = @iID_VayChiTiet AND DT1.iTrangThai = 1 AND DT1.iID_MaTrangThaiDuyet = 1");

            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;



        }
        //phuonglt15
        public static NameValueCollection LayThongTin(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = getDetail(iID_MaChungTu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            if (dt != null) dt.Dispose();
            return Data;
        }
        /// <summary>
        /// Chi tiết chừng từ
        /// </summary>
        /// <param name="iID_Vay">Mã chúng tư vay</param>
        /// <returns>Danh sách chứng từ</returns>
        public static DataTable getDetail(String iID_Vay)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM VN_Vay WHERE iID_Vay=@iID_Vay");
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết vay vốn chứng từ theo đơn vị
        /// </summary>
        /// <param name="iID_VayChiTiet">Mã chi tiết chứng từ vay vốn</param>
        /// <returns></returns>
        public static DataTable getDetailChiTiet(String iID_VayChiTiet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM VN_VayChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet");
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_dtCayChiTiet(String iID_Vay)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM VN_Vay WHERE iTrangThai=1 and bPublic=1 and iID_Vay=@iID_Vay ORDER BY iSTT DESC");
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable GetDuyetChungTu(String iID_Vay)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM VN_DuyetVayNo WHERE iID_Vay=@iID_Vay ORDER BY dNgayTao DESC");
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable getListDS(String iID_Vay)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = string.Format(@"SELECT  DT1.*, DT3.sTen sTenPhongBan,DT4.sTen sTenDonVi, DT5.sTenNoiDung  FROM VN_VayChiTiet DT1
                                        LEFT JOIN VN_DonVi_BQuanLy DT2 ON DT1.iID_MaDonVi = DT2.iID_MaDonVi
                                        LEFT JOIN NS_PhongBan DT3 ON DT3.iID_MaPhongBan = DT2.iID_MaPhongBan
                                        LEFT JOIN (SELECT * FROM  NS_DonVi WHERE iNamLamViec_DonVi={0}) DT4 ON DT4.iID_MaDonVi = DT1.iID_MaDonVi
                                        LEFT JOIN DC_DanhMucNoiDung DT5 ON DT5.iID_MaNoiDung = DT1.iID_MaNoiDung
                                        WHERE 1 = 1",NguoiDungCauHinhModels.iNamLamViec);
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";
            if (iID_Vay != Guid.Empty.ToString() && iID_Vay != string.Empty)
            {
                SQL += " AND DT1.iID_Vay= @iID_Vay";
                cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            }
            SQL += " ORDER BY dNgayVay DESC";

            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Hàm trả về danh sách đơn vị vay vốn
        /// </summary>
        /// <param name="iID_Vay">Mã vay vốn theo chứng từ</param>
        /// <returns></returns>
        public static DataTable getListDS_Sua(String iID_Vay, String MaDonVi)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT  DT1.*, DT3.sTen sTenPhongBan,DT4.sTen sTenDonVi, DT5.sTenNoiDung  FROM VN_VayChiTiet DT1
                                        LEFT JOIN VN_DonVi_BQuanLy DT2 ON DT1.iID_MaDonVi = DT2.iID_MaDonVi
                                        LEFT JOIN NS_PhongBan DT3 ON DT3.iID_MaPhongBan = DT2.iID_MaPhongBan
                                        LEFT JOIN (SELECT * FROM  NS_DonVi WHERE iNamLamViec_DonVi={0}) DT4 ON DT4.iID_MaDonVi = DT1.iID_MaDonVi
                                        LEFT JOIN DC_DanhMucNoiDung DT5 ON DT5.iID_MaNoiDung = DT1.iID_MaNoiDung
                                        WHERE 1 = 1", NguoiDungCauHinhModels.iNamLamViec);
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "dd/MM/yyyy";
            dtfi.DateSeparator = "/";
            if (iID_Vay != Guid.Empty.ToString() && iID_Vay != string.Empty)
            {
                SQL += " AND DT1.iID_Vay= @iID_Vay";
                cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            }
            if (String.IsNullOrEmpty(MaDonVi) == false && MaDonVi != "")
            {
                SQL += " AND DT1.iID_MaDonVi= @MaDonVi";
                cmd.Parameters.AddWithValue("@MaDonVi", MaDonVi);
            }
            SQL += " ORDER BY dNgayVay DESC";

            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChungTu(String Thang, String Nam, String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeTinDung, MaND);

            //if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            //{
            //    DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
            //    cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            //}
            if (String.IsNullOrEmpty(Thang) == false && Thang != "")
            {
                DK += " AND iThang = @Thang";
                cmd.Parameters.AddWithValue("@Thang", Thang);
            }
            if (String.IsNullOrEmpty(Nam) == false && Nam != "")
            {
                DK += " AND iNam = @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND sSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT * FROM VN_Vay WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChungTu(String MaND, int iThang, int iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeTinDung, MaND);
            if (CommonFunction.IsNumeric(iThang))
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            if (CommonFunction.IsNumeric(iNam))
            {
                DK += " AND iNam = @iNam";
                cmd.Parameters.AddWithValue("@iNam", iNam);
            }
            DK += "  AND iTrangThai=1   ORDER BY dNgayChungTu DESC";
            String SQL = String.Format("SELECT iID_Vay, sSoChungTu FROM VN_Vay WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static int Get_DanhSachChungTu_Count(String Thang, String Nam, String MaND = "", String SoChungTu = "", String TuNgay = "", String DenNgay = "", String iID_MaTrangThaiDuyet = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeTinDung, MaND);

            if (String.IsNullOrEmpty(Thang) == false && Thang != "")
            {
                DK += " AND iThang = @Thang";
                cmd.Parameters.AddWithValue("@Thang", Thang);
            }
            if (String.IsNullOrEmpty(Nam) == false && Nam != "")
            {
                DK += " AND iNam = @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            if (String.IsNullOrEmpty(MaND) == false && MaND != "" && MaND != "admin")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND sSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT COUNT(*) FROM VN_Vay WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = getDetail(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            if (dt != null) dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM VN_VayChiTiet WHERE iID_Vay=@iID_Vay AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_Vay", MaChungTu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String MaChungTu)
        {
            int vR = -1;
            DataTable dt = getDetail(MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            if (dt != null) dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaChungTu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            DuToan_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng DT_ChungTuChiTiet            
            String SQL = "UPDATE VN_Vay SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_Vay=@iID_Vay";
            //if (TrangThaiTrinhDuyet)
            //{
            //    SQL = "UPDATE VN_VayChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_Vay=@iID_Vay";
            //}
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_Vay", iID_MaChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        public static String InsertDuyetChungTu(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("VN_DuyetVayNo");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_Vay", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }
        public static Boolean UpdateRecord(String iID_MaChungTu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("VN_Vay");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }

        public static DataTable getDonVibyChungTu(string iID_Vay)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
           // string SQL = string.Format(@"SELECT v.iID_VayChiTiet, dv.sTen, v.iID_MaDonVi FROM VN_VayChiTiet AS v, NS_DonVi AS dv WHERE v.iTrangThai=1 AND v.iID_MaDonVi = dv.iID_MaDonVi AND v.iID_Vay=@iID_Vay AND iNamLamViec_DonVi={0}", NguoiDungCauHinhModels.iNamLamViec);
            string SQL = string.Format(@"SELECT distinct dv.sTen, v.iID_MaDonVi FROM VN_VayChiTiet AS v, NS_DonVi AS dv WHERE v.iTrangThai=1 AND v.iID_MaDonVi = dv.iID_MaDonVi AND v.iID_Vay=@iID_Vay AND iNamLamViec_DonVi={0}", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR; 
        }

        /// <summary>
        /// Sửa 1 chứng từ chi tiết
        /// </summary>
        /// <param name="ParentID">Phần trước của danh sách giá trị truyền vào</param>
        /// <param name="Values">Danh sách giá trị truyền vào (Đối với Form là Form.Request)</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns>Danh sách các lỗi của giá trị truyền vào</returns>
        public static NameValueCollection SuaChungTuChiTiet(String iID_MaChungTu, NameValueCollection Values, String MaND, String IPSua)
        {
            List<String> lst_iID_MaChungTuChiTiet = new List<string>();
            String iID_MaChungTuChiTiet;
            for (int i = 0; i < Values.Count; i++)
            {
                String[] arr = Values.Keys[i].Split('_');
                iID_MaChungTuChiTiet = arr[0];
                if (lst_iID_MaChungTuChiTiet.IndexOf(iID_MaChungTuChiTiet) < 0)
                {
                    lst_iID_MaChungTuChiTiet.Add(iID_MaChungTuChiTiet);
                }
            }
            DataTable dtChungTu = getDetail(iID_MaChungTu);
            for (int i = 0; i < lst_iID_MaChungTuChiTiet.Count; i++)
            {
                iID_MaChungTuChiTiet = lst_iID_MaChungTuChiTiet[i];
                //if (Values[iID_MaChungTuChiTiet + "_ThayDoi"] == "1")
                {
                    Bang bang = new Bang("VN_VayChiTiet");
                    bang.MaNguoiDungSua = MaND;
                    bang.IPSua = IPSua;
                    NameValueCollection arrLoi = bang.TruyenGiaTri(iID_MaChungTuChiTiet, Values);

                    if (arrLoi.Count == 0)
                    {
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                        String DSTruongTien = "rLaiSuat,bMienLai,rDuVonCu,rDuLaiCu,rVayTrongThang,dHanPhaiTra,rThoiGianThuVon,rThuVon,rThuLai";
                        String[] arrDSTruongTien = DSTruongTien.Split(',');
                        //Tong số=0 trong trường hợp từ chối tuannn (đã sửa)
                        //DataTable dtChiTiet = Connection.GetDataTable(String.Format("SELECT {0} FROM VN_VayChiTiet WHERE iID_VayChiTiet='{1}'", DSTruongTien, iID_MaChungTuChiTiet));

                        //if (dtChiTiet.Rows.Count > 0)
                        //{
                        //    for (int j = 0; j < arrDSTruongTien.Length; j++)
                        //    {
                        //        if (bang.CmdParams.Parameters.IndexOf("@" + arrDSTruongTien[j]) >= 0 && CommonFunction.IsNumeric(bang.CmdParams.Parameters["@" + arrDSTruongTien[j]].Value))
                        //        {
                        //            rTongSo += Convert.ToDouble(bang.CmdParams.Parameters["@" + arrDSTruongTien[j]].Value);
                        //        }
                        //        else
                        //        {
                        //            rTongSo += Convert.ToDouble(dtChiTiet.Rows[0][arrDSTruongTien[j]]);
                        //        }
                        //    }
                        //}
                        //Sai khi bị từ chối vì chưa có param TongSo đã sửa
                        //if (bang.CmdParams.Parameters.IndexOf("@rTongSo") >= 0)
                        //    bang.CmdParams.Parameters["@rTongSo"].Value = rTongSo;
                        //else
                        //    bang.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);

                        //if (bang.CmdParams.Parameters.IndexOf("@iID_MaDuyetChungTuChiTiet") >= 0)
                        //{

                        //    //Cập nhập bảng DT_DuyetChungTuChiTiet
                        //    Bang bangDuyet = new Bang("DT_DuyetChungTuChiTiet");
                        //    bangDuyet.MaNguoiDungSua = MaND;
                        //    bangDuyet.IPSua = IPSua;

                        //    String iID_MaDuyetChungTuChiTiet = Values[iID_MaChungTuChiTiet + "_iID_MaDuyetChungTuChiTiet"];
                        //    if (String.IsNullOrEmpty(iID_MaDuyetChungTuChiTiet))
                        //    {
                        //        bangDuyet.DuLieuMoi = true;
                        //        bangDuyet.CmdParams.Parameters.AddWithValue("@iID_MaDuyetChungTu", dtChungTu.Rows[0]["iID_MaDuyetDuToanCuoiCung"]);
                        //        bangDuyet.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                        //        bangDuyet.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);

                        //        //Thêm iID_MaDuyetChungTuChiTiet vào bảng DT_ChungTuChiTiet
                        //        bang.CmdParams.Parameters["@iID_MaDuyetChungTuChiTiet"].Value = bangDuyet.GiaTriKhoa;
                        //    }
                        //    else
                        //    {
                        //        bangDuyet.DuLieuMoi = false;
                        //        bangDuyet.GiaTriKhoa = iID_MaDuyetChungTuChiTiet;
                        //    }
                        //    bangDuyet.CmdParams.Parameters.AddWithValue("@sLyDo", bang.CmdParams.Parameters["@sLyDo"].Value);
                        //    bangDuyet.CmdParams.Parameters.AddWithValue("@bDongY", bang.CmdParams.Parameters["@bDongY"].Value);
                        //    bangDuyet.Save();
                        //}
                        bang.Save();
                    }
                    else
                    {
                        return arrLoi;
                    }
                }
            }
            if (dtChungTu != null) dtChungTu.Dispose();
            return null;
        }
        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaChungTu)
        {
            String iID_MaDuyetChungTu;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetChungTuCuoiCung FROM VN_Vay WHERE iID_Vay=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            iID_MaDuyetChungTu = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM VN_VayChiTiet WHERE iID_Vay=@iID_MaChungTu AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE VN_DuyetVayNo SET sSua=@sSua WHERE iID_MaDuyetVayNo=@iID_MaDuyetChungTu");
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTu", iID_MaDuyetChungTu);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static DataTable getDetailChungTuChitiet(String iID_VayChiTiet)
        {
            string sql = "SELECT vay.iID_VayChiTiet, vay.iID_MaDonVi, dv.sTen, vay.iID_MaNoiDung, dm.sTenNoiDung, vay.iID_Loai," +
                        " vay.iThang, vay.iNam, vay.dNgayVay, vay.rLaiSuat, vay.bMienLai, vay.rDuVonCu, vay.rDuLaiCu," +
                        " vay.rVayTrongThang, vay.dHanPhaiTra, vay.rThoiGianThuVon, vay.rThuVon, vay.rThuLai, vay.sGhiChu, vay.sGhiChu" +
                        " FROM VN_VayChiTiet AS vay INNER JOIN" +
                        " NS_DonVi AS dv ON vay.iID_MaDonVi = dv.iID_MaDonVi INNER JOIN" +
                        " DC_DanhMucNoiDung AS dm ON vay.iID_MaNoiDung = dm.iID_MaNoiDung WHERE iID_VayChiTiet=@iID_VayChiTiet";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable GetChungTu(String iID_Vay)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM VN_Vay WHERE iTrangThai=1 AND iID_Vay=@iID_Vay");
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Kiem tra so chung tu có tồn tại không
        /// </summary>
        /// <param name="SoChungTu"></param>
        /// <returns></returns>
        public static Boolean CheckChungTu(string MaChungTu)
        {
            Boolean vR = false;
            string sql = "SELECT iID_Vay FROM VN_Vay WHERE sSoChungTu=@sSoChungTu";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@sSoChungTu", MaChungTu);
            cmd.CommandText = sql;
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            cmd.Dispose();
            if (dt != null) dt.Dispose();
            return vR;
        }
        /// <summary>
        /// Tháng theo chứng từ
        /// </summary>
        /// <param name="iID_Vay">Mã số chứng từ</param>
        /// <returns></returns>
        public static int ThangChungTu(string iID_Vay)
        {
            String SQL = String.Format("SELECT iThang FROM VN_Vay WHERE iID_Vay=@iID_Vay");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            cmd.CommandText = SQL;
            int sValue = Convert.ToInt32(Connection.GetValue(cmd, DateTime.Now.Month));
            cmd.Dispose();
            return sValue;
        }
        /// <summary>
        /// Năm chứng từ
        /// </summary>
        /// <param name="iID_Vay">Mã số chứng từ</param>
        /// <returns>Lấy ra năm theo số chứng từ</returns>
        public static int NamChungTu(string iID_Vay)
        {
            String SQL = String.Format("SELECT iNam FROM VN_Vay WHERE iID_Vay=@iID_Vay");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_Vay", iID_Vay);
            cmd.CommandText = SQL;
            int sValue = Convert.ToInt32(Connection.GetValue(cmd, DateTime.Now.Year));
            cmd.Dispose();
            return sValue;
        }

        /// <summary>
        /// Tháng theo chứng từ
        /// </summary>
        /// <param name="iID_Vay">Mã số chứng từ</param>
        /// <returns></returns>
        public static int ThangChungTuChiTiet(string iID_VayChiTiet)
        {
            String SQL = String.Format("SELECT iThang FROM iID_VayChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = SQL;
            int sValue = Convert.ToInt32(Connection.GetValue(cmd, DateTime.Now.Month));
            cmd.Dispose();
            return sValue;
        }
        /// <summary>
        /// Năm chứng từ
        /// </summary>
        /// <param name="iID_Vay">Mã số chứng từ</param>
        /// <returns>Lấy ra năm theo số chứng từ</returns>
        public static int NamChungTuChiTiet(string iID_VayChiTiet)
        {
            String SQL = String.Format("SELECT iNam FROM iID_VayChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = SQL;
            int sValue = Convert.ToInt32(Connection.GetValue(cmd, DateTime.Now.Year));
            cmd.Dispose();
            return sValue;
        }
        public static int Delete_ChungTu(String iID_MaChungTu, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng DT_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM VN_VayChiTiet WHERE iID_Vay=@iID_Vay");
            cmd.Parameters.AddWithValue("@iID_Vay", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng DT_DotNganSach
            Bang bang = new Bang("VN_Vay");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
            bang.Delete();
            return 1;
        }
    }
}
