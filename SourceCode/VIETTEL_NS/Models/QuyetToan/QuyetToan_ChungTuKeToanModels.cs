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
    public class QuyetToan_ChungTuKeToanModels
    {
        public static DataTable Get_dtChiTietChungTuKeToan(String iID_MaMucLucNganSach, String iQuy, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            if (iID_MaMucLucNganSach == null) iID_MaMucLucNganSach = "00000000-0000-0000-0000-000000000000";

            DK = "iTrangThai=1 AND iID_MaMucLucNganSach=@iID_MaMucLucNganSach AND iQuy=@iQuy";
            cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);

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

            SQL = String.Format("SELECT * FROM QTA_ChungTu_KeToan WHERE {0} ORDER BY dNgayTao", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static void ChuyenSoLieuSangQuyetToan(String iID_MaMucLucNganSach, String iQuy, String iNamLamViec)
        {
            DataTable dtChungTuKeToan;
            String SQL, DK;
            SqlCommand cmd;
            int i;
            if (iID_MaMucLucNganSach != null && iID_MaMucLucNganSach != "00000000-0000-0000-0000-000000000000")
            {
                cmd = new SqlCommand();

                DK = "iTrangThai=1 AND iID_MaMucLucNganSach=@iID_MaMucLucNganSach AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
                cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                SQL = String.Format("SELECT iID_MaMucLucNganSach, iID_MaDonVi, SUM(rTuChi) AS TuChi, SUM(rHienVat) AS HienVat FROM QTA_ChungTu_KeToan WHERE {0} GROUP BY iID_MaMucLucNganSach,iID_MaDonVi", DK);
                cmd.CommandText = SQL;

                dtChungTuKeToan = Connection.GetDataTable(cmd);
                cmd.Dispose();

                if (dtChungTuKeToan.Rows.Count > 0)
                {
                    for (i = 0; i < dtChungTuKeToan.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("UPDATE QTA_ChungTuChiTiet SET rTuChi=@rTuChi, rHienVat=@rHienVat WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy");
                        cmd.Parameters.AddWithValue("@rTuChi", dtChungTuKeToan.Rows[i]["TuChi"]);
                        cmd.Parameters.AddWithValue("@rHienVat", dtChungTuKeToan.Rows[i]["HienVat"]);
                        cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", iID_MaMucLucNganSach);
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTuKeToan.Rows[i]["iID_MaDonVi"]);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                        cmd.Parameters.AddWithValue("@iQuy", iQuy);
                        Connection.UpdateDatabase(cmd);
                    }
                }
            }
        }

        public static void ChuyenSoLieuSangQuyetToan_All(String iQuy, String iNamLamViec)
        {
            DataTable dtChungTuKeToan;
            String SQL, DK;
            SqlCommand cmd;
            int i;

            cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            SQL = String.Format("SELECT iID_MaMucLucNganSach, iID_MaDonVi, SUM(rTuChi) AS TuChi, SUM(rHienVat) AS HienVat FROM QTA_ChungTu_KeToan WHERE {0} GROUP BY iID_MaMucLucNganSach, iID_MaDonVi", DK);
            cmd.CommandText = SQL;

            dtChungTuKeToan = Connection.GetDataTable(cmd);
            cmd.Dispose();

            if (dtChungTuKeToan.Rows.Count > 0)
            {
                for (i = 0; i < dtChungTuKeToan.Rows.Count; i++)
                {
                    cmd = new SqlCommand("UPDATE QTA_ChungTuChiTiet SET rTuChi=@rTuChi, rHienVat=@rHienVat WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach AND iID_MaDonVi=@iID_MaDonVi AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy");
                    cmd.Parameters.AddWithValue("@rTuChi", dtChungTuKeToan.Rows[0]["TuChi"]);
                    cmd.Parameters.AddWithValue("@rHienVat", dtChungTuKeToan.Rows[0]["HienVat"]);
                    cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtChungTuKeToan.Rows[i]["iID_MaMucLucNganSach"]);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", dtChungTuKeToan.Rows[i]["iID_MaDonVi"]);
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iQuy", iQuy);
                    Connection.UpdateDatabase(cmd);
                }
            }
        }
    }
}