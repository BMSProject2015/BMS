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
    public class QLDA_DuToan_NamModels
    {
        public static NameValueCollection LayThongTin(String iID_MaDuToanNam_QuyetDinh)
        {
            //NameValueCollection Data = new NameValueCollection();
            //Data["dNgayLap"] = dNgayLap;
            //return Data;
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_DuToanNam_QuyetDinh(iID_MaDuToanNam_QuyetDinh);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_Row_DuToanNam_QuyetDinh(String iID_MaDuToanNam_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_DuToan_Nam_ChungTu WHERE iTrangThai = 1 AND iID_MaDuToanNam_QuyetDinh=@iID_MaDuToanNam_QuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanNam_QuyetDinh", iID_MaDuToanNam_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_Grid_dtDuToanNam(String iID_MaDuToanNam_QuyetDinh, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            if (iID_MaDuToanNam_QuyetDinh == null) iID_MaDuToanNam_QuyetDinh = Guid.Empty.ToString();


            DK = "iTrangThai=1 AND iID_MaDuToanNam_QuyetDinh=@iID_MaDuToanNam_QuyetDinh";
            cmd.Parameters.AddWithValue("@iID_MaDuToanNam_QuyetDinh", iID_MaDuToanNam_QuyetDinh);
            //if (dNgayLap == null) dNgayLap = "";


            //DK = "iTrangThai=1 AND Convert(NVARCHAR(10),dNgayLap,103)=@dNgayLap";
            //cmd.Parameters.AddWithValue("@dNgayLap", dNgayLap);

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

            SQL = String.Format("SELECT * FROM QLDA_DuToan_Nam WHERE {0} ORDER BY sTenDuAn", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_Row_Data(String iID_MaDuToanNam)
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM QLDA_DuToan_Nam WHERE iTrangThai=1 AND iID_MaDuToanNam=@iID_MaDuToanNam");
            cmd.Parameters.AddWithValue("@iID_MaDuToanNam", iID_MaDuToanNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSach(String iNam, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iNamLamViec = @iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
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
            String SQL = String.Format("SELECT * FROM QLDA_DuToan_Nam WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayLap", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iNam, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            DK += " AND iNamLamViec = @iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
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
            String SQL = String.Format("SELECT Count(*) FROM QLDA_DuToan_Nam WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        public static DataTable Get_DanhSach_DuToanNam_QuyetDinh(String iDot, String TuNgay, String DenNgay, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(iDot) == false && iDot != "")
            {
                DK += " AND iDot = @iDot";
                cmd.Parameters.AddWithValue("@iDot", iDot);
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
            String SQL = String.Format("SELECT * FROM QLDA_DuToan_Nam_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_DuToanNam_QuyetDinh_Count(String iDot, String TuNgay, String DenNgay, String sNguoiDung)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(iDot) == false && iDot != "")
            {
                DK += " AND iDot = @iDot";
                cmd.Parameters.AddWithValue("@iDot", iDot);
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
            String SQL = String.Format("SELECT COUNT(*) FROM QLDA_DuToan_Nam_ChungTu WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_Row_DuToanNam_ChungTu(String iID_MaDuToanNam_QuyetDinh)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_DuToan_Nam_ChungTu WHERE iTrangThai = 1 AND iID_MaDuToanNam_QuyetDinh=@iID_MaDuToanNam_QuyetDinh";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanNam_QuyetDinh", iID_MaDuToanNam_QuyetDinh);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static String Get_Max_Dot(String iNam)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iDot) FROM QLDA_DuToan_Nam_ChungTu WHERE iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static int CheckExits_DuToan(object iID_MaDuToanNam_QuyetDinh)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(iID_MaDuToanNam) FROM QLDA_DuToan_Nam WHERE iID_MaDuToanNam_QuyetDinh=@iID_MaDuToanNam_QuyetDinh AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuToanNam_QuyetDinh", iID_MaDuToanNam_QuyetDinh);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int CheckExits_DuToanNam_ChungTu(object dNgayLap)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(iID_MaDuToanNam_QuyetDinh) FROM QLDA_DuToan_Nam_ChungTu WHERE  iTrangThai = 1 AND Convert(NVARCHAR(10),dNgayLap,103)=@dNgayLap";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@dNgayLap", dNgayLap);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}