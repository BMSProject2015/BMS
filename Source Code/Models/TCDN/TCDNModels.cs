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
    public class TCDNModels
    {
        public static int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeThongKeTCDN;
        public static DataTable getdsLoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iLoai", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iLoai"] = "0";
            dr["sTen"] = "Hồ sơ doanh nghiệp";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["iLoai"] = "1";
            dr["sTen"] = "Biểu số 1: CHỈ TIÊU HOẠT ĐỘNG SẢN XUẤT KINH DOANH";
            dt.Rows.InsertAt(dr, 1);

            dr = dt.NewRow();
            dr["iLoai"] = "2";
            dr["sTen"] = "Biểu số 2: CHỈ TIÊU THU CHI NGÂN SÁCH VÀ THU NHẬP";
            dt.Rows.InsertAt(dr, 2);

            dr = dt.NewRow();
            dr["iLoai"] = "3";
            dr["sTen"] = "Biểu số 3: CHỈ TIÊU TÀI CHÍNH";
            dt.Rows.InsertAt(dr, 3);

            dr = dt.NewRow();
            dr["iLoai"] = "4";
            dr["sTen"] = "Biểu số 4: CHỈ TIÊU ĐÁNH GIÁ HIỆU QUẢ";
            dt.Rows.InsertAt(dr, 4);
            return dt;
        }

        public static DataTable getdsLoaiBaoCaoTOngHop()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iLoai", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iLoai"] = "1";
            dr["sTen"] = "Biểu 01-Tổng hợp chỉ tiêu sản xuất kinh doanh";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["iLoai"] = "21";
            dr["sTen"] = "Biểu số 02a:Tổng hợp chỉ tiêu thu ngân sách ";
            dt.Rows.InsertAt(dr, 1);

            dr = dt.NewRow();
            dr["iLoai"] = "22";
            dr["sTen"] = "Biểu số 02b: Tổng hợp chỉ tiêu chi ngân sách";
            dt.Rows.InsertAt(dr, 2);

            dr = dt.NewRow();
            dr["iLoai"] = "23";
            dr["sTen"] = "Biểu số 02c: Tổng hợp chỉ tiêu tiền lương và thu nhập";
            dt.Rows.InsertAt(dr, 3);

            dr = dt.NewRow();
            dr["iLoai"] = "31";
            dr["sTen"] = "Biểu số 3- Tờ số 1: Chỉ tiêu tài chính-Tài sản";
            dt.Rows.InsertAt(dr, 4);
            dr = dt.NewRow();
            dr["iLoai"] = "32,33";
            dr["sTen"] = "Biểu số 3- Tờ số 2: Chỉ tiêu tài chính-Nguồn vốn";
            dt.Rows.InsertAt(dr, 5);
            dr = dt.NewRow();
            dr["iLoai"] = "34";
            dr["sTen"] = "Biểu số 3- Tờ số 3: Chỉ tiêu tài chính-Khác";
            dt.Rows.InsertAt(dr, 6);

            dr = dt.NewRow();
            dr["iLoai"] = "4";
            dr["sTen"] = "Biểu số 4: Chỉ tiêu đánh giá hiệu quả";
            dt.Rows.InsertAt(dr, 7);
            
            return dt;
        }
        public static DataTable getdsDVTo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DVT", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr = dt.NewRow();
            dr["DVT"] = "1";
            dr["sTen"] = "Triệu  đồng";
            dt.Rows.InsertAt(dr, 2);

          
            return dt;
        }
        public static DataTable LoaiDonViThanhVien()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iLoai", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            DataRow dr = dt.NewRow();
            dr["iLoai"] = "1";
            dr["sTen"] = "Công ty hạch toán độc lập";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["iLoai"] = "2";
            dr["sTen"] = "Công ty hạch toán phụ thuộc";
            dt.Rows.InsertAt(dr, 1);


            return dt;
        }
        public static  DataTable getChiTietdtDuAn(String iID_MaDuAn)
        {
            String SQL =
                      String.Format(
                          "SELECT * FROM TCDN_DuAnDauTu WHERE iTrangThai=1 AND iID_MaDuAn=@iID_MaDuAn");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDuAn", iID_MaDuAn);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable getChiTietCongTyLDLK(String iID_Ma)
        {
            String SQL =
                      String.Format(
                          "SELECT * FROM TCDN_CongTyLienDoanhLienKet WHERE iTrangThai=1 AND iID_Ma=@iID_Ma");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_Ma", iID_Ma);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable getChiTietDonViThanhVien(String iID_Ma)
        {
            String SQL =
                      String.Format(
                          "SELECT * FROM TCDN_DonViThanhVien WHERE iTrangThai=1 AND iID_Ma=@iID_Ma");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_Ma", iID_Ma);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable getChiTietLinhVuc(String iID_Ma)
        {
            String SQL =
                      String.Format(
                          "SELECT * FROM TCDN_LinhVuc WHERE iTrangThai=1 AND iID_Ma=@iID_Ma");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_Ma", iID_Ma);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
    
}