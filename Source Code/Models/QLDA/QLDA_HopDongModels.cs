using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Models
{
    public class QLDA_HopDongModels
    {
        public static NameValueCollection LayThongTin(String iID_MaHopDong)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_HopDong(iID_MaHopDong);
            String colName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_Grid_dtHopDong(String iID_MaHopDong, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_MaHopDong == null) iID_MaHopDong = "";

            DK = "iTrangThai=1";
            if (iID_MaHopDong != "") {
                DK += " AND iID_MaHopDong=@iID_MaHopDong";
                cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            }

            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }

            SQL = String.Format("SELECT * FROM QLDA_HopDongChiTiet WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_Row_HopDong(String iID_MaHopDong)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_HopDong WHERE iTrangThai=1 AND iID_MaHopDong=@iID_MaHopDong");
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Row_HopDongChiTiet(String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_HopDongChiTiet WHERE iTrangThai=1 AND iID_MaHopDong=@iID_MaHopDong AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn");
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_dtHopDong()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_HopDongChiTiet WHERE iTrangThai=1 ORDER BY sSoHopDong");
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DLL_HopDong(Boolean All = false)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_HopDong WHERE iTrangThai=1 ORDER BY sSoHopDong");
            vR = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = vR.NewRow();
                R["iID_MaHopDong"] = Guid.Empty;
                R["sSoHopDong"] = "--- Danh sách hợp đồng ---";
                vR.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return vR;
        }
        public static int Delete_HopDong(String iID_MaHopDong, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng TN_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM QLDA_HopDongChiTiet WHERE iID_MaHopDong=@iID_MaHopDong");
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng TN_ChungTu
            Bang bang = new Bang("QLDA_HopDong");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaHopDong;
            bang.Delete();
            return 1;
        }
        public static DataTable Get_DanhSach(String iID_MaDonViThiCong, String sSoHopDong, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (iID_MaDonViThiCong != Guid.Empty.ToString() && iID_MaDonViThiCong != null)
            {
                DK += " AND iID_MaDonViThiCong = @iID_MaDonViThiCong";
                cmd.Parameters.AddWithValue("@iID_MaDonViThiCong", iID_MaDonViThiCong);
            }
            if (sSoHopDong != null && sSoHopDong != "")
            {
                DK += " AND sSoHopDong = @sSoHopDong";
                cmd.Parameters.AddWithValue("@sSoHopDong", sSoHopDong);
            }            
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT * FROM QLDA_HopDongChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sSoHopDong", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iID_MaDonViThiCong, String sSoHopDong, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (iID_MaDonViThiCong != Guid.Empty.ToString() && iID_MaDonViThiCong != null)
            {
                DK += " AND iID_MaDonViThiCong = @iID_MaDonViThiCong";
                cmd.Parameters.AddWithValue("@iID_MaDonViThiCong", iID_MaDonViThiCong);
            }
            if (sSoHopDong != null && sSoHopDong != "")
            {
                DK += " AND sSoHopDong = @sSoHopDong";
                cmd.Parameters.AddWithValue("@sSoHopDong", sSoHopDong);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM QLDA_HopDongChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_DanhSachHopDong(String sSoHopDong, String TuNgay, String DenNgay,String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(sSoHopDong) == false && sSoHopDong != "")
            {
                DK += " AND sSoHopDong LIKE @sSoHopDong";
                cmd.Parameters.AddWithValue("@sSoHopDong", sSoHopDong);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT * FROM QLDA_HopDong WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachHopDong_Count(String sSoHopDong, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1";
            if (String.IsNullOrEmpty(sSoHopDong) == false && sSoHopDong != "")
            {
                DK += " AND sSoHopDong LIKE @sSoHopDong";
                cmd.Parameters.AddWithValue("@sSoHopDong", sSoHopDong);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM QLDA_HopDong WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Sum_Tien_HopDong(String iID_MaHopDong)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += "AND iID_MaHopDong=@iID_MaHopDong";
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);

            String SQL = String.Format("SELECT SUM(rSoTien) as rSoTien, SUM(rNgoaiTe) as rNgoaiTe, sTenNgoaiTe FROM QLDA_HopDongChiTiet WHERE {0} GROUP BY sTenNgoaiTe", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Sum_Tien_HopDong_Duan(String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += "AND iID_MaHopDong=@iID_MaHopDong AND iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn";
            cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);

            String SQL = String.Format("SELECT SUM(rSoTien) as rSoTien, SUM(rNgoaiTe) as rNgoaiTe, sTenNgoaiTe FROM QLDA_HopDongChiTiet WHERE {0} GROUP BY sTenNgoaiTe", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}