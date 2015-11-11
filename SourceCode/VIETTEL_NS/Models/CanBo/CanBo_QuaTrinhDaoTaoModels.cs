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
    public class CanBo_QuaTrinhDaoTaoModels
    {
        /// <summary>
        /// Lấy danh sách quá trình đào tạo
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach(String iID_MaCanBo = "" , int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM CB_QuaTrinhDaoTao WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);           
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iSTT DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách quá trình đào tạo
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach(String iID_MaCanBo = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM CB_QuaTrinhDaoTao WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY iSTT DESC");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Đếm sô dòng
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static int Get_DanhSach_Count(String iID_MaCanBo = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iID_MaCanBo = @iID_MaCanBo";
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            String SQL = String.Format("SELECT Count(*) FROM CB_QuaTrinhDaoTao WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chi tiết quá trình đào tạo
        /// </summary>
        /// <param name="iID_MaQuaTrinhCongTac"></param>
        /// <returns></returns>
        public static DataTable GetChiTiet(string iID_MaQuaTrinhCongTac)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_QuaTrinhDaoTao WHERE iTrangThai=1 AND iID_MaQuaTrinhCongTac=@iID_MaQuaTrinhCongTac");
            cmd.Parameters.AddWithValue("@iID_MaQuaTrinhCongTac", iID_MaQuaTrinhCongTac);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}