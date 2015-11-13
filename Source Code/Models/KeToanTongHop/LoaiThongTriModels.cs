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
    public class LoaiThongTriModels
    {
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="TKNo"></param>
        /// <param name="TKCo"></param>
        /// <param name="LoaiTT"></param>
        /// <param name="LoaiNS"></param>
        /// <param name="page"></param>
        public LoaiThongTriModels(String TKNo, String TKCo, String LoaiTT, String LoaiNS, String page, String UserName)
        {
            this.TKNo = TKNo;
            this.TKCo = TKCo;
            this.LoaiTT = LoaiTT;
            this.LoaiNS = LoaiNS;
   
            this.page = page;
            this.UserName = UserName;
        }
        public String TKNo { get; set; }
        public String TKCo { get; set; }
        public String LoaiTT { get; set; }
        public String LoaiNS { get; set; }
        public String page { get; set; }
        public String UserName { get; set; }
        /// <summary>
        /// Danh sách thông tri
        /// </summary>
        /// <param name="sTaiKhoan"></param>
        /// <param name="sKyHieu"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable getList(String LoaiNS = "", String LoaiTT = "", String TKCo = "", String TKNo = "", int Trang = 1, int SoBanGhi = 0, int Nam = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT * FROM KT_LoaiThongTri WHERE iTrangThai=1";
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
   /// <summary>
        /// Đếm số bản ghi của loại thông tri
   /// </summary>
   /// <param name="LoaiNS"></param>
   /// <param name="LoaiTT"></param>
   /// <param name="TKCo"></param>
   /// <param name="TKNo"></param>
   /// <returns></returns>
        public static int getList_Count(String LoaiNS = "", String LoaiTT = "", String TKCo = "", String TKNo = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "  iTrangThai=1";
            if (String.IsNullOrEmpty(LoaiNS) == false && LoaiNS != "")
            {
                DK += " AND sTenLoaiNS like N'%' + @sTenLoaiNS + '%'";
                cmd.Parameters.AddWithValue("@sTenLoaiNS", LoaiNS);
            }
            if (String.IsNullOrEmpty(LoaiTT) == false && LoaiTT != "")
            {
                DK += " AND sLoaiThongTri like N'%' + @sLoaiThongTri + '%'";
                cmd.Parameters.AddWithValue("@sLoaiThongTri", LoaiTT);
            }
            if (TKCo != Guid.Empty.ToString() && TKCo != "")
            {
                DK += " AND iID_MaTaiKhoanCo = @iID_MaTaiKhoanCo";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanCo", TKCo);
            }
            if (TKNo != Guid.Empty.ToString() && TKNo != "")
            {
                DK += " AND iID_MaTaiKhoanNo = @iID_MaTaiKhoanNo";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanNo", TKNo);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM KT_LoaiThongTri WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chi tiết thông tri
        /// </summary>
        /// <param name="iID_MaThongTri"></param>
        /// <returns></returns>
        public static DataTable getChiTiet(String iID_MaThongTri)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_LoaiThongTri WHERE iTrangThai=1 AND iID_MaThongTri=@iID_MaThongTri");
            cmd.Parameters.AddWithValue("@iID_MaThongTri", iID_MaThongTri);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// 
        /// lẤY LOẠI THONG TRI VÀ NGÂN SÁCH ĐƯA VÀO BÁO CÁO THÔNG TRI
        /// </summary>
        /// <param name="TaiKhoanNo"></param>
        /// <param name="TaiKhoanCo"></param>
        /// <param name="LoaiNS"></param>
        /// <param name="LoaiThongTri"></param>
        public static void LayLoaiThongTri(string TaiKhoanNo, string TaiKhoanCo, ref string LoaiNS, ref string LoaiThongTri)
        {           
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sLoaiThongTri, sTenLoaiNS FROM KT_LoaiThongTri WHERE iTrangThai=1";
            if (String.IsNullOrEmpty(TaiKhoanNo) == false && TaiKhoanNo != "")
            {
                SQL += " AND iID_MaTaiKhoanNo = @iID_MaTaiKhoanNo";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanNo", TaiKhoanNo);
            }
            if (String.IsNullOrEmpty(TaiKhoanCo) == false && TaiKhoanCo != "")
            {
                SQL += " AND iID_MaTaiKhoanCo = @iID_MaTaiKhoanCo";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanCo", TaiKhoanCo);
            }
            cmd.CommandText = SQL;
            var dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (HamChung.ConvertToString(dr["sLoaiThongTri"]) == "")
                    LoaiThongTri = "THÔNG TRI CẤP";
                else
                    LoaiThongTri = HamChung.ConvertToString(dr["sLoaiThongTri"]);
                if (HamChung.ConvertToString(dr["sTenLoaiNS"]) == "")
                    LoaiNS = "KINH PHÍ";
                else LoaiNS = HamChung.ConvertToString(dr["sTenLoaiNS"]);
            }
            else
            { 
                LoaiThongTri = "THÔNG TRI CẤP";
                LoaiNS = "KINH PHÍ";
            }
            cmd.Dispose();
            if (dt != null) dt.Dispose();
           
        }
    }
   
}