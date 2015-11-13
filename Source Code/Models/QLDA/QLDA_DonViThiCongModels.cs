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
    public class QLDA_DonViThiCongModels
    {
        public static DataTable Get_DanhSach(String sMaDonViThiCong, String sTenDonViThiCong, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";
            if (String.IsNullOrEmpty(sMaDonViThiCong) == false && sMaDonViThiCong != "")
            {
                DK += " AND iID_MaDonViThiCong LIKE '%" + sMaDonViThiCong + "%'";
            }
            if (String.IsNullOrEmpty(sTenDonViThiCong) == false && sTenDonViThiCong != "")
            {
                DK += " AND (sTen LIKE N'%" + sTenDonViThiCong + "%'";
                DK += " OR sTenVietTat LIKE N'%" + sTenDonViThiCong + "%')";
            }
            String SQL = String.Format("SELECT * FROM QLDA_DonViThiCong WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String sMaDonViThiCong, String sTenDonViThiCong)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";
            if (String.IsNullOrEmpty(sMaDonViThiCong) == false && sMaDonViThiCong != "")
            {
                DK += " AND iID_MaDonViThiCong LIKE '%" + sMaDonViThiCong + "%'";
            }
            if (String.IsNullOrEmpty(sTenDonViThiCong) == false && sTenDonViThiCong != "")
            {
                DK += " AND (sTen LIKE N'%" + sTenDonViThiCong + "%'";
                DK += " OR sTenVietTat LIKE N'%" + sTenDonViThiCong + "%')";
            }
            String SQL = String.Format("SELECT Count(*) FROM QLDA_DonViThiCong WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_Row_Data(String iID_MaDonViThiCong)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_DonViThiCong WHERE iTrangThai = 1 AND iID_MaDonViThiCong=@iID_MaDonViThiCong";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonViThiCong", iID_MaDonViThiCong);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable ddl_DonViThiCong(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDonViThiCong, sTen " +
                    "FROM QLDA_DonViThiCong WHERE iTrangThai = 1 ORDER By sTen");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonViThiCong"] = Guid.Empty;
                R["sTen"] = "--- Nhà thầu ---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
    }
}