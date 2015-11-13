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
    public class QLDA_ChuDauTuModels
    {
        public static DataTable Get_DanhSach(String sMaChuDauTu, String sTenChuDauTu, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";
            if (String.IsNullOrEmpty(sMaChuDauTu) == false && sMaChuDauTu != "")
            {
                DK += " AND sMa LIKE '%" + sMaChuDauTu + "%'";
            }
            if (String.IsNullOrEmpty(sTenChuDauTu) == false && sTenChuDauTu != "")
            {
                //quangvv 06/09/2012: them N để tìm kiếm được tiếng việt
                DK += " AND (sTen LIKE N'%" + sTenChuDauTu + "%'";
                DK += " OR sTenVietTat LIKE N'%" + sTenChuDauTu + "%')";
            }
            String SQL = String.Format("SELECT * FROM QLDA_ChuDauTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String sMaChuDauTu, String sTenChuDauTu)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 ";
            if (String.IsNullOrEmpty(sMaChuDauTu) == false && sMaChuDauTu != "")
            {
                DK += " AND sMa LIKE '%" + sMaChuDauTu + "%'";
            }
            if (String.IsNullOrEmpty(sTenChuDauTu) == false && sTenChuDauTu != "")
            {
                DK += " AND (sTen LIKE N'%" + sTenChuDauTu + "%'";
                DK += " OR sTenVietTat LIKE N'%" + sTenChuDauTu + "%')";
            }
            String SQL = String.Format("SELECT Count(*) FROM QLDA_ChuDauTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        public static DataTable Get_Row_ChuDauTu(String sMa) {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_ChuDauTu WHERE iTrangThai = 1 AND sMa=@iID_MaChuDauTu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChuDauTu", sMa);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// lay chu day tu
        /// </summary>
        /// <param name="iID_MaChuDauTu"></param>
        /// <returns></returns>
        public static DataTable Get_Row_ChuDauTuByID(String iID_MaChuDauTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_ChuDauTu WHERE iTrangThai = 1 AND iID_MaChuDauTu=@iID_MaChuDauTu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChuDauTu", iID_MaChuDauTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// quangvv 06/09/2012
        /// Ham kiểm tra trùng lặp 1 trong các dữ liệu sau: mã, tên, tên viết tắt, số tài khoản
        /// </summary>
        /// <param name="sfieldName">ten truong kiem tra trung</param>
        /// <param name="sSearchValue">gia tri kiem tra trung</param>
        /// <returns></returns>
        public static DataTable Get_Row_ChuDauTu_CheckExist(String sfieldName, string sSearchValue, string sEdit, string sMaChuDauTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_ChuDauTu WHERE iTrangThai = 1 AND " + sfieldName + "=@sSearchValue";
                         //"AND (iID_MaChuDauTu=@iID_MaChuDauTu OR sTen = @sTen OR sTenVietTat = @sTenVietTat OR sSoTaiKhoan = @sSoTaiKhoan)";
            if (sEdit.Equals("0"))
            {
                SQL += " AND sMa <> @sMaChuDauTu ";
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sSearchValue", sSearchValue);
            if (sEdit.Equals("0"))
            {
                cmd.Parameters.AddWithValue("@sMaChuDauTu", sMaChuDauTu);
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// quangvv 06/09/2012
        /// Ham kiểm tra trùng lặp 1 trong các dữ liệu sau: mã, tên, tên viết tắt, số tài khoản
        /// </summary>
        /// <param name="sfieldName">ten truong kiem tra trung</param>
        /// <param name="sSearchValue">gia tri kiem tra trung</param>
        ///  <param name="iID">gia tri kiem tra trung</param>
        /// <returns></returns>
        public static DataTable Get_Row_ChuDauTu_CheckExistByMaAndID(String sfieldName, string sSearchValue, string sEdit, string sMaChuDauTu, string iID)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM QLDA_ChuDauTu WHERE iTrangThai = 1 AND " + sfieldName + "=@sSearchValue";
            //"AND (iID_MaChuDauTu=@iID_MaChuDauTu OR sTen = @sTen OR sTenVietTat = @sTenVietTat OR sSoTaiKhoan = @sSoTaiKhoan)";
            if (sEdit.Equals("0"))
            {
                SQL += " AND iID_MaChuDauTu <> @iID_MaChuDauTu ";
            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sSearchValue", sSearchValue);
            if (sEdit.Equals("0"))
            {
                cmd.Parameters.AddWithValue("@iID_MaChuDauTu", iID);
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}