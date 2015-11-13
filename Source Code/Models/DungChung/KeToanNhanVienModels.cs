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
    public class KeToanNhanVienModels
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
                DK = " iID_MaNhanVien=@iID_MaNhanVien";
                cmd.Parameters.AddWithValue("@iID_MaNhanVien", iID_MaNhanVien);
                String SQL = "SELECT * FROM KT_NhanVien WHERE iTrangThai=1 AND {0}";
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
                DK = " AND iID_MaNhanVien=@iID_MaNhanVien";
                cmd.Parameters.AddWithValue("@iID_MaNhanVien", iID_MaNhanVien);
            }
            String SQL = "SELECT * FROM KT_NhanVien WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_dtDanhMucNhanVien(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM KT_NhanVien WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaNhanVien", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucNhanVien()
        {

            String SQL = "SELECT COUNT(*) FROM KT_NhanVien WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

        public static String Get_TenNhanVien(String iID_MaNhanVien)
        {
            String SQL = "SELECT sTenNhanVien FROM KT_NhanVien WHERE iID_MaNhanVien=@iID_MaNhanVien AND iTrangThai=1";
            SqlCommand cmd = new SqlCommand(SQL);

            cmd.Parameters.AddWithValue("@iID_MaNhanVien", iID_MaNhanVien);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
    }
}