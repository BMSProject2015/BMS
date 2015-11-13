using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;


namespace VIETTEL.Models
{
    public class KeToanTongHop_GiaiThichTaiKhoanModels
    {
        public static DataTable Get_DSGiaiThich(String iID_MaTaiKhoan)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = "SELECT * FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 {0} ORDER BY sKyHieu";
            String DK= "";
            if (String.IsNullOrEmpty(iID_MaTaiKhoan) == false && iID_MaTaiKhoan != "")
            {
                DK=" AND iID_MaTaiKhoan= @iID_MaTaiKhoan ";                
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            }
            SQL =String.Format(SQL, DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy danh sách giải thích
        /// </summary>
        /// <param name="sTaiKhoan"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable getList(String sTaiKhoan = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = string.Format(@"SELECT GT.iID_MaTaiKhoanGiaiThich, GT.iID_MaTaiKhoan + ' - ' + TK.sTen AS TaiKhoan, GT.sTen FROM KT_TaiKhoan AS TK, KT_TaiKhoanGiaiThich AS GT WHERE TK.iID_MaTaiKhoan = GT.iID_MaTaiKhoan");

            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                SQL += " AND GT.iID_MaTaiKhoan= @sTaiKhoan";
                cmd.Parameters.AddWithValue("@sTaiKhoan", sTaiKhoan);
            }           
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "GT.dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
       /// <summary>
       /// Đếm số dòng
       /// </summary>
       /// <param name="sTaiKhoan"></param>
       /// <returns></returns>
        public static int getList_Count(String sTaiKhoan = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = " 1=1";
            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                DK += " AND iID_MaTaiKhoan= @sTaiKhoan";
                cmd.Parameters.AddWithValue("@sTaiKhoan", sTaiKhoan);
            }

            String SQL = String.Format("SELECT COUNT(*) FROM KT_TaiKhoanGiaiThich WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết
        /// </summary>
        /// <param name="iID_MaGiaiThich"></param>
        /// <returns></returns>
        public static DataTable getDetail(String iID_MaGiaiThich)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = "SELECT * FROM KT_TaiKhoanGiaiThich WHERE iID_MaGiaiThich=@iID_MaGiaiThich";
            cmd.Parameters.AddWithValue("@iID_MaGiaiThich", iID_MaGiaiThich);
            cmd.CommandText = sql;
            return Connection.GetDataTable(cmd);
        }

    }
}