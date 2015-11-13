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
    public class KeToanNgoaiTeModels
    {
        public static NameValueCollection LayThongTinNhanVien(String iID_MaNhanVien)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucNhanVien(iID_MaNhanVien);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_ChiTietDanhMucNhanVien(String iID_MaNhanVien)
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaNhanVien) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaNgoaiTe=@iID_MaNgoaiTe";
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iID_MaNhanVien);
                String SQL = "SELECT * FROM KTKB_NgoaiTe WHERE iTrangThai=1 AND {0}";
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
        public static DataTable Get_dtDanhMucNhanVien(String iID_MaNhanVien = "")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaNhanVien) == false)
            {
                DK = " AND iID_MaNgoaiTe=@iID_MaNgoaiTe";
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iID_MaNhanVien);
            }
            String SQL = "SELECT * FROM KTKB_NgoaiTe WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_dtDanhMucNhanVien(int Trang = 1, int SoBanGhi = 0, int iNamLamViec= 2012, int iThangLamViec = 1)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM KTKB_NgoaiTe WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThangLamViec=@iThangLamViec");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangLamViec", iThangLamViec);
            vR = CommonFunction.dtData(cmd, "iID_MaNgoaiTe", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucNhanVien(int iNamLamViec, int iThangLamViec)
        {

            String SQL = "SELECT COUNT(*) FROM KTKB_NgoaiTe WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThangLamViec=@iThangLamViec";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangLamViec", iThangLamViec);
            int value = Convert.ToInt16(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return value;
        }

        public static String Get_TenNhanVien(String iID_MaNhanVien)
        {
            String SQL = "SELECT sTen FROM KTKB_NgoaiTe WHERE iID_MaNgoaiTe=@iID_MaNgoaiTe AND iTrangThai=1";
            SqlCommand cmd = new SqlCommand(SQL);

            cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iID_MaNhanVien);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
    }
    
}