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
    public class KTCT_KhoBac_ChuongTrinhMucTieuModels
    {
        /// <summary>
        /// Lấy danh sách trong bảng chương trình mục tiêu
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_dtChuongTrinhMucTieu()
        {
            String SQL = "SELECT * FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1 ORDER BY iSTT";
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
        /// <summary>
        /// Lấy danh sách chương trình mục tiêu có phân trang
        /// </summary>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1";
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iSTT", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số danh sách chương trình mục tiêu
        /// </summary>
        /// <returns></returns>
        public static int Get_DanhSach_Count()
        {
            int vR;
            String SQL = "SELECT COUNT(*) FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1";
            vR = Convert.ToInt32(Connection.GetValue(SQL, 0));
            return vR;
        }
        /// <summary>
        /// Lấy thông tin một chương trình mục tiêu
        /// </summary>
        /// <param name="iID_MaChuongTrinhMucTieu"></param>
        /// <returns></returns>
        public static DataTable Get_RowChuongTrinhMucTieu(String iID_MaChuongTrinhMucTieu)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1 AND iID_MaChuongTrinhMucTieu = @iID_MaChuongTrinhMucTieu");
            cmd.Parameters.AddWithValue("@iID_MaChuongTrinhMucTieu", iID_MaChuongTrinhMucTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin tên chương trình mục tiêu
        /// </summary>
        /// <param name="iID_MaChuongTrinhMucTieu"></param>
        /// <returns></returns>
        public static String Get_TenChuongTrinhMucTieu(String iID_MaChuongTrinhMucTieu)
        {
            String vR = "";
            SqlCommand cmd = new SqlCommand("SELECT sTen FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1 AND iID_MaChuongTrinhMucTieu = @iID_MaChuongTrinhMucTieu");
            cmd.Parameters.AddWithValue("@iID_MaChuongTrinhMucTieu", iID_MaChuongTrinhMucTieu);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            return vR;
        }
    }
}