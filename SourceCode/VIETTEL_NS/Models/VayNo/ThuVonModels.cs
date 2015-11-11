using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
namespace VIETTEL.Models
{
    public class ThuVonModels
    {
        public static DataTable getList(String iID_VayChiTiet, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT iID_VayChiTiet, iID_ThuVonChiTiet, sNguoiTra, rThuVon, rThuLai, dNgayTra, sCMNDNguoiTra FROM VN_ThuVonChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet");
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);           
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int getList_Count(String iID_VayChiTiet = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT COUNT(*) ROWNUMBER FROM VN_ThuVonChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet";
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = SQL;        
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int TongTienTraVon(String iID_VayChiTiet = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT SUM(rThuVon) AS rThuVon FROM VN_ThuVonChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet";
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int TongTienTraLai(String iID_VayChiTiet = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT SUM(rThuLai) AS rThuLai FROM VN_ThuVonChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet";
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static Boolean UpdateVayNo(string iID_VayChiTiet, DateTime dNgayTra)
        {
            int iThuVon = TongTienTraVon(iID_VayChiTiet);
            int iTraLai = TongTienTraLai(iID_VayChiTiet);
            DataTable dt = VayNoModels.getDetailChiTiet(iID_VayChiTiet);
            Boolean rMienLai = false;
            decimal rLaiSuat = 0, rVayTrongThang=0, rDuLaiCu=0;
            DateTime dNgayVay;         
            //if (dt.Rows.Count>0)
            //{
            //    DataRow dr = dt.Rows[0];
            //    rMienLai = Convert.ToBoolean(dr["rMienLai"]);
            //    rLaiSuat = Convert.ToDecimal(dr["rLaiSuat"]);
            //    rVayTrongThang = Convert.ToDecimal(dr["rVayTrongThang"]);
            //    dNgayVay = Convert.ToDateTime(dr["dNgayVay"]);
            //    //tính lãi suất
            //   // nếu ngày trả sau ngày 15 tính lãi suất 1 tháng, ngược lại lãi suất =0
            //    if (rMienLai == false && dNgayTra.Day >= 15) rDuLaiCu = rLaiSuat * rVayTrongThang / 100;
            //    else rDuLaiCu = 0;
            //}
            //string sql = "UPDATE VN_VayChiTiet SET rDuVonCu=rVayTrongThang - @rThuVon, rDuLaiCu=@rThuLai - @TienLai, rThuVon=@rThuVon, rThuLai=@rThuLai WHERE iID_VayChiTiet=@iID_VayChiTiet";
           // string sql = "UPDATE VN_VayChiTiet SET rDuVonCu=rVayTrongThang - @rThuVon, rDuLaiCu=rDuLaiCu - @rDuLaiCu, rThuVon=@rThuVon, rThuLai=@rThuLai WHERE iID_VayChiTiet=@iID_VayChiTiet";
            string sql = "UPDATE VN_VayChiTiet SET rDuVonCu=rVayTrongThang - @rThuVon, rDuLaiCu=rDuLaiCu - @rDuLaiCu, rThuVon=@rThuVon, rThuLai=@rThuLai WHERE iID_VayChiTiet=@iID_VayChiTiet";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@rThuVon", iThuVon);
            cmd.Parameters.AddWithValue("@rThuLai", iTraLai);
            cmd.Parameters.AddWithValue("@rDuLaiCu", rDuLaiCu);
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_VayChiTiet);
            cmd.CommandText = sql;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        /// <summary>
        /// Chi tiết thông tin thu vốn
        /// </summary>
        /// <param name="iID_ThuVonChiTiet">Mã thu vốn</param>
        /// <returns></returns>
        public static DataTable getDetail(string iID_ThuVonChiTiet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM VN_ThuVonChiTiet WHERE iID_ThuVonChiTiet=@iID_ThuVonChiTiet");
            cmd.Parameters.AddWithValue("@iID_ThuVonChiTiet", iID_ThuVonChiTiet);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}