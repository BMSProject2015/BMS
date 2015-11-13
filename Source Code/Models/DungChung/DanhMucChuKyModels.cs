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
    public class DanhMucChuKyModels
    {
        #region Danh mục chữ ký

        public static NameValueCollection LayThongTinChuKy(String iID_MaChuKy)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucChuKy(iID_MaChuKy);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_ChiTietDanhMucChuKy(String iID_MaChuKy)
        {
            DataTable dt = null;
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaChuKy) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaChuKy=@iID_MaChuKy";
                cmd.Parameters.AddWithValue("@iID_MaChuKy", iID_MaChuKy);
                String SQL = "SELECT * FROM NS_DanhMucChuKy WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }


            return dt;
        }

        public static DataTable Get_dtDanhMucChuKy()
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            String SQL = "SELECT sKyHieu+' - ' + sTen AS TenHienThi ,* FROM NS_DanhMucChuKy WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static String LayTenChuKy(String iID_MaChuKy)
        {
            String vR="";
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaChuKy) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaChuKy=@iID_MaChuKy";
                cmd.Parameters.AddWithValue("@iID_MaChuKy", iID_MaChuKy);
                String SQL = "SELECT sTen FROM NS_DanhMucChuKy WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                vR = Connection.GetValueString(cmd,"");
                cmd.Dispose();
            }


            return vR;
        }

        /// <summary>
        /// Lấy datatable Danh mục chữ ký
        /// </summary>
        /// <param name="iID_MaBacLuong">Mã chữ ký  nếu bằng "" lấy tất cả danh mục chữ ký</param>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucChuKy(String iID_MaChuKy = "")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaChuKy) == false)
            {
                DK = " AND iID_MaChuKy=@iID_MaChuKy";
                cmd.Parameters.AddWithValue("@iID_MaChuKy", iID_MaChuKy);
            }
            String SQL = "SELECT * FROM NS_DanhMucChuKy WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            return dt;
        }

        public static DataTable Get_dtDanhMucChuKy(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM NS_DanhMucChuKy WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sKyHieu", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucChuKy()
        {

            String SQL = "SELECT COUNT(*) FROM NS_DanhMucChuKy WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

        #endregion

    }
}