using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;

namespace VIETTEL.Models
{
    public class PhanHe_TrangThaiDuyetModel
    {
        public static DataTable NS_PhanHe_TrangThaiDuyet(String MaPhanHe)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand();
            String DK = " ";
            if (String.IsNullOrEmpty(MaPhanHe) == false)
            {
                DK = "  WHERE iID_MaPhanHe=@iID_MaPhanHe";
                cmd.Parameters.AddWithValue("@iID_MaPhanHe", MaPhanHe);
            }

            String SQL = String.Format("SELECT * FROM NS_PhanHe_TrangThaiDuyet {0} order by iSTT", DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable DT_PhanHe(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaPhanHe,sTen FROM NS_PhanHe");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaPhanHe"] = -1;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static DataTable DT_NguoiDung(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaNhomNguoiDung,sTen FROM QT_NhomNguoiDung WHERE iTrangThai=1");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaNhomNguoiDung"] = "";
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static DataTable DT_LoaiTrangThaiDuyet(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("iLoaiTrangThaiDuyet",typeof(int));
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("sTen");
            dt.Columns.Add(dc2);
            if (ThemDongTieuDe)
            {
                //Them dong tieu de
                DataRow R = dt.NewRow();
                R["iLoaiTrangThaiDuyet"] = -1;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
                //Them dong tao moi
                R = dt.NewRow();
                R["iLoaiTrangThaiDuyet"] = 1;
                R["sTen"] = "Tạo mới";
                dt.Rows.InsertAt(R, 1);
                //Them dong dang duyet
                R = dt.NewRow();
                R["iLoaiTrangThaiDuyet"] = 2;
                R["sTen"] = "Đang duyệt";
                dt.Rows.InsertAt(R, 2);
                //Them dong tu choi
                R = dt.NewRow();
                R["iLoaiTrangThaiDuyet"] = 3;
                R["sTen"] = "Từ chối";
                dt.Rows.InsertAt(R, 3);
                //Them dong da duyet
                R = dt.NewRow();
                R["iLoaiTrangThaiDuyet"] = 4;
                R["sTen"] = "Đã duyệt";
                dt.Rows.InsertAt(R, 4);
            }
            return dt;
        }
        public static DataTable GetRow_PhanHe_TrangThaiDuyet(String MaTrangThaiDuyet)
        {
            DataTable dt;
            String SQL = "SELECT * FROM NS_PhanHe_TrangThaiDuyet WHERE iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public static DataTable DT_TrangThaiDuyet(Boolean ThemDongTieuDe, String sDongTieuDe, String iID_MaPhanHe)
        {
            DataTable dt;
            String SQL = String.Format("SELECT iID_MaTrangThaiDuyet,sTen FROM NS_PhanHe_TrangThaiDuyet WHERE iID_MaPhanHe=@iID_MaPhanHe ORDER BY iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            dt = Connection.GetDataTable(cmd);
            if (ThemDongTieuDe)
            {
                DataRow R = dt.NewRow();
                R["iID_MaTrangThaiDuyet"] = -1;
                R["sTen"] = sDongTieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }



    }
}