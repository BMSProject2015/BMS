using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace VIETTEL.Models
{
    public class LuongModels
    {
        public const int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeLuong;

        #region Danh mục ngạch lương
        public static NameValueCollection LayThongTinNgachLuong(String iID_MaNgachLuong)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucNgachLuong(iID_MaNgachLuong);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_ChiTietDanhMucNgachLuong(String iID_MaNgachLuong)
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaNgachLuong) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaNgachLuong=@iID_MaNgachLuong";
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
                String SQL = "SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }


            return dt;
        }
        /// <summary>
        /// Lấy datatable Danh mục ngạch lương
        /// </summary>
        /// <param name="iID_MaBacLuong">Mã ngạch lương  nếu bằng "" lấy tất cả danh mục ngạch lương</param>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucNgachLuong(String iID_MaNgachLuong = "")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaNgachLuong) == false)
            {
                DK = " AND iID_MaNgachLuong=@iID_MaNgachLuong";
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
            }
            String SQL = "SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_dtDanhMucNgachLuong(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaNgachLuong", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucNgachLuong()
        {

            String SQL = "SELECT COUNT(*) FROM L_DanhMucNgachLuong WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

        public static String Get_TenNgachLuong(String iID_MaNgachLuong)
        {
            String SQL = "SELECT sTenNgachLuong FROM L_DanhMucNgachLuong WHERE iID_MaNgachLuong=@iID_MaNgachLuong AND iTrangThai=1";
            SqlCommand cmd = new SqlCommand(SQL);

            cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
        #endregion

        #region Danh mục bậc lương
        public static NameValueCollection LayThongTinBacLuong(String iID_MaBacLuong, String iID_MaNgachLuong)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucBacLuong(iID_MaNgachLuong, iID_MaBacLuong);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_ChiTietDanhMucBacLuong(String iID_MaNgachLuong, String iID_MaBacLuong)
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaBacLuong) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaBacLuong=@iID_MaBacLuong AND iID_MaNgachLuong=@iID_MaNgachLuong";
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
                String SQL = "SELECT * FROM L_DanhMucBacLuong WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }


            return dt;
        }
        /// <summary>
        /// Lấy datatable Danh mục bậc lương
        /// </summary>
        /// <param name="iID_MaBacLuong">Mã bậc lương  nếu bằng "" lấy tất cả danh mục bậc lương</param>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucBacLuong(String iID_MaBacLuong = "")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaBacLuong) == false)
            {
                DK = " AND iID_MaBacLuong=@iID_MaBacLuong";
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
            }
            String SQL = "SELECT * FROM L_DanhMucBacLuong WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_dtDanhMucBacLuong(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucBacLuong WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaBacLuong", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucBacLuong()
        {

            String SQL = "SELECT COUNT(*) FROM L_DanhMucBacLuong WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

        public static String Get_TenBacLuong(String iID_MaBacLuong)
        {
            String SQL = "SELECT sTenBacLuong FROM L_DanhMucBacLuong WHERE iID_MaBacLuong=@iID_MaBacLuong AND iTrangThai=1";
            SqlCommand cmd = new SqlCommand(SQL);

            cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }

        #endregion

        #region Danh mục tham số
        public static NameValueCollection LayThongTinThamSo(String iID_MaThamSo)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucThamSo(iID_MaThamSo);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        /// <summary>
        /// Lấy chi tiết danh mục tham số
        /// </summary>
        /// <param name="iID_MaThamSo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhMucThamSo_SapXep(DateTime ThoiGian)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM L_DanhMucThamSo " +
                         "WHERE iTrangThai=1 AND " +
                              "(CONVERT(VARCHAR(5),dThoiGianApDung_BatDau,111)>=@ThoiGian) AND " +
                              "(dThoiGianApDung_KetThuc IS NULL OR " +
                               "CONVERT(VARCHAR(5),dThoiGianApDung_KetThuc,111)<=@ThoiGian)" +
                         "ORDER BY dThoiGianApDung_BatDau,dThoiGianApDung_KetThuc";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@ThoiGian", ThoiGian.ToString("yy/MM"));
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy chi tiết danh mục tham số
        /// </summary>
        /// <param name="iID_MaThamSo"></param>
        /// <returns></returns>
        public static DataTable Get_ChiTietDanhMucThamSo(String iID_MaThamSo = "")
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaThamSo) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaThamSo=@iID_MaThamSo";
                cmd.Parameters.AddWithValue("@iID_MaThamSo", iID_MaThamSo);
                String SQL = "SELECT * FROM L_DanhMucThamSo WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                String SQL = "SELECT * FROM L_DanhMucThamSo WHERE iTrangThai=1 ORDER BY dThoiGianApDung_BatDau,dThoiGianApDung_KetThuc";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Lấy datatable Danh mục tham số
        /// </summary>
        /// <param name="iID_MaThamSo">Mã tham số  nếu bằng "" lấy tất cả danh mục tham số</param>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucThamSo(String iID_MaThamSo = "")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaThamSo) == false)
            {
                DK = " AND iID_MaThamSo=@iID_MaThamSo";
                cmd.Parameters.AddWithValue("@iID_MaThamSo", iID_MaThamSo);
            }
            String SQL = "SELECT * FROM L_DanhMucThamSo WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_dtDanhMucThamSo(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucThamSo WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sNoiDung", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucThamSo()
        {

            String SQL = "SELECT COUNT(*) FROM L_DanhMucThamSo WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }
        /// <summary>
        /// lấy giá trị lương tối thiểu
        /// </summary>
        /// <param name="MaThamSo">=50 là lương tối thiểu</param>
        /// <returns></returns>
        public static Decimal ThamSo_LuongToiThieu(String iID_MaThamSo)
        {
            String SQL = "SELECT sThamSo FROM L_DanhMucThamSo WHERE iID_MaThamSo=@iID_MaThamSo";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaThamSo", iID_MaThamSo);
            String vR = Connection.GetValueString(cmd, "0");
            if (CommonFunction.IsNumeric(vR))
                return Convert.ToDecimal(vR);
            else
                return 0;

        }

        #endregion

        #region Danh mục phụ cấp
        public static NameValueCollection LayThongTinPhuCap(String iID_MaPhuCap)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_dtChiTietPhuCap(iID_MaPhuCap);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_dtChiTietPhuCap(String iID_MaPhuCap)
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM L_DanhMucPhuCap WHERE iTrangThai=1 AND iID_MaPhuCap=@iID_MaPhuCap";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaPhuCap", iID_MaPhuCap);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        /// <summary>
        /// Lấy datatable danh mục phụ cấp
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucPhuCap()
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM L_DanhMucPhuCap WHERE iTrangThai=1";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        #endregion

        #region Bảng lương
        public static NameValueCollection LayThongTinBangLuong(String iID_MaBangLuong)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietBangLuong(iID_MaBangLuong);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_ChiTietBangLuong(String iID_MaBangLuong)
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaBangLuong) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaBangLuong=@iID_MaBangLuong";
                cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                String SQL = "SELECT * FROM L_BangLuong WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            return dt;
        }
        public static String Get_iID_MaBangLuong(String iID_MaDonVi, int iNamBangLuong, int iThangBangLuong)
        {
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            DK = " sDanhSachMaDonVi LIKE @iID_MaDonVi AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong";
            cmd.Parameters.AddWithValue("@iID_MaDonVi", "%" + iID_MaDonVi + ",%");
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            String SQL = "SELECT iID_MaBangLuong FROM L_BangLuong WHERE iTrangThai=1 AND {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }

        public static DataTable LayDanhSachDonViCuaBangLuong(String iID_MaBangLuong, Boolean All = false, String TextHienThi = "")
        {
            String SQL = "SELECT NS_DonVi.iID_MaDonVi,sTen,NS_DonVi.iID_MaDonVi+'-'+ sTen AS TenHT FROM( (SELECT distinct iID_MaDonVi FROM L_BangLuongChiTiet WHERE iID_MaBangLuong=@iID_MaBangLuong)L_BangLuongChiTiet";
            SQL += " INNER JOIN NS_DonVi ON NS_DonVi.iID_MaDonVi=L_BangLuongChiTiet.iID_MaDonVi)";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sTen"] = TextHienThi;
                R["TenHT"] = TextHienThi;
                dt.Rows.InsertAt(R, 0);
            }
            return dt;

        }

        /// <summary>
        /// Insert vào bảng DuyetBangLuong và lấy mã duyệt bảng lương
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="NoiDung"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertDuyetBangLuong(String iID_MaBangLuong, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChungTu;
            Bang bang = new Bang("L_DuyetBangLuong");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng BH_ChungTu, BH_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaBangLuong, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaBangLuong, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng BH_ChungTuChiTiet            
            String SQL = "UPDATE L_BangLuongChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaBangLuong=@iID_MaBangLuong";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE L_BangLuongChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaBangLuong=@iID_MaBangLuong";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        /// <summary>
        /// Cập nhập dữ liệu 1 Record của bảng lương
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="Params">Params là của cmd.Parameters</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean UpdateRecord(String iID_MaBangLuong, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("L_BangLuong");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaBangLuong;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }

        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaBangLuong)
        {
            String iID_MaDuyetBangLuong;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetBangLuongCuoiCung FROM L_BangLuong WHERE iID_MaBangLuong=@iID_MaBangLuong");
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            iID_MaDuyetBangLuong = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM L_BangLuongChiTiet WHERE bPhanTruyLinh=0 AND iTrangThai=1 AND iID_MaBangLuong=@iID_MaBangLuong AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}<br/>", dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE L_DuyetBangLuong SET sSua=@sSua WHERE iID_MaDuyetBangLuong=@iID_MaDuyetBangLuong");
            cmd.Parameters.AddWithValue("@iID_MaDuyetBangLuong", iID_MaDuyetBangLuong);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// Lấy danh sách bảng lương
        /// </summary>
        /// <param name="iNamBangLuong"></param>
        /// <param name="iThangBangLuong"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_dtBangLuong(String iNamBangLuong, String iThangBangLuong, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT iID_MaBangLuong, sTen, iID_MaTrangThaiDuyet, iThangBangLuong, iNamBangLuong FROM L_BangLuong WHERE iTrangThai=1 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountBangLuong(String iNamBangLuong, String iThangBangLuong)
        {
            Int16 vR = 0;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT COUNT(*) FROM L_BangLuong WHERE iTrangThai=1 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            cmd.Dispose();
            vR = Convert.ToInt16(Connection.GetValue(cmd, 0));
            return vR;
        }


        public static DataTable dt_TruongPhuCap()
        {

            DataTable dtBangLuongChiTiet = Get_dtBangLuongChiTiet(Guid.Empty.ToString(), null, Luong_BangDuLieu.iLoaiBangLuong_BangChiTiet);
            DataTable dt = new DataTable();
            dt.Columns.Add("sTenTruongPhuCap", typeof(String));
            dt.Columns.Add("sTruongPhuCap", typeof(String));

            DataRow R;
            String _columnsname;
            for (int i = 0; i < dtBangLuongChiTiet.Columns.Count; i++)
            {
                _columnsname=dtBangLuongChiTiet.Columns[i].ColumnName;
                if (_columnsname.StartsWith("r") && _columnsname.IndexOf("_HeSo") < 0)
                {
                    R = dt.NewRow();
                    R["sTenTruongPhuCap"] = _columnsname;
                    R["rTruongPhuCap"] = _columnsname;
                    dt.Rows.Add(R);
                }
            }
            return dt;
        }

        #endregion

        #region Bảng lương chi tiết
        /// <summary>
        /// Lấy data thông tin chi tiết bảng lương chi tiết
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTinBangLuongChiTiet(String iID_MaBangLuongChiTiet)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietBangLuongChiTiet(iID_MaBangLuongChiTiet);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        /// <summary>
        /// Lấy datatable chi tiết bảng lương chi tiết
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <returns></returns>
        public static DataTable Get_ChiTietBangLuongChiTiet(String iID_MaBangLuongChiTiet)
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaBangLuongChiTiet) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet";
                cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                String SQL = "SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            return dt;
        }
        /// <summary>
        /// Lấy danh sách bảng lương chi tiết
        /// </summary>
        /// <param name="iNamBangLuong"></param>
        /// <param name="iThangBangLuong"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_dtBangLuongChiTiet(String iID_MaBangLuong, Dictionary<String, String> arrGiaTriTimKiem, int iLoaiBangLuong)
        {
            DataTable vR;
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iID_MaBangLuong=@iID_MaBangLuong";
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            switch (iLoaiBangLuong)
            {
                case Luong_BangDuLieu.iLoaiBangLuong_BangChiTiet:
                    //Bảng lương chi tiết
                    //DK += " AND bOmDaiNgay=0";
                    DK += " AND bPhanTruyLinh=0";
                    break;

                case Luong_BangDuLieu.iLoaiBangLuong_BangThueTNCN:
                    //Bảng lương thuế TNCN
                    DK += " AND bPhanTruyLinh=0";
                    break;

                case Luong_BangDuLieu.iLoaiBangLuong_BangBaoHiem:
                    //Bảng lương bảo hiểm
                    //DK += " AND bOmDaiNgay=1";
                    DK += " AND bPhanTruyLinh=0";
                    break;

                case Luong_BangDuLieu.iLoaiBangLuong_BangTruyLinh:
                    //Bảng lương bảo hiểm
                    //DK += " AND bOmDaiNgay=1";
                    DK += " AND bPhanTruyLinh=1";
                    break;
            }
            //if (arrGiaTriTimKiem != null)
            //{
            //    String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            //    String[] arrDSTruong = DSTruong.Split(',');
            //    for (int i = 0; i < arrDSTruong.Length; i++)
            //    {
            //        if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
            //        {
            //            DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
            //            cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
            //        }
            //    }
            //}
            String SQL = String.Format("SELECT * FROM L_BangLuongChiTiet WHERE {0} ORDER BY iID_MaDonVi,sTen_CanBo,sHoDem_CanBo", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách bảng phụ cấp của cá nhân
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <returns></returns>
        public static DataTable Get_dtBangLuongChiTiet_PhuCap(String iID_MaBangLuongChiTiet)
        {
            DataTable vR;
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            String SQL = String.Format("SELECT * FROM L_BangLuongChiTiet_PhuCap WHERE iTrangThai=1 AND iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet ORDER BY iID_MaPhuCap");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String Get_MaBangLuongChiTiet(String iID_MaCanBo)
        {
            String SQL = String.Format("SELECT iID_MaBangLuongChiTiet FROM L_BangLuongChiTiet WHERE bPhanTruyLinh=0 AND iID_MaCanBo='{0}'", iID_MaCanBo);
            return Connection.GetValueString(SQL, "");
        }

        /// <summary>
        /// Lấy danh sách bảng phụ cấp của cá nhân
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <returns></returns>
        public static DataTable Get_dtBangLuongChiTiet_PhuCap_HienThi(String iID_MaBangLuongChiTiet)
        {
            DataTable vR;
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            String SQL = String.Format("SELECT * FROM L_BangLuongChiTiet_PhuCap WHERE iTrangThai=1 AND bLuonCo=0 AND iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet ORDER BY iID_MaPhuCap");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        private static int KhoangCachThang(DateTime TuThoiGian, DateTime DenThoiGian)
        {
            int vR = 0;
            while (String.Compare(DenThoiGian.ToString("yyyy/MM"), TuThoiGian.ToString("yyyy/MM")) > 0)
            {
                vR++;
                DenThoiGian = DenThoiGian.AddMonths(-1);
            }
            return vR;
        }

        public static object get_NamThamNien(int Thang, int Nam, object dNgayNhapNgu, object dNgayXuatNgu, object dNgayTaiNgu)
        {
            object vR = 0;
            int SoThang = -1;
            DateTime dHienTai = new DateTime(Nam, Thang, 1);
            if (dNgayTaiNgu != null && dNgayTaiNgu != DBNull.Value)
            {
                SoThang = 0;
                SoThang += KhoangCachThang(Convert.ToDateTime(dNgayNhapNgu), Convert.ToDateTime(dNgayXuatNgu));
                SoThang += KhoangCachThang(Convert.ToDateTime(dNgayTaiNgu), Convert.ToDateTime(dHienTai));
            }
            else if (dNgayNhapNgu != null && dNgayNhapNgu != DBNull.Value)
            {
                SoThang = 0;
                SoThang += KhoangCachThang(Convert.ToDateTime(dNgayNhapNgu), Convert.ToDateTime(dHienTai));
            }
            if (SoThang >= 0)
            {
                vR = SoThang / 12;
            }
            return vR;
        }

        public static String get_sNXTNgu(object dNgayNhapNgu, object dNgayXuatNgu, object dNgayTaiNgu)
        {
            String vR = "";
            if (dNgayNhapNgu != null && dNgayNhapNgu != DBNull.Value)
            {
                vR = Convert.ToDateTime(dNgayNhapNgu).ToString("MM_yy");
            }
            if (dNgayXuatNgu != null && dNgayXuatNgu != DBNull.Value)
            {
                vR += "," + Convert.ToDateTime(dNgayXuatNgu).ToString("MM_yy");
            }
            if (dNgayTaiNgu != null && dNgayTaiNgu != DBNull.Value)
            {
                vR += "," + Convert.ToDateTime(dNgayTaiNgu).ToString("MM_yy");
            }
            return vR;
        }
        /// <summary>
        /// Lấy danh sách bảng lương chi tiết theo tháng làm việc và năm làm việc
        /// </summary>
        /// <param name="iNamBangLuong"></param>
        /// <param name="iThangBangLuong"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_dtBangLuongChiTiet(String iNamBangLuong, String iThangBangLuong, Boolean DaDuyet = false)
        {
            DataTable vR;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (DaDuyet)
            {
                DK = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(iID_MaPhanHe));
            }

            String SQL = String.Format("SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong {0}", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách bảng lương chi tiết có phân trang
        /// </summary>
        /// <param name="iNamBangLuong"></param>
        /// <param name="iThangBangLuong"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_dtBangLuongChiTiet(String iNamBangLuong, String iThangBangLuong, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            vR = CommonFunction.dtData(cmd, "iThangBangLuong", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Đếm số bản ghi bảng lương chi tiết
        /// </summary>
        /// <param name="iNamBangLuong"></param>
        /// <param name="iThangBangLuong"></param>
        /// <returns></returns>
        public static int Get_CountBangLuongChiTiet(String iNamBangLuong, String iThangBangLuong)
        {
            String SQL = "SELECT COUNT(*) FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND iNamBangLuong=@iNamBangLuong AND iThangBangLuong=@iThangBangLuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
            cmd.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            cmd.Dispose();
            return Convert.ToInt16(Connection.GetValue(cmd, 0));
        }

        public static DataTable Get_dtChiTietThongTinCanBo(String iID_MaCanBo)
        {
            DataTable dt = null;
            String SQL = "SELECT * FROM CB_CanBo WHERE iID_MaCanBo=@iID_MaCanBo ";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtCanBoDangCongTacCuaDonVi(String iID_MaDonVi)
        {
            DataTable dt = null;
            String SQL = "SELECT * FROM CB_CanBo c WHERE c.iID_MaDonVi=@iID_MaDonVi AND c.iID_MaTinhTrangCanBo>=0 AND c.iTrangThai=1 AND NOT EXISTS (SELECT 1 from L_BangLuongChiTiet l where c.iID_MaCanBo=l.iID_MaCanBo)";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_DSDonViCuaBangLuong(String iID_MaBangLuong, int iNamLamViec)
        {
            String SQL = "SELECT sDanhSachMaDonVi FROM L_BangLuong WHERE iTrangThai=1 AND iID_MaBangLuong=@iID_MaBangLuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            String dsMaDonVi = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            String[] arrMaDonVi = dsMaDonVi.Split(',');
            cmd = new SqlCommand();
            String DK = "";
            for (int i = 0; i < arrMaDonVi.Length; i++)
            {
                if (arrMaDonVi[i] != "")
                {
                    if (DK != "") DK += " OR ";
                    DK += "iID_MaDonVi=@iID_MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrMaDonVi[i]);
                }
            }
            DK += " AND iNamLamViec_DonVi=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            SQL = "SELECT * FROM NS_DonVi WHERE {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách phụ cấp của cán bộ
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <returns></returns>
        public static DataTable Get_dtLuongPhuCap(String iID_MaBangLuongChiTiet)
        {
            DataTable dt = null;
            String SQL = "SELECT * FROM L_BangLuongChiTiet_PhuCap WHERE iTrangThai=1 AND iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }


        #endregion

        #region "Danh mục loại công thức"
        public static DataTable Get_dtDanhMucLoaiCongThuc()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucLoaiCongThuc WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        #endregion

        #region "Danh mục mục lục ngân sách theo trường"
        public static DataTable Get_dtDanhMucTruong_MucLucNganSach()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucTruong_MucLucNganSach WHERE iTrangThai=1 ORDER BY sXauNoiMa,sMaTruong,iID_MaNgachLuong,iID_MaBacLuong");
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_dtDanhMucTruong_MucLucNganSach_ChiTiet(String sMaTruong, String iID_MaNgachLuong, String iID_MaBacLuong)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1";
            DK += " AND sMaTruong=@sMaTruong";
            cmd.Parameters.AddWithValue("@sMaTruong", sMaTruong);
            if (String.IsNullOrEmpty(iID_MaNgachLuong))
            {
                DK += " AND iID_MaNgachLuong=@iID_MaNgachLuong";
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
            }
            if (String.IsNullOrEmpty(iID_MaBacLuong))
            {
                DK += " AND iID_MaBacLuong=@iID_MaBacLuong";
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", iID_MaBacLuong);
            }
            String SQL = String.Format("SELECT * FROM L_DanhMucTruong_MucLucNganSach WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        #endregion

        #region "Tao SSL"
        /// <summary>
        /// Tạo số sổ lương
        /// </summary>
        /// <param name="sKyHieu"></param>
        /// <param name="sSSL_Max"></param>
        /// <returns></returns>
        public static string TaoSSL(string sKyHieu, int sSSL_Max)
        {
            string vR = "";
            //string sql = "SELECT substring(MAX(sSoSoLuong_CanBo),2, len(MAX(sSoSoLuong_CanBo))) AS SSL FROM L_BangLuongChiTiet WHERE iTrangThai=1";
            // SqlCommand cmd = new SqlCommand(sql);
            // int sSSL_Max = Convert.ToInt32(Connection.GetValue(cmd, 0));
            // cmd.Dispose();

            if (sSSL_Max > 0 && sSSL_Max < 9)
                vR += sKyHieu + "00000" + (sSSL_Max + 1);
            else if (sSSL_Max >= 10 && sSSL_Max < 99)
                vR += sKyHieu + "0000" + (sSSL_Max + 1);
            else if (sSSL_Max >= 100 && sSSL_Max < 999)
                vR += sKyHieu + "000" + (sSSL_Max + 1);
            else if (sSSL_Max >= 1000 && sSSL_Max < 9999)
                vR += sKyHieu + "00" + (sSSL_Max + 1);
            else if (sSSL_Max >= 10000 && sSSL_Max < 99999)
                vR += sKyHieu + "0" + (sSSL_Max + 1);
            else vR += sKyHieu + "" + (sSSL_Max + 1);
            return vR;
        }
        //lấy số sổ lương lớn nhất
        public static int SSL_Max()
        {
            string sql = "SELECT substring(MAX(sSoSoLuong_CanBo),2, len(MAX(sSoSoLuong_CanBo))) AS SSL FROM L_BangLuongChiTiet WHERE iTrangThai=1";
            SqlCommand cmd = new SqlCommand(sql);
            int sSSL_Max = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return sSSL_Max;
        }
        /// <summary>
        /// Cập nhật số sổ lương
        /// </summary>
        public static void UpdateSSL()
        {
            DataTable vR;
            string SQL = @"SELECT L.iID_MaBangLuongChiTiet, N.sKyHieu FROM L_BangLuongChiTiet AS L INNER JOIN 
L_DanhMucNgachLuong AS N ON L.iID_MaNgachLuong_CanBo = N.iID_MaNgachLuong WHERE (L.sSoSoLuong_CanBo = '') OR (L.sSoSoLuong_CanBo IS NULL) ORDER BY L.sTen_CanBo, L.sHoDem_CanBo";
            SqlCommand cmd = new SqlCommand(SQL);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int sSSL_Max = SSL_Max();
            if (vR != null && vR.Rows.Count > 0)
            {
                for (int i = 0; i < vR.Rows.Count; i++)
                {
                    DataRow dr = vR.Rows[i];
                    string sKyHieu = dr["sKyHieu"].ToString();
                    string iID_MaBangLuongChiTiet = dr["iID_MaBangLuongChiTiet"].ToString();
                    string SSL = TaoSSL(sKyHieu, sSSL_Max + i + 1);
                    String mySQL = "UPDATE L_BangLuongChiTiet SET sSoSoLuong_CanBo=@sSoSoLuong_CanBo WHERE iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet";
                    SqlCommand cmdSQL = new SqlCommand(mySQL);
                    cmdSQL.Parameters.AddWithValue("@sSoSoLuong_CanBo", SSL);
                    cmdSQL.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                    Connection.UpdateDatabase(cmdSQL);
                    cmdSQL.Dispose();
                }
            }

        }
        #endregion
    }
}