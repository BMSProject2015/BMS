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
    public class LoaiTaiSanModels
    {
         /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="TKNo"></param>
        /// <param name="TKCo"></param>
        /// <param name="LoaiTT"></param>
        /// <param name="LoaiNS"></param>
        /// <param name="page"></param>
        public LoaiTaiSanModels(String sKyHieu, String sTen, String page)
        {
            this.sKyHieu = sKyHieu;
            this.sTen = sTen;
            
            this.page = page;
        }
        public String sKyHieu { get; set; }
        public String sTen { get; set; }
      
        public String page { get; set; }

        /// <summary>
        /// Danh sách thông tri
        /// </summary>
        /// <param name="sTaiKhoan"></param>
        /// <param name="sKyHieu"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable getList(String KyHieu = "", String Ten = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = "SELECT iID_MaLoaiTaiSan, sKyHieu, sTen, sMoTa, rTyLeKhauHao FROM KTCS_LoaiTaiSan WHERE 1=1";
            if (String.IsNullOrEmpty(KyHieu) == false && KyHieu != "")
            {
                SQL += " AND sKyHieu= @KyHieu";
                cmd.Parameters.AddWithValue("@KyHieu", KyHieu);
            }
            if (String.IsNullOrEmpty(Ten) == false && Ten != "")
            {
                SQL += " AND sTen= @Ten";
                cmd.Parameters.AddWithValue("@Ten", Ten);
            }
           
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
   /// <summary>
        /// Đếm số bản ghi 
   /// </summary>
   /// <param name="LoaiNS"></param>
   /// <param name="LoaiTT"></param>
   /// <param name="TKCo"></param>
   /// <param name="TKNo"></param>
   /// <returns></returns>
        public static int getList_Count(String KyHieu = "", String Ten = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = " 1=1";        
            if (String.IsNullOrEmpty(KyHieu) == false && KyHieu != "")
            {
                DK += " AND sKyHieu= @KyHieu";
                cmd.Parameters.AddWithValue("@KyHieu", KyHieu);
            }
            if (String.IsNullOrEmpty(Ten) == false && Ten != "")
            {
                DK += " AND sTen= @Ten";
                cmd.Parameters.AddWithValue("@Ten", Ten);
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
        public static DataTable getChiTiet(String iID_MaLoaiTaiSan)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 AND iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chi tiết thông tri
        /// </summary>
        /// <param name="iID_MaThongTri"></param>
        /// <returns></returns>
        public static Double get_rSoNamKhauHao(String iID_MaLoaiTaiSan)
        {
            Double vR;
            SqlCommand cmd = new SqlCommand("SELECT rSoNamKhauHao FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 AND iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
            vR = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách loại tài sản
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable DT_LoaiTS(Boolean All = false, String TieuDe = "")
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaLoaiTaiSan, sKyHieu +' - '+ sTen AS TenHT FROM KTCS_LoaiTaiSan ORDER By iSTT");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaLoaiTaiSan"] = Guid.Empty;
                R["TenHT"] = TieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
    }
}