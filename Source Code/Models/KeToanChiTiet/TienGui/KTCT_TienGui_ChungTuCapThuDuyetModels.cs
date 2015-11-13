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
    public class KTCT_TienGui_ChungTuCapThuDuyetModels
    {
        public static NameValueCollection LayThongTin(String iID_MaChungTu_Duyet)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_Row_ChungTu_Duyet(iID_MaChungTu_Duyet);
            String colName = "";
            if (dt != null && dt.Rows.Count > 0)
            {


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            if (dt != null) dt.Dispose();
            return Data;
        }
        public static DataTable Get_Row_ChungTu_Duyet(String iID_MaChungTu_Duyet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM KTTG_ChungTuCapThu_Duyet WHERE iTrangThai = 1 AND iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static Double Get_SoTienRutDuToan(String iID_MaChungTu_Duyet)
        {
            Double vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT SUM(rDTRut) FROM KTKB_ChungTuChiTiet WHERE iTrangThai = 1 AND iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            vR = Convert.ToDouble(Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }
        public static String Get_Max_ChungTu(String iNam)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT MAX(iSoChungTu) FROM KTTG_ChungTuCapThu_Duyet WHERE iNamLamViec=@iNamLamViec AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_So_ChungTu(String sSoUyNhiemChi)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sSoChungTu FROM KTTG_ChungTuCapThu_Duyet WHERE sSoChungTu=@sSoChungTu AND iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sSoChungTu", sSoUyNhiemChi);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachChungTu(String iNamLamViec, String sNguoiDung, int Trang = 1, int SoBanGhi = 0)
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
            String SQL = String.Format("SELECT * FROM KTTG_ChungTuCapThu_Duyet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String iNamLamViec, String sNguoiDung)
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
            String SQL = String.Format("SELECT COUNT(*) FROM KTTG_ChungTuCapThu_Duyet WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_Grid_dtChungTu(String iID_MaCapPhat, String iID_MaDonVi, String dTuNgay, String dDenNgay, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();


            DK = "iTrangThai=1  AND bDuyet = 0 ";
            String[] arrMaCapPhat = iID_MaCapPhat.Split(',');
            if (arrMaCapPhat.Length > 0)
            {
                DK += " AND ( ";
                for (int i = 0; i < arrMaCapPhat.Length; i++)
                {
                    DK += " iID_MaCapPhat=@iID_MaCapPhat" + i;
                    if (i < arrMaCapPhat.Length - 1)
                        DK += " OR ";
                    cmd.Parameters.AddWithValue("@iID_MaCapPhat" + i, arrMaCapPhat[i]);
                }
                DK += " ) ";
            }
            if (iID_MaDonVi != null && iID_MaDonVi != "")
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(dTuNgay) == false && dTuNgay != "")
            {
                DK += " AND dNgayTao >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(dTuNgay));
            }
            if (String.IsNullOrEmpty(dDenNgay) == false && dDenNgay != "")
            {
                DK += " AND dNgayTao <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(dDenNgay));
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

            SQL = String.Format(@"SELECT sTienToChungTu+CAST(iSoCapPhat as nvarchar) as sSoCapPhat,KTTG_ChungTuChiTietCapThu.*
                                    FROM (
                                    SELECT * FROM CP_CapPhat
                                    ) as CP_CapPhat
                                    INNER JOIN (
                                    SELECT * FROM KTTG_ChungTuChiTietCapThu WHERE {0} ) as KTTG_ChungTuChiTietCapThu
                                    ON CP_CapPhat.iID_MaCapPhat=KTTG_ChungTuChiTietCapThu.iID_MaCapPhat ORDER BY sSoCapPhat
                                ", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static int Check_Ma_ChungTuChiTiet(String iID_MaChungTuChiTiet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT COUNT(*) FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet ANDD iTrangThai = 1";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Grid_dtChungTu_ChiTiet_Duyet(String iID_MaChungTu_Duyet, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "A.iTrangThai = 1 AND  A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet ";
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
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

            SQL = String.Format(" SELECT C.bLoai, A.iID_MaChungTuChiTiet_Duyet, A.iID_MaChungTu_Duyet, B.* FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet INNER JOIN   KTTG_TinhChatCapThu as C  ON b.iID_MaTinhChatCapThu=c.iID_MaTinhChatCapThu WHERE {0} ORDER BY dNgayTao DESC", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}