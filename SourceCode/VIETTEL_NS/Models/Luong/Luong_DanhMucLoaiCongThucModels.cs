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
    public class Luong_DanhMucLoaiCongThucModels
    {
        public static NameValueCollection LayThongTinLoaiCongThuc(String iID_MaDanhMucLoaiCongThuc)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietLoaiCongThuc(iID_MaDanhMucLoaiCongThuc);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_ChiTietLoaiCongThuc(String iID_MaDanhMucLoaiCongThuc)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM L_DanhMucLoaiCongThuc WHERE iTrangThai=1 AND iID_MaDanhMucLoaiCongThuc=@iID_MaDanhMucLoaiCongThuc";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDanhMucLoaiCongThuc", iID_MaDanhMucLoaiCongThuc);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtDanhMucLoaiCongThuc(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM L_DanhMucLoaiCongThuc WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucLoaiCongThuc()
        {

            String SQL = "SELECT COUNT(*) FROM L_DanhMucLoaiCongThuc WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }
    }
}