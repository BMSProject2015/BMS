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
    public class QLDA_QuyetToanHoanThanhModels
    {
        public static String Get_Max_Dot(String iNam)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iSoQuyetToan) FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static int Delete_Dot_QuyetToan(String iID_MaQuyetToanHoanThanh_SoPhieu, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng TN_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM QLDA_QuyetToanHoanThanh WHERE iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu");
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng TN_ChungTu
            Bang bang = new Bang("QLDA_QuyetToanHoanThanh_SoPhieu");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaQuyetToanHoanThanh_SoPhieu;
            bang.Delete();
            return 1;
        }
        public static DataTable Get_Row_DotQuyetToan(String iID_MaQuyetToanHoanThanh_SoPhieu, String iNam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu AND iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachQuyetToan_SoPhieu(String iNamLamViec, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iNamLamViec=@iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT * FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayQuyetToan DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachQuyetToan_SoPhieu_Count(String iNamLamViec, String sNguoiDung)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 AND iNamLamViec=@iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(sNguoiDung) == false && sNguoiDung != "")
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sNguoiDung);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }
        public static NameValueCollection LayThongTin(String iID_MaQuyetToanHoanThanh_SoPhieu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_QuyetToanHoanThanh_SoPhieu(iID_MaQuyetToanHoanThanh_SoPhieu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_Row_QuyetToanHoanThanh_SoPhieu(String iID_MaQuyetToanHoanThanh_SoPhieu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE iTrangThai = 1 AND iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Grid_dtQuyetToanHoanThanh(String iID_MaQuyetToanHoanThanh_SoPhieu, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_MaQuyetToanHoanThanh_SoPhieu == null) iID_MaQuyetToanHoanThanh_SoPhieu = "";


            DK = "iTrangThai=1 AND iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu";
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);

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

            SQL = String.Format("SELECT * FROM QLDA_QuyetToanHoanThanh WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            SQL = String.Format(@"SELECT dNgayQuyetToan FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE iID_MaQuyetToanHoanThanh_SoPhieu=@iID_MaQuyetToanHoanThanh_SoPhieu");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaQuyetToanHoanThanh_SoPhieu", iID_MaQuyetToanHoanThanh_SoPhieu);
            String dNgayQuyetToan = Connection.GetValueString(cmd, "01/01/2000");
            //Lay so cap phat
            SQL = String.Format(@"SELECT iID_MaDanhMucDuAn,sXauNoiMa,SUM(rDeNghiPheDuyetThanhToan) as rSoTienCap
                                    FROM QLDA_CapPhat
                                    WHERE rDeNghiPheDuyetThanhToan<>0 AND iTrangThai=1  AND  iID_MaDotCapPhat IN (SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE dNgayLap<=@dNgay AND iTrangThai=1)
                                    GROUP BY iID_MaDanhMucDuAn,sXauNoiMa");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", dNgayQuyetToan);
            DataTable dtCapPhat = Connection.GetDataTable(cmd);
            //Lay So quyet toan
            SQL = String.Format(@"SELECT iID_MaDanhMucDuAn,sXauNoiMa,SUM(rSoTienQuyetToan+rSoTienDieuChinh) as rSoTienQuyetToan
                                    FROM QLDA_QuyetToan
                                    WHERE   iTrangThai=1  AND  iID_MaQuyetToan_SoPhieu IN (SELECT iID_MaQuyetToan_SoPhieu FROM QLDA_QuyetToanHoanThanh_SoPhieu WHERE dNgayQuyetToan<=@dNgay AND iTrangThai=1)
                                    GROUP BY iID_MaDanhMucDuAn,sXauNoiMa
                                    HAVING SUM(rSoTienQuyetToan+rSoTienDieuChinh)<>0");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dNgay", dNgayQuyetToan);
            DataTable dtQuyetToan = Connection.GetDataTable(cmd);
            for (int i = 0; i < vR.Rows.Count; i++)
            {
                DataRow r = vR.Rows[i];
                String iID_MaDanhMucDuAn = Convert.ToString(r["iID_MaDanhMucDuAn"]);
                String sXauNoiMa = Convert.ToString(r["sXauNoiMa"]);
                DataRow[] rCapPhat = dtCapPhat.Select("iID_MaDanhMucDuAn='" + iID_MaDanhMucDuAn + "' AND sXauNoiMa='" + sXauNoiMa + "'");
                DataRow[] rQuyetToan = dtQuyetToan.Select("iID_MaDanhMucDuAn='" + iID_MaDanhMucDuAn + "' AND sXauNoiMa='" + sXauNoiMa + "'");
                if (rCapPhat.Length > 0)
                {
                    r["rSoTienCap"] = rCapPhat[0]["rSoTienCap"];
                }
                if (rQuyetToan.Length > 0)
                {
                    r["rSoTienQuyetToan"] = rQuyetToan[0]["rSoTienQuyetToan"];
                }
            }
          
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_DanhSach(String iID_MaDanhMucDuAn, String iNam, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iNamLamViec = @iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            if (iID_MaDanhMucDuAn != Guid.Empty.ToString() && iID_MaDanhMucDuAn != null)
            {
                DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            }
            //if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            //{
            //    DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
            //    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //}
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
            String SQL = String.Format("SELECT * FROM QLDA_QuyetToanHoanThanh WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayLap", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iID_MaDanhMucDuAn, String iNam, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iNamLamViec = @iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            if (iID_MaDanhMucDuAn != Guid.Empty.ToString() && iID_MaDanhMucDuAn != null)
            {
                DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            }
            //if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            //{
            //    DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
            //    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //}
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
            String SQL = String.Format("SELECT Count(*) FROM QLDA_QuyetToanHoanThanh WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}